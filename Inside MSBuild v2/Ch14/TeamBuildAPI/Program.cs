using System;
using System.Linq;
using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.Client;

namespace TeamBuildAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            //TfsTeamProjectCollection tpc = TfsTeamProjectCollectionFactory
            //    .GetTeamProjectCollection(new Uri("http://TFSRTM10:8080"));

            TfsTeamProjectCollection tpc = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(
                new Uri("http://TFSRTM10:8080"),
                new UICredentialsProvider()
            );

            IBuildServer buildServer = tpc.GetService<IBuildServer>();
            QueryBuildServiceHosts(buildServer);
            QueryBuildDefinitions(buildServer);
            BulkUpdateController(buildServer);
            CreateBuildDefinition(buildServer);
            QueueAllBuildDefinitions(buildServer);
            CancelQueuedBuilds(buildServer);
            QueryBuildHistory(buildServer);
        }

        static void QueryBuildServiceHosts(IBuildServer buildServer)
        {
            Console.WriteLine();
            Console.WriteLine("QueryBuildServiceHosts:");

            IBuildServiceHost[] buildServiceHosts = buildServer.QueryBuildServiceHosts("*");
            foreach (var buildServiceHost in buildServiceHosts)
            {
                Console.WriteLine("\t{0}", buildServiceHost.BaseUrl);
            }
        }

        static void QueryBuildDefinitions(IBuildServer buildServer)
        {
            Console.WriteLine();
            Console.WriteLine("QueryBuildDefinitions:");

            IBuildDefinition[] buildDefinitions = buildServer.QueryBuildDefinitions("Contoso");
            foreach (var buildDefinition in buildDefinitions)
            {
                Console.WriteLine("\t{0}", buildDefinition.Name);
            }
        }

        static void BulkUpdateController(IBuildServer buildServer)
        {
            IBuildDefinition[] buildDefinitions = buildServer.QueryBuildDefinitions("Contoso");

            IBuildController defaultBuildController = buildServer.QueryBuildServiceHosts("*")
                .Where(bsh => bsh.Controller != null).Select(bsh => bsh.Controller).First();

            foreach (IBuildDefinition buildDefinition in buildDefinitions)
            {
                buildDefinition.BuildController = defaultBuildController;
                buildDefinition.Save();
            }

        }

        static void CreateBuildDefinition(IBuildServer buildServer)
        {
            IBuildController defaultBuildController = buildServer.QueryBuildServiceHosts("*")
                .Where(bsh => bsh.Controller != null).Select(bsh => bsh.Controller).First();

            IBuildDefinition buildDefinition = buildServer.CreateBuildDefinition("Contoso");
            buildDefinition.Name = "HelloWorld";
            buildDefinition.ContinuousIntegrationType = ContinuousIntegrationType.Individual;
            buildDefinition.BuildController = defaultBuildController;
            buildDefinition.DefaultDropLocation = @"\\CONTOSO\Projects\HelloWorld\drops";

            IRetentionPolicy retentionPolicy = buildDefinition.RetentionPolicyList.Where(
                rp => rp.BuildReason == BuildReason.Triggered
                    || rp.BuildStatus == BuildStatus.Succeeded)
                .First();
            retentionPolicy.NumberToKeep = 2;
            retentionPolicy.DeleteOptions = DeleteOptions.All;

            buildDefinition.Save(); 
        }

        static void QueueAllBuildDefinitions(IBuildServer buildServer)
        {
            foreach (IBuildDefinition buildDefinition in buildServer.QueryBuildDefinitions("Contoso"))
            {
                IBuildRequest request;
                request = buildDefinition.CreateBuildRequest();

                buildServer.QueueBuild(request);
            }
        }

        static void CancelQueuedBuilds(IBuildServer buildServer)
        {
            IQueuedBuildsView queuedBuildsView = buildServer.CreateQueuedBuildsView("Contoso");
            queuedBuildsView.StatusFilter = QueueStatus.Queued;
            queuedBuildsView.QueryOptions = QueryOptions.Definitions | QueryOptions.Controllers;
            queuedBuildsView.Refresh(false);

            foreach (IQueuedBuild queuedBuild in queuedBuildsView.QueuedBuilds) {
                queuedBuild.Cancel();
            }
        }

        static void QueryBuildHistory(IBuildServer buildServer)
        {
            Console.WriteLine();
            Console.WriteLine("QueryBuildHistory:");

            IBuildDetailSpec spec = buildServer.CreateBuildDetailSpec("Contoso");
            spec.MinFinishTime = DateTime.Now.AddDays(-5);
            IBuildDetail[] builds = buildServer.QueryBuilds(spec).Builds;

            foreach (IBuildDetail build in builds)
            {
                Console.WriteLine(build.BuildNumber);
            }
        }
    }
}
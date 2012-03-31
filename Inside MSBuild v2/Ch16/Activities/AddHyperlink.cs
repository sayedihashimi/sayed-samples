using Microsoft.TeamFoundation.Build.Client;
using System.Activities;
using System;

[BuildActivity(HostEnvironmentOption.All)]
public class AddHyperlink : CodeActivity
{
    protected override void Execute(CodeActivityContext context)
    {
        var buildDetail = context.GetExtension<IBuildDetail>();
        var externalLink = InformationNodeConverters.AddExternalLink(buildDetail.Information,
            "ScorchWorkspace has been obsoleted. Click for more information.",
            new Uri("http://buildweb/help/scorchworkspace.html"));
        externalLink.Save();
    }
}

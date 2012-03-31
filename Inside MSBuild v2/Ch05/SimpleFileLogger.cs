namespace Examples.Loggers
{
    using System.Text;
    using Microsoft.Build.Framework;

    /// <summary>
    /// This class is simply for demonstration purposes, a better file logger to use is the
    /// Microsoft.Build.Engine.FileLogger class.
    /// 
    /// Author: Sayed Ibrahim Hashimi (sayed.hashimi@gmail.com)
    /// This class has not been throughly tested and is offered with no warranty.
    /// copyright Sayed Ibrahim Hashimi 2005
    /// </summary>
    public class SimpleFileLogger : Microsoft.Build.Utilities.Logger
    {
        #region Fields
        private string fileName;
        private StringBuilder messages;
        #endregion

        #region ILogger Members
        public override void Initialize(IEventSource eventSource)
        {
            fileName = "simple.log";
            messages = new StringBuilder();

            //Register for the events here
            eventSource.BuildStarted +=
                new BuildStartedEventHandler(this.BuildStarted);
            eventSource.BuildFinished +=
                new BuildFinishedEventHandler(this.BuildFinished);
            eventSource.ProjectStarted +=
                new ProjectStartedEventHandler(this.ProjectStarted);
            eventSource.ProjectFinished +=
                new ProjectFinishedEventHandler(this.ProjectFinished);
            eventSource.ErrorRaised +=
                new BuildErrorEventHandler(this.BuildError);
            eventSource.WarningRaised +=
                new BuildWarningEventHandler(this.BuildWarning);
            eventSource.MessageRaised +=
                new BuildMessageEventHandler(this.BuildMessage);
        }
        public override void Shutdown()
        {
            System.IO.File.WriteAllText(fileName, messages.ToString());
        }
        #endregion
        #region Logging handlers
        void BuildStarted(object sender, BuildStartedEventArgs e)
        {
            AppendLine("BuildStarted: " + e.Message);
        }
        void BuildFinished(object sender, BuildFinishedEventArgs e)
        {
            AppendLine("BuildFinished: " + e.Message);
        }
        void ProjectStarted(object sender, ProjectStartedEventArgs e)
        {
            AppendLine("ProjectStarted: " + e.Message);
        }
        void ProjectFinished(object sender, ProjectFinishedEventArgs e)
        {
            AppendLine("ProjectFinished: " + e.Message);
        }
        void TargetStarted(object sender, TargetStartedEventArgs e)
        {
            AppendLine("TargetStarted: " + e.Message);
        }
        void TargetFinished(object sender, TargetFinishedEventArgs e)
        {
            AppendLine("TargetFinished: " + e.Message);
        }
        void TaskStarted(object sender, TaskStartedEventArgs e)
        {
            AppendLine("TaskStarted: " + e.Message);
        }
        void TaskFinished(object sender, TaskFinishedEventArgs e)
        {
            AppendLine("TaskFinished: " + e.Message);
        }
        void BuildError(object sender, BuildErrorEventArgs e)
        {
            AppendLine("ERROR: " + e.Message);
        }
        void BuildWarning(object sender, BuildWarningEventArgs e)
        {
            AppendLine("Warning: " + e.Message);
        }
        void BuildMessage(object sender, BuildMessageEventArgs e)
        {
            AppendLine("BuildMessage: " + e.Message);
        }
        #endregion
        protected void AppendLine(string line)
        {
            messages.AppendLine(line);
        }
    }

}

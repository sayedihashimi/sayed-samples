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
    public class IndentFileLogger : Microsoft.Build.Utilities.Logger
    {
        #region Fields
        private string fileName;
        private StringBuilder messages;
        private int currentIndent = 0;
        #endregion

        #region ILogger Members
        public override void Initialize(IEventSource eventSource)
        {
            fileName = "indent.log";
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
            eventSource.TargetStarted +=
                new TargetStartedEventHandler(this.TargetStarted);
            eventSource.TargetFinished +=
                new TargetFinishedEventHandler(this.TargetFinished);
            eventSource.TaskStarted +=
                new TaskStartedEventHandler(this.TaskStarted);
            eventSource.TaskFinished +=
                new TaskFinishedEventHandler(this.TaskFinished);
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
            string indentStr = new string('\t', currentIndent++);
            AppendLine(string.Format("{0}BuildStarted:{1}", indentStr, e.Message));
        }
        void BuildFinished(object sender, BuildFinishedEventArgs e)
        {
            string indentStr = new string('\t', currentIndent--);
            AppendLine(string.Format("{0}BuildFinished:{1}", indentStr, e.Message));
        }
        void ProjectStarted(object sender, ProjectStartedEventArgs e)
        {
            string indentStr = new string('\t', currentIndent++);
            AppendLine(string.Format("{0}ProjectStarted:{1}", indentStr, e.Message));
        }
        void ProjectFinished(object sender, ProjectFinishedEventArgs e)
        {
            string indentStr = new string('\t', currentIndent--);
            AppendLine(string.Format("{0}ProjectFinished:{1}", indentStr, e.Message));
        }
        void TargetStarted(object sender, TargetStartedEventArgs e)
        {
            string indentStr = new string('\t', currentIndent++);
            AppendLine(string.Format("{0}TargetStarted:{1}", indentStr, e.Message));
        }
        void TargetFinished(object sender, TargetFinishedEventArgs e)
        {
            string indentStr = new string('\t', currentIndent--);
            AppendLine(string.Format("{0}TargetFinished:{1}", indentStr, e.Message));
        }
        void TaskStarted(object sender, TaskStartedEventArgs e)
        {
            string indentStr = new string('\t', currentIndent++);
            AppendLine(string.Format("{0}TaskStarted:{1}", indentStr, e.Message));
        }
        void TaskFinished(object sender, TaskFinishedEventArgs e)
        {
            string indentStr = new string('\t', currentIndent--);
            AppendLine(string.Format("{0}TaskFinished:{1}", indentStr, e.Message));
        }
        void BuildError(object sender, BuildErrorEventArgs e)
        {
            string indentStr = new string('\t', currentIndent);
            AppendLine(string.Format("{0}ERROR:{1}", indentStr, e.Message));
        }
        void BuildWarning(object sender, BuildWarningEventArgs e)
        {
            string indentStr = new string('\t', currentIndent);
            AppendLine(string.Format("{0}Warning:{1}", indentStr, e.Message));
        }
        void BuildMessage(object sender, BuildMessageEventArgs e)
        {
            string indentStr = new string('\t', currentIndent);
            AppendLine(string.Format("{0}BuildMessage:{1}", indentStr, e.Message));
        }
        #endregion
        protected void AppendLine(string line)
        {
            messages.AppendLine(line);
        }
    }
}

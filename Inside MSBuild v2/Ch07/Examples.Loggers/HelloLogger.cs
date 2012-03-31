namespace Examples.Loggers
{
    using System;
    using System.IO;
    using Microsoft.Build.Framework;

    public class HelloLogger : ILogger
    {
        #region Non-public properties
        protected StreamWriter writer;
        #endregion

        #region ILogger Members
        
        public void Initialize(IEventSource eventSource)
        {
            //always writes to a log with this name
            string logFile = "hello.log";
            if (File.Exists(logFile))
            { File.Delete(logFile); }

            //initialize the writer
            writer = new StreamWriter(logFile);
            writer.AutoFlush = true;
            //this write must be closed in the Shutdown() method

            //register to the events you are interested in here
            eventSource.AnyEventRaised +=
                new AnyEventHandler(AnyEventRaised);
            eventSource.BuildStarted +=
                new BuildStartedEventHandler(BuildStarted);
            eventSource.BuildFinished +=
                new BuildFinishedEventHandler(BuildFinished);
            eventSource.CustomEventRaised +=
                new CustomBuildEventHandler(CustomEvent);
            eventSource.ErrorRaised +=
                new BuildErrorEventHandler(ErrorRaised);
            eventSource.MessageRaised +=
                new BuildMessageEventHandler(MessageRaised);
            eventSource.ProjectStarted +=
                new ProjectStartedEventHandler(ProjectStarted);
            eventSource.ProjectStarted +=
                new ProjectStartedEventHandler(ProjectFinished);
            eventSource.StatusEventRaised +=
                new BuildStatusEventHandler(StatusEvent);
            eventSource.TargetStarted +=
                new TargetStartedEventHandler(TargetStarted);
            eventSource.TargetFinished +=
                new TargetFinishedEventHandler(TargetFinished);
            eventSource.TaskStarted +=
                new TaskStartedEventHandler(TaskStarted);
            eventSource.TaskFinished +=
                new TaskFinishedEventHandler(TaskFinished);
            eventSource.WarningRaised +=
                new BuildWarningEventHandler(WarningRaised);
        }

        #region Build event handlers
        void WarningRaised(object sender, BuildWarningEventArgs e)
        { writer.WriteLine(GetLogMessage("WarningRaised", e)); }
        void TaskFinished(object sender, TaskFinishedEventArgs e)
        { writer.WriteLine(GetLogMessage("TaskFinished", e)); }
        void TaskStarted(object sender, TaskStartedEventArgs e)
        { writer.WriteLine(GetLogMessage("TaskStarted", e)); }
        void TargetFinished(object sender, TargetFinishedEventArgs e)
        { writer.WriteLine(GetLogMessage("TargetFinished", e)); }
        void TargetStarted(object sender, TargetStartedEventArgs e)
        { writer.WriteLine(GetLogMessage("TargetStarted", e)); }
        void ProjectFinished(object sender, ProjectStartedEventArgs e)
        { writer.WriteLine(GetLogMessage("ProjectFinished", e)); }
        void ProjectStarted(object sender, ProjectStartedEventArgs e)
        { writer.WriteLine(GetLogMessage("ProjectStarted", e)); }
        void MessageRaised(object sender, BuildMessageEventArgs e)
        { writer.WriteLine(GetLogMessage("MessageRaised", e)); }
        void ErrorRaised(object sender, BuildErrorEventArgs e)
        { writer.WriteLine(GetLogMessage("ErrorRaised", e)); }
        void CustomEvent(object sender, CustomBuildEventArgs e)
        { writer.WriteLine(GetLogMessage("CustomEvent", e)); }
        void BuildFinished(object sender, BuildFinishedEventArgs e)
        { writer.WriteLine(GetLogMessage("BuildFinished", e)); }
        void StatusEvent(object sender, BuildStatusEventArgs e)
        { writer.WriteLine(GetLogMessage("StatusEvent", e)); }
        void AnyEventRaised(object sender, BuildEventArgs e)
        { writer.WriteLine(GetLogMessage("AnyEventRaised", e)); }


        void BuildStarted(object sender, BuildStartedEventArgs e)
        { writer.WriteLine(GetLogMessage("BuildStarted", e)); }
        #endregion

        /// <summary>
        /// This is set by the MSBuild engine
        /// </summary>
        public string Parameters
        { get; set; }
        /// <summary>
        /// Called by MSBuild engine to give you a chance to
        /// perform any cleanup
        /// </summary>
        public void Shutdown()
        {
            //close the writer
            if (writer != null)
            {
                writer.Flush();
                writer.Close();
                writer = null;
            }
        }
        public LoggerVerbosity Verbosity
        { get; set; }


        protected string GetLogMessage(string eventName, BuildEventArgs e)
        {
            if (string.IsNullOrEmpty(eventName)) { throw new ArgumentNullException("eventName"); }

            ////e.SenderName is typically set to MSBuild when called through msbuild.exe
            string eMessage = string.Format("{0}\t{1}\t{2}",
                        eventName,
                        FormatString(e.Message),
                        FormatString(e.HelpKeyword)
                        );
            return eMessage;
        }
        protected string FormatString(string str)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(str))
            {
                result = str.Replace("\t", "    ")
                    .Replace("\r\n", "\r\n\t\t\t\t");
            }
            return result;
        }

        #endregion
    }
}

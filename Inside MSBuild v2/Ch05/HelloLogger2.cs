namespace Examples.Loggers
{
    using System;
    using System.IO;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    public class HelloLogger2 : Logger
    {
        #region Non-public properties
        protected StreamWriter writer;
        protected string LogFile { get; set; }
        #endregion

        #region ILogger Members
        public override void Initialize(IEventSource eventSource)
        {
            //parse the values passed in as parameters
            InitializeParameters();

            if (string.IsNullOrEmpty(LogFile))
            {
                //apply default log name here
                LogFile = "hello2.log";
            }

            //always writes to a log with this name
            if (File.Exists(LogFile))
            { File.Delete(LogFile); }

            //initialize the writer
            writer = new StreamWriter(LogFile);

            //register to the events you are interested in here
            eventSource.BuildStarted += BuildStarted;
            eventSource.BuildFinished += BuildFinished;
            eventSource.CustomEventRaised += CustomEvent;
            eventSource.ErrorRaised += ErrorRaised;
            eventSource.MessageRaised += MessageRaised;
            eventSource.ProjectStarted += ProjectStarted;
            eventSource.ProjectStarted += ProjectFinished;
            eventSource.TargetStarted += TargetStarted;
            eventSource.TargetFinished += TargetFinished;
            eventSource.TaskStarted += TaskStarted;
            eventSource.TaskFinished += TaskFinished;
            eventSource.WarningRaised += WarningRaised;
        }

        #region Build event handlers
        void ErrorRaised(object sender, BuildErrorEventArgs e)
        {
            //always write out errors
            writer.WriteLine(GetLogMessage("ErrorRaised", e));
        }
        void WarningRaised(object sender, BuildWarningEventArgs e)
        {
            //always log warnings
            writer.WriteLine(GetLogMessage("WarningRaised", e));
        }
        void BuildStarted(object sender, BuildStartedEventArgs e)
        {
            if (IsVerbosityAtLeast(LoggerVerbosity.Normal))
            {
                writer.WriteLine(GetLogMessage("BuildStarted", e));
            }
        }
        void BuildFinished(object sender, BuildFinishedEventArgs e)
        {
            if (IsVerbosityAtLeast(LoggerVerbosity.Normal))
            {
                writer.WriteLine(GetLogMessage("BuildFinished", e));
            }
        }
        void TaskFinished(object sender, TaskFinishedEventArgs e)
        {
            if (IsVerbosityAtLeast(LoggerVerbosity.Detailed))
            {
                writer.WriteLine(GetLogMessage("TaskFinished", e));
            }
        }
        void TaskStarted(object sender, TaskStartedEventArgs e)
        {
            if (IsVerbosityAtLeast(LoggerVerbosity.Detailed))
            {
                writer.WriteLine(GetLogMessage("TaskStarted", e));
            }
        }
        void TargetFinished(object sender, TargetFinishedEventArgs e)
        {
            if (IsVerbosityAtLeast(LoggerVerbosity.Detailed))
            {
                writer.WriteLine(GetLogMessage("TargetFinished", e));
            }
        }
        void TargetStarted(object sender, TargetStartedEventArgs e)
        {
            if (IsVerbosityAtLeast(LoggerVerbosity.Detailed))
            {
                writer.WriteLine(GetLogMessage("TargetStarted", e));
            }
        }
        void ProjectFinished(object sender, ProjectStartedEventArgs e)
        {
            if (IsVerbosityAtLeast(LoggerVerbosity.Detailed))
            {
                writer.WriteLine(GetLogMessage("ProjectFinished", e));
            }
        }
        void ProjectStarted(object sender, ProjectStartedEventArgs e)
        {
            if (IsVerbosityAtLeast(LoggerVerbosity.Detailed))
            {
                writer.WriteLine(GetLogMessage("ProjectStarted", e));
            }
        }
        void MessageRaised(object sender, BuildMessageEventArgs e)
        {
            bool logMessage = false;

            switch (e.Importance)
            {
                case MessageImportance.High:
                    logMessage = IsVerbosityAtLeast(LoggerVerbosity.Minimal);
                    break;
                case MessageImportance.Normal:
                    logMessage = IsVerbosityAtLeast(LoggerVerbosity.Normal);
                    break;
                case MessageImportance.Low:
                    logMessage = IsVerbosityAtLeast(LoggerVerbosity.Detailed);
                    break;
                default:
                    throw new LoggerException(
                        string.Format(
                        "Unrecognized value for MessageImportance: [{0}]",
                        e.Importance));
            }

            if (logMessage)
            {
                writer.WriteLine(GetLogMessage("MessageRaised", e));
            }
        }

        void CustomEvent(object sender, CustomBuildEventArgs e)
        {
            if (IsVerbosityAtLeast(LoggerVerbosity.Detailed))
            {
                writer.WriteLine(GetLogMessage("CustomEvent", e));
            }
        }


        #endregion
        /// <summary>
        /// Called by MSBuild engine to give you a chance to
        /// perform any cleanup
        /// </summary>
        public override void Shutdown()
        {
            //close the writer
            if (writer != null)
            {
                writer.WriteLine("SHUTDOWN");
                writer.Flush();
                writer.Close();
                writer = null;
            }
        }
        #endregion

        #region Helper methods
        protected string GetLogMessage(string eventName, BuildEventArgs e)
        {
            if (string.IsNullOrEmpty(eventName)) { throw new ArgumentNullException("eventName"); }

            string eventMessage = e.Message;
            if (e is BuildWarningEventArgs)
            {
                eventMessage = base.FormatWarningEvent((BuildWarningEventArgs)e);
            }
            else if (e is BuildErrorEventArgs)
            {
                eventMessage = base.FormatErrorEvent((BuildErrorEventArgs)e);
            }

            string eMessage = string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}",
                        e.Timestamp,
                        e.ThreadId,
                        FormatString(e.SenderName),
                        eventName,
                        FormatString(eventMessage),
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
        /// <summary>
        /// Read values form <c>Parameters</c> string and populate
        /// other properties.
        /// </summary>
        protected virtual void InitializeParameters()
        {
            try
            {
                if (!string.IsNullOrEmpty(Parameters))
                {
                    //Parameters string should be in the format:
                    //  Prop1=value1;Prop2=value2;Prop3=value;...
                    foreach (string paramString in this.Parameters.Split(new char[] { ';' }))
                    {
                        //now we have Prop1=value1
                        string[] keyValue = paramString.Split(new char[] { '=' });
                        if (keyValue == null || keyValue.Length < 2)
                        {
                            continue;
                        }
                        //keyValue[0] = Prop1
                        //keyValue[1] = value1
                        this.ProcessParam(keyValue[0].ToLower(), keyValue[1]);
                    }
                }
            }
            catch (Exception e)
            {
                throw new LoggerException(
                    string.Format("Unable to Initialize parameterss; message={0}",
                                e.Message),
                    e);
            }
        }
        /// <summary>
        /// Method that will process the parameter value. If either <code>name</code> or
        /// <code>value</code> is empty then this parameter will not be processed.
        /// </summary>
        /// <param name="name">name of the paramater</param>
        /// <param name="value">value of the parameter</param>
        protected virtual void ProcessParam(string name, string value)
        {
            try
            {
                if (!string.IsNullOrEmpty(name) &&
                    !string.IsNullOrEmpty(value))
                {
                    switch (name.Trim().ToUpper())
                    {
                        case ("LOGFILE"):
                        case ("L"):
                            this.LogFile = value;
                            break;

                        case ("VERBOSITY"):
                        case ("V"):
                            ProcessVerbosity(value);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                string message = string.Format(
                    "Unable to process parameters;[name={0},value={1}] message={2}",
                    name, value, e.Message);
                throw new LoggerException(message, e);
            }
        }
        /// <summary>
        /// This will set the verbosity level from the parameter
        /// </summary>
        /// <param name="level"></param>
        protected virtual void ProcessVerbosity(string level)
        {
            if (!string.IsNullOrEmpty(level))
            {
                switch (level.Trim().ToUpper())
                {
                    case ("QUIET"):
                    case ("Q"):
                        this.Verbosity = LoggerVerbosity.Quiet;
                        break;

                    case ("MINIMAL"):
                    case ("M"):
                        this.Verbosity = LoggerVerbosity.Minimal;
                        break;

                    case ("NORMAL"):
                    case ("N"):
                        this.Verbosity = LoggerVerbosity.Normal;
                        break;

                    case ("DETAILED"):
                    case ("D"):
                        this.Verbosity = LoggerVerbosity.Detailed;
                        break;

                    case ("DIAGNOSTIC"):
                    case ("DIAG"):
                        this.Verbosity = LoggerVerbosity.Diagnostic;
                        break;

                    default:
                        throw new LoggerException(
                            string.Format("Unable to process the verbosity: {0}",
                            level));
                }
            }
        }
        #endregion
    }
}
namespace Examples.Loggers
{
    using System;
    using System.IO;
    using Microsoft.Build.BuildEngine;
    using Microsoft.Build.Framework;

    public class CustomFileLogger : ConsoleLogger
    {
        #region Properties
        protected string LogFile { get; set; }
        protected bool Append { get; set; }
        protected StreamWriter FileWriter { get; set; }
        #endregion

        public override void Initialize(IEventSource eventSource, int nodeCount)
        {
            //default value
            Append = false;

            ParseCustomParameters();
            base.Initialize(eventSource, nodeCount);

            if (string.IsNullOrEmpty(LogFile))
            {
                //apply the default value
                LogFile = "custom.build.log";
            }

            FileWriter = new StreamWriter(LogFile, Append);
            FileWriter.AutoFlush = true;

            base.WriteHandler = new WriteHandler(HandleWrite);
        }
        public override void Shutdown()
        {
            base.Shutdown();
            if (FileWriter != null)
            {
                FileWriter.Close();
                FileWriter = null;
            }
        }
        private void HandleWrite(string text)
        {
            FileWriter.Write(text);
        }
        public virtual void ParseCustomParameters()
        {
            if (!string.IsNullOrEmpty(Parameters))
            {
                string[] paramPairs = Parameters.Split(';');
                for (int i = 0; i < paramPairs.Length; i++)
                {
                    if (paramPairs[i].Length > 0)
                    {
                        string[] paramPair = paramPairs[i].Split('=');
                        if (!string.IsNullOrEmpty(paramPair[0]))
                        {
                            if (paramPair.Length > 1)
                            {
                                ApplyParam(paramPair[0], paramPair[1]);
                            }
                            else
                            {
                                ApplyParam(paramPair[0], null);
                            }
                        }
                    }
                }
            }
        }
        public virtual void ApplyParam(string paramName, string paramValue)
        {
            if (!string.IsNullOrEmpty(paramName))
            {
                string paramNameUpper = paramName.ToUpperInvariant();
                switch (paramNameUpper)
                {
                    case "LOGFILE":
                    case "L":
                        LogFile = paramValue;
                        break;
                    case "APPEND":
                        if (string.Compare(paramValue, "true", true) == 0)
                        {
                            Append = true;
                        }
                        else
                        {
                            Append = false;
                        }
                        break;
                }
            }
        }
    }
}

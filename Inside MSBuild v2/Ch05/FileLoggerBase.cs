namespace Examples.Loggers
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    /// <summary>
    /// This is the base class to MSBuild file based loggers. This class provides some common
    /// functionality for file based MSBuild loggers.
    /// 
    /// The primary purpose of this class is to read in the parameters and to parse the ones that this class
    /// is aware of.
    /// Parameters that it is aware of are:
    ///  FileName
    ///  Verbosity
    ///  Append
    ///  ShowSummary
    /// This class will set the values for these 4 values if they are specificed at the command line.
    /// If not the default values will be used in its place. You can use the appropriate properites to poll
    /// their values.
    /// This class is NOT responsible for doing anything with these values, only to make them assessible
    /// the the implementing class writer. That is you can determine what the verbosity level was set to
    /// but it is not the responsibility of this class to do anything with that information.
    /// If you want a parameter that is not defined in the above 4 then you simply have to use the
    /// GetParameterValue(string) method to get it's value.
    /// Parameters should be specified like:
    ///  logFile=log.xml;verbosity=d;showsummary=true;append=true
    /// The parameter names and short names are shown below
    ///     logFile     l
    ///     verbosity   v
    ///     showsummary s  
    ///     append      a
    /// These are case-insensitive.
    /// If you create new parameters make sure to not conflict with those defined above.
    /// 
    /// Author: Sayed Ibrahim Hashimi (sayed.hashimi@gmail.com)
    /// This class has not been throughly tested and is offered with no warranty.
    /// copyright Sayed Ibrahim Hashimi 2005
    /// </summary>
    public abstract class FileLoggerBase : Logger
    {
        #region Fields
        /// <summary>
        /// File name that should be written to
        /// </summary>
        private string fileName;
        /// <summary>
        /// Determines wether to append to an existing file or overwrite it.
        /// </summary>
        private bool append;
        /// <summary>
        /// Determines is a summary should be displayed or not.
        /// </summary>
        private bool showSummary;
        /// <summary>
        /// Map that will contain all the parameters.
        /// </summary>
        private IDictionary<string, string> paramaterBag;
        #endregion
        public string LogFile
        {
            get { return this.fileName; }
            set { this.fileName = value; }
        }
        public bool Append
        {
            get { return this.append; }
            set { this.append = value; }
        }
        public bool ShowSummary
        {
            get { return this.showSummary; }
            set { this.showSummary = value; }
        }



        public override void Initialize(IEventSource eventSource)
        {
            try
            {
                InitializeParameters();
            }
            catch (Exception e)
            {
                throw new LoggerException("Unable to initialize the logger", e);
            }
        }
        /// <summary>
        /// This will read in the parameters and process them.
        /// The parameter string should look like: paramName1=val1;paramName2=val2;paramName3=val3
        /// This method will also cause the known parameter properties of this class to be set if they
        /// are present.
        /// </summary>
        protected virtual void InitializeParameters()
        {
            try
            {
                this.paramaterBag = new Dictionary<string, string>();
                if (!string.IsNullOrEmpty(Parameters))
                {
                    foreach (string paramString in this.Parameters.Split(";".ToCharArray()))
                    {
                        string[] keyValue = paramString.Split("=".ToCharArray());
                        if (keyValue == null || keyValue.Length < 2)
                        {
                            continue;
                        }
                        this.ProcessParam(keyValue[0].ToLower(), keyValue[1]);
                    }
                }
            }
            catch (Exception e)
            {
                throw new LoggerException("Unable to initialize parameterss; message=" + e.Message, e);
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
                    //add to param bag so subclasses have easy method to fetch other parameter values
                    AddToParameters(name, value);
                    switch (name.Trim().ToUpper())
                    {
                        case ("LOGFILE"):
                        case ("L"):
                            this.fileName = value;
                            break;

                        case ("VERBOSITY"):
                        case ("V"):
                            ProcessVerbosity(value);
                            break;

                        case ("APPEND"):
                        case ("A"):
                            ProcessAppend(value);
                            break;

                        case ("SHOWSUMMARY"):
                        case ("S"):
                            ProcessShowSummary(value);
                            break;
                    }
                }
            }
            catch (LoggerException /*le*/)
            {
                throw;
            }
            catch (Exception e)
            {
                string message = "Unable to process parameters;[name=" + name + ",value=" + value + " message=" + e.Message;
                throw new LoggerException(message, e);
            }
        }
        /// <summary>
        /// This will set the verbosity level from the parameter
        /// </summary>
        /// <param name="level"></param>
        protected virtual void ProcessVerbosity(string level)
        {
            if (string.IsNullOrEmpty(level))
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
                        throw new LoggerException("Unable to process the verbosity: " + level);
                }
            }
        }
        /// <summary>
        /// This will process the append value based on the parameter.
        /// NOTE: In this implementation a <code>LoggerException</code> is thrown if the string
        /// cause an exception to be raised when <code>bool.Parse()</code> is called on it. If you require a
        /// different behavior simply override this method.
        /// </summary>
        /// <param name="appendStr"></param>
        protected virtual void ProcessAppend(string appendStr)
        {
            try
            {
                Append = bool.Parse(appendStr);
            }
            catch (Exception e)
            {
                throw new LoggerException("Unable to process append parameter [" + appendStr + "]", e);
            }
        }
        /// <summary>
        /// This will process the show summary value based on the parameter.
        /// NOTE: In this implementation a <code>LoggerException</code> is thrown if the string
        /// cause an exception to be raised when <code>bool.Parse()</code> is called on it. If you require a
        /// different behavior simply override this method.
        /// </summary>
        /// <param name="summaryStr"></param>
        protected virtual void ProcessShowSummary(string summaryStr)
        {
            try
            {
                this.showSummary = bool.Parse(summaryStr);
            }
            catch (Exception e)
            {
                throw new LoggerException("Unable to process summary parameter [" + summaryStr + "]", e);
            }
        }
        /// <summary>
        /// Adds the given name & value to the <code>_parameterBag</code>.
        /// If the bag already contains the name as a key, this value will replace the previous value.
        /// </summary>
        /// <param name="name">name of the parameter</param>
        /// <param name="value">value for the paramter</param>
        protected virtual void AddToParameters(string name, string value)
        {
            if (name == null) { throw new ArgumentNullException("name"); }
            if (value == null) { throw new ArgumentException("value"); }

            string paramKey = name.ToUpper();
            try
            {
                if (paramaterBag.ContainsKey(paramKey))
                { paramaterBag.Remove(paramKey); }

                paramaterBag.Add(paramKey, value);
            }
            catch (Exception e)
            {
                throw new LoggerException("Unable to add to parameters bag", e);
            }
        }
        /// <summary>
        /// This can be used to get the values of parameter that this class is not aware of.
        /// If the value is not present then string.Empty is returned.
        /// </summary>
        /// <param name="name">name of the parameter to fetch</param>
        /// <returns></returns>
        protected virtual string GetParameterValue(string name)
        {
            if (name == null) { throw new ArgumentNullException("name"); }

            string paramName = name.ToUpper();

            string value = null;
            if (paramaterBag.ContainsKey(paramName))
            {
                value = paramaterBag[paramName];
            }

            return value;
        }
        /// <summary>
        /// Will return a colleciton of parameters that have been defined.
        /// </summary>
        protected virtual ICollection<string> DefiniedParameters
        {
            get
            {
                ICollection<string> value = null;
                if (paramaterBag != null)
                {
                    value = paramaterBag.Keys;
                }

                return value;
            }
        }
    }
}

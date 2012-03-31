namespace Examples.Loggers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Xml;
    using Microsoft.Build.Framework;

    /// <summary>
    /// This class will be used as an MSBuild logger that will output an XML log file.
    /// 
    /// Author: Sayed Ibrahim Hashimi (sayed.hashimi@gmail.com)
    /// This class has not been throughly tested and is offered with no warranty.
    /// copyright Sayed Ibrahim Hashimi 2005
    /// </summary>
    public class XmlLogger : FileLoggerBase
    {
        #region Fields
        private IList<string> errorList;
        private IList<string> warningList;

        private XmlDocument xmlDoc;
        private XmlElement rootElement;
        private XmlElement errorsElement;
        private XmlElement warningsElement;


        private Stack<XmlElement> buildElements;
        private Stack<XmlElement> projectElements;
        private Stack<XmlElement> targetElements;
        private Stack<XmlElement> taskElements;

        /// <summary>
        /// Used to help determine what the last thing to build was for placement of message/warning/error
        /// </summary>
        private Stack<BuildType> buildTypeList;
        #endregion

        public override void Initialize(IEventSource eventSource)
        {
            errorList = new List<string>();
            warningList = new List<string>();

            buildElements = new Stack<XmlElement>();
            projectElements = new Stack<XmlElement>();
            targetElements = new Stack<XmlElement>();
            taskElements = new Stack<XmlElement>();
            buildTypeList = new Stack<BuildType>();

            //apply default values
            LogFile = "build.log.xml";
            Append = false;
            ShowSummary = false;

            //have base init the parameters
            base.Initialize(eventSource);

            this.InitializeEvents(eventSource);

            this.InitializeXmlDoc();
        }
        /// <summary>
        /// This will regesiter all the events with MSBuild
        /// </summary>
        /// <param name="eventSource"></param>
        protected void InitializeEvents(IEventSource eventSource)
        {
            try
            {
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
            catch (Exception e)
            {
                string message = string.Format(
                    "Unable to initialize events; message={0}",
                    e.Message);
                throw new LoggerException(message, e);
            }
        }
        protected void InitializeXmlDoc()
        {
            this.xmlDoc = new XmlDocument();
            if (Append)
            {
                try
                {
                    xmlDoc.Load(LogFile);
                    rootElement = xmlDoc.DocumentElement;
                }
                catch (Exception e)
                {
                    string message = string.Format("Unable to load the xml document from: {0}", LogFile);
                    throw new LoggerException(message, e);
                }
            }
            else
            {
                rootElement = this.xmlDoc.CreateElement("MSBuild");
            }

            XmlAttribute createAtt = xmlDoc.CreateAttribute("Started");
            createAtt.Value = DateTime.UtcNow.ToString();

            this.xmlDoc.AppendChild(rootElement);
        }
        /// <summary>
        /// This is automagically called by MSBuild at the end of the build
        /// </summary>
        public override void Shutdown()
        {
            try
            {
                //TODO: Add error and warning elements here
                this.xmlDoc.Save(LogFile);
            }
            catch (Exception e)
            {
                string message = "Unable to save the log to [" + LogFile + "]";
                throw new LoggerException(message, e);
            }
        }
        #region Logging handlers
        void BuildStarted(object sender, BuildStartedEventArgs e)
        {
            buildTypeList.Push(BuildType.Build);

            XmlElement buildElement = xmlDoc.CreateElement("Build");

            rootElement.AppendChild(buildElement);
            buildElement.Attributes.Append(
                CreateStartedAttribute(e.Timestamp));
            buildElement.Attributes.Append(
                CreateAttribute("Verbosity", this.Verbosity.ToString()));

            if (this.Parameters != null &&
                base.IsVerbosityAtLeast(LoggerVerbosity.Detailed))
            {
                //log all the parameters that were passed to the logger
                XmlElement paramElement =
                    xmlDoc.CreateElement("LoggerParameters");
                buildElement.AppendChild(paramElement);
                foreach (string current in DefiniedParameters)
                {
                    XmlElement currentElement =
                        xmlDoc.CreateElement("Parameter");
                    currentElement.InnerText =
                        current + "=" + GetParameterValue(current);
                    paramElement.AppendChild(currentElement);
                }
            }

            buildElement.AppendChild(CreateMessageElement(e.Message));

            buildElements.Push(buildElement);
        }
        void BuildFinished(object sender, BuildFinishedEventArgs e)
        {
            XmlElement buildElement = buildElements.Pop();
            buildElement.Attributes.Append(CreateFinishedAttribute(e.Timestamp));
            buildElement.Attributes.Append(CreateAttribute("Succeeded", e.Succeeded.ToString()));

            buildElement.AppendChild(CreateMessageElement(e.Message));

            buildTypeList.Pop();
        }
        void ProjectStarted(object sender, ProjectStartedEventArgs e)
        {
            buildTypeList.Push(BuildType.Project);

            XmlElement projectElement = xmlDoc.CreateElement("Project");
            projectElements.Push(projectElement);

            buildElements.Peek().AppendChild(projectElement);

            projectElement.Attributes.Append(
                CreateAttribute("Name", e.ProjectFile));

            projectElement.Attributes.Append(
                CreateAttribute("Message", e.Message));
            projectElement.Attributes.Append(
                CreateStartedAttribute(e.Timestamp));

            if (base.IsVerbosityAtLeast(LoggerVerbosity.Detailed))
            {
                projectElement.Attributes.Append(
                    CreateAttribute("SenderName", e.SenderName));
            }

            if (base.IsVerbosityAtLeast(LoggerVerbosity.Diagnostic))
            {
                XmlElement propertiesElement =
                    xmlDoc.CreateElement("Properties");
                projectElement.AppendChild(propertiesElement);

                foreach (DictionaryEntry current in e.Properties)
                {
                    if (current.Equals(null) ||
                        current.Key == null ||
                        string.IsNullOrEmpty(current.Key.ToString()) ||
                        current.Value == null ||
                        string.IsNullOrEmpty(current.Value.ToString()))
                    {
                        continue;
                    }
                    XmlElement newElement =
                        xmlDoc.CreateElement(current.Key.ToString());
                    newElement.InnerText = current.Value.ToString();
                    propertiesElement.AppendChild(newElement);
                }
            }
        }
        void ProjectFinished(object sender, ProjectFinishedEventArgs e)
        {
            XmlElement projectElement = projectElements.Pop();

            projectElement.Attributes.Append(CreateFinishedAttribute(e.Timestamp));

            buildTypeList.Pop();
        }
        void TargetStarted(object sender, TargetStartedEventArgs e)
        {
            buildTypeList.Push(BuildType.Target);

            XmlElement targetElement = xmlDoc.CreateElement("Target");
            targetElements.Push(targetElement);
            projectElements.Peek().AppendChild(targetElement);

            targetElement.Attributes.Append(CreateStartedAttribute(e.Timestamp));
            targetElement.Attributes.Append(CreateAttribute("Name", e.TargetName));

            targetElement.Attributes.Append(CreateAttribute("Message", e.Message));

            if (base.IsVerbosityAtLeast(LoggerVerbosity.Detailed))
            {
                targetElement.Attributes.Append(CreateAttribute("TargetFile", e.TargetFile));
                targetElement.Attributes.Append(CreateAttribute("ProjectFile", e.ProjectFile));
            }

        }
        void TargetFinished(object sender, TargetFinishedEventArgs e)
        {
            XmlElement targetElement = targetElements.Pop();
            targetElements.Push(targetElement);
            targetElement.Attributes.Append(CreateFinishedAttribute(e.Timestamp));

            targetElement.Attributes.Append(CreateAttribute("Succeeded", e.Succeeded.ToString()));

            if (base.IsVerbosityAtLeast(LoggerVerbosity.Detailed))
            {
                targetElement.Attributes.Append(CreateAttribute("FinishMessage", e.Message));
            }

            buildTypeList.Pop();
        }
        void TaskStarted(object sender, TaskStartedEventArgs e)
        {
            buildTypeList.Push(BuildType.Task);

            XmlElement taskElemet = xmlDoc.CreateElement("Task");
            taskElements.Push(taskElemet);
            targetElements.Peek().AppendChild(taskElemet);

            taskElemet.Attributes.Append(CreateStartedAttribute(e.Timestamp));

            taskElemet.Attributes.Append(CreateAttribute("Name", e.TaskName));
        }
        void TaskFinished(object sender, TaskFinishedEventArgs e)
        {
            XmlElement taskElement = taskElements.Pop();
            taskElement.Attributes.Append(CreateFinishedAttribute(e.Timestamp));

            if (base.IsVerbosityAtLeast(LoggerVerbosity.Detailed))
            {
                taskElement.Attributes.Append(CreateAttribute("FinishMessage", e.Message));
                taskElement.Attributes.Append(CreateAttribute("ProjectFile", e.ProjectFile));
                taskElement.Attributes.Append(CreateAttribute("TaskFile", e.TaskFile));
            }
            buildTypeList.Pop();
        }
        void BuildError(object sender, BuildErrorEventArgs e)
        {
            XmlElement errorElement = xmlDoc.CreateElement("Error");

            GetCurrentElement().AppendChild(errorElement);

            if (ShowSummary)
            {
                if (errorsElement == null)
                {
                    errorsElement = xmlDoc.CreateElement("Errors");
                    buildElements.Peek().AppendChild(errorsElement);
                }
                errorsElement.AppendChild(errorElement);
            }
            errorElement.AppendChild(CreateMessageElement(FormatErrorEvent(e)));

            errorElement.Attributes.Append(CreateAttribute("File", e.File));
            errorElement.Attributes.Append(CreateAttribute("Code", e.Code));
            errorElement.Attributes.Append(CreateAttribute("Subcategory", e.Subcategory));
            if (e.HelpKeyword != null && !e.HelpKeyword.Trim().Equals(string.Empty))
            {
                errorElement.Attributes.Append(CreateAttribute("Hint", e.HelpKeyword));
            }

            XmlElement locElement = xmlDoc.CreateElement("Location");
            errorElement.AppendChild(locElement);
            locElement.Attributes.Append(CreateAttribute("Line", e.LineNumber.ToString()));
            if (e.LineNumber != e.EndLineNumber && e.EndLineNumber > 0)
            {
                locElement.Attributes.Append(CreateAttribute("EndLine", e.EndLineNumber.ToString()));
            }

            locElement.Attributes.Append(CreateAttribute("ColumnNumber", e.ColumnNumber.ToString()));
            if (e.ColumnNumber != e.EndColumnNumber && e.EndColumnNumber > 0)
            {
                locElement.Attributes.Append(CreateAttribute("EndColumnNumber", e.EndColumnNumber.ToString()));
            }
        }
        void BuildWarning(object sender, BuildWarningEventArgs e)
        {
            //first see if we are interested in logging this
            //if (!base.IsVerbosityAtLeast(LoggerVerbosity.Normal))
            //{
            //    return;
            //}

            XmlElement warningElement = xmlDoc.CreateElement("Warning");
            //figure out where to place this element

            GetCurrentElement().AppendChild(warningElement);

            //if (ShowSummary)
            {
                if (warningsElement == null)
                {
                    warningsElement = xmlDoc.CreateElement("Warnings");
                    buildElements.Peek().AppendChild(warningsElement);
                }
                warningsElement.AppendChild(warningElement);
            }

            warningElement.AppendChild(CreateMessageElement(FormatWarningEvent(e)));

            warningElement.Attributes.Append(CreateAttribute("Code", e.Code));
            warningElement.Attributes.Append(CreateAttribute("Subcategory", e.Subcategory));

            if (e.HelpKeyword != null && !e.HelpKeyword.Trim().Equals(string.Empty))
            {
                warningElement.Attributes.Append(CreateAttribute("Hint", e.HelpKeyword));
            }

            if (base.IsVerbosityAtLeast(LoggerVerbosity.Detailed))
            {
                XmlElement locElement = xmlDoc.CreateElement("Location");
                warningElement.AppendChild(locElement);
                locElement.Attributes.Append(CreateAttribute("Line", e.LineNumber.ToString()));
                if (e.LineNumber != e.EndLineNumber && e.EndLineNumber > 0)
                {
                    locElement.Attributes.Append(CreateAttribute("EndLine", e.EndLineNumber.ToString()));
                }

                locElement.Attributes.Append(CreateAttribute("ColumnNumber", e.ColumnNumber.ToString()));
                if (e.ColumnNumber != e.EndColumnNumber && e.EndColumnNumber > 0)
                {
                    locElement.Attributes.Append(CreateAttribute("EndColumnNumber", e.EndColumnNumber.ToString()));
                }
            }
        }
        void BuildMessage(object sender, BuildMessageEventArgs e)
        {
            //first figure out if we are interested in actually logging this
            switch (e.Importance)
            {
                case (MessageImportance.High):
                    break;
                case (MessageImportance.Normal):
                    if (!base.IsVerbosityAtLeast(LoggerVerbosity.Normal))
                        return;
                    break;
                case (MessageImportance.Low):
                    if (!base.IsVerbosityAtLeast(LoggerVerbosity.Detailed))
                        return;
                    break;
            }

            XmlElement messageElement = xmlDoc.CreateElement("Message");
            GetCurrentElement().AppendChild(messageElement);

            messageElement.InnerText = e.Message;

            messageElement.Attributes.Append(CreateAttribute("Importance", e.Importance.ToString()));
            if (e.HelpKeyword != null && !e.HelpKeyword.Trim().Equals(string.Empty))
            {
                messageElement.Attributes.Append(CreateAttribute("MessageKeyword", e.HelpKeyword));
            }

            if (base.IsVerbosityAtLeast(LoggerVerbosity.Detailed))
            {
                messageElement.Attributes.Append(CreateAttribute("SenderName", e.SenderName));
            }
        }
        void CustomEvent(object sender, CustomBuildEventArgs e)
        {
            //Do nothing, really a place holder for sub-classes
        }
        #endregion

        #region Convienence methods
        protected XmlElement GetCurrentElement()
        {
            try
            {
                //when you override targets a message is created before any other events get fired
                if (buildTypeList.Count <= 0)
                {
                    return this.rootElement;
                }


                switch (buildTypeList.Peek())
                {
                    case (BuildType.Build):
                        return buildElements.Peek();

                    case (BuildType.Project):
                        return projectElements.Peek();

                    case (BuildType.Target):
                        return targetElements.Peek();

                    case (BuildType.Task):
                        return taskElements.Peek();

                    default:
                        return rootElement;    //should never get here
                }
            }
            catch (Exception e)
            {
                this.xmlDoc.Save(LogFile);
                throw new LoggerException("Unable to get the current element", e);
            }
        }
        protected XmlAttribute CreateAttribute(string name, string value)
        {
            try
            {
                XmlAttribute att = xmlDoc.CreateAttribute(name);
                att.Value = value;
                return att;
            }
            catch (Exception /*e*/)
            {
                string message = "Unable to create attribute; name=" + name + ", value=" + value;
                throw new LoggerException(message);
            }
        }
        protected XmlAttribute CreateFinishedAttribute(DateTime time)
        {
            try
            {
                XmlAttribute att = xmlDoc.CreateAttribute("Finished");
                att.Value = time.ToString();
                return att;
            }
            catch (Exception /*e*/)
            {
                throw new LoggerException("Unable to create finished attribute");
            }
        }
        protected XmlAttribute CreateStartedAttribute(DateTime time)
        {
            try
            {
                XmlAttribute att = xmlDoc.CreateAttribute("Started");
                att.Value = time.ToString();
                return att;
            }
            catch (Exception /*e*/)
            {
                throw new LoggerException("Unable to create started att");
            }
        }
        protected XmlElement CreateMessageElement(string message)
        {
            try
            {
                XmlElement messageElement = xmlDoc.CreateElement("Message");
                messageElement.InnerText = message;
                return messageElement;
            }
            catch (Exception /*e*/)
            {
                throw new LoggerException("Unable to create message element");
            }
        }
        #endregion
    }

    enum BuildType
    {
        Build = 4,
        Project = 3,
        Target = 2,
        Task = 1
    }
}

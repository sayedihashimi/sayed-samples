namespace Examples.Tasks.TaskFactories
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Microsoft.Build.Framework;

public class BatchFileTaskFactory : ITaskFactory
{
    protected string TaskXmlBody
    { get; set; }

    protected IDictionary<string, TaskPropertyInfo> ParameterGroup
    { get; private set; }

    public virtual void CleanupTask(ITask task)
    {
        Contract.Requires(task != null);
        // If  the task is disposable then dispose it
        IDisposable disposableTask = task as IDisposable;
        if (disposableTask != null)
        {
            disposableTask.Dispose();
        }
    }

    public virtual ITask CreateTask(IBuildEngine taskFactoryLoggingHost)
    { return new BatchFileTask(this.TaskXmlBody); }

    public string FactoryName
    {
        get { return this.GetType().Name; }
    }

    public virtual TaskPropertyInfo[] GetTaskParameters()
    { return this.ParameterGroup.Values.ToArray(); }

    public virtual bool Initialize(
        string taskName, 
        IDictionary<string, TaskPropertyInfo> parameterGroup, 
        string taskBody, 
        IBuildEngine taskFactoryLoggingHost)
    {
        Contract.Requires(!string.IsNullOrEmpty(taskName));
        Contract.Requires(parameterGroup != null);
        Contract.Requires(taskBody != null);
        Contract.Requires(taskFactoryLoggingHost != null);

        this.TaskXmlBody = taskBody;
        this.ParameterGroup = parameterGroup;

        return true;
    }

    public Type TaskType
    {
        get { return typeof(BatchFileTask); }
    }
}
}
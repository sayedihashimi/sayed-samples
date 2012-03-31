namespace Examples.Tasks
{
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

public class HelloWorld : ITask
{
    public IBuildEngine BuildEngine
    { get; set; }

    public ITaskHost HostObject
    { get; set; }

    public bool Execute()
    {
        //set up support for logging
        TaskLoggingHelper loggingHelper = new TaskLoggingHelper(this);
        loggingHelper.LogMessageFromText(
            "Hello MSBuild", MessageImportance.High);

        return true;
    }
}
}

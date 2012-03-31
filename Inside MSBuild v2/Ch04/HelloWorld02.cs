namespace Examples.Tasks
{
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    public class HelloWorld02 : Task
    {
        public override bool Execute()
        {
            Log.LogMessageFromText
                ("Hello MSBuild from Task!", MessageImportance.High);
            return true;
        }
    }
}
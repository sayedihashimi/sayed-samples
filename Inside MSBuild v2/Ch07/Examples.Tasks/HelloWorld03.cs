namespace Examples.Tasks
{
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    public class HelloWorld03 : Task
    {
        #region Properties
        /// <summary>
        /// First name, this is required.
        /// </summary>
        [Required]
        public string FirstName
        { get; set; }
        /// <summary>
        /// Last name, this is optional
        /// </summary>
        public string LastName
        { get; set; }
        #endregion

        public override bool Execute()
        {
            Log.LogMessage(string.Format("Hello {0} {1}", FirstName, LastName));

            return true;
        }
    }
}

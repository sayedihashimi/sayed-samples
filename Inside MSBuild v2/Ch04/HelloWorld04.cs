namespace Examples.Tasks
{
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    public class HelloWorld04 : Task
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

        /// <summary>
        /// A message from the task,
        /// this is an output property.
        /// </summary>
        [Output]
        public string Message
        { get; set; }
        #endregion

        public override bool Execute()
        {
            Message = string.Format("Fullname: {0} {1}", FirstName, LastName);
            Log.LogMessage(string.Format("Hello {0} {1}", FirstName, LastName));

            return true;
        }
    }
}
namespace Examples.Tasks
{
    using System;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    public class GetDate : Task
    {
        #region Properties
        /// <summary>
        /// Optional input that specifies the format for the date that will be placed in
        /// the <code>FormattedDate</code> output.
        /// </summary>
        public string Format
        { get; set; }

        [Output]
        public string Date
        { get; private set; }
        #endregion

        public override bool Execute()
        {
            DateTime now = DateTime.Now;
            Date = now.ToString(Format, null);
            return true;
        }
    }
}

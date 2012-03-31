using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Common;
using Common.IO;
using Common.UI;

namespace SNotepad
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            string str = Common1.Class1Str +
                            CommonIO1.CommionIO1Str +
                            CommonUI1.CommionUI1Str;
            System.Diagnostics.Debug.WriteLine(str);
        }
    }
}

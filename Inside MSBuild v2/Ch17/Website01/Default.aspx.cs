using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string logsDir = Server.MapPath("logs");
        if (!File.Exists(Path.Combine(logsDir, "log.txt")))
        {
            File.WriteAllText(Path.Combine(logsDir, "log.txt"), "log file contents");
        }
    }
}
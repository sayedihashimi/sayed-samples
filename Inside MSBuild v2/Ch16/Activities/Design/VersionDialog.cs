using System;
using System.Windows.Forms;

namespace Activities.Design
{
    public partial class VersionDialog : Form
    {
        public VersionDialog()
            : this(new VersionWrapper() { Version = new Version(1, 0) })
        {
        }

        public VersionDialog(VersionWrapper version)
        {
            InitializeComponent();

            major.Text = version.Version.Major.ToString();
            minor.Text = version.Version.Minor.ToString();
            build.Text = version.Version.Build.ToString();
            revision.Text = version.Version.Revision.ToString();
        }

        public VersionWrapper Version
        {
            get
            {
                return new VersionWrapper()
                {
                    Version = new Version(
                        int.Parse(major.Text),
                        int.Parse(minor.Text),
                        int.Parse(build.Text),
                        int.Parse(revision.Text)
                        )
                };
            }
        }
    }
}

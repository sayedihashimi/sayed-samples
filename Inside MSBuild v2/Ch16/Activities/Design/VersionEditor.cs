using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Activities.Design;

namespace Activities
{
    public class VersionEditor : UITypeEditor
    {
        public override object EditValue(ITypeDescriptorContext context,
            IServiceProvider provider,
            object value)
        {

            var editorService = (IWindowsFormsEditorService)provider.GetService(
                typeof(IWindowsFormsEditorService)
            );

            if (editorService != null)
            {
                var versionDialog = new VersionDialog((VersionWrapper)value);
                if (editorService.ShowDialog(versionDialog) == DialogResult.OK)
                {
                    value = versionDialog.Version;
                }
            }

            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }
}

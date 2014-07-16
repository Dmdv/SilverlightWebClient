using System;
using WebClient.ICS.Client.Commands;

namespace WebClient.ICS.Client.Editors
{
    public partial class DateTimeEditor : IActiveAware
    {
        public DateTimeEditor()
        {
            InitializeComponent();
        }

        public bool IsActive
        {
            get { return DateTimeEditorControl.IsActive; }
            set
            {
                DateTimeEditorControl.IsActive = value;
                InvokeIsActiveChanged(null);
            }
        }

        public event EventHandler IsActiveChanged;

        public void InvokeIsActiveChanged(EventArgs e)
        {
            EventHandler handler = IsActiveChanged;
            if (handler != null) handler(this, e);
        }
    }
}

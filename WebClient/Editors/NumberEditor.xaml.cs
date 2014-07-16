using System;
using System.Windows.Controls;
using System.Windows.Data;
using WebClient.ICS.Client.Controls;
using WebClient.ICS.Client.Commands;

namespace WebClient.ICS.Client.Editors
{
    /// <summary>
    /// Number Editor
    /// </summary>
    public partial class NumberEditor : IActiveAware
    {
        private readonly Binding _binding;
        private bool _isActive;

        public event EventHandler IsActiveChanged;

        public NumberEditor()
        {
            InitializeComponent();

            var bindingExpression = NumberEditorControl.GetBindingExpression(TextBox.TextProperty);
            if (bindingExpression != null)
            {
                _binding = bindingExpression.ParentBinding;
            }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                if (_isActive == value) return;

                _isActive = value;

                if (_isActive)
                {
                    NumberEditorControl.SetBinding(TextBox.TextProperty, _binding);
                }
                else
                {
                    NumberEditorControl.ResetBinding(TextBox.TextProperty);
                }

                if (IsActiveChanged != null)
                {
                    IsActiveChanged(this, null);
                }
            }
        }
    }
}
using System;
using System.Windows.Controls;
using System.Windows.Data;
using WebClient.ICS.Client.Controls;
using WebClient.ICS.Client.Commands;

namespace WebClient.ICS.Client.Editors
{
    /// <summary>
    /// Text Editor
    /// </summary>
    public partial class TextEditor : IActiveAware
    {
        private readonly Binding _binding;
        private bool _isActive;

        public event EventHandler IsActiveChanged;

        public TextEditor()
        {
            InitializeComponent();
            
            var bindingExpression = TextEditorControl.GetBindingExpression(TextBox.TextProperty);
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
                    TextEditorControl.SetBinding(TextBox.TextProperty, _binding);
                }
                else
                {
                    TextEditorControl.ResetBinding(TextBox.TextProperty);
                }

                if (IsActiveChanged != null)
                {
                    IsActiveChanged(this, null);
                }
            }
        }
    }
}

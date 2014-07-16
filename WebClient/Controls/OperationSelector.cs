using System.Windows;
using System.Windows.Controls;

namespace WebClient.ICS.Client.Controls
{
    /// <summary>
    /// OperationSelector.
    /// </summary>
    public class OperationSelector : ComboBox
    {
        /// <summary>
        /// The <see cref="Text" /> dependency property's name.
        /// </summary>
        private const string TextPropertyName = "Text";

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
            TextPropertyName, 
            typeof (string), 
            typeof (OperationSelector),
            new PropertyMetadata(null, (s, e) => ((OperationSelector) s).RefreshContent()));

        private ContentPresenter _selectedContent;

        public OperationSelector()
        {
            DefaultStyleKey = typeof (ComboBox);
        }

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// Строит визуальное дерево для объекта <see cref="T:System.Windows.Controls.ComboBox"/>
        /// при применении нового шаблона.
        /// </summary>
        public override void OnApplyTemplate()
        {
            _selectedContent = GetTemplateChild("ContentPresenter") as ContentPresenter;
            RefreshContent();
            base.OnApplyTemplate();
            SelectionChanged += (s, e) =>
                                    {
                                        // Cancel selection
                                        SelectedItem = null;
                                        RefreshContent();
                                    };
        }

        private void RefreshContent()
        {
            if (_selectedContent == null) return;

            var tb = (TextBlock) _selectedContent.Content;
            tb.Text = Text;
        }
    }
}
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using WebClient.ICS.Client.Model;

namespace WebClient.ICS.Client.Views
{
    /// <summary>
    /// Setting history.
    /// </summary>
    public partial class HistoryTab
    {
        /// <summary>
        /// Initializes a new instance of the HistoryTab class.
        /// </summary>
        public HistoryTab()
        {
            InitializeComponent();
        }

        private void ShortInfoListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;
            var listBox = sender.Cast<ListBox>();
            var bindingExpression = listBox.GetBindingExpression(Selector.SelectedItemProperty);
            if (bindingExpression != null) bindingExpression.UpdateSource();
        }
    }
}
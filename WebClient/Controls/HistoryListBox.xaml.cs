using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WebClient.ICS.Client.Controls
{
    /// <summary>
    /// История элемента.
    /// </summary>
    public class HistoryListBox : ListBox
    {
        /// <summary>
        /// Имя свойства в HistoryInfo.
        /// </summary>
        private const string SuccessPropertyName = "Success";

        public HistoryListBox()
        {
            SelectionMode = SelectionMode.Single;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            var item = new HistoryListItem();
            SetSuccessBinding(item);
            return item;
        }

        private static void SetSuccessBinding(FrameworkElement item)
        {
            var expandedBinding = new Binding(SuccessPropertyName) {Mode = BindingMode.TwoWay};
            item.SetBinding(HistoryListItem.IsSuccessfullProperty, expandedBinding);
        }
    }
}
using System;
using System.Windows;
using System.Windows.Controls;

namespace WebClient.ICS.Client.Controls
{
    /// <summary>
    /// LazyTreeView.
    /// </summary>
    public class LazyTreeView : TreeView
    {
        public event EventHandler ItemExpanded;

        public event EventHandler ItemClicked;

        protected override DependencyObject GetContainerForItemOverride()
        {
            var item = new LazyTreeViewItem();

            // Expanded
            item.ItemExpanded += (s, e) => RaiseEvent(ItemExpanded, s);

            // Clicked
            item.ItemClicked += (s, e) => RaiseEvent(ItemClicked, s);

            return item;
        }

        private static void RaiseEvent(EventHandler handler, object sender)
        {
            if (handler != null) handler(sender, EventArgs.Empty);
        }
    }
}
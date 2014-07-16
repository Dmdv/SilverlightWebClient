using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace WebClient.ICS.Client.Controls
{
    /// <summary>
    /// Custom ItemsControl with Header.
    /// </summary>
    public class HeaderItemsControl : ItemsControl
    {
        public static readonly DependencyProperty HeadersProperty =
            DependencyProperty.Register("Headers", 
            typeof (ObservableCollection<object>),
            typeof (HeaderItemsControl),
            null);

        public HeaderItemsControl()
        {
            Headers = new ObservableCollection<object>();
        }

        public ObservableCollection<object> Headers
        {
            get { return (ObservableCollection<object>) GetValue(HeadersProperty); }
            private set { SetValue(HeadersProperty, value); }
        }
    }
}
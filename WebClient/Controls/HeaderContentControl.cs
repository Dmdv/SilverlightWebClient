using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace WebClient.ICS.Client.Controls
{
    /// <summary>
    /// Custom ContentControl with Header.
    /// </summary>
    public class HeaderContentControl : ContentControl
    {
        // ReSharper disable MemberCanBePrivate.Global
        public static readonly DependencyProperty HeadersProperty =
            DependencyProperty.Register(
                "Headers",
                typeof (ObservableCollection<object>),
                typeof(HeaderContentControl),
                null);

        public HeaderContentControl()
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
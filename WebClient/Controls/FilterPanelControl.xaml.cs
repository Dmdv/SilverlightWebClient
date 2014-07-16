using System.Threading;

namespace WebClient.ICS.Client.Controls
{
    /// <summary>
    /// Панель для фильтрации истории.
    /// </summary>
    public partial class FilterPanelControl
    {
        public FilterPanelControl()
        {
            InitializeComponent();

            var format = "  (" + Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern + ")";

            ToLabel.Text += format;
            FromLabel.Text += format;
        }
    }
}
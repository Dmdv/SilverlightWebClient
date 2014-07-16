using System;
using System.Windows;
using WebClient.ICS.Client.Model;

namespace WebClient.ICS.Client.Controls
{
    /// <summary>
    /// Панель для вывода информации об ошибке.
    /// </summary>
    public partial class PopupPanel
    {
        /// <summary>
        /// The <see cref="Message" /> dependency property's name.
        /// </summary>
        private const string MessagePropertyName = "Message";

        /// <summary>
        /// The <see cref="ShowButton" /> dependency property's name.
        /// </summary>
        public const string ShowButtonPropertyName = "ShowButton";

        /// <summary>
        /// Identifies the <see cref="Message" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(
            MessagePropertyName,
            typeof (string),
            typeof (PopupPanel),
            new PropertyMetadata(OnMessagChanged));

        /// <summary>
        /// Identifies the <see cref="ShowButton" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowButtonProperty = DependencyProperty.Register(
            ShowButtonPropertyName,
            typeof (bool),
            typeof (PopupPanel),
            null);

        public PopupPanel()
        {
            InitializeComponent();
            Text = string.Empty;
        }

        /// <summary>
        /// Gets or sets the value of the <see cref="ShowButton" />
        /// property. This is a dependency property.
        /// </summary>
        public bool ShowButton
        {
            get { return (bool) GetValue(ShowButtonProperty); }
            set
            {
                SetValue(ShowButtonProperty, value);
                CloseButton.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Gets or sets the value of the <see cref="Message" />
        /// </summary>
        public string Message
        {
            get { return (string) GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        private string Text { get; set; }

        private static void OnMessagChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var popupPanel = d.Cast<PopupPanel>();
            var value = e.NewValue.TryCast<string>();

            if (popupPanel.Text == value) return;
            popupPanel.Text = value;
            
            if (string.IsNullOrEmpty(value))
            {
                HidePanel(popupPanel);
            }
            else
            {
                ShowPanel(popupPanel);
            }
        }

        private static void ShowPanel(PopupPanel popupPanel)
        {
            popupPanel.FadeOut.SkipToFill();
            popupPanel.FadeOut.Stop();

            popupPanel.MessageBox.Text = popupPanel.Text;
            popupPanel.FadeIn.Completed += popupPanel.FadeInCompleted;
            popupPanel.FadeIn.Begin();
        }

        private static void HidePanel(PopupPanel popupPanel)
        {
            popupPanel.FadeIn.SkipToFill();
            popupPanel.FadeIn.Stop();

            popupPanel.FadeOut.Completed += popupPanel.FadeOutCompleted;
            popupPanel.FadeOut.Begin();
        }

        private void FadeOutCompleted(object sender, EventArgs e)
        {
            MessageBox.Text = string.Empty;
            FadeOut.Completed -= FadeOutCompleted;
        }

        private void FadeInCompleted(object sender, EventArgs e)
        {
            MessageBox.Text = Text;
            FadeIn.Completed -= FadeInCompleted;
        }
    }
}
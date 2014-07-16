using System.Windows;
using System.Windows.Controls;
using WebClient.ICS.Client.Model;

namespace WebClient.ICS.Client.Controls
{
    /// <summary>
    /// Кнопка с поддержкой состояния: нажата,  не нажата.
    /// </summary>
    public class ActiveStateButton : Button
    {
        private const string IsActivePropertyName = "IsActive";

        /// <summary>
        /// Identifies the <see cref="IsActive" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register(
            IsActivePropertyName, typeof (bool), typeof (ActiveStateButton),
            new PropertyMetadata(false, OnIsActiveChanged));

        public ActiveStateButton()
        {
            DefaultStyleKey = typeof (ActiveStateButton);
            Loaded += (o, args) => Deactivate(false);
            IsTabStop = true;
        }

        /// <summary>
        /// Gets or sets the value of the <see cref="IsActive" />
        /// </summary>
        public bool IsActive
        {
            get { return (bool) GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        private static void OnIsActiveChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue.Cast<bool>())
            {
                d.Cast<ActiveStateButton>().Activate(true);
            }
            else
            {
                d.Cast<ActiveStateButton>().Deactivate(true);
            }
        }

        private void Activate(bool usetransition)
        {
            VisualStateManager.GoToState(this, "Selected", usetransition);
        }

        private void Deactivate(bool usetransition)
        {
            VisualStateManager.GoToState(this, "Unselected", usetransition);
        }
    }
}
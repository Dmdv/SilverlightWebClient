using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WebClient.ICS.Client.Model;

namespace WebClient.ICS.Client.Controls
{
    /// <summary>
    /// Элемент Short History.
    /// </summary>
    [TemplateVisualState(Name = "FailNormal", GroupName = "FailState")]
    [TemplateVisualState(Name = "FailMouseOver", GroupName = "FailState")]
    [TemplateVisualState(Name = "FailSelected", GroupName = "FailState")]
    [TemplateVisualState(Name = "SuccessNormal", GroupName = "SuccessState")]
    [TemplateVisualState(Name = "SuccessMouseOver", GroupName = "SuccessState")]
    [TemplateVisualState(Name = "SuccessSelected", GroupName = "SuccessState")]
    public class HistoryListItem : ListBoxItem
    {
        private const string IsSuccessfullPropertyName = "IsSuccessfull";

        private bool _isFocused;

        private bool _isMouseOver;

        /// <summary>
        /// Identifies the <see cref="IsSuccessfull" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsSuccessfullProperty = DependencyProperty.Register(
            IsSuccessfullPropertyName, typeof (bool), typeof (HistoryListItem),
            new PropertyMetadata(OnSuccessfullChanged));

        public HistoryListItem()
        {
            DefaultStyleKey = typeof (HistoryListItem);
            Loaded += (o, args) => ChangeVisualStateCustom(false);
        }

        /// <summary>
        /// Gets or sets the value of the <see cref="IsSuccessfull" />
        /// </summary>
        public bool IsSuccessfull
        {
            get { return (bool) GetValue(IsSuccessfullProperty); }
            set { SetValue(IsSuccessfullProperty, value); }
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            _isFocused = true;
            ChangeVisualStateCustom(true);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            _isFocused = false;
            ChangeVisualStateCustom(true);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            _isMouseOver = true;
            ChangeVisualStateCustom(true);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            _isMouseOver = false;
            ChangeVisualStateCustom(true);
        }

        /// <summary>
        /// Требует определенных состояний: FailNormal, FailMouseOver, FailSelected, 
        /// FailUnselected, FailFocused, FailUnfocused
        /// </summary>
        private void ChangeVisualStateCustom(bool useTransitions)
        {
            if (IsSuccessfull)
            {
                ChangeVisualStateDefault(useTransitions);
                return;
            }

            ChangeVisualStateFailed(useTransitions);
        }

        private void ChangeVisualStateFailed(bool useTransitions)
        {
            if (!IsEnabled)
            {
                VisualStateManager.GoToState(this, (Content is Control) ? "FailNormal" : "Disabled", useTransitions);
            }
            else if (_isMouseOver)
            {
                VisualStateManager.GoToState(this, "FailMouseOver", useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, "FailNormal", useTransitions);
            }

            //if (IsSelected)
            //{
            //    VisualStateManager.GoToState(this, "FailSelected", useTransitions);
            //}

            if (_isFocused)
            {
                VisualStateManager.GoToState(this, "FailSelected", useTransitions);
            }
        }

        private void ChangeVisualStateDefault(bool useTransitions)
        {
            if (!IsEnabled)
            {
                VisualStateManager.GoToState(this, (Content is Control) ? "SuccessNormal" : "Disabled", useTransitions);
            }
            else if (_isMouseOver)
            {
                VisualStateManager.GoToState(this, "SuccessMouseOver", useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, "SuccessNormal", useTransitions);
            }

            //if (IsSelected && !_isFocused)
            //{
            //    Debug.WriteLine(">>> (State = Selected)");
            //    VisualStateManager.GoToState(this, "Selected", useTransitions);
            //}
            //else if (!IsSelected && !_isFocused)
            //{
            //    Debug.WriteLine(">>> (State = Unselected)");
            //    VisualStateManager.GoToState(this, "Unselected", useTransitions);
            //}

            if (_isFocused)
            {
                VisualStateManager.GoToState(this, "SuccessSelected", useTransitions);
            }
        }

        private static void OnSuccessfullChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var historyListItem = d.Cast<HistoryListItem>();
            historyListItem.ChangeVisualStateCustom(false);
        }
    }
}
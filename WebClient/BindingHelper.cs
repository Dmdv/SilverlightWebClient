using System.Windows;
using System.Windows.Controls;

namespace KL.ICS.Client.Behaviours
{
    /// <summary>
    /// UpdateSourceTrigger.
    /// </summary>
    public static class BindingHelper
    {
        // ReSharper disable MemberCanBePrivate.Global

        public static readonly DependencyProperty UpdateSourceOnChangeProperty = DependencyProperty.RegisterAttached(
            "UpdateSourceOnChange",
            typeof (bool),
            typeof (BindingHelper),
            new PropertyMetadata(false, OnPropertyChanged));

        public static bool GetUpdateSourceOnChange(this DependencyObject obj)
        {
            return (bool) obj.GetValue(UpdateSourceOnChangeProperty);
        }

        public static void SetUpdateSourceOnChange(this DependencyObject obj, bool value)
        {
            obj.SetValue(UpdateSourceOnChangeProperty, value);
        }

        private static void OnPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var txt = obj as TextBox;
            if (txt == null) return;

            if ((bool) e.NewValue)
            {
                txt.TextChanged += OnTextChanged;
            }
            else
            {
                txt.TextChanged -= OnTextChanged;
            }
        }

        private static void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var txt = sender as TextBox;
            if (txt == null) return;

            var be = txt.GetBindingExpression(TextBox.TextProperty);

            if (be != null)
            {
                //be.UpdateSource();
            }
        }
    }
}
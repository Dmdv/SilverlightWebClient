using System.Windows;

namespace WebClient.ICS.Client.Controls
{
    /// <summary>
    /// UIElementExtensions
    /// </summary>
    public static class UIElementExtensions
    {
        /// <summary>
        /// Clears binding.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="property"></param>
        public static void ResetBinding (this UIElement element, DependencyProperty property)
        {
            element.SetValue(property, DependencyProperty.UnsetValue);
        }
    }
}

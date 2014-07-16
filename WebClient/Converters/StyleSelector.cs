using System.Windows;

namespace WebClient.ICS.Client.Converters
{
    /// <summary>
    /// StyleSelector.
    /// </summary>
    public class StyleSelector
    {
        /// <summary>
        /// SelectStyle.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public virtual Style SelectStyle(object item, DependencyObject container)
        {
            return null;
        }
    }
}
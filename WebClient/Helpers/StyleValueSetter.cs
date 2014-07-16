using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using WebClient.ICS.Client.Contracts;
using WebClient.ICS.Client.Model;

namespace WebClient.ICS.Client.Helpers
{
    // Silverlight bug for more than a year since posted still not resolved:
    // One cannot bind property in Style.
    // http://bea.stollnitz.com/blog/?p=55
    // http://blogs.msdn.com/b/delay/archive/2009/05/07/one-more-platform-difference-more-or-less-tamed-settervaluebindinghelper-makes-silverlight-setters-better.aspx

    /// <summary>
    /// Class that implements a workaround for a Silverlight XAML parser
    /// limitation that prevents the following syntax from working:
    ///    &lt;Setter Property="IsSelected" Value="{Binding IsSelected}"/&gt;
    /// </summary>
    [ContentProperty("Values")]
    public class StyleValueSetter
    {
        private Collection<StyleValueSetter> _values;

        /// <summary>
        /// PropertyBinding attached DependencyProperty.
        /// </summary>
        public static readonly DependencyProperty PropertyBindingProperty =
            DependencyProperty.RegisterAttached("PropertyBinding",
                                                typeof (StyleValueSetter), typeof (StyleValueSetter),
                                                new PropertyMetadata(null, OnPropertyBindingPropertyChanged));

        /// <summary>
        /// Optional type parameter used to specify the type of an attached
        /// DependencyProperty as an assembly-qualified name, full name, or
        /// short name.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Property name for the normal/attached DependencyProperty on which
        /// to set the Binding.
        /// </summary>
        public string Property { get; set; }

        /// <summary>
        /// Binding to set on the specified property.
        /// </summary>
        public Binding Binding { get; set; }

        /// <summary>
        /// Collection of StyleValueSetter instances to apply to the
        /// target element.
        /// </summary>
        /// <remarks>
        /// Used when multiple Bindings need to be applied to the same element.
        /// </remarks>
        public Collection<StyleValueSetter> Values
        {
            get
            {
                // Defer creating collection until needed
                return _values ?? (_values = new Collection<StyleValueSetter>());
            }
        }

        /// <summary>
        /// Gets the value of the PropertyBinding attached DependencyProperty.
        /// </summary>
        /// <param name="element">Element for which to get the property.</param>
        /// <returns>Value of PropertyBinding attached DependencyProperty.</returns>
        public static StyleValueSetter GetPropertyBinding(FrameworkElement element)
        {
            Guard.NotNull(element);
            return (StyleValueSetter) element.GetValue(PropertyBindingProperty);
        }

        /// <summary>
        /// Sets the value of the PropertyBinding attached DependencyProperty.
        /// </summary>
        /// <param name="element">Element on which to set the property.</param>
        /// <param name="value">Value forPropertyBinding attached DependencyProperty.</param>
        public static void SetPropertyBinding(FrameworkElement element, StyleValueSetter value)
        {
            Guard.NotNull(element);
            element.SetValue(PropertyBindingProperty, value);
        }


        ///// <summary>
        ///// Returns a stream of assemblies to search for the provided type name.
        ///// </summary>
        // private static IEnumerable<Assembly> AssembliesToSearch
        // {
        //    get
        //    {
        //        // Start with the System.Windows assembly (home of all core controls)
        //        yield return typeof (Control).Assembly;

        //        // Fall back by trying each of the assemblies in the Deployment's Parts list
        //        foreach (var part in Deployment.Current.Parts)
        //        {
        //            var streamResourceInfo = Application.GetResourceStream(new Uri(part.Source, UriKind.Relative));
        //            using (var stream = streamResourceInfo.Stream)
        //            {
        //                yield return part.Load(stream);
        //            }
        //        }
        //    }
        // }

        /// <summary>
        /// Change handler for the PropertyBinding attached DependencyProperty.
        /// </summary>
        /// <param name="d">Object on which the property was changed.</param>
        /// <param name="e">Property change arguments.</param>
        private static void OnPropertyBindingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Get/validate parameters
            var element = d.Cast<FrameworkElement>();
            var item = e.NewValue.Cast<StyleValueSetter>();

            if ((null == item.Values) || (0 == item.Values.Count))
            {
                // No children; apply the relevant binding
                ApplyBinding(element, item);
            }
            else
            {
                // Apply the bindings of each child
                foreach (var child in item.Values)
                {
                    if ((null != item.Property) || (null != item.Binding))
                    {
                        throw new ArgumentException(
                            "A StyleValueSetter with Values may not have its Property or Binding set.");
                    }
                    if (0 != child.Values.Count)
                    {
                        throw new ArgumentException("Values of a StyleValueSetter may not have Values themselves.");
                    }
                    ApplyBinding(element, child);
                }
            }
        }

        /// <summary>
        /// Applies the Binding represented by the StyleValueSetter.
        /// </summary>
        /// <param name="element">Element to apply the Binding to.</param>
        /// <param name="item">StyleValueSetter representing the Binding.</param>
        private static void ApplyBinding(FrameworkElement element, StyleValueSetter item)
        {
            if ((null == item.Property) || (null == item.Binding))
            {
                throw new ArgumentException(
                    "StyleValueSetter's Property and Binding must both be set to non-null values.");
            }

            // Get the type on which to set the Binding
            var type = null == item.Type ? element.GetType() : System.Type.GetType(item.Type);

            // Get the DependencyProperty for which to set the Binding
            DependencyProperty property = null;
            const BindingFlags Flags = BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Static;
            var field = type.GetField(item.Property + "Property", Flags);
            if (field != null)
            {
                property = field.GetValue(null) as DependencyProperty;
            }
            if (property == null)
            {
                // Unable to find the requsted property
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                                                        "Unable to access DependencyProperty \"{0}\" on type \"{1}\".",
                                                        item.Property, type.Name));
            }

            // Set the specified Binding on the specified property
            element.SetBinding(property, item.Binding);
        }
    }
}
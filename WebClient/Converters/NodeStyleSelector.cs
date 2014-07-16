using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using WebClient.ICS.Client.ServiceReference;

namespace WebClient.ICS.Client.Converters
{
    /// <summary>
    /// Зачитывается свойство IsNode. Если true, значит NodeStyle возвращается, а если нет, то SettingStyle.
    /// </summary>
    public class NodeStyleSelector : StyleSelector, IValueConverter
    {
        /// <summary>
        /// Стиль узла.
        /// </summary>
        public object NodeStyle { private get; set; }

        /// <summary>
        /// Стиль настройки.
        /// </summary>
        public object SettingStyle { private get; set; }

        /// <summary>
        /// Изменяет источник данных перед их передачей целевому объекту для отображения в пользовательском интерфейсе.
        /// </summary>
        /// <returns>
        /// Значение, передаваемое целевому свойству зависимостей.
        /// </returns>
        /// <param name="value">Исходные данные, передаваемые целевому объекту.
        /// </param><param name="targetType">Тип <see cref="T:System.Type"/> 
        /// данных, ожидаемый целевым свойством зависимостей.</param><param name="parameter">
        /// Необязательный параметр для использования в логике преобразователя.</param><param name="culture">
        /// Язык и региональные параметры преобразования.</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && !(bool) value ? SettingStyle : NodeStyle;
        }

        /// <summary>
        /// Изменяет целевые данные перед их передачей исходному объекту.  
        /// Этот метод вызывается только в привязках <see cref="F:System.Windows.Data.BindingMode.TwoWay"/>.
        /// </summary>
        /// <returns>
        /// Значению, которое следует передать исходному объекту.
        /// </returns>
        /// <param name="value">Целевые данные, передаваемые исходному объекту.
        /// </param><param name="targetType">Тип <see cref="T:System.Type"/> 
        /// данных, ожидаемый исходным объектом.</param><param name="parameter">
        /// Необязательный параметр для использования в логике преобразователя.</param><param name="culture">
        /// Язык и региональные параметры преобразования.</param>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// SelectStyle.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public override Style SelectStyle(object item, DependencyObject container)
        {
            return (Style) (item is Setting ? SettingStyle : (item is Node ? NodeStyle : null));
        }
    }
}
using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;
using WebClient.ICS.Client.Model;

namespace WebClient.ICS.Client.Converters
{
	/// <summary>
	/// 	Converter for combobox. Используется в combobox при выборе типа данных.
	/// </summary>
	public class DataTypeToStringConverter : IValueConverter
	{
		/// <summary>
		/// 	Принимает DataTypeEnum от DataContext и возвращает string.
		/// </summary>
		/// <returns> Значение, передаваемое целевому свойству зависимостей. </returns>
		/// <param name="value"> Исходные данные, передаваемые целевому объекту. </param>
		/// <param name="targetType"> Тип <see cref="T:System.Type" /> 
		/// данных, ожидаемый целевым свойством зависимостей. </param>
		/// <param name="parameter"> Необязательный параметр для использования в логике преобразователя. </param>
		/// <param name="culture"> Язык и региональные параметры преобразования. </param>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Debug.WriteLine("DataTypeToStringConverter");

			if (value != null)
			{
				var type = (DataTypeEnum) value;
				return type.ToString();
			}

			return null;
		}

		/// <summary>
		/// 	Изменяет целевые данные перед их передачей исходному объекту. 
		/// Этот метод вызывается только в привязках <see
		///  	cref="F:System.Windows.Data.BindingMode.TwoWay" /> .
		/// </summary>
		/// <returns> Значению, которое следует передать исходному объекту. </returns>
		/// <param name="value"> Целевые данные, передаваемые исходному объекту. </param>
		/// <param name="targetType"> Тип <see cref="T:System.Type" /> данных, ожидаемый исходным объектом. </param>
		/// <param name="parameter"> Необязательный параметр для использования в логике преобразователя. </param>
		/// <param name="culture"> Язык и региональные параметры преобразования. </param>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Enum.Parse(typeof (DataTypeEnum), (string) value, true);
		}
	}
}
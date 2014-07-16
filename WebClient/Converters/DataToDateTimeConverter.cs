using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace WebClient.ICS.Client.Converters
{
	/// <summary>
	/// 	Преобразование из Data типа object в DateTime. Используется при инициализации контрола DateTimeControl.
	/// </summary>
	public class DataToDateTimeConverter : IValueConverter
	{
		/// <summary>
		/// 	Data ==> DateTime control.
		/// </summary>
		/// <param name="value"> </param>
		/// <param name="targetType"> </param>
		/// <param name="parameter"> </param>
		/// <param name="culture"> </param>
		/// <returns> </returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Debug.WriteLine("DataToDateTimeConverter");
			Debug.WriteLine(string.Format("[CONVERT DateTime] {1} From {0} to DateTime", 
				value == null ? "null" : value.GetType().ToString(), value));

			var returnValue = value;

			if (!(value is DateTime) && (targetType == typeof (DateTime?) || targetType == typeof (DateTime)))
			{
				returnValue = DateTime.Now;
			}

			Debug.WriteLine("[CONVERT DateTime RESULT] = " + returnValue);

			return returnValue;
		}

		/// <summary>
		/// 	DateTime control ==> Data.
		/// </summary>
		/// <param name="value"> </param>
		/// <param name="targetType"> </param>
		/// <param name="parameter"> </param>
		/// <param name="culture"> </param>
		/// <returns> </returns>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Debug.WriteLine("DataToDateTimeConverter");
			Debug.WriteLine(string.Format("[LAST DATA] = '{0}'", value));
			return value;
		}
	}
}
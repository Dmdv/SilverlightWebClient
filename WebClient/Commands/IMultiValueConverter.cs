using System;
using System.Globalization;

namespace WebClient.ICS.Client.Commands
{
	/// <summary>
	/// 	see: http://msdn.microsoft.com/en-us/library/system.windows.data.imultivalueconverter.aspx
	/// </summary>
	public interface IMultiValueConverter
	{
		/// <summary>
		/// 	Convert.
		/// </summary>
		/// <param name="values"> </param>
		/// <param name="targetType"> </param>
		/// <param name="parameter"> </param>
		/// <param name="culture"> </param>
		/// <returns> </returns>
		object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);

		/// <summary>
		/// 	ConvertBack.
		/// </summary>
		/// <param name="value"> </param>
		/// <param name="targetTypes"> </param>
		/// <param name="parameter"> </param>
		/// <param name="culture"> </param>
		/// <returns> </returns>
		object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture);
	}
}
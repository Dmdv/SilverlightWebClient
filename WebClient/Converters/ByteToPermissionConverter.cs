using System;
using System.Globalization;
using System.Windows.Data;
using WebClient.ICS.Client.Model;
using WebClient.ICS.Client.ServiceReference;

namespace WebClient.ICS.Client.Converters
{
	/// <summary>
	/// Преобразует массив байт в разрешение.
	/// </summary>
	public class ByteToPermissionConverter : IValueConverter
	{
		/// <summary>
		/// Modifies the source data before passing it to the target for display in the UI.
		/// </summary>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value == null ? null : new RightsAdapter(value.Cast<Permission>());
		}

		/// <summary>
		/// Modifies the target data before passing it to the source object.
		/// </summary>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
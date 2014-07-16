using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebClient.ICS.Client.Model;

namespace WebClient.ICS.Client.Converters
{
	/// <summary>
	/// 	BytesConverter.
	/// </summary>
	public static class BytesConverter
	{
		/// <summary>
		/// 	ToBytes.
		/// </summary>
		/// <param name="value"> </param>
		/// <returns> </returns>
		public static byte[] ToBytes(this object value)
		{
			if (value == null)
			{
				return null;
			}

			if (value is string)
				return Encoding.Unicode.GetBytes(value as string);
			if (value is int)
				return BitConverter.GetBytes((int) value);
			if (value is long)
				return BitConverter.GetBytes((long) value);
			if (value is double)
				return BitConverter.GetBytes((double) value);
			if (value is DateTime)
				return BitConverter.GetBytes(((DateTime) value).Ticks);
			if (value is bool)
				return BitConverter.GetBytes((bool) value);
			if (value is IEnumerable<byte>)
				return value.Cast<IEnumerable<byte>>().ToArray();

			throw new ArgumentException("value");
		}

		/// <summary>
		/// 	ToObject.
		/// </summary>
		/// <param name="bytes"> </param>
		/// <param name="dataType"> </param>
		/// <returns> </returns>
		public static object ToObject(this byte[] bytes, DataTypeEnum? dataType)
		{
			if (!dataType.HasValue)
				return null;

			switch (dataType)
			{
				case DataTypeEnum.Bin:
					return bytes;
				case DataTypeEnum.Bool:
					return bytes != null && ToBoolean(bytes);
				case DataTypeEnum.DateTime:
					return bytes == null ? DateTime.Now : ToDateTime(bytes);
				case DataTypeEnum.Double:
					return bytes == null ? 0 : ToDouble(bytes);
				case DataTypeEnum.Long:
					return bytes == null ? 0 : ToLong(bytes);
				case DataTypeEnum.String:
					return bytes == null ? string.Empty : ToString(bytes);
				case DataTypeEnum.Xml:
					return bytes == null ? string.Empty : ToString(bytes);
				default:
					throw new NotImplementedException("ToObject");
			}
		}

		/// <summary>
		/// 	ToString.
		/// </summary>
		/// <param name="value"> </param>
		/// <returns> </returns>
		public static string ToString(this byte[] value)
		{
			return value == null ? null : Encoding.Unicode.GetString(value, 0, value.Length);
		}

		/// <summary>
		/// 	ToInt.
		/// </summary>
		/// <param name="value"> </param>
		/// <returns> </returns>
		public static int ToInt(this byte[] value)
		{
			return value == null ? 0 : BitConverter.ToInt32(value, 0);
		}

		/// <summary>
		/// 	ToLong.
		/// </summary>
		/// <param name="value"> </param>
		/// <returns> </returns>
		public static long ToLong(this byte[] value)
		{
			return value == null ? 0 : BitConverter.ToInt64(value, 0);
		}

		/// <summary>
		/// 	ToDouble.
		/// </summary>
		/// <param name="value"> </param>
		/// <returns> </returns>
		public static double ToDouble(this byte[] value)
		{
			return value == null ? 0 : BitConverter.ToDouble(value, 0);
		}

		/// <summary>
		/// 	ToDateTime.
		/// </summary>
		/// <param name="value"> </param>
		/// <returns> </returns>
		public static DateTime ToDateTime(this byte[] value)
		{
			return value == null ? DateTime.Now : new DateTime(BitConverter.ToInt64(value, 0));
		}

		/// <summary>
		/// 	ToBoolean.
		/// </summary>
		/// <param name="value"> </param>
		/// <returns> </returns>
		public static bool ToBoolean(this byte[] value)
		{
			return value != null && BitConverter.ToBoolean(value, 0);
		}
	}
}
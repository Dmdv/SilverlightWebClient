using System;

namespace WebClient.ICS.Client.Model
{
    /// <summary>
    /// Extensions.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Cast. Throws InvalidCastException if fails.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <exception cref="InvalidCastException"></exception>
        /// <returns></returns>
        public static T Cast<T>(this object obj)
        {
            if (obj == null)
            {
                throw new NullReferenceException("Cast object cannot be equal null");
            }

            T value;

            try
            {
                value = (T)obj;
            }
            catch (InvalidCastException)
            {
                throw new InvalidCastException(string.Format("Failed to convert from '{0}' to '{1}'", obj.GetType(), typeof (T)));
            }

            return value;
        }

        /// <summary>
        /// TryCast. Throws no exception. If failed returns null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T TryCast<T>(this object obj) where T : class
        {
            return obj as T;
        }

        /// <summary>
        /// Default.
        /// </summary>
        /// <param name="datatype"></param>
        /// <returns></returns>
        public static object Default(this DataTypeEnum? datatype)
        {
            switch (datatype)
            {
                case DataTypeEnum.Bool:
                    return false;
                case DataTypeEnum.Long:
                    return 0L.Cast<long>();
                case DataTypeEnum.Double:
                    return 0.0D.Cast<double>();
                case DataTypeEnum.String:
                    return string.Empty;
                case DataTypeEnum.DateTime:
                    return DateTime.Now;
                case DataTypeEnum.Xml:
                case DataTypeEnum.Bin:
                    return new byte[0];
                default:
                    throw new ArgumentOutOfRangeException("datatype");
            }
        }
    }
}

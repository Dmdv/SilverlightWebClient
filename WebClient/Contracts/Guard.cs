using System;

namespace WebClient.ICS.Client.Contracts
{
    /// <summary>
    /// Guard.
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// NotNull.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <exception cref="ArgumentException">Throws exception if object is null.</exception>
        public static void NotNull<T>(T obj) where T : class
        {
            if (obj == null)
            {
                throw new ArgumentException(string.Format("Argument of type = '{0}' is null", typeof(T)));
            }
        }

        /// <summary>
        /// NotNull.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="name">Имя параметра.</param>
        public static void NotNull<T>(T obj, string name) where T : class
        {
            if (obj == null)
            {
                var message = string.Format("Argument of type = '{0}' and name = '{1}' is null", typeof (T), name);
                throw new ArgumentException(message);
            }
        }
    }
}
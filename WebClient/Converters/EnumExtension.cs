using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WebClient.ICS.Client.Model;

namespace WebClient.ICS.Client.Converters
{
    /// <summary>
    /// EnumExtension
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// GetNames.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetNames(this Enum e)
        {
            return e.GetType().GetFields(BindingFlags.Static | BindingFlags.Public).Select(fi => fi.Name).ToList();
        }

        /// <summary>
        /// DataTypeEnum
        /// </summary>
        public static IEnumerable<string> DataTypeEnum
        {
            get
            {
                return
                    typeof (DataTypeEnum)
                        .GetFields(BindingFlags.Static | BindingFlags.Public).Select(fi => fi.Name).ToList();
            }
        }
    }
}
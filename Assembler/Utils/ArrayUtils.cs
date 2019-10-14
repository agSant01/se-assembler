using System;
using System.Text;

namespace Assembler.Utils
{
    /// <summary>
    /// Custom made functionalities for arrays
    /// </summary>
    public static class ArrayUtils
    {
        /// <summary>
        /// Converts an array of objects to a string
        /// </summary>
        /// <param name="arr">Array of objects</param>
        /// <returns></returns>
        public static string ArrayToString(object[] arr)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("[");

            builder.AppendJoin(",", arr);

            builder.Append("]");

            return builder.ToString();
        }

        internal static object ArrayToString(sbyte[] arr)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("[");

            builder.AppendJoin(",", arr);

            builder.Append("]");

            return builder.ToString();
        }
    }
}

﻿using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace ConsoleHelpers
{
    /// <summary>
    /// Helpers for requesting Console Input.
    /// </summary>
    public static class ConsoleHelper
    {
        /// <summary>
        /// Requests user input from the <see cref="Console"/>.
        /// </summary>
        /// <returns>The user input or <see cref="string.Empty"/> if an exception occurred.</returns>
        private static string GetInput()
        {
            string input;
            try
            {
                input = Console.ReadLine()?.ToLower(CultureInfo.CurrentCulture) ?? string.Empty;
            }
            catch (Exception ex)
            when (ex is IOException || ex is OutOfMemoryException || ex is ArgumentOutOfRangeException)
            {
                input = string.Empty;
            }

            return input;
        }

        /// <summary>
        /// Prompts a user for input. Parses the input into a nullable primitive type.
        /// </summary>
        /// <param name="result">Contains a value of <typeparamref name="T"/>if parsed. Otherwise null.</param>
        /// <param name="prompt">The message to prompt a user with.</param>
        /// <typeparam name="T">The primitive data type to request.</typeparam>
        /// <returns>True if successfully parsed. Otherwise false.</returns>
        public static bool TryGetInput<T>(out T? result, string prompt = "") where T: struct, IConvertible
        {
            Console.Write(prompt ?? "");
            var input = GetInput();
            if (input.Length == 0)
            {
                result = null;
                return false;
            }

            result = null;
            var parsed = false;

            // Only support unboxing to built-in primitive types using type codes.
            var typeCode = Type.GetTypeCode(typeof(T));

            // Switch between 13 type codes. This avoids slowdowns from using reflection.
            switch (typeCode)
            {
                case TypeCode.Boolean:
                {
                    var truthyValues = new[] {"y", "yes", "true"};
                    var falsyValues = new[] {"n", "no", "false"};
                    if (truthyValues.Contains(input, StringComparer.OrdinalIgnoreCase))
                    {
                        parsed = true;
                        result = (T) Convert.ChangeType(true, typeof(bool), CultureInfo.CurrentCulture);
                    }else if (falsyValues.Contains(input, StringComparer.OrdinalIgnoreCase))
                    {
                        parsed = true;
                        result = (T) Convert.ChangeType(false, typeof(bool), CultureInfo.CurrentCulture);
                    }

                    break;
                }
                // Try to parse input. Then unbox the output into T result. result is null if parsing fails.
                case TypeCode.Byte:
                {
                    parsed = byte.TryParse(input, out var output);
                    result = parsed ? (T?) Convert.ChangeType(output, typeof(byte), CultureInfo.CurrentCulture) : null;
                    break;
                }
                case TypeCode.SByte:
                {
                    parsed = sbyte.TryParse(input, out var output);
                    result = parsed ? (T?) Convert.ChangeType(output, typeof(sbyte), CultureInfo.CurrentCulture) : null;
                    break;
                }
                case TypeCode.Char:
                {
                    parsed = char.TryParse(input, out var output);
                    result = parsed ? (T?) Convert.ChangeType(output, typeof(char), CultureInfo.CurrentCulture) : null;
                    break;
                }
                case TypeCode.Single:
                {
                    parsed = float.TryParse(input, out var output);
                    result = parsed ? (T?) Convert.ChangeType(output, typeof(float), CultureInfo.CurrentCulture) : null;
                    break;
                }
                case TypeCode.Double:
                {
                    parsed = double.TryParse(input, out var output);
                    result = parsed ? (T?) Convert.ChangeType(output, typeof(double), CultureInfo.CurrentCulture) : null;
                    break;
                }
                case TypeCode.Decimal:
                {
                    parsed = decimal.TryParse(input, out var output);
                    result = parsed ? (T?) Convert.ChangeType(output, typeof(decimal), CultureInfo.CurrentCulture) : null;
                    break;
                }
                case TypeCode.Int16:
                {
                    parsed = short.TryParse(input, out var output);
                    result = parsed ? (T?) Convert.ChangeType(output, typeof(short), CultureInfo.CurrentCulture) : null;
                    break;
                }
                case TypeCode.Int32:
                {
                    parsed = int.TryParse(input, out var output);
                    result = parsed ? (T?) Convert.ChangeType(output, typeof(int), CultureInfo.CurrentCulture) : null;
                    break;
                }
                case TypeCode.Int64:
                {
                    parsed = long.TryParse(input, out var output);
                    result = parsed ? (T?) Convert.ChangeType(output, typeof(long), CultureInfo.CurrentCulture) : null;
                    break;
                }
                case TypeCode.UInt16:
                {
                    parsed = ushort.TryParse(input, out var output);
                    result = parsed ? (T?) Convert.ChangeType(output, typeof(ushort), CultureInfo.CurrentCulture) : null;
                    break;
                }
                case TypeCode.UInt32:
                {
                    parsed = uint.TryParse(input, out var output);
                    result = parsed ? (T?) Convert.ChangeType(output, typeof(uint), CultureInfo.CurrentCulture) : null;
                    break;
                }
                case TypeCode.UInt64:
                {
                    parsed = ulong.TryParse(input, out var output);
                    result = parsed ? (T?) Convert.ChangeType(output, typeof(ulong), CultureInfo.CurrentCulture) : null;
                    break;
                }
            }

            return parsed;
        }
    }
}

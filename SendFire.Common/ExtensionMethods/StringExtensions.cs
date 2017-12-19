using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SendFire.Common.Interfaces;

namespace SendFire.Common.ExtensionMethods
{
    public static class StringExtensions
    {
        /// <summary>
        /// More succinct usage of IsNullOrEmpty()
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Appends a name / value pair to a URL properly encoded.
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void AppendUriEncoded(this StringBuilder sb, string name, string value)
        {
            if (sb.Length > 0)
            {
                sb.Append("&");
            }
            sb.Append(Uri.EscapeDataString(name));
            sb.Append("=");
            sb.Append(Uri.EscapeDataString(value));
        }

        /// <summary>
        /// Combines two paths together in the proper way.
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <returns></returns>
        public static string PathCombine(this string path1, string path2)
        {
            return Path.Combine(path1, path2);
        }

        public static bool CompareNoCase(this string stringA, string stringB)
        {
            return string.Compare(stringA, stringB, StringComparison.OrdinalIgnoreCase) == 0;
        }

        public static bool ContainsNoCase(this string stringA, string stringB)
        {
            return stringA.IndexOf(stringB, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static bool CompareWithCase(this string stringA, string stringB)
        {
            return string.Compare(stringA, stringB, StringComparison.Ordinal) == 0;
        }

        public static string SafeToUpper(this string value)
        {
            return value.IsNullOrEmpty() ? "" : value.ToUpper();
        }
        
        public static string SafeTrim(this string value)
        {
            return value.IsNullOrEmpty() ? "" : value.Trim();
        }

        public static string SafeTrimStart(this string value)
        {
            return value.IsNullOrEmpty() ? "" : value.TrimStart();
        }

        public static string SafeTrimEnd(this string value)
        {
            return value.IsNullOrEmpty() ? "" : value.TrimEnd();
        }

        public static string SafeTrimUpper(this string value)
        {
            return value.IsNullOrEmpty() ? "" : value.Trim().ToUpper();
        }

        public static string VerifySize(this string value, int maxLength = 0)
        {
            if (value.IsNullOrEmpty()) return "";

            var ret = value.Trim();
            if (maxLength > 0 && ret.Length >= maxLength)
            {
                ret = ret.Substring(0, maxLength);
            }

            return (ret);
        }
        
        public static void ThrowIfEmpty(this string value, string name)
        {
            if (value.SafeTrim().IsNullOrEmpty())
            {
                throw new ArgumentException($"{name} was not passed with a proper value.");
            }
        }

        public static T DeserializeXml<T>(this string value) where T : class
        {
            var encoding = new UTF8Encoding();

            using (var stream = new MemoryStream(encoding.GetBytes(value)))
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                return serializer.Deserialize(stream) as T;
            }
        }

        public static string SafeLeft(this string value, int length)
        {
            if (value.IsNullOrEmpty()) return "";
            var strLen = value.Length;
            if (length > strLen) return value;

            var result = value.Substring(0, length);
            return result;
        }

        public static string SafeRight(this string value, int length)
        {
            if (value.IsNullOrEmpty()) return "";
            var strLen = value.Length;
            if (length > strLen) return value;

            if (value == string.Empty) return value;
            var result = value.Substring(strLen - length, length);
            return result;
        }

        public static bool IsDouble(this string anyString)
        {
            return IsDouble(anyString, out var dummyValue);
        }

        public static bool IsDouble(this string anyString, out double numericValue)
        {
            numericValue = 0;
            if (anyString.IsNullOrEmpty()) return false;
            var cultureInfo = new System.Globalization.CultureInfo("en-US", true);
            return double.TryParse(anyString, System.Globalization.NumberStyles.Any, cultureInfo.NumberFormat, out numericValue);
        }

        public static bool IsInt(this string anyString)
        {
            return IsInt(anyString, out var dummyValue);
        }

        public static bool IsInt(this string anyString, out int numericValue)
        {
            numericValue = 0;
            if (anyString.IsNullOrEmpty()) return false;

            var cultureInfo = new System.Globalization.CultureInfo("en-US", true);
            return int.TryParse(anyString, System.Globalization.NumberStyles.Any, cultureInfo.NumberFormat, out numericValue);
        }

        public static string ReplaceEnvValue(this string sourceString, string[] envVariables, IEnvironmentManager environmentManager)
        {
            if (sourceString.IsNullOrEmpty()) return string.Empty;
            foreach (var envVariable in envVariables)
            {
                var envValue = environmentManager.GetEnvironmentVariable(envVariable);
                sourceString = sourceString.Replace(string.Format("@{0}@", envVariable), envValue);
            }
            return sourceString;
        }

        public static string ReplaceBuildPropertyValue(this string sourceString, string buildProperty, string buildValue)
        {
            if (sourceString.IsNullOrEmpty()) return string.Empty;
            return sourceString.Replace(string.Format("@{0}@", buildProperty), buildValue);
        }

        public static string SanitizeInputs(this string str, int maxLength)
        {
            if (str == null)
            {
                str = string.Empty;
            }
            else
            {
                // Maximum Length gotcha
                if (str.Length > maxLength)
                    str = str.Substring(0, maxLength - 3) + "...";
            }

            return (str);
        }

        public static bool IsSameCommandLineArg(this string arg, string argExpected)
        {
            argExpected = argExpected.StripToArgument();
            arg = arg.StripToArgument();
            return arg.CompareNoCase(argExpected);
        }

        public static bool StartsWithCommandLineArg(this string arg, string argExpected)
        {
            argExpected = argExpected.StripToArgument().ToUpper();
            arg = arg.SafeToUpper();
            return arg.StartsWith($"/{argExpected}") || arg.StartsWith($"-{argExpected}") || arg.StartsWith($"--{argExpected}");

        }

        public static string StripToArgument(this string arg)
        {
            if (arg.IsNullOrEmpty()) return "";
            if (arg.StartsWith("--"))
            {
                return arg.Substring(2);
            }
            if (arg.StartsWith("/") || arg.StartsWith("-"))
            {
                return arg.Substring(1);
            }
            return arg;
        }

        public static bool IsTrue(this string arg)
        {
            if (arg.IsNullOrEmpty()) return false;
            if (arg.CompareNoCase("true") || arg == "1" || arg.CompareNoCase("t") || arg.CompareNoCase("y") || arg.CompareNoCase("yes")) return true;
            return false;
        }

        public static bool IsFalse(this string arg)
        {
            if (arg.IsNullOrEmpty()) return false;
            if (arg.CompareNoCase("false") || arg == "0" || arg.CompareNoCase("f") || arg.CompareNoCase("n") || arg.CompareNoCase("no")) return true;
            return false;
        }

        public static string WrapAtLines(this string text, int lineLength)
        {
            using (var reader = new StringReader(text))
                return reader.ReadToEnd(lineLength);
        }

        public static string[] SplitAtLines(this string text, int lineLength)
        {
            using (var reader = new StringReader(text))
                return reader.ReadLines(lineLength).ToArray();
        }

        public static string ReadToEnd(this TextReader reader, int lineLength)
        {
            return string.Join(System.Environment.NewLine, reader.ReadLines(lineLength));
        }

        public static IEnumerable<string> ReadLines(this TextReader reader, int lineLength)
        {
            var line = new StringBuilder();
            foreach (var word in reader.ReadWords())
                if (line.Length + word.Length <= lineLength)
                    line.Append($"{word} ");
                else
                {
                    yield return line.ToString().Trim();
                    line = new StringBuilder($"{word} ");
                }

            if (line.Length > 0)
                yield return line.ToString().Trim();
        }

        public static IEnumerable<string> ReadWords(this TextReader reader)
        {
            while (!reader.IsEof())
            {
                var word = new StringBuilder();
                while (!reader.IsBreak())
                {
                    word.Append(reader.Text());
                    reader.Read();
                }

                reader.Read();
                if (word.Length > 0)
                    yield return word.ToString();
            }
        }

        static bool IsBreak(this TextReader reader) => reader.IsEof() || reader.IsNullOrWhiteSpace();
        static bool IsNullOrWhiteSpace(this TextReader reader) => string.IsNullOrWhiteSpace(reader.Text());
        static string Text(this TextReader reader) => char.ConvertFromUtf32(reader.Peek());
        static bool IsEof(this TextReader reader) => reader.Peek() == -1;
    }
}

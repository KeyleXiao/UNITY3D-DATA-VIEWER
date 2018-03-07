/// Tim Tryzbiak, ootii, LLC
using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace SmartDataViewer.Helpers
{
    /// <summary>
    /// Static functions to help us
    /// </summary>
    public class StringHelper
    {
        /// <summary>
        /// Total number of strings that can happen with our pre-allocated arrays
        /// </summary>
        public const int MAX_STRINGS = 100;

        /// <summary>
        /// Simple buffer to hold our string element
        /// </summary>
        public static string[] SharedStrings = new string[MAX_STRINGS];

        /// <summary>
        /// Provides simple output for different values
        /// </summary>
        /// <param name="rValue"></param>
        /// <returns></returns>
        public static string ToSimpleString(int rValue)
        {
            return string.Format("{0}", rValue);
        }

        /// <summary>
        /// Provides simple output for different values
        /// </summary>
        /// <param name="rValue"></param>
        /// <returns></returns>
        public static string ToSimpleString(float rValue)
        {
            return string.Format("{0:f2}", rValue);
        }

        /// <summary>
        /// Provides simple output for different values
        /// </summary>
        /// <param name="rValue"></param>
        /// <returns></returns>
        public static string ToSimpleString(bool rValue)
        {
            return (rValue ? "true" : "false");
        }

        /// <summary>
        /// Provides simple output for different values
        /// </summary>
        /// <param name="rValue"></param>
        /// <returns></returns>
        public static string ToSimpleString(string rValue)
        {
            return rValue;
        }

        /// <summary>
        /// Provides simple output for different values
        /// </summary>
        /// <param name="rValue"></param>
        /// <returns></returns>
        public static string ToSimpleString(Vector2 rValue)
        {
            return string.Format("{0:f2}, {1:f2}", rValue.x, rValue.y);
        }

        /// <summary>
        /// Provides simple output for different values
        /// </summary>
        /// <param name="rValue"></param>
        /// <returns></returns>
        public static string ToSimpleString(Vector3 rValue)
        {
            return string.Format("{0:f2}, {1:f2}, {2:f2}", rValue.x, rValue.y, rValue.z);
        }

        /// <summary>
        /// Provides simple output for different values
        /// </summary>
        /// <param name="rValue"></param>
        /// <returns></returns>
        public static string ToSimpleString(Vector4 rValue)
        {
            return string.Format("{0:f2}, {1:f2}, {2:f2}, {3:f2}", rValue.x, rValue.y, rValue.z, rValue.w);
        }

        /// <summary>
        /// Provides simple output for different values
        /// </summary>
        /// <param name="rValue"></param>
        /// <returns></returns>
        public static string ToSimpleString(Quaternion rValue)
        {
            Vector3 lEuler = rValue.eulerAngles;
            return string.Format("{0:f5}, {1:f5}, {2:f5}", lEuler.x, lEuler.y, lEuler.z);
        }

        /// <summary>
        /// Provides simple output for different values
        /// </summary>
        /// <param name="rValue"></param>
        /// <returns></returns>
        public static string ToSimpleString(Transform rValue)
        {
            if (rValue == null) { return "null"; }
            return string.Format("{0}", rValue.name);
        }

        /// <summary>
        /// Provides simple output for different values
        /// </summary>
        /// <param name="rValue"></param>
        /// <returns></returns>
        public static string ToSimpleString(GameObject rValue)
        {
            if (rValue == null) { return "null"; }
            return string.Format("{0}", rValue.name);
        }

        /// <summary>
        /// Provides simple output for different values
        /// </summary>
        /// <param name="rValue"></param>
        /// <returns></returns>
        public static string ToSimpleString(UnityEngine.Object rValue)
        {
            if (rValue == null) { return "null"; }
            return string.Format("{0}", rValue.name);
        }

        /// <summary>
        /// Provides simple output for different values
        /// </summary>
        /// <param name="rValue"></param>
        /// <returns></returns>
        public static string ToSimpleString(object rValue)
        {
            if (rValue == null) { return "null"; }
            return string.Format("{0}", rValue.ToString());
        }

        /// <summary>
        /// Converts the data to a string value
        /// </summary>
        /// <returns>The string.</returns>
        /// <param name="rInput">Value to convert</param>
        public static string ToString(Vector3 rInput)
        {
            return String.Format("[m:{0:f6} x:{1:f6} y:{2:f6} z:{3:f6}]", rInput.magnitude, rInput.x, rInput.y, rInput.z);
        }

        /// <summary>
        /// Converts the data to a string value
        /// </summary>
        /// <returns>The string.</returns>
        /// <param name="rInput">Value to convert</param>
        public static string ToString(Quaternion rInput)
        {
            Vector3 lEuler = rInput.eulerAngles;

            float lAngle = 0f;
            Vector3 lAxis = Vector3.zero;
            rInput.ToAngleAxis(out lAngle, out lAxis);

            return String.Format("[p:{0:f4} y:{1:f4} r:{2:f4} x:{3:f7} y:{4:f7} z:{5:f7} w:{6:f7} angle:{7:f7} axis:{8}]", lEuler.x, lEuler.y, lEuler.z, rInput.x, rInput.y, rInput.z, rInput.w, lAngle, ToString(lAxis));
        }

        /// <summary>
        /// Adds a space between camel cased text
        /// </summary>
        /// <param name="rInput"></param>
        /// <returns></returns>
        public static string FormatCamelCase(string rInput)
        {
            return Regex.Replace(Regex.Replace(rInput, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), @"(\p{Ll})(\P{Ll})", "$1 $2");
        }

        /// <summary>
        /// Adds a space between camel cased text
        /// </summary>
        /// <param name="rInput"></param>
        /// <returns></returns>
        public static string CleanString(string rInput)
        {
            return rInput.Replace(" ", String.Empty).Replace("_", String.Empty).ToLower();
        }

        /// <summary>
        /// Provides an allocation free way of splitting strings
        /// </summary>
        /// <param name="rString"></param>
        /// <param name="rDelimiter"></param>
        /// <returns></returns>
        public static int Split(string rString, char rDelimiter)
        {
            int resultIndex = 0;
            int startIndex = 0;

            // Find the mid-parts
            for (int i = 0; i < rString.Length; i++)
            {
                if (rString[i] == rDelimiter)
                {
                    SharedStrings[resultIndex] = rString.Substring(startIndex, i - startIndex);
                    resultIndex++;
                    startIndex = i + 1;
                }
            }

            // Find the last part
            SharedStrings[resultIndex] = rString.Substring(startIndex, rString.Length - startIndex);
            resultIndex++;

            return resultIndex;
        }

        /// <summary>
        /// Splits a string, but respecting qualifiers around the whole delimited segments so
        /// we can use the delimiter (say a comma) in the segment.
        /// </summary>
        /// <param name="rString"></param>
        /// <param name="rDelimiter"></param>
        /// <param name="rQualifier"></param>
        /// <param name="rIgnoreCase"></param>
        /// <returns></returns>
        public static string[] Split(string rString, string rDelimiter, string rQualifier, bool rIgnoreCase)
        {
            int lStartIndex = 0;
            bool lQualifierState = false;
            ArrayList lValues = new ArrayList();

            // Walk the original string one character at a time
            for (int lIndex = 0; lIndex < rString.Length - 1; lIndex++)
            {
                // Check if the qualifier exists for this character
                if (rQualifier != null && string.Compare(rString.Substring(lIndex, rQualifier.Length), rQualifier, rIgnoreCase) == 0)
                {
                    lQualifierState = !(lQualifierState);
                }
                // If we're not in a qualifier, check for a delimiter
                else if (!lQualifierState && (string.Compare(rString.Substring(lIndex, rDelimiter.Length), rDelimiter, rIgnoreCase) == 0))
                {
                    lValues.Add(rString.Substring(lStartIndex, lIndex - lStartIndex));
                    lStartIndex = lIndex + rDelimiter.Length;
                }
            }

            // Add the last part of the string
            if (lStartIndex < rString.Length)
            {
                lValues.Add(rString.Substring(lStartIndex, rString.Length - lStartIndex));
            }

            // Copy the results into an array
            string[] lReturnValues = new string[lValues.Count];
            lValues.CopyTo(lReturnValues);

            // Return the array
            return lReturnValues;
        }
        
        
        /**
         *  随机数函数
         */
        static char[] constant = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};

        static public string GenerateRandomNumber(int length)
        {
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder();
            System.Random rd = new System.Random();
            for (int i = 0; i < length; i++)
            {
                newRandom.Append(constant[rd.Next(10)]);
            }

            return newRandom.ToString();
        }
    }

    /// <summary>
    /// String extensions
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Allows us to check if a subsctring exists with insensativity
        /// </summary>
        /// <param name="source"></param>
        /// <param name="toCheck"></param>
        /// <param name="comp"></param>
        /// <returns></returns>
        public static bool Contains(this string rSource, string rValue, StringComparison rComparison)
        {
            return rSource.IndexOf(rValue, rComparison) >= 0;
        }
    }
}


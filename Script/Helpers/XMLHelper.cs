/// Tim Tryzbiak, ootii, LLC
using System;
using System.Text;
using System.Xml;

namespace SmartDataViewer.Helpers
{
    /// <summary>
    /// Provies suppo rt for reading and manipulating XML
    /// </summary>
    public class XMLHelper
    {
        /// <summary>
        /// Sets the attribute value. If it doesn't exist, it creates the attribute in the XML
        /// </summary>
        /// <param name="rXML">XmlNode that holds the attributes</param>
        /// <param name="rName">Name of the attribute to set</param>
        /// <param name="rValue">Value of the attribute to set</param>
        public static void SetAttribute(XmlNode rXML, string rName, string rValue)
        {
            if (rXML == null) { return; }

            XmlElement lElement = (XmlElement)rXML;
            lElement.SetAttribute(rName, rValue);
        }

        /// <summary>
        /// Returns the attribute value as a specific type
        /// </summary>
        /// <typeparam name="T">Type to return</typeparam>
        /// <param name="rXML">XmlNode that holds the attributes</param>
        /// <param name="rName">Name of the attribute to retrieve</param>
        /// <returns></returns>
        public static T GetAttribute<T>(XmlNode rXML, string rName)
        {
            T lValue = default(T);
            if (rXML == null) { return lValue; }

            XmlElement lElement = (XmlElement)rXML;
            if (!lElement.HasAttribute(rName)) { return lValue; }

            string lStringValue = lElement.GetAttribute(rName);
            lValue = (T)Convert.ChangeType(lStringValue, typeof(T));

            return lValue;
        }

        /// <summary>
        /// Returns the attribute value as a specific type
        /// </summary>
        /// <param name="rXML">string that holds the attributes</param>
        /// <param name="rName">Name of the attribute to retrieve</param>
        /// <returns></returns>
        public static string GetAttribute(string rXML, string rName)
        {
            int lStart = rXML.IndexOf("t=\"");
            if (lStart >= 0)
            {
                int lEnd = rXML.IndexOf("\"", lStart + 3);
                if (lEnd >= 0)
                {
                    return rXML.Substring(lStart + 3, lEnd - (lStart + 3));
                }
            }

            return "";
        }
    }
}

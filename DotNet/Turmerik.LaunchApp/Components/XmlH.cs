using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Turmerik.LaunchApp.Components
{
    public static class XmlH
    {
        public static T TryDeserializeXmlFromFile<T>(
            string xmlFilePath,
            Type type = null)
        {
            T data;

            if (File.Exists(xmlFilePath))
            {
                string xml = File.ReadAllText(xmlFilePath);
                data = TryDeserializeXml<T>(xml, type);
            }
            else
            {
                data = default;
            }

            return data;
        }

        public static T TryDeserializeXml<T>(
            string xml,
            Type type = null)
        {
            T data;
            type = type ?? typeof(T);
            var serializer = new XmlSerializer(type);

            using (var sr = new StringReader(xml))
            {
                data = (T)serializer.Deserialize(sr);
            }

            return data;
        }

        public static void SerializeXmlToFile<T>(
            T obj,
            string xmlFilePath,
            Type type = null)
        {
            string xml = SerializeXml(obj, type);
            File.WriteAllText(xmlFilePath, xml);
        }

        public static string SerializeXml<T>(
            T obj,
            Type type = null)
        {
            string xml;
            type = type ?? typeof(T);
            var serializer = new XmlSerializer(typeof(T));

            using (var sw = new StringWriter())
            {
                serializer.Serialize(sw, obj);
                xml = sw.ToString();
            }

            return xml;
        }
    }
}

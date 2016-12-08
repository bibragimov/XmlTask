using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace XmlTask.Utils
{
    public class XmlParser
    {
        public static T ParseXml<T>(string filePath, string xmlRootName) where T : class
        {
            var xdoc = new XmlDocument();
            xdoc.Load(filePath);

            var serializer = new XmlSerializer(typeof (T), new XmlRootAttribute(xmlRootName));
            var stringReader = new StringReader(xdoc.InnerXml);

            var offerList = (T) serializer.Deserialize(stringReader);

            return offerList;
        }

        public static string SerializeObject<T>(T model)
        {
            var serializerNamespaces = new XmlSerializerNamespaces(new[] {XmlQualifiedName.Empty});
            var serializer = new XmlSerializer(typeof (T));
            var outStream = new Utf8StringWriter();
            serializer.Serialize(outStream, model, serializerNamespaces);
            return outStream.ToString();
        }
    }
}
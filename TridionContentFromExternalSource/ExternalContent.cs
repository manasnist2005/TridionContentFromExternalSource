using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace TridionContentFromExternalSource
{
    [Serializable]
    [XmlRoot(ElementName = "Paragraph", Namespace = "uuid:df6c2542-0658-4232-812a-97cac70a3e2c")]
    public class ParagraphXMLEntities
    {
        # region Paragraph XML Properties
        [XmlElement("paragraph")]
        public string Paragraph { get; set; }
        #endregion

        public ParagraphXMLEntities(ParagraphXMLEntities _ParagraphXMLEntities)
        {
            this.Paragraph = _ParagraphXMLEntities.Paragraph;
        }
        public ParagraphXMLEntities() { }
        public string Serialize()
        {
            XmlWriterSettings settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true,
                Encoding = Encoding.ASCII
            };

            using (MemoryStream stream = new MemoryStream())
            using (XmlWriter writer = XmlWriter.Create(stream, settings))
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add(string.Empty, "Paragraph");
                XmlSerializer serializer = new XmlSerializer(typeof(ParagraphXMLEntities));
                serializer.Serialize(writer, this, ns);
                string xml = Encoding.ASCII.GetString(stream.ToArray());
                return xml;
            }
        }

        public static ParagraphXMLEntities Deserialize(string xml)
        {
            ParagraphXMLEntities result;
            using (StringReader stringReader = new StringReader(xml))
            using (XmlReader xmlReader = XmlReader.Create(stringReader))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ParagraphXMLEntities));
                result = (ParagraphXMLEntities)serializer.Deserialize(xmlReader);
            }
            return result;
        }
    }

    [Serializable]
    [XmlRoot(ElementName = "url")]
    public class Url
    {
        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces Namespaces;

        [XmlAttribute("type", Namespace = "http://www.w3.org/1999/xlink")]
        public string Type { get; set; }
        [XmlAttribute("href", Namespace = "http://www.w3.org/1999/xlink")]
        public string href { get; set; } 
       
        
        [XmlText]
        public string Name { get; set; }

        public Url(Url _url)
        {
            this.Name = _url.Name;
            this.Type = _url.Type;
            this.href = _url.href;
           
        }
        public Url()
        {
            Namespaces = new XmlSerializerNamespaces();
            Namespaces.Add("xlink", "http://www.w3.org/1999/xlink"); 
        }
    }

    /// <summary>
    /// Class for mapping datacontract entities of xml attribute to xml element
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName = "Content", Namespace = "uuid:df6c2542-0658-4232-812a-97cac70a3e2c")]
    public class ExternalContentXMLEntities
    {
        private ParagraphXMLEntities _ParagraphXMLEntities = new ParagraphXMLEntities();
        private Url _url = new Url();
        public ExternalContentXMLEntities(ExternalContentXMLEntities _ExternalContentXMLEntities)
        {
            this.Header = _ExternalContentXMLEntities.Header;
            this.Description = _ExternalContentXMLEntities.Description;
            this.Url = _ExternalContentXMLEntities.Url;
            this.Paragraph = _ExternalContentXMLEntities.Paragraph;
        }

        public ExternalContentXMLEntities()
        {
        }

        [XmlIgnore]
        public string TcmId { get; set; }
        [XmlElement("header")]
        public string Header { get; set; }
        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("url")]    
        public Url Url { get; set; }

        
        [XmlElement("paragraph")]
        public ParagraphXMLEntities Paragraph { get; set; }

        public string Serialize()
        {
            XmlWriterSettings settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true,
                Encoding = Encoding.ASCII
            };

            using (MemoryStream stream = new MemoryStream())
            using (XmlWriter writer = XmlWriter.Create(stream, settings))
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add(string.Empty, "uuid:df6c2542-0658-4232-812a-97cac70a3e2c");
                XmlSerializer serializer = new XmlSerializer(typeof(ExternalContentXMLEntities));
                serializer.Serialize(writer, this, ns);
                string xml = Encoding.ASCII.GetString(stream.ToArray());
                return xml;
            }
        }

        public static ExternalContentXMLEntities Deserialize(string xml)
        {
            ExternalContentXMLEntities result;
            using (StringReader stringReader = new StringReader(xml))
            using (XmlReader xmlReader = XmlReader.Create(stringReader))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ExternalContentXMLEntities));
                result = (ExternalContentXMLEntities)serializer.Deserialize(xmlReader);
            }
            return result;
        }
    }

    public class ExternalContent
    {      
        public string Header { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
    }
       
}
    
        
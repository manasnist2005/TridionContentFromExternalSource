using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Tridion.ContentManager.CoreService.Client;

namespace TridionContentFromExternalSource
{
    /// <summary>
    /// Class for mapping datacontract entities of xml attribute to xml element
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName = "Content", Namespace = "uuid:df6c2542-0658-4232-812a-97cac70a3e2c")]
    public class PolicyXMLEntities
    {

        # region Policy XML Properties
        [XmlIgnore]
        public string TcmId { get; set; }

        [XmlElement("policy_id")]
        public string PolicyId { get; set; }

        [XmlElement("policy_name")]
        public string PolicyName { get; set; }

        [XmlElement("policy_description")]
        public string PolicyDescription { get; set; }

        #endregion


        # region Method for serialization

        /// <summary>
        /// Serialize data contract entities in xml class
        /// </summary>
        /// <returns>xml string</returns>
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
                ns.Add(string.Empty, IngestionServiceConstants.PolicyEntityNamespace);
                XmlSerializer serializer = new XmlSerializer(typeof(PolicyXMLEntities));
                serializer.Serialize(writer, this, ns);
                string xml = Encoding.ASCII.GetString(stream.ToArray());


                return xml;
            }
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static PolicyXMLEntities Deserialize(string xml)
        {
            PolicyXMLEntities result;
            using (StringReader stringReader = new StringReader(xml))
            using (XmlReader xmlReader = XmlReader.Create(stringReader))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(PolicyXMLEntities));
                result = (PolicyXMLEntities)serializer.Deserialize(xmlReader);
            }
            return result;
        }
        public class NewsOneXMLEntities
        {



        }
        public class SaveComponent
        {
            public string SetPolicyComponent(SessionAwareCoreServiceClient client, NewsOneXMLEntities policy, string policyId)
            {
                ComponentData comp = null;
                ComponentData newComp = null;
                try
                {
                    comp.Id = "tcm:0-0-0";//hard coding will be placed in some constent.
                    comp.Title = policy.PolicyName + "_" + policy.PolicyId;
                    comp.ComponentType = ComponentType.Normal;
                    comp.LocationInfo = new LocationInfo() { OrganizationalItem = new LinkToOrganizationalItemData() { IdRef = "tcm:10-440-2" } };
                    comp.Schema = new LinkToSchemaData() { IdRef = "tcm:10-7344-8" };
                    comp.Content = policy.Serialize();
                    string tcmID = GetTcmId(client, policyId, "tcm:10-440-2");//Create method to check id the component already exists or not
                    if (String.IsNullOrEmpty(tcmID))
                    {
                        newComp = client.Create(comp, new ReadOptions()) as ComponentData;
                    }
                    else
                    {
                        //newComp = UpdateComponent(client, tcmID, policy) as ComponentData;//You can write the logic to update the component in UpdateComponent Method
                    }
                    return newComp.Id;
                }
                catch (Exception ex)
                {
                    return null;
                }
                finally
                {
                    comp = null;
                    newComp = null;
                }
            }

            public string GetTcmId(SessionAwareCoreServiceClient client, string Id, string locationTcmID)
            {
                try
                {
                    string tcmID = string.Empty;
                    //get the XML list of component from the folder
                    var productsXML =
                    client.GetListXml(locationTcmID,
                    new OrganizationalItemItemsFilterData
                    {
                        ItemTypes = new[] { ItemType.Component }
                    });
                    //loop through each item and find out if it is the product we want
                    foreach (var product in productsXML.Elements())
                    {
                        tcmID = product.Attribute("ID").Value;
                        var productData = client.Read(product.Attribute("ID").Value, null) as ComponentData;

                        var schemaFields = client.ReadSchemaFields(productData.Schema.IdRef, false, null);
                        XNamespace ns = schemaFields.NamespaceUri;
                        //check if the product id's match

                        if ((XDocument.Parse(productData.Content)).Root.Element(ns + "policy_id") != null)
                        {
                            if (Id == (XDocument.Parse(productData.Content)).Root.Element(ns + "policy_id").Value)
                            {
                                return tcmID;
                            }
                        }
                    }
                    return String.Empty;
                }
                catch (Exception Ex)
                {
                    return string.Empty;
                }

            }
        }


    }
}
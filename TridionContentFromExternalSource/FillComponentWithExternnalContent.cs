using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Tridion.ContentManager.CoreService.Client;

namespace TridionContentFromExternalSource
{
    public static class FillComponentWithExternnalContent
    {
        public static List<ExternalContent> getDatafromXml()
        {
            XDocument xmlcontent = XDocument.Load(@"C:\Manas\TridionContentFromExternalSource\TridionContentFromExternalSource\Data\data.xml");
            List<ExternalContent> contents = new List<ExternalContent>();
            foreach (var child in xmlcontent.Descendants().Descendants())
            {
                if (child.HasAttributes)
                {
                    ExternalContent content = new ExternalContent();
                    content.Header = child.Descendants().ElementAt(0).Value;
                    content.Description = child.Descendants().ElementAt(1).Value;
                    content.Url = child.Descendants().ElementAt(2).Value;
                    contents.Add(content);
                }
            }
            
                
            return contents;
        }
        public static ExternalContentXMLEntities GetExternalcontent(ExternalContent content)
        {
            ExternalContentXMLEntities _externalContent = new ExternalContentXMLEntities();
            _externalContent.Header = content.Header;
            _externalContent.Description = content.Description;

            Url _url = new Url();
            _url.Name = "";
            _url.Type = "simple";
            _url.href = content.Url;
            _externalContent.Url = _url;

            //ParagraphXMLEntities _paragraphXMLEntities = new ParagraphXMLEntities();
            //_paragraphXMLEntities.Paragraph = "Abc";
            //_externalContent.Paragraph = _paragraphXMLEntities;

            
            return _externalContent;
        }

        public static void FillComponent()
        {
            foreach (ExternalContent content in getDatafromXml())
            {
                ExternalContentXMLEntities _externalContent = GetExternalcontent(content);
                ComponentData comp = new ComponentData();
                //string webDavUrl = "webdav/650%20Tieto%20Gadgets%20Online/Building%20Blocks/Content/Tieto%20Content/External_Resource/";
                string schemaId = "tcm:2033-9689-8";
                comp.Id = "tcm:0-0-0";
                comp.LocationInfo = new LocationInfo() { OrganizationalItem = new LinkToOrganizationalItemData() { IdRef = "tcm:2033-4364-2" } };
                comp.Schema = new LinkToSchemaData() { IdRef = schemaId };
                comp.Title = _externalContent.Header;
                comp.Content = _externalContent.Serialize();
                CoreService cs = new CoreService();
                cs.CreateComponent(comp);
            }
           
        }
        
        
    }
}

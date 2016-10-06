using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Tridion.ContentManager.CoreService.Client;

namespace TridionContentFromExternalSource
{
   public class CoreService : IDisposable
    {
    private readonly SessionAwareCoreServiceClient _client;
    
    public CoreService()
    {
        _client = new SessionAwareCoreServiceClient("wsHttp");
        _client.Impersonate(WindowsIdentity.GetCurrent().Name);
        
        
    }
    public SessionAwareCoreServiceClient GetClient
    {
        get
        {
            return _client;
        }
    }
    public UserData GetCurrentUser()
    {
        return _client.GetCurrentUser();
    }
    public void SaveApplicationData(string subjectId, ApplicationData[] applicationData)
    {
        _client.SaveApplicationData(subjectId, applicationData);
    }
    public ApplicationData ReadApplicationData(string subjectId, string applicationId)
    {
        return _client.ReadApplicationData(subjectId, applicationId);
    }

    public XElement GetAllRootCategories(string publicationId)
    {
        CategoriesFilterData filterData = new CategoriesFilterData();
        filterData.IsRoot = true;
        return _client.GetListXml(publicationId, filterData);
       
    }
    public XElement GetAllComponentandSchema(string publicationId)
    {
        RepositoryItemsFilterData filterData = new RepositoryItemsFilterData();
        filterData.ItemTypes = new[]
	    {
		    //ItemType.Component,
		    ItemType.Schema
	    };
            filterData.Recursive = true;
        return _client.GetListXml(publicationId, filterData);
    }

    public void CheckInItemsWithUserComment(string componentId)
    {
        ReadOptions _ReadOption = new ReadOptions();
        ComponentData component =
            (ComponentData)_client.CheckOut(componentId, true, _ReadOption);
        component.Title = "Article Updated";
        
        component = (ComponentData)_client.Save(component, _ReadOption);
        _client.CheckIn(id: component.Id, removePermanentLock: true,
        userComment: "Title Updated", readBackOptions: _ReadOption);
    }

    public void CreateComponent(ComponentData comp)
    {
        ComponentData newComp = _client.Create(comp, new ReadOptions()) as ComponentData;
    }

    public void Dispose()
    {
        if (_client.State == CommunicationState.Faulted)
        {
            _client.Abort();
        }
        else
        {
            _client.Close();
            
        }
    } 
   }
}

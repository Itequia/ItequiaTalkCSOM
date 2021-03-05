using ItequiaTalkCSOM.Interfaces;
using ItequiaTalkCSOM.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItequiaTalkCSOM.Services
{
  public class CSOMService : ICSOMService
  {
    private readonly IConfiguration _configuration;
    public CSOMService(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    public async Task<ListInfo> GetListInfo(string url, string listName)
    {
      ClientContext context = GetSharepointContext(url);
      List list = context.Web.Lists.GetByTitle(listName);
      context.Load(list, l => l.Fields, l => l.Id, l => l.Title);
      await context.ExecuteQueryAsync();
      List<string> columnsInternalNames = new List<string>();
      foreach (var field in list.Fields)
      {
        columnsInternalNames.Add(field.InternalName);
      }
      ListInfo result = new ListInfo(list.Title, list.Id.ToString(), columnsInternalNames);
      return result;
    }

    public async Task<List<ItemInfo>> GetListItems(string url, string listName)
    {
      ClientContext context = GetSharepointContext(url);
      List list = context.Web.Lists.GetByTitle(listName);
      context.Load(list);
      await context.ExecuteQueryAsync();
      var items = list.GetItems(new CamlQuery());
      context.Load(items);
      await context.ExecuteQueryAsync();
      List<ItemInfo> result = new List<ItemInfo>();
      foreach (var item in items)
      {

        List<ItemInfoValue> values = new List<ItemInfoValue>();
        foreach (var field in item.FieldValues)
        {

          if (field.Value != null)
          {
            //tratar el campo tipo user o grupo
            if (field.Value is FieldUserValue)
            {
              ItemInfoValue toAdd = new ItemInfoValue(field.Key, null);
              FieldUserValue value = (FieldUserValue)item[field.Key];
              toAdd.Value = value.LookupId.ToString();
              values.Add(toAdd);
            }
            //tratar el campo multichoice
            else if (field.Value is string[])
            {
              ItemInfoValue toAdd = new ItemInfoValue(field.Key, null);
              string[] options = (string[])item[field.Key]; 
              foreach (var option in options)
              {
                toAdd.Value += option + "; ";
              }
              values.Add(toAdd);
            }
            else
            {
              ItemInfoValue toAdd = new ItemInfoValue(field.Key, field.Value.ToString());
              values.Add(toAdd);
            }
          }
          else
          {
            ItemInfoValue toAdd = new ItemInfoValue(field.Key, null);
            values.Add(toAdd);
          }

        }
        ItemInfo actual = new ItemInfo(values);
        result.Add(actual);

      }
      return result;
    }

    public async Task<List<ItemInfo>> GetListItemsInLargeList(string url, string listName)
    {
      ClientContext context = GetSharepointContext(url);
      List list = context.Web.Lists.GetByTitle(listName);
      context.Load(list);
      await context.ExecuteQueryAsync();
      List<ItemInfo> result = new List<ItemInfo>();
      ListItemCollectionPosition position = null;
      var page = 1;
      do
      {
        CamlQuery camlQuery = new CamlQuery();
        camlQuery.ViewXml = @"<View Scope='RecursiveAll'>
                                      <Query>
                                      </Query><RowLimit>5000</RowLimit>
                                  </View>";
        camlQuery.ListItemCollectionPosition = position;

        var listItems = list.GetItems(camlQuery);
        context.Load(listItems, a => a.ListItemCollectionPosition);
        await context.ExecuteQueryAsync();

        position = listItems.ListItemCollectionPosition;
        foreach (var item in listItems)
        {
          List<ItemInfoValue> values = new List<ItemInfoValue>();
          foreach (var field in item.FieldValues)
          {

            if (field.Value != null)
            {
              //tratar el campo tipo user o grupo
              if (field.Value is FieldUserValue)
              {
                ItemInfoValue toAdd = new ItemInfoValue(field.Key, null);
                FieldUserValue value = (FieldUserValue)item[field.Key];
                toAdd.Value = value.Email;
                values.Add(toAdd);
              }
              //tratar el campo multichoice
              else if (field.Value is string[])
              {
                ItemInfoValue toAdd = new ItemInfoValue(field.Key, null);
                string[] options = (string[])item[field.Key];
                foreach (var option in options)
                {
                  toAdd.Value += option + "; ";
                }
                values.Add(toAdd);
              }
              else
              {
                ItemInfoValue toAdd = new ItemInfoValue(field.Key, field.Value.ToString());
                values.Add(toAdd);
              }
            }
            else
            {
              ItemInfoValue toAdd = new ItemInfoValue(field.Key, null);
              values.Add(toAdd);
            }

          }
          ItemInfo actual = new ItemInfo(values);
          result.Add(actual);

        }

        page++;
      }
      while (position != null);
      return result;
    }

    public async Task<List<ItemInfo>> GetFilteredItems(string url, string listName)
    {
      try
      {
        ClientContext context = GetSharepointContext(url);
        List list = context.Web.Lists.GetByTitle(listName);
        context.Load(list);
        await context.ExecuteQueryAsync();
        List<ItemInfo> result = new List<ItemInfo>();
        CamlQuery camlQuery = new CamlQuery();
        camlQuery.ViewXml = @"<View>
                              <Query>
                                <Where>
                                  <And>
                                    <And>
                                      <Eq>
                                       <FieldRef Name='UserOrGroup' LookupId='TRUE'/>
                                        <Value Type='Lookup'>26</Value>
                                      </Eq>
                                      <Eq>
                                       <FieldRef Name='Choice'/>
                                        <Value Type='Text'>Ole Betis</Value>
                                      </Eq>
                                     </And>
                                      <Eq>
                                       <FieldRef Name='Sevilla'/>
                                        <Value Type='Text'>El sevilla es malo</Value>
                                      </Eq>
                                  </And>
                                    
                                </Where></Query><RowLimit>1</RowLimit></View>";

        var items = list.GetItems(camlQuery);
        context.Load(items);
        await context.ExecuteQueryAsync();


        foreach (var item in items)
        {

          List<ItemInfoValue> values = new List<ItemInfoValue>();
          foreach (var field in item.FieldValues)
          {

            if (field.Value != null)
            {
              //tratar el campo tipo user o grupo
              if (field.Value is FieldUserValue)
              {
                ItemInfoValue toAdd = new ItemInfoValue(field.Key, null);
                FieldUserValue value = (FieldUserValue)item[field.Key];
                toAdd.Value = value.Email;
                values.Add(toAdd);
              }
              //tratar el campo multichoice
              else if (field.Value is string[])
              {
                ItemInfoValue toAdd = new ItemInfoValue(field.Key, null);
                string[] options = (string[])item[field.Key];
                foreach (var option in options)
                {
                  toAdd.Value += option + "; ";
                }
                values.Add(toAdd);
              }
              else
              {
                ItemInfoValue toAdd = new ItemInfoValue(field.Key, field.Value.ToString());
                values.Add(toAdd);
              }
            }
            else
            {
              ItemInfoValue toAdd = new ItemInfoValue(field.Key, null);
              values.Add(toAdd);
            }

          }
          ItemInfo actual = new ItemInfo(values);
          result.Add(actual);

        }
        return result;
      }
      catch(Exception Ex)
      {
        return new List<ItemInfo>();
      }
    }

    public async Task CreateListItem (TalkListItem item)
    {
      ClientContext context = GetSharepointContext(item.Url);
      List list = context.Web.Lists.GetByTitle(item.ListName);
      context.Load(list);
      await context.ExecuteQueryAsync();
      ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();
      ListItem newItem = list.AddItem(itemCreateInfo);
      newItem["Title"] = item.Title;
      newItem["Description"] = item.Description;
      newItem["Number"] = item.Number;
      newItem["Boolean"] = item.Boolean;
      newItem["DateTime"] = item.Datetime;
      newItem["Choice"] = item.Choice;
      //User or group
      FieldUserValue userValue = new FieldUserValue();
      userValue.LookupId = item.UserOrGroup;
      newItem["UserOrGroup"] = userValue;
      newItem["Sevilla"] = item.Sevilla;
      newItem.Update();
      await context.ExecuteQueryAsync();
    }

    public async Task UpdateListItem(TalkListItem item, int id)
    {
      ClientContext context = GetSharepointContext(item.Url);
      List list = context.Web.Lists.GetByTitle(item.ListName);
      context.Load(list);
      await context.ExecuteQueryAsync();
      ListItem itemToUpdate = list.GetItemById(id);
      itemToUpdate["Title"] = item.Title;
      itemToUpdate["Description"] = item.Description;
      itemToUpdate["Number"] = item.Number;
      itemToUpdate["Boolean"] = item.Boolean;
      itemToUpdate["DateTime"] = item.Datetime;
      itemToUpdate["Choice"] = item.Choice;
      //User or group
      FieldUserValue userValue = new FieldUserValue();
      userValue.LookupId = item.UserOrGroup;
      itemToUpdate["UserOrGroup"] = userValue;
      itemToUpdate["Sevilla"] = item.Sevilla;
      itemToUpdate.Update();
      await context.ExecuteQueryAsync();
    }

    public async Task DeleteListItem(TalkListItem item, int id)
    {
      ClientContext context = GetSharepointContext(item.Url);
      List list = context.Web.Lists.GetByTitle(item.ListName);
      context.Load(list);
      await context.ExecuteQueryAsync();
      ListItem itemToDelete = list.GetItemById(id);
      itemToDelete.DeleteObject();
      await context.ExecuteQueryAsync();
    }

    public async Task<PermissionList> GetPermissions(string url, string listName, int id)
    {
      PermissionList result = new PermissionList();
      ClientContext context = GetSharepointContext(url);
      List list = context.Web.Lists.GetByTitle(listName);
      context.Load(list);
      await context.ExecuteQueryAsync();
      var item = list.GetItemById(id);
      context.Load(item, a => a.HasUniqueRoleAssignments);
      await context.ExecuteQueryAsync();
      if (item.HasUniqueRoleAssignments)
      {
        result.IsInheriting = false;
      }
      else
      {
        result.IsInheriting = true;
      }
      context.Load(item, a => a.RoleAssignments.Include(roleAsg => roleAsg.Member.LoginName,
                    roleAsg => roleAsg.RoleDefinitionBindings.Include(roleDef => roleDef.Name,
                    roleDef => roleDef.Description)));
      await context.ExecuteQueryAsync();
      foreach (var roleAsg in item.RoleAssignments)
      {
        Permission p = new Permission();
        p.GroupName = roleAsg.Member.LoginName;
        List<string> roles = new List<string>();
        foreach (var role in roleAsg.RoleDefinitionBindings)
        {
          p.Roles.Add(role.Name);
        }
        if ((p.Roles.Contains("Limited Access") && p.Roles.Count() == 1) || (p.GroupName.Contains("SharingLinks")))
        {

        }
        else
        {
          result.Permissions.Add(p);
        }

      }
      return result;
    }

    public async Task BreakInheritOrInheritAgain(int id, bool breakinherit, TalkListItem itemList)
    {
      ClientContext context = GetSharepointContext(itemList.Url);
      List list = context.Web.Lists.GetByTitle(itemList.ListName);
      context.Load(list);
      await context.ExecuteQueryAsync();
      var item = list.GetItemById(id);
      context.Load(item);
      await context.ExecuteQueryAsync();
      if (breakinherit)
      {
        item.BreakRoleInheritance(breakinherit, false);
        await context.ExecuteQueryAsync();
      }
      else
      {
        item.ResetRoleInheritance();
        await context.ExecuteQueryAsync();
      }


    }

    public async Task GivePermissions(PermissionAndRole permission)
    {
      ClientContext context = GetSharepointContext(permission.Url);
      List list = context.Web.Lists.GetByTitle(permission.ListName);
      context.Load(list);
      await context.ExecuteQueryAsync();
      var item = list.GetItemById(permission.ItemId);
      context.Load(item);
      await context.ExecuteQueryAsync();
      Principal user;
      if (permission.IsGroup)
      {
        user = context.Web.SiteGroups.GetById(permission.Id);
      }
      else
      {
        user = context.Web.SiteUsers.GetById(permission.Id);
        context.Load(user);
        await context.ExecuteQueryAsync();
      }
      RoleDefinition writeDefinition = context.Web.RoleDefinitions.GetByName(permission.roleDefinitionName);
      RoleDefinitionBindingCollection roleDefCollection = new RoleDefinitionBindingCollection(context);
      roleDefCollection.Add(writeDefinition);
      RoleAssignment newRoleAssignment = item.RoleAssignments.Add(user, roleDefCollection);
      await context.ExecuteQueryAsync();
    }

    public async Task DeletePermissions(PermissionAndRole permission)
    {
      ClientContext context = GetSharepointContext(permission.Url);
      List list = context.Web.Lists.GetByTitle(permission.ListName);
      context.Load(list);
      await context.ExecuteQueryAsync();
      var item = list.GetItemById(permission.ItemId);
      context.Load(item);
      await context.ExecuteQueryAsync();
      Principal user;
      if (permission.IsGroup)
      {
        user = context.Web.SiteGroups.GetById(permission.Id);
      }
      else
      {
        user = context.Web.SiteUsers.GetById(permission.Id);
      }
      context.Load(user);
      await context.ExecuteQueryAsync();
      item.RoleAssignments.GetByPrincipal(user).DeleteObject();
      await context.ExecuteQueryAsync();
    }

    private ClientContext GetSharepointContext(string spHost)
    {
      ClientContext clientContext = new ClientContext(spHost)
      {
        Credentials = new SharePointOnlineCredentials(_configuration["Sharepoint:AdminUser"],
                _configuration["Sharepoint:AdminPassword"])
      };

      return clientContext;
    }
  }
}

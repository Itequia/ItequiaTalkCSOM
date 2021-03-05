using ItequiaTalkCSOM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItequiaTalkCSOM.Interfaces
{
  public interface ICSOMService
  {
    Task<ListInfo> GetListInfo(string url, string listName);
    Task<List<ItemInfo>> GetListItems(string url, string listName);
    Task<List<ItemInfo>> GetListItemsInLargeList(string url, string listName);
    Task<List<ItemInfo>> GetFilteredItems(string url, string listName);
    Task CreateListItem(TalkListItem item);
    Task UpdateListItem(TalkListItem item, int id);
    Task DeleteListItem(TalkListItem item, int id);
    Task<PermissionList> GetPermissions(string url, string listName, int id);
    Task BreakInheritOrInheritAgain(int id, bool breakinherit, TalkListItem itemList);
    Task GivePermissions(PermissionAndRole permission);
    Task DeletePermissions(PermissionAndRole permission);
  }
}

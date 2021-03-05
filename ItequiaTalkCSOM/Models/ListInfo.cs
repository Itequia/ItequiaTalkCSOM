using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItequiaTalkCSOM.Models
{
  public class ListInfo
  {
    public ListInfo(string listName, string id, List<string> colummns)
    {
      ListName = listName;
      Id = id;
      Columns = colummns;
    }
    public string ListName { get; set; }
    public string Id { get; set; }
    public List<string> Columns { get; set; }
  }
}

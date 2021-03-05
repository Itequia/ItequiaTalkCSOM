using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItequiaTalkCSOM.Models
{
  public class PermissionAndRole
  {
    public string Url { get; set; }
    public string ListName { get; set; }
    public int ItemId { get; set; }
    public bool IsGroup { get; set; }
    public int Id { get; set; }
    public string roleDefinitionName { get; set; }
  }
}

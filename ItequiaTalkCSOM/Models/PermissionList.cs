using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItequiaTalkCSOM.Models
{
  public class PermissionList
  {
    public PermissionList()
    {
      Permissions = new List<Permission>();
    }
    public bool IsInheriting { get; set; }
    public List<Permission> Permissions { get; set; }
  }
}

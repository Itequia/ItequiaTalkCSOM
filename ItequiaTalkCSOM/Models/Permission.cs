using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItequiaTalkCSOM.Models
{
  public class Permission
  {
    public Permission()
    {
      Roles = new List<string>();
    }
    public string GroupName { get; set; }
    public List<string> Roles { get; set; }
  }
}

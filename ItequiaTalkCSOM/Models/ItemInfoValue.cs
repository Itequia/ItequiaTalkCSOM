using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItequiaTalkCSOM.Models
{
  public class ItemInfoValue
  {
    public ItemInfoValue(string column, string value)
    {
      Column = column;
      Value = value;
    }
    public string Column { get; set; }
    public string Value { get; set; }
  }
}

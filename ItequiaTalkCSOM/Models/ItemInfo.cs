using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItequiaTalkCSOM.Models
{
  public class ItemInfo
  {
    public ItemInfo (List<ItemInfoValue> values)
    {
      Values = values;
    }
    public List<ItemInfoValue> Values { get; set; }
  }
}

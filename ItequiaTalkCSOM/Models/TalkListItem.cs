using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItequiaTalkCSOM.Models
{
  public class TalkListItem
  {
    public string Url { get; set; }
    public string ListName { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Number { get; set; }
    public bool Boolean { get; set; }
    public int UserOrGroup { get; set; }
    public DateTime Datetime { get; set; }
    public string Choice { get; set; }
    public string[] Sevilla { get; set; }
   }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.Domain.Items
{
  public class Person : Item
  {
    public string Function { get; set; }
    public List<SocialMediaUrl> SocialMediaUrls { get; set; }
  }
}

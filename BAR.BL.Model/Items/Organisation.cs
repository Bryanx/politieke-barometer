using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.Domain.Items
{
  public class Organisation : Item
  {
    public string Site { get; set; }
    public List<SocialMediaName> SocialMediaUrls { get; set; }
  }
}

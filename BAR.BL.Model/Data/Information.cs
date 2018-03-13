using BAR.BL.Domain.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.Domain.Data
{
  public class Information
  {
    public int InformationId { get; set; }
    public Item Item { get; set; }
    public Source Source { get; set; }
    public int UserSourceId { get; set; }
    public IDictionary<Property, ICollection<PropertyValue>> Properties { get; set; }
  }
}

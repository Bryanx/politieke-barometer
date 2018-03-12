using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.Domain.Data
{
  public class PropertyValue
  {
    public int PropertyValueId { get; set; }
    public Property Property { get; set; }
    public Information Information { get; set; }
    public string Value { get; set; }
    public double Confidence { get; set; }
  }
}

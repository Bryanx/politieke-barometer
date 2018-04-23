using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.Domain.Users
{
  public class Area
  {
    public int AreaId { get; set; }
    public string Residence { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }

    public ICollection<User> Users { get; set; }
  }
}

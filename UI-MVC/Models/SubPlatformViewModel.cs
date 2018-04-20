using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAR.UI.MVC.Models
{
  public class SubPlatformViewModel : BaseViewModel
  {
    public IEnumerable<SubPlatformDTO> SubPlatforms { get; set; }
  }
}
using BAR.BL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.Managers
{
  public interface ISubplatformManager
  {
    //Read
    SubPlatform GetSubPlatform(string subplatformName);
  }
}

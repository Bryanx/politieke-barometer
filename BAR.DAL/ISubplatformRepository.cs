using BAR.BL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.DAL
{
  public interface ISubplatformRepository
  {
    SubPlatform ReadSubPlatform(string subplatformName);
  }
}

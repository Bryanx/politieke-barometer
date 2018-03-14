using BAR.BL.Domain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.DAL
{
  public interface IInformationRepository
  {
    int ReadNumberInfo(int itemId, DateTime since);
    IEnumerable<Information> ReadAllInfoForId(int itemId);
  }
}

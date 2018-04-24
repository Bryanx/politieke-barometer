using BAR.BL.Domain.Data;
using BAR.BL.Domain.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.Managers
{
	public interface IDataManager
	{
		int GetNumberInfo(int itemId, DateTime since);
		IEnumerable<Information> GetAllInformationForId(int itemId);
    bool SynchronizeData(string json);
    SynchronizeAudit GetLastAudit();
    SynchronizeAudit AddAudit(DateTime timestamp, bool succes);
    SynchronizeAudit GetAudit(int synchronizeAuditId);
    SynchronizeAudit ChangeAudit(int synchronizeAuditId);
    bool IsJsonEmpty(string json);
    IEnumerable<Source> GetAllSources();
  }
}
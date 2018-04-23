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
		//Informations
		IEnumerable<Information> GetInformationsForItemid(int itemId);
		IEnumerable<Information> GetInformationsForWidgetid(int widgetId);
		IEnumerable<Information> GetInformationsWithTimestamp(int widgetId);

		int GetNumberInfo(int itemId, DateTime since);
		IDictionary<string, double> GetNumberOfMentionsForItem(int itemId);

		//Items
		IEnumerable<Item> SynchronizeData(string json);

		//Audits
		SynchronizeAudit GetLastAudit();
		SynchronizeAudit AddAudit(DateTime timestamp, bool succes);
		SynchronizeAudit GetAudit(int synchronizeAuditId);
		SynchronizeAudit ChangeAudit(int synchronizeAuditId);	
	}
}
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
		Information GetInformationWithPropvals(int infoId);
		IEnumerable<Information> GetInformationsForItemid(int itemId);
		IEnumerable<Information> GetInformationsForWidgetid(int widgetId);
		IEnumerable<Information> GetInformationsWithTimestamp(int widgetId);
		
		PropertyValue GetPropvalWithProperty(int propvalId);

		int GetNumberInfo(int itemId, DateTime since);
		IDictionary<string, double> GetNumberOfMentionsForItem(int itemId, int widgetId);
		IDictionary<string, List<PropertyValue>> GetPropvaluesForWidget(int itemid, int widgetId);

		//Items
		IEnumerable<Item> SynchronizeData(string json);

		//Audits
		SynchronizeAudit GetLastAudit();
		SynchronizeAudit AddAudit(DateTime timestamp, bool succes);
		SynchronizeAudit GetAudit(int synchronizeAuditId);
		SynchronizeAudit ChangeAudit(int synchronizeAuditId);	
	}
}
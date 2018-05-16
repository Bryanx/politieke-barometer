using BAR.BL.Domain.Data;
using BAR.BL.Domain.Widgets;
using BAR.BL.Domain.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAR.BL.Domain.Users;

namespace BAR.BL.Managers
{
	public interface IDataManager
	{
		//Informations
		IEnumerable<Information> GetInformationsForItemid(int itemId);
		IEnumerable<Information> GetInformationsForWidgetid(int widgetId);
		IEnumerable<Information> GetInformationsWithTimestamp(int widgetId);
		IEnumerable<Information> GetInformationsWithAllInfoForItem(int itemId);

		int GetNumberInfo(int itemId, DateTime since);

		WidgetData GetNumberOfMentionsForItem(int itemId, int widgetId, string dateFormat, DateTime? startDate = null);
		WidgetData GetPropvaluesForWidget(int itemid, int widgetId, string proptag, DateTime? startDate = null);
		WidgetData GetUserActivitiesData(ActivityType type, DateTime? timestamp = null);
		WidgetData GetGeoLocationData(DateTime? timestamp = null);

		//Items
		bool SynchronizeData(string json);

		//Audits
		SynchronizeAudit AddAudit(DateTime timestamp, bool succes);
		SynchronizeAudit GetAudit(int synchronizeAuditId);
		SynchronizeAudit GetLastAudit();
		SynchronizeAudit ChangeAudit(int synchronizeAuditId);

		//Sources		
		IEnumerable<Source> GetAllSources();
		Source AddSource(string name, string site);
		void RemoveSource(string sourceId);

        //DataSources
        IEnumerable<DataSource> GetAllDataSources();
        DataSource GetDataSource(int dataSourceId);
        void RemoveDataSource(int dataSourceId);
        void ChangeDataSource(int dataSourceId);
		//Other
		bool IsJsonEmpty(string json);
		IEnumerable<string> GetUrlsForItem(int itemId);

		//Sources
		Source GetSource(string sourceName);
	}
}

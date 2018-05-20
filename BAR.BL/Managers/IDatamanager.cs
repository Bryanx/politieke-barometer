using BAR.BL.Domain.Data;
using BAR.BL.Domain.Widgets;
using System;
using System.Collections.Generic;
using BAR.BL.Domain.Users;

namespace BAR.BL.Managers
{
	public interface IDataManager
	{
		//Informations
		IEnumerable<Information> GetInformationsForItemid(int itemId);
		IEnumerable<Information> GetInformationsWithAllInfoForItem(int itemId);

		//Widgetdatas
		WidgetData GetNumberOfMentionsForItem(int itemId, int widgetId, string dateFormat, DateTime? startDate = null);
		WidgetData GetPropvaluesForWidget(int itemid, int widgetId, string proptag, DateTime? startDate = null);
		WidgetData GetUserActivitiesData(ActivityType type, DateTime? timestamp = null);
		WidgetData GetGeoLocationData(DateTime? timestamp = null);
		WidgetData GetOrganisationData(int itemId, string dateFormat, DateTime? timestamp = null);

		//Audits
		SynchronizeAudit AddAudit(DateTime timestamp, bool succes);
		SynchronizeAudit GetAudit(int synchronizeAuditId);
		SynchronizeAudit GetLastAudit();
		SynchronizeAudit ChangeAudit(int synchronizeAuditId);

		//Sources
		Source GetSource(string sourceName);
		IEnumerable<Source> GetAllSources();
		Source AddSource(string name, string site);
		void RemoveSource(string sourceId);

        //DataSources
        IEnumerable<DataSource> GetAllDataSources();
        DataSource GetDataSource(int dataSourceId);
        void RemoveDataSource(int dataSourceId);
       void ChangeDataSource(int dataSourceId, int interval);
               int GetInterval(int dataSourceId);
               string GetStartTimer(int dataSourceId);
               int ChangeInterval(int dataSourceId, int interval);
               string ChangeStartTimer(int dataSourceId, string startTimer);
               DateTime ChangeLastTimeChecked(int dataSourceId, DateTime date);


		//Others
		bool SynchronizeData(string json);
		bool IsJsonEmpty(string json);
		IEnumerable<string> GetUrlsForItem(int itemId);
	}
}

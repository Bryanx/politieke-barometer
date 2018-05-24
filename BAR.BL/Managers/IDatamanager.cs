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
		SynchronizeAudit GetLastAudit();
		SynchronizeAudit AddAudit(DateTime timestamp, bool succes);
		SynchronizeAudit GetAudit(int synchronizeAuditId);
		SynchronizeAudit ChangeAudit(int synchronizeAuditId);

		//Sources
		Source GetSource(string sourceName);
		IEnumerable<Source> GetAllSources();
		Source AddSource(string name, string site);
		void RemoveSource(string sourceId);

		//DataSources
		DataSource GetDataSource(int dataSourceId);
		IEnumerable<DataSource> GetAllDataSources();
		DataSource ChangeTimerInterval(int dataSourceId, int interval);
		DataSource ChangeStartTimer(int dataSourceId, string startTimer);
		DataSource ChangeLastTimeCheckedTime(int dataSourceId, DateTime date);
		void RemoveDataSource(int dataSourceId);

		//Others
		bool SynchronizeData(string json);
		bool IsJsonEmpty(string json);
		IEnumerable<string> GetUrlsForItem(int itemId);
	}
}

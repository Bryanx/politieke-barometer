using BAR.BL.Domain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.DAL
{
	public interface IDataRepository
	{
		//Read
		Information ReadInformationWitlAllInfo(int informationId);
		IEnumerable<Information> ReadAllInformations();
		IEnumerable<Information> ReadInformationsForItemid(int itemId);
		IEnumerable<Information> ReadInformationsWithAllInfoForItem(int itemId);
		Property ReadProperty(string propertyName);
		List<Property> ReadAllProperties();
		Source ReadSource(string sourceName);
		Source ReadSource(int sourceId);
		List<Source> ReadAllSources();
		SynchronizeAudit ReadLastAudit();
		SynchronizeAudit ReadAudit(int auditId);
        DataSource ReadDataSource(int dataSourceId);
		IEnumerable<DataSource> ReadAllDataSources();
		int ReadInterval(int dataSourceId);
        string ReadStartTime(int dataSourceId);

		//Create
		int CreateInformations(IEnumerable<Information> infos);
		int CreateAudit(SynchronizeAudit audit);
		int CreateSource(Source source);
        int CreateDataSource(DataSource dataSource);

		//Update
		int UpdateInformations(IEnumerable<Information> infos);
		int UpdateAudit(SynchronizeAudit audit);
        int UpdateDataSource(DataSource dataSource, int interval);
        int UpdateInterval(int dataSourceId, int interval);
        int UpdateStartTime(int dataSourceId, string setTime);
        int UpdateLastTimeChecked(int dataSourceId, DateTime date);

		//Delete
		int DeleteSource(Source source);
        int DeleteDataSource(DataSource dataSource);
    }
}

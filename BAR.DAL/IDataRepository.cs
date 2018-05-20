using BAR.BL.Domain.Data;
using System.Collections.Generic;

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

		//Create
		int CreateInformations(IEnumerable<Information> infos);
		int CreateAudit(SynchronizeAudit audit);
		int CreateSource(Source source);
        int CreateDataSource(DataSource dataSource);

		//Update
		int UpdateInformations(IEnumerable<Information> infos);
		int UpdateAudit(SynchronizeAudit audit);
        int UpdateDataSource(DataSource dataSource);

		//Delete
		int DeleteSource(Source source);
        int DeleteDataSource(DataSource dataSource);
    }
}

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
		int ReadNumberInfo(int itemId, DateTime since);
		Information ReadInformation(int informationid);
		Information ReadInformationWitlAllInfo(int informationId);
		IEnumerable<Information> ReadInformationsWithAllInfoForItem(int itemId);
		IEnumerable<Information> ReadAllInformations();
		IEnumerable<Information> ReadInformationsForItemid(int itemId);
		Property ReadProperty(string propertyName);
		List<Property> ReadAllProperties();
		Source ReadSource(string sourceName);
		Source ReadSource(int sourceId);
		List<Source> ReadAllSources();
		SynchronizeAudit ReadLastAudit();
		SynchronizeAudit ReadAudit(int auditId);
        IEnumerable<DataSource> ReadAllDataSources();
        DataSource ReadDataSource(int dataSourceId);
        int ReadInterval(int dataSourceId);
        string ReadStartTime(int dataSourceId);

        //Create
        int CreateInformations(IEnumerable<Information> infos);
		int CreateAudit(SynchronizeAudit audit);
		int CreateSource(Source source);
        int CreateDataSource(DataSource dataSource);

		//Update
		int UpdateInformation(Information info);
		int UpdateInformations(IEnumerable<Information> infos);
		int UpdateAudit(SynchronizeAudit audit);
        int UpdateDataSource(DataSource dataSource);
        int UpdateInterval(int dataSourceId, int interval);
        int UpdateStartTime(int dataSourceId, string setTime);

        //Delete
        int DeleteInformation(int infoId);
		int DeleteInformations(IEnumerable<int> infoIds);
		int DeleteSource(Source source);
        int DeleteDataSource(DataSource dataSource);
    }
}

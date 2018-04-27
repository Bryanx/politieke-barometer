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
		Information ReadInformationWithPropValues(int informationId);
		IEnumerable<Information> ReadAllInformations();
		IEnumerable<Information> ReadInformationsForItemid(int itemId);
		IEnumerable<Information> ReadInformationsForDate(int itemId, DateTime since);
    Property ReadProperty(string propertyName);
    Source ReadSource(string sourceName);
    Source ReadSource(int sourceId);
    SynchronizeAudit ReadLastAudit();
    SynchronizeAudit ReadAudit(int synchronizeAuditId);
    IEnumerable<Property> ReadAllProperties();
    IEnumerable<Source> ReadAllSources();
		IEnumerable<Information> ReadInformationsWithAllInfoForItem(int itemId);

		//Create
		int CreateInformations(List<Information> infos);
    int CreateAudit(SynchronizeAudit synchronizeAudit);
    int CreateSource(Source source);

    //Update
    int UpdateInformation(Information info);
		int UpdateInformations(IEnumerable<Information> infos);
    int UpdateAudit(SynchronizeAudit synchronizeAudit);

    //Delete
    int DeleteInformation(int infoId);
		int DeleteInformations(IEnumerable<int> infoIds);
    int DeleteSource(Source source);
	}
}

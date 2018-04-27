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
		IEnumerable<Information> ReadAllInformations();
		IEnumerable<Information> ReadInformationsForItemid(int itemId);
		IEnumerable<Information> ReadInformationsForDate(int itemId, DateTime since);
		IEnumerable<Information> ReadInformationsWithAllInfoForItem(int itemId);
		Property ReadProperty(string propertyName);
		Source ReadSource(string sourceName);
		SynchronizeAudit ReadLastAudit();
		SynchronizeAudit ReadAudit(int synchronizeAuditId);
		IEnumerable<Property> ReadAllProperties();
		IEnumerable<Source> ReadAllSources();

		//Create
		int CreateInformations(List<Information> infos);
		int CreateAudit(SynchronizeAudit synchronizeAudit);

		//Update
		int UpdateInformation(Information info);
		int UpdateInformations(IEnumerable<Information> infos);
		int UpdateAudit(SynchronizeAudit synchronizeAudit);

		//Delete
		int DeleteInformation(int infoId);
		int DeleteInformations(IEnumerable<int> infoIds);
	}
}

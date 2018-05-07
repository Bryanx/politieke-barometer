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
		
		//Create
		int CreateInformations(IEnumerable<Information> infos);
		int CreateAudit(SynchronizeAudit audit);
		int CreateSource(Source source);

		//Update
		int UpdateInformation(Information info);
		int UpdateInformations(IEnumerable<Information> infos);
		int UpdateAudit(SynchronizeAudit audit);

		//Delete
		int DeleteInformation(int infoId);
		int DeleteInformations(IEnumerable<int> infoIds);
		int DeleteSource(Source source);
	}
}

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
		IEnumerable<Information> ReadAllInfoForId(int itemId);
		IEnumerable<Information> ReadInformationsForDate(int itemId, DateTime since);
    Property ReadProperty(string propertyName);
    Source ReadSource(string sourceName);

		//Create
		int CreateInformations(List<Information> infos);

		//Update
		int UpdateInformation(Information info);
		int UpdateInformations(IEnumerable<Information> infos);

		//Delete
		int DeleteInformation(int infoId);
		int DeleteInformations(IEnumerable<int> infoIds);
	}
}

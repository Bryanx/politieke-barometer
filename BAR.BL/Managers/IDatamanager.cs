﻿using BAR.BL.Domain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.Managers
{
	public interface IDataManager
	{
		int GetNumberInfo(int itemId, DateTime since);
		IEnumerable<Information> GetAllInformationForId(int itemId);
    bool SynchronizeData(string json);

  }
}
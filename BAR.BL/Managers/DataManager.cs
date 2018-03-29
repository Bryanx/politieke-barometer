using System;
using System.Collections.Generic;
using BAR.BL.Domain.Data;
using BAR.DAL;

namespace BAR.BL.Managers
{
	/// <summary>
	/// Responsable for working with 
	/// data we received from TextGain.
	/// </summary>
	public class DataManager : IDataManager
	{
		private IInformationRepository infoRepo;
		private UnitOfWorkManager uowManager;

		/// <summary>
		/// When unit of work is present, it will effect
		/// initRepo-method. (see documentation of initRepo)
		/// </summary>
		public DataManager(UnitOfWorkManager uowManager = null)
		{
			this.uowManager = uowManager;
		}

		/// <summary>
		/// Gets the number of informations of a specific given item.
		/// </summary
		public int GetNumberInfo(int itemId, DateTime since)
		{
			InitRepo();			
			return infoRepo.ReadNumberInfo(itemId, since);
		}
	
		/// <summary>
		/// Returns a list of informations for
		/// a specific item id.
		/// </summary>
		public IEnumerable<Information> getAllInformationForId(int itemId)
		{
			InitRepo();
			return infoRepo.ReadAllInfoForId(itemId);
		}

		/// <summary>
		/// Determines if the repo needs a unit of work
		/// if the unitOfWorkManager is present.
		/// </summary>
		private void InitRepo()
		{
			if (uowManager == null) infoRepo = new InformationRepository();
			else infoRepo = new InformationRepository(uowManager.UnitOfWork);
		}

	}
}



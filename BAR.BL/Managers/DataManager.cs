using System;
using BAR.DAL;

namespace BAR.BL.Managers
{
    /// <summary>
    /// Responsable for working with 
    /// data we received from TextGain.
    /// </summary>
    public class DataManager : IDataManager
    {
		private InformationRepository infoRepo;
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
        /// Gets the number of informations of a specific given item
        /// form since till now.
        /// </summary
        public int GetNumberInfo(int itemId, DateTime since)
        {
            IInformationRepository infoRepo = new InformationRepository();
            return infoRepo.ReadNumberInfo(itemId, since);
        }

		/// <summary>
		/// Determines if the repo needs a unit of work
		/// if the unitOfWorkManager is present
		/// </summary>
		public void InitRepo()
		{
			if (uowManager == null) infoRepo = new InformationRepository();
			else infoRepo = new InformationRepository(uowManager.UnitOfWork);
		}
	}
}



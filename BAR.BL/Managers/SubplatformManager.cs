using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAR.BL.Domain;
using BAR.DAL;

namespace BAR.BL.Managers
{
	/// <summary>
	/// Responsible for managing SubPlatform
	/// </summary>
	class SubplatformManager : ISubplatformManager
	{
		private ISubplatformRepository platformRepo;
		private UnitOfWorkManager uowManager;

		public SubPlatform getSubPlatform(string subplatformName)
		{
			SubPlatform subPlatform = platformRepo.ReadSubPlatform(subplatformName);
			return subPlatform;
		}

		/// <summary>
		/// When unit of work is present, it will effect
		/// initRepo-method. (see documentation of initRepo)
		/// </summary>
		public SubplatformManager(UnitOfWorkManager uowManager = null)
		{
			this.uowManager = uowManager;
		}

    /// <summary>
		/// Determines if the repo needs a unit of work
		/// if the unitOfWorkManager is present
		/// </summary>
		private void InitRepo()
    {
      if (uowManager == null) platformRepo = new SubplatformRepository();
      else platformRepo = new SubplatformRepository(uowManager.UnitOfWork);
    }
  }
}

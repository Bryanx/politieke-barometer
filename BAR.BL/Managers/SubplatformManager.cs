using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAR.BL.Domain;
using BAR.DAL;
using BAR.BL.Domain.Core;

namespace BAR.BL.Managers
{
	/// <summary>
	/// Responsible for managing SubPlatform
	/// </summary>
	public class SubplatformManager : ISubplatformManager
	{
		private ISubplatformRepository platformRepo;
		private UnitOfWorkManager uowManager;

		/// <summary>
		/// When unit of work is present, it will effect
		/// initRepo-method. (see documentation of initRepo)
		/// </summary>
		public SubplatformManager(UnitOfWorkManager uowManager = null)
		{
			this.uowManager = uowManager;
		}

		/// <summary>
		/// Makes an subplatform and persist that to the database
		/// </summary>
		public SubPlatform CreateSubplatform(string name)
		{
			InitRepo();

			//Make subplatform
			SubPlatform platform = new SubPlatform()
			{
				Name = name,
				CreationDate = DateTime.Now,
				NumberOfUsers = 0,
				Questions = new List<Question>()
			};

			//Create subplatform
			platformRepo.CreateSubplatform(platform);

			return platform;
		}

		/// <summary>
		/// Returns a speicif subplatform
		/// </summary>
		public SubPlatform GetSubPlatform(string subplatformName)
		{
			InitRepo();
			return platformRepo.ReadSubPlatform(subplatformName);
		}

		/// <summary>
		/// Returns all subplatforms
		/// </summary>
		public IEnumerable<SubPlatform> GetSubplatforms()
		{
			InitRepo();
			return platformRepo.ReadSubPlatforms().AsEnumerable();
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

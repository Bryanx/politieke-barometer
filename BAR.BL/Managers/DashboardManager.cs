using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAR.BL.Domain.Widgets;
using BAR.DAL;

namespace BAR.BL.Managers
{
	/// <summary>
	/// Responsable for managing widgets
	/// and their dashboards 
	/// </summary>
	public class DashboardManager : IDashboardManager
	{
		private IDashboardRepository infoRepo;
		private UnitOfWorkManager uowManager;

		/// <summary>
		/// When unit of work is present, it will effect
		/// initRepo-method. (see documentation of initRepo)
		/// </summary>
		public DashboardManager(UnitOfWorkManager uowManager = null)
		{
			this.uowManager = uowManager;
		}

		/// <summary>
		/// Gives back a list of
		/// </summary>
		public IEnumerable<Widget> GetWidgets(int dashboardId)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Determines if the repo needs a unit of work
		/// if the unitOfWorkManager is present.
		/// </summary>
		private void InitRepo()
		{
			if (uowManager == null) infoRepo = new DashboardRepository();
			else infoRepo = new DashboardRepository(uowManager.UnitOfWork);
		}
	}
}

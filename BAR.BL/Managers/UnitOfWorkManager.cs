using BAR.DAL;

namespace BAR.BL.Managers
{
	public class UnitOfWorkManager
	{
		/// <summary>
		/// Private member to store DAL.UnitOfWork.
        /// Which stores the instance of the Context class.
		/// </summary>
		private UnitOfWork uof;

		internal UnitOfWork UnitOfWork
		{
			get
			{   
				//This construction is used to make sure there won't be any extra UnitOfWork instances
				if (uof == null) uof = new UnitOfWork();
				return uof;
			}
		}

		/// <summary>
		/// Commits changes to the database
		/// when unit of work is used.
		/// </summary>
		public void Save()
		{
			UnitOfWork.CommitChanges();
		}
	}
}

using BAR.DAL.EF;

namespace BAR.DAL
{
	/// <summary>
	/// The UnitOfWork class is used to create a unique context object 
	/// that is placed over the instances of the repositories.
	/// there is a private member and an internal property context that
	/// we can call from anywhere within the same project.
	/// We give to the instance of the context that our context is inside a UnitOfWork class,
	/// so the DelaySave parameter of the context class will be 'true'.
	/// </summary>
	public class UnitOfWork
	{
		private BarometerDbContext ctx;

		internal BarometerDbContext Context
		{
			get
			{
				if (ctx == null) ctx = new BarometerDbContext(true);
				return ctx;
			}
		}

		/// <summary>
		/// This method will persist all changed domain objects to the database.
		/// </summary>
		public void CommitChanges()
		{
			ctx.CommitChanges();
		}
	}
}

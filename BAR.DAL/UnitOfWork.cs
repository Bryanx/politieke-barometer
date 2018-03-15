using BAR.DAL.EF;

namespace BAR.DAL
{
	/// <summary>
	/// Deze klasse wordt gebruikt om over instanties van de repositories heen
	/// een uniek context object te creëeren. Dit doen we door een private member bij te houden
	/// en een 'internal' property Context die je van buiten af binnen hetzelfde project kan oproepen.
	/// Bij de instantatie van de context geven we aan onze context mee dat deze leeft
	/// binnen een UnitOfWork klasse, zodanig dat de private member 'DelaySave' van de context klasse
	/// op true komt te staan.
    /// 
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
		/// Deze methode zorgt ervoor dat alle tot hier toe aangepaste domein objecten
		/// worden gepersisteert naar de databank.
        /// 
        /// This method will persist all changed domain objects to the database.
		/// </summary>
		public void CommitChanges()
		{
			ctx.CommitChanges();
		}
	}
}

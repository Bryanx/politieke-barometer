using BAR.BL.Domain.Data;
using BAR.BL.Domain.Items;
using BAR.BL.Domain.Users;
using System;
using System.Data.Entity;

namespace BAR.DAL.EF
{
	public class BarometerDbContext : DbContext
	{
		/// <summary>
		/// DelaySave zorgt ervoor dat de gewone SaveChanges niet uitgevoerd wordt
		/// indien deze boolean op true staat. 
		/// Wordt gebruikt in het kader van het unit-of-work pattern. 
		/// </summary>
		private readonly bool delaySave;

		/// <summary>
		/// Constructor of PizzaDbContext, loads the connectionstring based on de
		/// configuration key "PizzaDB"
		/// </summary>
		/// Indicates is this context class operates under a Unit-Of-Work pattern. If so
		/// SaveChanges will not be executed on the database, instead you'll need to use
		/// CommitChanges (but that method is not public available)
		/// By default, unitOfWorkPresent will be set to false
		/// </param>
		public BarometerDbContext(bool useUOF = false) : base("BAR_DB")
		{
			delaySave = useUOF;
		}

		//Data package
		public DbSet<Information> Informations { get; set; }
		public DbSet<Property> Properties { get; set; }

		//User package
		public DbSet<User> Users { get; set; }
		public DbSet<Subscription> Subscriptions { get; set; }

		//Item package
		public DbSet<Item> Items { get; set; }

		/// <summary>
		/// We overridden de standaard SaveChanges implementatie, omdat we een extra
		/// controle willen inbouwen. Indien de boolean 'delaySave' op true staat, willen
		/// we niet dat we ineens gegevens gaan bewaren, maar mag dit commando enkel en alleen
		/// maar doorgevoerd worden vanuit de UnitOfWork klasse.
		/// </summary>
		public override int SaveChanges()
		{
			if (delaySave) return -1;
			return base.SaveChanges();
		}

		/// <summary>
		/// Om onze wijzigingen toch te kunnen bewaren, voorzien we de CommitChanges methode.
		/// Deze gaat de gegevens effectief in de databank bewaren indien de member 'delaySave'
		/// effectief op true staat.
		/// Indien je deze oproept zonder dat die boolean op true staat, wordt er een Exception gegooid
		/// </summary>
		internal int CommitChanges()
		{
			if (delaySave)
			{
				return base.SaveChanges();
			}
			throw new InvalidOperationException("No UnitOfWork present, use SaveChanges instead");
		}
	}
}

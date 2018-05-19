using BAR.BL.Domain;
using BAR.BL.Domain.Core;
using BAR.BL.Domain.Data;
using BAR.BL.Domain.Items;
using BAR.BL.Domain.Users;
using BAR.BL.Domain.Widgets;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;

namespace BAR.DAL.EF
{
	public class BarometerDbContext : IdentityDbContext<User>
	{
		/// <summary>
		/// DelaySave makes sure that the SaveChanges method will not be executed
		/// if this boolean is true.
		/// Will be used with the UnitOfWork pattern.
		/// </summary>
		private readonly bool delaySave;

		/// <summary>
		/// Indicates if this context class operates under a Unit-Of-Work pattern. If so,
		/// SaveChanges will not be executed on the database, instead you'll need to use
		/// CommitChanges (but that method is not publicly available)
		/// By default, unitOfWorkPresent will be set to false
		/// </summary>
		public BarometerDbContext(bool useUOF = false) : base("BAR_DB")
		{
			delaySave = useUOF;
		}

		//Data package
		public DbSet<Source> Sources { get; set; }
		public DbSet<DataSource> DataSources { get; set; }
		public DbSet<Information> Informations { get; set; }
		public DbSet<Property> Properties { get; set; }
		public DbSet<SynchronizeAudit> SynchronizeAudits { get; set; }

		//User package
		public DbSet<Subscription> Subscriptions { get; set; }
		public DbSet<Alert> Alerts { get; set; }
		public DbSet<Area> Areas { get; set; }

		//Item package
		public DbSet<Item> Items { get; set; }

		//Widget package
		public DbSet<Dashboard> Dashboards { get; set; }
		public DbSet<Widget> Widgets { get; set; }
		public DbSet<WidgetData> WidgetDatas { get; set; }

		//Core
		public DbSet<SubPlatform> SubPlatforms { get; set; }
		public DbSet<Question> Questions { get; set; }
		public DbSet<UserActivity> UserActivities { get; set; }

		//Static method for creating and returning a BarometerDbContext (used as in Singleton pattern)
		public static BarometerDbContext Create()
		{
			return new BarometerDbContext();
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			//Very important! This class will call the same method from the base class
			//which needs to be executed
			base.OnModelCreating(modelBuilder);

			//Change default names of Identity tables
			modelBuilder.Entity<User>().ToTable("Users");
			modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles");
			modelBuilder.Entity<IdentityUserLogin>().ToTable("SocialLogins");
			modelBuilder.Entity<IdentityRole>().ToTable("Roles");
			modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims");
		}

		/// <summary>
		/// We override the standard SaveChanges implementation, because we want to build in more control.
		/// If the boolean 'DelaySave' is true we want to save the changes through the UnitOfWork class.
		/// </summary>
		public override int SaveChanges()
		{
			if (delaySave) return -1;
			else return base.SaveChanges();
		}

		/// <summary>
		/// This method is used to save changes. It will save all changes to the database if DelaySave is true.
		/// effectief op true staat.
		/// If DelaySave is false, there will be an exception.
		/// </summary>
		internal int CommitChanges()
		{
			if (delaySave) return base.SaveChanges();
			else throw new InvalidOperationException("No UnitOfWork present, use SaveChanges instead");
		}
	}
}

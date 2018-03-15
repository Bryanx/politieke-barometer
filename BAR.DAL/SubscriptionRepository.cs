using BAR.BL.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAR.DAL.EF;
using System.Data.Entity;

namespace BAR.DAL
{
	/// <summary>
	/// At this moment the repository works HC.
	/// </summary
	public class SubscriptionRepository : ISubscriptionRepository
	{
		private BarometerDbContext ctx;

		public SubscriptionRepository(UnitOfWork uow = null)
		{
			if (uow == null) ctx = new BarometerDbContext();
			else ctx = uow.Context;
		}

		/// <summary>
		/// Create's a new subscription and persist that
		/// subscription to the database.
		/// </summary>
		public void CreateSubscription(Subscription sub)
		{
			ctx.Subscriptions.Add(sub);
			ctx.SaveChanges();
		}

		/// <summary>
		/// Gives back a collection of alerts from a specific user.
		/// 
		/// If showalbe is true, the method will return a list of alerts
		/// where you can alse access the item and the user of a specific alert.
		/// </summary>
		public IEnumerable<Alert> ReadAlerts(int userId, bool showable = false)
		{
			IEnumerable<Subscription> userSubs;
			if (!showable)
			{
				userSubs = ctx.Subscriptions.Include(sub => sub.Alerts)
				.Where(sub => sub.SubscribedUser.UserId == userId).AsEnumerable();				
			} else
			{
				userSubs = ctx.Subscriptions.Include(sub => sub.Alerts)
											.Include(sub => sub.SubscribedItem)
											.Include(sub => sub.SubscribedUser)
				.Where(sub => sub.SubscribedUser.UserId == userId).AsEnumerable();
			}

			List<Alert> alersToRead = new List<Alert>();
			foreach (Subscription sub in userSubs) alersToRead.AddRange(sub.Alerts);
			return alersToRead.AsEnumerable();
		}

		/// <summary>
		/// Gives back a collection of subscriptions form a specific item.
		/// </summary>
		public IEnumerable<Subscription> ReadSubscriptions(int itemId)
		{
			return ctx.Subscriptions.Where(sub => sub.SubscribedItem.ItemId == itemId).AsEnumerable();
		}

		/// <summary>
		/// Returns a list of subscriptions with their alerts.
		/// </summary>
		public IEnumerable<Subscription> ReadSubscritpionsWithAlerts(int itemId)
		{
			return ctx.Subscriptions.Include(sub => sub.Alerts)
				.Where(sub => sub.SubscribedItem.ItemId == itemId).AsEnumerable();
		}

		/// <summary>
		/// Update a specific subscription.
		/// </summary>
		public void UpdateSubScription(Subscription sub)
		{
			ctx.Entry(sub).State = EntityState.Modified;
			ctx.SaveChanges();
		}

		/// <summary>
		/// Updates all the subscriptions when alerts are added.
		/// </summary>
		public void UpdateSubscriptions(IEnumerable<Subscription> subs)
		{
			foreach (Subscription sub in subs) ctx.Entry(sub).State = EntityState.Modified;
			ctx.SaveChanges();
		}
	}
}

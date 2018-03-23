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

		/// <summary>
		/// If uow is present then the constructor
		/// will get the context from uow.
		/// </summary>
		public SubscriptionRepository(UnitOfWork uow = null)
		{
			if (uow == null) ctx = new BarometerDbContext();
			else ctx = uow.Context;
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
			}
			else
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
		/// Returns the alert from a specific user.
		/// Because a user can have multiple subscriptions, we look for the alert in each subscription
		/// To update the alert you have to update its Subscription (alerts have no DbSet)
		/// </summary>
		public Alert ReadAlert(int userId, int alertId) 
		{
			foreach (Subscription sub in ReadSubscriptionsForUser(userId).ToList()) {
				if (sub.Alerts != null) {
					foreach (Alert alert in sub.Alerts) {
						if (alert.AlertId == alertId) return alert;
					}
				}
			}
			return null;
		}

		/// <summary>
		/// Gives back a collection of subscriptions form a specific item.
		/// </summary>
		public IEnumerable<Subscription> ReadSubscriptionsForItem(int itemId)
		{
			return ctx.Subscriptions.Where(sub => sub.SubscribedItem.ItemId == itemId).AsEnumerable();
		}

		/// <summary>
		/// Returns a list of subscriptions with their alerts.
		/// for a specific item.
		/// </summary>
		public IEnumerable<Subscription> ReadSubscritpionsWithAlerts(int itemId)
		{
			return ctx.Subscriptions.Include(sub => sub.Alerts)
				.Where(sub => sub.SubscribedItem.ItemId == itemId).AsEnumerable();
		}
		
		/// <summary>
		/// Returns a list of subscriptions of a user.
		/// </summary>
		public IEnumerable<Subscription> ReadSubscriptionsForUser(int userId)
		{
			return ctx.Subscriptions.Where(sub => sub.SubscribedUser.UserId == userId).AsEnumerable();
		}

		/// <summary>
		/// Returns a list of subscriptions with their alerts.
		/// </summary>
		public IEnumerable<Subscription> ReadSubscriptionsWithAlertsForUser(int userId)
		{
			return ctx.Subscriptions.Include(sub => sub.Alerts).Where(sub => sub.SubscribedUser.UserId == userId).AsEnumerable();
		}
		
		/// <summary>
		/// Returns a list of subscriptions with their items.
		/// </summary>
		public IEnumerable<Subscription> ReadSubscriptionsWithItemsForUser(int userId)
		{
			return ctx.Subscriptions.Include(sub => sub.SubscribedItem).Where(sub => sub.SubscribedUser.UserId == userId).AsEnumerable();
		}

		/// <summary>
		/// Gives a subscription object based on the subscription id.
		/// </summary>
		public Subscription ReadSubscription(int subscriptionId)
		{
			return ctx.Subscriptions.Find(subscriptionId);
		}

		/// <summary>
		/// Gives back all the subscritons.
		/// </summary>
		public IEnumerable<Subscription> ReadAllSubscriptions()
		{
			return ctx.Subscriptions.AsEnumerable();
		}

		/// <summary>
		/// Create's a new subscription and persist that
		/// subscription to the database.
		/// Returns -1 if SaveChanges() is delayed by unit of work.
		/// </summary>
		public int CreateSubscription(Subscription sub)
		{
			ctx.Subscriptions.Add(sub);
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Update a specific subscription.
		/// Returns -1 if SaveChanges() is delayed by unit of work.
		/// </summary>
		public int UpdateSubScription(Subscription sub)
		{
			ctx.Entry(sub).State = EntityState.Modified;
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Updates all the subscriptions when alerts are added.
		/// Returns -1 if SaveChanges() is delayed by unit of work.
		/// </summary>
		public int UpdateSubscriptions(IEnumerable<Subscription> subs)
		{
			foreach (Subscription sub in subs) ctx.Entry(sub).State = EntityState.Modified;
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Updates all the subscriptions for a specific user.
		/// Returns -1 if SaveChanges() is delayed by unit of work.
		/// </summary>
		public int UpdateSubscriptionsForUser(int userId)
		{
			IEnumerable<Subscription> subs = ReadSubscriptionsForUser(userId);
			return UpdateSubscriptions(subs);
		}

		/// <summary>
		/// Updates all the subscriptions for a specific item.
		/// Returns -1 if SaveChanges() is delayed by unit of work.
		/// </summary>
		public int UpdateSubscriptionsForItem(int itemId)
		{
			IEnumerable<Subscription> subs = ReadSubscriptionsForItem(itemId);
			return UpdateSubscriptions(subs);
		}

		/// <summary>
		/// Deletes a subscription from the database.
		/// Returns -1 if SaveChanges() is delayed by unit of work.
		/// </summary>
		public int DeleteSubScription(Subscription sub)
		{
			ctx.Subscriptions.Remove(sub);
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Deletes a list of subscriptions from the database.
		/// Returns -1 if SaveChanges() is delayed by unit of work.
		/// </summary>
		public int DeleteSubscriptions(IEnumerable<Subscription> subs)
		{
			foreach (Subscription sub in subs) ctx.Subscriptions.Remove(sub);
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Deletes all the subscriptons for a specific user.
		/// Returns -1 if SaveChanges() is delayed by unit of work.
		/// </summary>
		public int DeleteSubscriptionsForUser(int userId)
		{
			IEnumerable<Subscription> subs = ReadSubscriptionsForUser(userId);
			return DeleteSubscriptions(subs);
		}

		/// <summary>
		/// Deletes all the subscriptons for a specific item.
		/// Returns -1 if SaveChanges() is delayed by unit of work.
		/// </summary>
		public int DeleteSubscriptionsForItem(int itemId)
		{
			IEnumerable<Subscription> subs = ReadSubscriptionsForItem(itemId);
			return DeleteSubscriptions(subs);
		}		
	}
}

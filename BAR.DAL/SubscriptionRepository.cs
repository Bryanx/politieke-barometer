using BAR.BL.Domain.Users;
using System.Collections.Generic;
using System.Linq;
using BAR.DAL.EF;
using System.Data.Entity;

namespace BAR.DAL
{
	/// <summary>
	/// This class is used for the persistance of
	/// subscriptions and their alerts.
	/// </summary>
	public class SubscriptionRepository : ISubscriptionRepository
	{
		private readonly BarometerDbContext ctx;

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
		/// returns a collection of alerts
		/// </summary>
		public IEnumerable<Alert> ReadAllAlerts()
		{
			return ctx.Alerts.AsEnumerable();
		}

		/// <summary>
		/// Returns a list of subscriptions with their items.
		/// </summary>
		public IEnumerable<Subscription> ReadSubscriptionsWithItemsForUser(string userId)
		{
			return ctx.Subscriptions.Include(sub => sub.SubscribedItem)
									.Where(sub => sub.SubscribedUser.Id.Equals(userId))
									.AsEnumerable();
		}

		/// <summary>
		/// Returns a subscription object based on the subscription id.
		/// </summary>
		public Subscription ReadSubscription(int subscriptionId)
		{
			return ctx.Subscriptions.Find(subscriptionId);
		}

		/// <summary>
		/// Gives back a subscription with their alerts
		/// </summary>
		public Subscription ReadSubscriptionWithAlerts(int subscriptionId)
		{
			return ctx.Subscriptions.Include(sub => sub.Alerts)
									.Where(sub => sub.SubscriptionId == subscriptionId)
									.SingleOrDefault();
		}

		/// <summary>
		/// Gives back an subscription with:
		/// - Alerts
		/// - User
		/// - Item
		/// </summary>
		public Subscription ReadEditableSubscription(int subscriptionId)
		{
			return ctx.Subscriptions.Include(sub => sub.Alerts)
									.Include(sub => sub.SubscribedItem)
									.Include(sub => sub.SubscribedUser)
									.Where(sub => sub.SubscriptionId == subscriptionId)
									.SingleOrDefault();
		}

		/// <summary>
		/// Gives back a list of subscriptions for a specific item
		/// Gives back a list of subscriptions with:
		/// - Alerts
		/// - User
		/// - Item
		/// </summary>
		public IEnumerable<Subscription> ReadEditableSubscriptionsForItem(int itemId)
		{
			return ctx.Subscriptions.Include(sub => sub.Alerts)
									.Include(sub => sub.SubscribedItem)
									.Include(sub => sub.SubscribedUser)
									.Where(sub => sub.SubscribedItem.ItemId == itemId)
									.AsEnumerable();
		}

		/// <summary>
		/// Creates a new subscription and persist that
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
		/// Deletes a subscription and all the alerts from that specific subscritption
		/// Returns -1 if SaveChanges() is delayed by unit of work.
		/// </summary>
		public int DeleteSubscription(int subId)
		{
			Subscription subscripton = ReadSubscriptionWithAlerts(subId);
			if (subscripton != null) ctx.Subscriptions.Remove(subscripton);
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Gives back a subalert based on the user -and alert Id
		/// </summary>
		public SubAlert ReadSubAlert(string userId, int alertId)
		{
			return ctx.Alerts.OfType<SubAlert>().Where(alert => alert.AlertId == alertId).SingleOrDefault();
		}

		/// <summary>
		/// Gives back a useralert based on the user -and alert Id
		/// </summary>
		public UserAlert ReadUserAlert(string userId, int alertId)
		{
			return ctx.Alerts.OfType<UserAlert>().Where(alert => alert.AlertId == alertId).SingleOrDefault();
		}

		/// <summary>
		/// Gives back all the subalerts
		/// </summary>
		public IEnumerable<SubAlert> ReadAllSubAlerts()
		{
			return ctx.Alerts.OfType<SubAlert>()
							 .Include(alert => alert.Subscription)
							 .Include(alert => alert.Subscription.SubscribedUser)
							 .Include(alert => alert.Subscription.SubscribedItem)
							 .AsEnumerable();
		}

		/// <summary>
		/// Gives back all the useralerts
		/// </summary>
		public IEnumerable<UserAlert> ReadAllUserAlerts()
		{
			return ctx.Alerts.OfType<UserAlert>()
							 .Include(alert => alert.User)
							 .AsEnumerable();
		}

		/// <summary>
		/// Deletes a specific alert from the database
		/// </summary>
		public int DeleteAlert(Alert alert)
		{
			ctx.Alerts.Remove(alert);
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Gives back an alert for a spefic alertId
		/// </summary>
		public Alert ReadAlert(int alertId)
		{
			return ctx.Alerts.Find(alertId);
		}

		/// <summary>
		/// Updates an alert
		/// </summary>
		public int UpdateAlert(Alert alert)
		{
			ctx.Entry(alert).State = EntityState.Modified;
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Gives back all the subscription alerts for a specific userId
		/// </summary>
		public IEnumerable<SubAlert> ReadSubAlerts(string userId)
		{
			return ctx.Alerts.OfType<SubAlert>()
							 .Include(alert => alert.Subscription)
							 .Include(alert => alert.Subscription.SubscribedUser)
							 .Include(alert => alert.Subscription.SubscribedItem)
							 .Where(alert => alert.Subscription.SubscribedUser.Id.Equals(userId))
							 .AsEnumerable();
		}

		/// <summary>
		/// Gives back all the subscription alerts for a specific userId
		/// </summary>
		public IEnumerable<UserAlert> ReadUserAlerts(string userId)
		{
			return ctx.Alerts.OfType<UserAlert>().Include(alert => alert.User)
												 .Where(alert => alert.User.Id.Equals(userId))
												 .AsEnumerable();
		}

		/// <summary>
		/// Updates a given subalert in the database
		/// </summary>
		public int UpdateSubAlert(SubAlert subAlert)
		{
			ctx.Entry(subAlert).State = EntityState.Modified;
			return ctx.SaveChanges();
		}
	}
}

﻿using BAR.BL.Domain.Users;
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
		/// Returns an a alert of a specific origin
		/// </summary>
		public Alert ReadAlert(string userId, int alertId, bool readUserAlert)
		{
			if (readUserAlert) return ReadUserAlert(userId, alertId);
			else return ReadSubAlert(userId, alertId);
		}

		/// <summary>
		/// Returns a collection of subscriptions from a specific item.
		/// </summary>
		public IEnumerable<Subscription> ReadSubscriptionsForItem(int itemId)
		{
			return ctx.Subscriptions.Where(sub => sub.SubscribedItem.ItemId == itemId)
									.AsEnumerable();
		}

		/// <summary>
		/// Returns a list of subscriptions with their alerts
		/// for a specific item.
		/// </summary>
		public IEnumerable<Subscription> ReadSubscritpionsWithAlerts(int itemId)
		{
			return ctx.Subscriptions.Include(sub => sub.Alerts)
									.Where(sub => sub.SubscribedItem.ItemId == itemId)
									.AsEnumerable();
		}

		/// <summary>
		/// Returns a list of subscriptions of a user.
		/// </summary>
		public IEnumerable<Subscription> ReadSubscriptionsForUser(string userId)
		{
			return ctx.Subscriptions.Where(sub => sub.SubscribedUser.Id.Equals(userId))
									.AsEnumerable();
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
		/// Returns all the subscriptons.
		/// </summary>
		public IEnumerable<Subscription> ReadAllSubscriptions()
		{
			return ctx.Subscriptions.AsEnumerable();
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
		/// Updates all the subscriptions for a specific user.
		/// Returns -1 if SaveChanges() is delayed by unit of work.
		/// </summary>
		public int UpdateSubscriptionsForUser(string userId)
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
		public int DeleteSubscription(int subId)
		{
			Subscription subscripton = ReadSubscriptionWithAlerts(subId);
			if (subscripton != null) ctx.Subscriptions.Remove(subscripton);
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
		public int DeleteSubscriptionsForUser(string userId)
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
	}
}

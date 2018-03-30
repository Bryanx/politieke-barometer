using System;
using BAR.DAL;
using BAR.BL.Domain.Users;
using System.Collections.Generic;
using System.Linq;
using BAR.BL.Domain.Items;

namespace BAR.BL.Managers
{
	/// <summary>
	/// Responsable for managing subcriptions
	/// and their alerts.
	/// </summary>
	public class SubscriptionManager : ISubscriptionManager
	{
		private ISubscriptionRepository subRepo;
		private UnitOfWorkManager uowManager;

		/// <summary>
		/// When unit of work is present, it will effect
		/// initRepo-method. (see documentation of initRepo)
		/// </summary>
		public SubscriptionManager(UnitOfWorkManager uowManager = null)
		{
			this.uowManager = uowManager;
		}

		/// <summary>
		/// Creates a new subscription for a specific user and item
		/// with a given threshold.
		/// 
		/// NOTE
		/// THIS METHOD USES UNIT OF WORK
		/// </summary>		
		public Subscription CreateSubscription(string userId, int itemId, int threshold = 10)
		{
			uowManager = new UnitOfWorkManager();
			InitRepo();

			//get user
			IUserManager userManager = new UserManager(uowManager);
			User user = userManager.GetUser(userId);
			if (user == null) return null;

			//get item
			IItemManager itemManager = new ItemManager(uowManager);
			Item item = itemManager.GetItem(itemId);
			if (item == null) return null;

			//Check if sub already exists
			//if possible item is null, then we can make a subscription
			IEnumerable<Subscription> subs = GetSubscriptionsWithItemsForUser(userId);
			if (subs.Where(sub => sub.SubscribedItem.ItemId == itemId).Count() != 0) return null;


			//make subscription		
			Subscription subscription = new Subscription()
			{
				SubscribedUser = user,
				SubscribedItem = item,
				Threshold = threshold,
				DateSubscribed = DateTime.Now,
				Alerts = new List<Alert>()
			};
			item.NumberOfFollowers++;
			subRepo.CreateSubscription(subscription);
			uowManager.Save();

			return subscription;
		}

		/// <summary>
		/// Generates new alerts for a specific item.
		/// When a user's threshold is met, an alert will be generated.
		/// </summary>
		public void GenerateAlerts(int itemId)
		{
			InitRepo();

			IItemManager itemManager = new ItemManager();
			double per = (itemManager.GetTrendingPer(itemId) * 100) - 100;

			List<Subscription> subsToUpdate = new List<Subscription>();
			IEnumerable<Subscription> subs = subRepo.ReadEditableSubscriptionsForItem(itemId);
			foreach (Subscription sub in subs)
			{
				double thresh = sub.Threshold;
				if (per >= thresh && sub.SubscribedUser.AlertsViaWebsite && sub.SubscribedUser.IsActive)
				{
					sub.Alerts.Add(new Alert()
					{
						Subscription = sub,
						AlertType = new AlertType()
						{
							Name = "Trending alert"
						},
						TimeStamp = DateTime.Now,
						IsRead = false
					});
					subsToUpdate.Add(sub);
				}

			}
			subRepo.UpdateSubscriptions(subsToUpdate);
		}

		/// <summary>
		/// Gets all the alerts for a specific user
		/// </summary>
		public IEnumerable<Alert> GetAllAlerts(string userId)
		{
			InitRepo();
			return subRepo.ReadAlerts(userId, true);
		}

		/// <summary>
		/// Retrieves a single alert for a specific user.
		/// </summary>
		public Alert GetAlert(string userId, int alertId) 
		{
			InitRepo();
			return subRepo.ReadAlert(userId, alertId);
		}

		/// <summary>
		/// Changed the isRead property of an Alert to true.
		/// </summary>
		public Alert ChangeAlertToRead(string userId, int alertId) 
		{
			InitRepo();

			//Get Alert
			Alert alertToUpdate = GetAlert(userId, alertId);
			if (alertToUpdate == null) return null;

			//Change alert
			alertToUpdate.IsRead = true;

			//Update database
			subRepo.UpdateSubScription(alertToUpdate.Subscription);

			return alertToUpdate;
		}

		/// <summary>
		/// Removes a specific alert for a specific user.
		/// </summary>
		public void RemoveAlert(string userId, int alertId)
		{
			InitRepo();

			//Get alert
			Alert alertToRemove = GetAlert(userId, alertId);
			if (alertToRemove == null) return;

			//Remove alert
			Subscription sub = alertToRemove.Subscription;
			sub.Alerts.Remove(alertToRemove);

			//Update database
			subRepo.UpdateSubScription(sub);
		}

		/// <summary>
		/// Gets the subscription of a specific user, with alerts.
		/// </summary>
		public IEnumerable<Subscription> GetSubscriptionsWithAlertsForUser(string userId) 
		{
			InitRepo();
			return subRepo.ReadSubscriptionsWithAlertsForUser(userId);
		}

		/// <summary>
		/// Gets the subscription of a specific user, with items.
		/// </summary>
		public IEnumerable<Subscription> GetSubscriptionsWithItemsForUser(string userId) 
		{
			InitRepo();
			return subRepo.ReadSubscriptionsWithItemsForUser(userId);
		}

		/// <summary>
		/// Gets a subscription by Subscription id.
		/// </summary>
		public Subscription GetSubscription(int subId)
		{
			InitRepo();
			return subRepo.ReadSubscription(subId);
		}

		/// <summary>
		/// Removes a subscription by Subscription id.
		/// </summary>
		public void RemoveSubscription(int subId)
		{
			InitRepo();
			Subscription subscriptionToRemove = subRepo.ReadEditableSubscription(subId);
			if (subscriptionToRemove == null) return;

			subscriptionToRemove.SubscribedItem.NumberOfFollowers--;
			//id parameter is needed to delete alers with subscription in repo
			subRepo.DeleteSubscription(subId);
		}

		/// <summary>
		/// Updates the treshold of a specific user
		/// </summary>
		public Subscription ChangeSubscriptionTresh(int subId, int treshhold)
		{
			InitRepo();

			//Get sub
			Subscription subToUpdate = GetSubscription(subId);
			if (subToUpdate == null) return null;

			//Change sub
			subToUpdate.Threshold = treshhold;

			//Update database
			subRepo.UpdateSubScription(subToUpdate);

			return subToUpdate;
		}

		/// <summary>
		/// Determines if the repo needs a unit of work
		/// if the unitOfWorkManager is present.
		/// </summary>
		private void InitRepo()
		{
			if (uowManager == null) subRepo = new SubscriptionRepository();
			else subRepo = new SubscriptionRepository(uowManager.UnitOfWork);
		}
	}
}

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
		private SubscriptionRepository subRepo;
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
		public Subscription CreateSubscription(int userId, int itemId, int threshold = 10)
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

			//make subscription		
			Subscription sub = new Subscription()
			{
				SubscribedUser = user,
				SubscribedItem = item,
				Threshold = threshold,
				DateSubscribed = DateTime.Now,
				Alerts = new List<Alert>()
			};
			item.NumberOfFollowers++;
			subRepo.CreateSubscription(sub);
			uowManager.Save();

			return sub;
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
			IEnumerable<Subscription> subs = subRepo.ReadSubscritpionsWithAlerts(itemId);
			foreach (Subscription sub in subs)
			{
				double thresh = sub.Threshold;
				if (per >=  thresh)
				{
					sub.Alerts.Add(new Alert()
					{
						Subscription = sub,
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
		public IEnumerable<Alert> GetAllAlerts(int userId)
		{
			InitRepo();
			return subRepo.ReadAlerts(userId, true);
		}
		
		/// <summary>
		/// Retrieves a single alert for a specific user.
		/// </summary>
		public Alert GetAlert(int userId, int alertId) 
		{
			InitRepo();
			return subRepo.ReadAlert(userId, alertId);
		}

		/// <summary>
		/// Changed the isRead property of an Alert to true.
		/// </summary>
		public void ChangeAlertToRead(int userId, int alertId) 
		{
			InitRepo();
			Alert alert = GetAlert(userId, alertId);
			if (alert != null) 
			{
				alert.IsRead = true;
				subRepo.UpdateSubScription(alert.Subscription);
			}
		}

		/// <summary>
		/// Removes a specific alert for a specific user.
		/// </summary>
		public void RemoveAlert(int userId, int alertId)
		{
			InitRepo();
			Alert alert = GetAlert(userId, alertId);
				if (alert != null) 
				{
					Subscription sub = alert.Subscription;
					sub.Alerts.Remove(alert);
					subRepo.UpdateSubScription(sub);
				}
		}
		
		/// <summary>
		/// Gets the subscription of a specific user, with alerts.
		/// </summary>
		public IEnumerable<Subscription> GetSubscriptionsWithAlertsForUser(int userId) 
		{
			InitRepo();
			return subRepo.ReadSubscriptionsWithAlertsForUser(userId);
		}
		
		/// <summary>
		/// Gets the subscription of a specific user, with items.
		/// </summary>
		public IEnumerable<Subscription> GetSubscriptionsWithItemsForUser(int userId) 
		{
			InitRepo();
			return subRepo.ReadSubscriptionsWithItemsForUser(userId);
		}
		
		/// <summary>
		/// Gets a subscription by Subscription id.
		/// </summary>
		public Subscription GetSubscription(int subId) {
			InitRepo();
			return subRepo.ReadSubscription(subId);
		}
		
		/// <summary>
		/// Removes a subscription by Subscription id.
		/// </summary>
		public void RemoveSubscription(int subId) {
			InitRepo();
			subRepo.DeleteSubscription(subId);
		}

		/// <summary>
		/// Updates the treshold of a specific user
		/// </summary>
		public Subscription ChangeSubscriptionTresh(int subId, int treshhold)
		{
			InitRepo();

			//Get sub
			Subscription subToUpdate = subRepo.ReadSubscription(subId);
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

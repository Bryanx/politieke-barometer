using BAR.BL.Domain.Items;
using BAR.BL.Domain.Users;
using BAR.DAL;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

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
		/// Determines if the repo needs a unit of work
		/// if the unitOfWorkManager is present.
		/// </summary>
		private void InitRepo()
		{
			if (uowManager == null) subRepo = new SubscriptionRepository();
			else subRepo = new SubscriptionRepository(uowManager.UnitOfWork);
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
			//if the counter is 0 then a new subscrption will not be created.
			IEnumerable<Subscription> subs = GetSubscriptionsWithItemsForUser(userId);
			if (subs.Where(sub => sub.SubscribedItem.ItemId == itemId).Count() != 0) return null;

			//make subscription		
			Subscription subscription = new Subscription()
			{
				SubscribedUser = user,
				SubscribedItem = item,
				Threshold = threshold,
				DateSubscribed = DateTime.Now,
				Alerts = new List<SubAlert>()
			};
			item.NumberOfFollowers++;

			//Save changes to the database
			subRepo.CreateSubscription(subscription);
			uowManager.Save();
			uowManager = null;
			return subscription;
		}

		/// <summary>
		/// Generates new alerts for a specific item.
		/// </summary>
		public void GenerateAlerts(int itemId)
		{
			InitRepo();

			//Get subscriptions
			List<Subscription> subsToUpdate = new List<Subscription>();
			IEnumerable<Subscription> subs = subRepo.ReadEditableSubscriptionsForItem(itemId);
			if (subs == null || subs.Count() == 0) return;

			//Generate alerts
			foreach (Subscription sub in subs)
			{
				if (sub.SubscribedUser.AlertsViaWebsite && sub.SubscribedUser.IsActive)
				{
					sub.Alerts.Add(new SubAlert()
					{
						Subscription = sub,
						AlertType = AlertType.Trending,
						TimeStamp = DateTime.Now,
						IsRead = false,
						IsSend = false
					});
					subsToUpdate.Add(sub);
				}
			}
			subRepo.UpdateSubscriptions(subsToUpdate);

			//Send emails
			IEnumerable<Subscription> usersToSendEmail = subs.Where(sub => sub.SubscribedUser.AlertsViaEmail).AsEnumerable();
			SendTrendingEmails(itemId, usersToSendEmail);
		}

		/// <summary>
		/// Sends an email to the users who wants te receive an email via
		/// if a person is trending
		/// </summary>
		private void SendTrendingEmails(int itemId, IEnumerable<Subscription> subs)
		{
			//Get item
			Item item = new ItemManager().GetItem(itemId);
			if (item == null) return;

			//Send email
			foreach (Subscription sub in subs)
			{
				IdentityMessage message = new IdentityMessage()
				{
					Destination = sub.SubscribedUser.Email,
					Subject = item.Name + " is nu trending!",
					Body = "<strong>" + item.Name + " is nu trending</strong> met  " + item.NumberOfMentions + " vermeldingen!</br>" +
					"Ga nu naar de website en ontdenk waarom het item trending is!"
				};
				new EmailService().Send(message);
			}
		}

		/// <summary>
		/// Gets all the alerts for a specific user
		/// </summary>
		public IEnumerable<Alert> GetAllAlerts()
		{
			InitRepo();
			return subRepo.ReadAllAlerts().AsEnumerable();
		}

		/// <summary>
		/// Retrieves a single alert for a specific user.
		/// </summary>
		public Alert GetAlert(int alertId)
		{
			InitRepo();
			return subRepo.ReadAlert(alertId);
		}

		/// <summary>
		/// Changed the isRead property of an Alert to true.
		/// </summary>
		public Alert ChangeAlertToRead(int alertId)
		{
			InitRepo();

			//Get alert
			Alert alertToUpdate = subRepo.ReadAlert(alertId);
			if (alertToUpdate == null) return null;

			//Change alert
			alertToUpdate.IsRead = true;

			//Update alert
			subRepo.UpdateAlert(alertToUpdate);
			return alertToUpdate;
		}

		/// <summary>
		/// Removes a specific alert for a specific user.
		/// </summary>
		public void RemoveAlert(int alertId)
		{
			InitRepo();

			//Get alert
			Alert alertToRemove = subRepo.ReadAlert(alertId);
			if (alertToRemove == null) return;

			//Delete alert
			subRepo.DeleteAlert(alertToRemove);
		}

		/// <summary>
		/// Gets the subscription of a specific user, with items.
		/// </summary>
		public IEnumerable<Subscription> GetSubscriptionsWithItemsForUser(string userId)
		{
			InitRepo();
			return subRepo.ReadSubscriptionsWithItemsForUser(userId).AsEnumerable();
		}

		/// <summary>
		/// Gets the subscribed items for a specific user.
		/// </summary>
		public IEnumerable<Item> GetSubscribedItemsForUser(string userId)
		{
			InitRepo();
			return GetSubscriptionsWithItemsForUser(userId).Select(sub => sub.SubscribedItem).AsEnumerable();
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
		/// Toggles a subscription on wether the subscription exists or not.
		/// </summary>
		public void ToggleSubscription(string userId, int itemId)
		{
			//Get subs
			IEnumerable<Subscription> subs = GetSubscriptionsWithItemsForUser(userId);
			if (subs == null) return;

			//Toggle subs
			if (subs.Select(sub => sub.SubscribedItem.ItemId).Contains(itemId))
			{
				RemoveSubscription(subs.First(sub => sub.SubscribedItem.ItemId == itemId).SubscriptionId);
			}
			else
			{
				CreateSubscription(userId, itemId);
			}
		}

		/// <summary>
		/// Removes a subscription by Subscription id.
		/// </summary>
		public void RemoveSubscription(int subId)
		{
			InitRepo();

			//Get sub
			Subscription subscriptionToRemove = subRepo.ReadEditableSubscription(subId);
			if (subscriptionToRemove == null) return;

			//Remove sub
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
		/// Gives back a subalert based on the user -and alert Id
		/// </summary>
		public SubAlert GetSubAlert(string userId, int alertId)
		{
			InitRepo();
			return subRepo.ReadSubAlert(userId, alertId);
		}

		/// <summary>
		/// Gives back a subalert based on the user -and alert Id
		/// </summary>
		public UserAlert GetUserAlert(string userId, int alertId)
		{
			InitRepo();
			return subRepo.ReadUserAlert(userId, alertId);
		}

		/// <summary>
		/// Gives back all the subalerts
		/// </summary>
		public IEnumerable<SubAlert> GetAllSubAlerts()
		{
			InitRepo();
			return subRepo.ReadAllSubAlerts().AsEnumerable();
		}

		/// <summary>
		/// Gives back all the useralerts
		/// </summary>
		public IEnumerable<UserAlert> GetAllUserAlerts()
		{
			InitRepo();
			return subRepo.ReadAllUserAlerts().AsEnumerable();
		}

		/// <summary>
		/// Gives back all the sub alerts for a specific userId
		/// </summary>
		public IEnumerable<SubAlert> GetSubAlerts(string userId)
		{
			InitRepo();
			return subRepo.ReadSubAlerts(userId);
		}

		/// <summary>
		/// Gives back all the user alerts for a specific userId
		/// </summary>
		public IEnumerable<UserAlert> GetUserAlerts(string userId)
		{
			InitRepo();
			return subRepo.ReadUserAlerts(userId);
		}

		/// <summary>
		/// Gets all the sub alerts that were not yet sent to the
		/// android devices
		/// </summary>
		public IEnumerable<SubAlert> GetUnsendedSubAlerts()
		{
			InitRepo();
			return GetAllSubAlerts().Where(alert => !alert.IsSend);
		}

		/// <summary>
		/// Change status to send
		/// </summary>
		public void ChangeSubAlertToSend(SubAlert subAlert)
		{
			InitRepo();

			//Change alert status
			if (subAlert == null) return;
			subAlert.IsSend = true;

			//Update database
			subRepo.UpdateSubAlert(subAlert);
		}
	}
}

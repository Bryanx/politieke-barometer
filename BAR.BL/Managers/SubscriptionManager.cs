using System;
using BAR.DAL;
using BAR.BL.Domain.Users;
using System.Collections.Generic;
using BAR.BL.Domain.Items;

namespace BAR.BL.Managers
{
	/// <summary>
	/// Resposable for managing subcriptions
	/// and their alerts
	/// </summary>
	public class SubscriptionManager : ISubscriptionManager
	{
		/// <summary>
		/// Creates a new subscription for a specific user and item
		/// with a given treshhold.
		/// 
		/// WARNING
		/// THIS METHOD WILL NOT WORK BECAUSE UNIT OF WORK IS YET TO BE IMPLEMENTED.
		/// </summary>		
		public void CreateSubscription(int userId, int itemId, int treshhold = 10)
		{
			//get user
			IUserManager userManager = new UserManager();
			User user = userManager.GetUser(userId);

			//get item
			IItemManager itemManager = new ItemManager();
			Item item = itemManager.GetItem(itemId);

			//make subscription
			ISubscriptionRepository subRepo = new SubscriptionRepository();
			Subscription sub = new Subscription()
			{
				SubscribedUser = user,
				SubscribedItem = item,
				Treshhold = treshhold
			};
			subRepo.CreateSubscription(sub);

		}

		/// <summary>
		/// Generates new alerts for a specific item
		/// When a user treshhold is met, a alert will bed generated
		/// </summary>
		public void GenerateAlerts(int itemId)
		{
			IItemManager itemManager = new ItemManager();
			double per = itemManager.GetTrendingPer(itemId);

			ISubscriptionRepository subRepo = new SubscriptionRepository();
			IEnumerable<Subscription> subs = subRepo.ReadSubscriptions(itemId);
			foreach (Subscription sub in subs)
			{
				double tresh = sub.Treshhold;
				if (tresh <= (per - 100))
				{
					sub.Alerts.Add(new Alert());
				}
			}		
			subRepo.UpdateSubscriptions(subs);
		}

		/// <summary>
		/// Gets all the alerts for a specific user
		/// </summary>
		public IEnumerable<Alert> GetAllAlerts(int userId)
		{
			ISubscriptionRepository subRepo = new SubscriptionRepository();
			return subRepo.ReadAlerts(userId);
		}
	}
}

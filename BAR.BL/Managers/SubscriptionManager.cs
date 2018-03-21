using System;
using BAR.DAL;
using BAR.BL.Domain.Users;
using System.Collections.Generic;
using System.Linq;
using BAR.BL.Domain.Items;

namespace BAR.BL.Managers
{
	/// <summary>
	/// Resposable for managing subcriptions
	/// and their alerts
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
		/// with a given treshhold.
		/// 
		/// NOTE
		/// THIS METHOD USES UNIT OF WORK
		/// </summary>		
		public void CreateSubscription(int userId, int itemId, int treshhold = 10)
		{
			uowManager = new UnitOfWorkManager();
			InitRepo();

			//get user
			IUserManager userManager = new UserManager(uowManager);
			User user = userManager.GetUser(userId);

			//get item
			IItemManager itemManager = new ItemManager(uowManager);
			Item item = itemManager.GetItem(itemId);

			//make subscription		
			Subscription sub = new Subscription()
			{
				SubscribedUser = user,
				SubscribedItem = item,
				Treshhold = treshhold,
				Alerts = new List<Alert>()
			};
			subRepo.CreateSubscription(sub);

			uowManager.Save();
		}

		/// <summary>
		/// Generates new alerts for a specific item
		/// When a user treshhold is met, a alert will bed generated
		/// </summary>
		public void GenerateAlerts(int itemId)
		{
			InitRepo();

			IItemManager itemManager = new ItemManager();
			double per = (itemManager.GetTrendingPer(itemId) * 100) - 100;

			IEnumerable<Subscription> subs = subRepo.ReadSubscritpionsWithAlerts(itemId);
			foreach (Subscription sub in subs)
			{
				double tresh = sub.Treshhold;
				if (per >=  tresh)
				{
					sub.Alerts.Add(new Alert()
					{
						Subscription = sub,
						TimeStamp = DateTime.Now,
						IsRead = false
					});
					//subRepo.UpdateSubScription(sub);
				}
				
			}
			subRepo.UpdateSubscriptions(subs);
		}

		/// <summary>
		/// Gets all the alerts for a specific user
		/// </summary>
		public IEnumerable<Alert> GetAllAlerts(int userId)
		{
			InitRepo();
			return subRepo.ReadAlerts(userId, true);
		}

		public void RemoveAlert(int userId, int alertId) {
			InitRepo();
			subRepo.DeleteAlert(userId, alertId);
		}
		
		/// <summary>
		/// Determines if the repo needs a unit of work
		/// if the unitOfWorkManager is present
		/// </summary>
		private void InitRepo()
		{
			if (uowManager == null) subRepo = new SubscriptionRepository();
			else subRepo = new SubscriptionRepository(uowManager.UnitOfWork);
		}
	}
}

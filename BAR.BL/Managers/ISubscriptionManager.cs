using BAR.BL.Domain.Users;
using System.Collections.Generic;
using BAR.BL.Domain.Items;

namespace BAR.BL.Managers
{
	public interface ISubscriptionManager
	{
		//Subscriptions
		Subscription GetSubscription(int subId);
		IEnumerable<Subscription> GetSubscriptionsWithItemsForUser(string userId);
		IEnumerable<Item> GetSubscribedItemsForUser(string userId);

		Subscription CreateSubscription(string userId, int itemId, int treshhold = 10);
		void ToggleSubscription(string userId, int itemId);
		Subscription ChangeSubscriptionTresh(int subId, int treshhold);

		void RemoveSubscription(int subId);

		//Alerts
		Alert GetAlert(string userId, int alertId, bool userAlert);
		SubAlert GetSubAlert(string userId, int alertId);
		UserAlert GetUserAlert(string userid, int alertId);
		IEnumerable<Alert> GetAllAlerts(string userId);

		Alert ChangeAlertToRead(string id, int alertId, bool userAlert);

		void RemoveAlert(string userId, int alertId, bool userAlert);

		void GenerateAlerts(int itemId);
	}
}

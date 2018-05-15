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
		Alert GetAlert(int alertId);
		SubAlert GetSubAlert(string userId, int alertId);
		UserAlert GetUserAlert(string userId, int alertId);
		IEnumerable<Alert> GetAllAlerts();

		Alert ChangeAlertToRead(int alertId);

		void RemoveAlert(int alertId);

		void GenerateAlerts(int itemId);
	}
}

using BAR.BL.Domain.Users;
using System.Collections.Generic;

namespace BAR.BL.Managers
{
	public interface ISubscriptionManager
	{
		//Subscriptions
		Subscription GetSubscription(int subId);
		IEnumerable<Subscription> GetSubscriptionsWithAlertsForUser(string userId);
		IEnumerable<Subscription> GetSubscriptionsWithItemsForUser(string userId);

		Subscription CreateSubscription(string userId, int itemId, int treshhold = 10);
		void ToggleSubscription(string userId, int itemId);
		Subscription ChangeSubscriptionTresh(int subId, int treshhold);

		void RemoveSubscription(int subId);

		//Alerts
		Alert GetAlert(string userId, int alertId);
		IEnumerable<Alert> GetAllAlerts(string userId);

		Alert ChangeAlertToRead(string id, int alertId);

		void RemoveAlert(string userId, int alertId);

		void GenerateAlerts(int itemId);
	}
}

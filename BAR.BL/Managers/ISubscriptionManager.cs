using BAR.BL.Domain.Users;
using System.Collections.Generic;

namespace BAR.BL.Managers
{
	public interface ISubscriptionManager
	{
		//Subscriptions
		Subscription GetSubscription(int subId);
		IEnumerable<Subscription> GetSubscriptionsWithAlertsForUser(int userId);
		IEnumerable<Subscription> GetSubscriptionsWithItemsForUser(int userId);

		Subscription CreateSubscription(int userId, int itemId, int treshhold = 10);

		Subscription ChangeSubscriptionTresh(int subId, int treshhold);

		void RemoveSubscription(int subId);

		//Alerts
		Alert GetAlert(int userId, int alertId);
		IEnumerable<Alert> GetAllAlerts(int userId);

		Alert ChangeAlertToRead(int id, int alertId);

		void RemoveAlert(int userId, int alertId);

		void GenerateAlerts(int itemId);
	}
}

using BAR.BL.Domain.Users;
using System.Collections.Generic;

namespace BAR.BL.Managers
{
	public interface ISubscriptionManager
	{
		//Subscriptions
		void CreateSubscription(int userId, int itemId, int treshhold = 10);
		IEnumerable<Alert> GetAllAlerts(int userId);
		void RemoveAlert(int userId, int alertId);
		
		void ChangeAlertToRead(int id, int alertId);
		IEnumerable<Subscription> GetSubscriptionsWithAlertsForUser(int userId);
		IEnumerable<Subscription> GetSubscriptionsWithItemsForUser(int userId);
		void RemoveSubscription(int subId);
		void GenerateAlerts(int itemId);
	}
}

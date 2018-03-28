using BAR.BL.Domain.Users;
using System.Collections.Generic;

namespace BAR.BL.Managers
{
	public interface ISubscriptionManager
	{
		void GenerateAlerts(int itemId);
		IEnumerable<Alert> GetAllAlerts(string userId);
		void RemoveAlert(string userId, int alertId);
		void CreateSubscription(string userId, int itemId, UserManager userManager, int treshhold = 10);
		void ChangeAlertToRead(string userId, int alertId);
		IEnumerable<Subscription> GetSubscriptionsWithAlertsForUser(string userId);
		IEnumerable<Subscription> GetSubscriptionsWithItemsForUser(string userId);
		void RemoveSubscription(int subId);
	}
}

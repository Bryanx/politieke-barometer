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

		Subscription CreateSubscription(string userId, int itemId, int treshhold = 10);

		Subscription ChangeSubscriptionTresh(int subId, int treshhold);

		void RemoveSubscription(int subId);

		void ToggleSubscription(string userId, int itemId);

		//Alerts
		Alert GetAlert(int alertId);
		SubAlert GetSubAlert(string userId, int alertId);
		UserAlert GetUserAlert(string userId, int alertId);
		IEnumerable<Alert> GetAllAlerts();
		IEnumerable<SubAlert> GetAllSubAlerts();
		IEnumerable<UserAlert> GetAllUserAlerts();
		IEnumerable<SubAlert> GetSubAlerts(string userId);
		IEnumerable<UserAlert> GetUserAlerts(string userId);
		IEnumerable<SubAlert> GetUnsendedSubAlerts();

		Alert ChangeAlertToRead(int alertId);
		void ChangeSubAlertToSend(SubAlert subAlert);

		void RemoveAlert(int alertId);

		void GenerateAlerts(int itemId);

		//Items
		IEnumerable<Item> GetSubscribedItemsForUser(string userId);
	}
}

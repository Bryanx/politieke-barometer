using BAR.BL.Domain.Users;
using System.Collections.Generic;

namespace BAR.DAL
{
	public interface ISubscriptionRepository
	{
		//Read
		Subscription ReadSubscription(int subscriptionId);
		Subscription ReadSubscriptionWithAlerts(int subscriptionId);
		Subscription ReadEditableSubscription(int subscriptionId);
		IEnumerable<Subscription> ReadAllSubscriptions();
		IEnumerable<Subscription> ReadSubscriptionsForItem(int itemId);
		IEnumerable<Subscription> ReadSubscriptionsForUser(string userId);	
		IEnumerable<Subscription> ReadSubscriptionsWithItemsForUser(string userId);
		IEnumerable<Subscription> ReadSubscritpionsWithAlerts(int itemId);
		IEnumerable<Subscription> ReadEditableSubscriptionsForItem(int itemId);
		Alert ReadAlert(int alertId);
		SubAlert ReadSubAlert(string userId, int alertId);
		UserAlert ReadUserAlert(string userId, int alertId);
		IEnumerable<Alert> ReadAllAlerts();
		IEnumerable<SubAlert> ReadAllSubAlerts();
		IEnumerable<UserAlert> ReadAllUserAlerts();
		IEnumerable<SubAlert> ReadSubAlerts(string userId);
		IEnumerable<UserAlert> ReadUserAlerts(string userId);

		//Create
		int CreateSubscription(Subscription sub);

		//Update
		int UpdateAlert(Alert alert);
		int UpdateSubScription(Subscription sub);
		int UpdateSubscriptions(IEnumerable<Subscription> subs);

		//Delete
		int DeleteAlert(Alert alert);
		int DeleteSubscription(int subId);
	}
}

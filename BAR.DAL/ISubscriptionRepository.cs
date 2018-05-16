using BAR.BL.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		Alert ReadAlert(string userId, int alertId, bool readUserAlert);
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
		int UpdateSubscriptionsForUser(string userId);
		int UpdateSubscriptionsForItem(int itemId);

		//Delete
		int DeleteAlert(Alert alert);
		int DeleteSubscription(int subId);
		int DeleteSubscriptions(IEnumerable<Subscription> subs);
		int DeleteSubscriptionsForUser(string userId);
		int DeleteSubscriptionsForItem(int itemId);
	}
}

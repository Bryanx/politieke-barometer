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
		IEnumerable<Subscription> ReadAllSubscriptions();
		IEnumerable<Subscription> ReadSubscriptionsForItem(int itemId);
		IEnumerable<Subscription> ReadSubscriptionsForUser(int userId);
		IEnumerable<Subscription> ReadSubscriptionsWithAlertsForUser(int userId);
		IEnumerable<Subscription> ReadSubscriptionsWithItemsForUser(int userId);
		IEnumerable<Subscription> ReadSubscritpionsWithAlerts(int itemId);
		IEnumerable<Alert> ReadAlerts(int userId, bool showable = false);

		//Create
		int CreateSubscription(Subscription sub);

		//Update
		int UpdateSubScription(Subscription sub);
		int UpdateSubscriptions(IEnumerable<Subscription> subs);
		int UpdateSubscriptionsForUser(int userId);
		int UpdateSubscriptionsForItem(int itemId);

		//Delete
		int DeleteSubScription(Subscription sub);
		int DeleteSubscriptions(IEnumerable<Subscription> subs);
		int DeleteSubscriptionsForUser(int userId);
		int DeleteSubscriptionsForItem(int itemId);
	}
}

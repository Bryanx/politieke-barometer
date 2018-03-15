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
		IEnumerable<Subscription> ReadSubscriptions(int itemId);
		IEnumerable<Subscription> ReadSubscritpionsWithAlerts(int itemId);
		void UpdateSubscriptions(IEnumerable<Subscription> subs);
		void UpdateSubScription(Subscription sub);
		IEnumerable<Alert> ReadAlerts(int userId, bool showable = false);
		void CreateSubscription(Subscription sub);
	}
}

using BAR.BL.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.DAL
{
    /// <summary>
    /// At this moment the repository works HC.
    /// </summary>
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private List<Subscription> subs;

        public SubscriptionRepository()
        {
            subs = new List<Subscription>();
        }

        /// <summary>
        /// Gives back a collection of alerts from a specific user.
        /// </summary>
        public IEnumerable<Alert> ReadAlerts(int userId)
        {           
            IEnumerable<Subscription> userSubs = subs.Where(sub => sub.SubscribedUser.UserId == userId);

            List<Alert> alersToRead = new List<Alert>();
            foreach (Subscription sub in userSubs) alersToRead.AddRange(sub.Alerts);
            return alersToRead.AsEnumerable();           
        }

        /// <summary>
        /// Gives back a collection of subscriptions form a specific item.
        /// </summary>
        public IEnumerable<Subscription> ReadSubscriptions(int itemId)
        {
            return subs.Where(sub => sub.SubscribedItem.ItemId == itemId).AsEnumerable();
        }

        /// <summary>
        /// Updates all the subscriptions when alerts are added.
        /// </summary>
        public void UpdateSubscriptions(IEnumerable<Subscription> subs)
        {
            // all data lives in memory
            // everything refers to the same objects
        }
    }
}

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
    public class SubscriptionRepository
    {
        private List<Subscription> subs;

        public SubscriptionRepository()
        {
            subs = new List<Subscription>();
        }

        public IEnumerable<Subscription> ReadSubscriptions(int itemId)
        {    
            return subs.Where(sub => sub.SubscribedItem.ItemId == itemId).AsEnumerable();
        }

        public void UpdateSubscriptions(IEnumerable<Subscription> subs)
        {
            // all data lives in memory
            // everything refers to the same objects
        }
    }
}

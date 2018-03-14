using BAR.BL.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAR.DAL.EF;
using System.Data.Entity;

namespace BAR.DAL
{
    /// <summary>
    /// At this moment the repository works HC.
    /// </summary>
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private BarometerDbContext ctx;

        public SubscriptionRepository()
        {
            ctx = new BarometerDbContext();
        }

        /// <summary>
        /// Gives back a collection of alerts from a specific user.
        /// </summary>
        public IEnumerable<Alert> ReadAlerts(int userId)
        {
            IEnumerable<Subscription> userSubs = ctx.Subscriptions.Where(sub => sub.SubscribedUser.UserId == userId).AsEnumerable();

            List<Alert> alersToRead = new List<Alert>();
            foreach (Subscription sub in userSubs) alersToRead.AddRange(sub.Alerts);
            return alersToRead.AsEnumerable();
        }

        /// <summary>
        /// Gives back a collection of subscriptions form a specific item.
        /// </summary>
        public IEnumerable<Subscription> ReadSubscriptions(int itemId)
        {
            return ctx.Subscriptions.Where(sub => sub.SubscribedItem.ItemId == itemId).AsEnumerable();
        }

        /// <summary>
        /// Updates all the subscriptions when alerts are added.
        /// </summary>
        public void UpdateSubscriptions(IEnumerable<Subscription> subs)
        {
            foreach (Subscription sub in subs) ctx.Entry(sub).State = EntityState.Modified;
            ctx.SaveChanges();
        }
    }
}

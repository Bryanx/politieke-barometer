﻿using BAR.BL.Domain.Users;
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

        public IEnumerable<Alert> ReadAllAlerts(int userId)
        {           
            IEnumerable<Subscription> userSubs = subs.Where(sub => sub.SubscribedUser.UserId == userId);

            List<Alert> alersToRead = new List<Alert>();
            foreach (Subscription sub in userSubs) alersToRead.AddRange(sub.Alerts);
            return alersToRead.AsEnumerable();           
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
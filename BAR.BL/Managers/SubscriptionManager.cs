using System;
using BAR.DAL;
using BAR.BL.Domain.Users;
using System.Collections.Generic;

namespace BAR.BL.Managers
{
    /// <summary>
    /// Resposable for managing subcriptions
    /// and their alerts
    /// </summary>
    public class SubscriptionManager : ISubscriptionManager
    {
        public void GenerateAlerts(int itemId)
        {
            IItemManager itemManager = new ItemManager();
            double per = itemManager.GetTrendingPer(itemId);

            ISubscriptionRepository subRepo = new SubscriptionRepository();
            IEnumerable<Subscription> subs = subRepo.ReadSubscriptions(itemId);
            foreach (Subscription sub in subs)
            {
                double tresh = sub.Treshhold;
                if (tresh <= (per - 100))
                {
                    sub.AddAlert(new Alert());
                }
            }

            subRepo.UpdateSubscriptions(subs);
        }
    }
}

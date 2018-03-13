using BAR.BL.Managers;
using System;

namespace BAR.BL.Controllers
{
    /// <summary>
    /// Controller used for backgroundtasks.
    /// </summary>
    public class SysController
    {
        /// <summary>
        /// When new data arrives this method will be triggerd to determine
        /// a new baseline and a new trending percentage for a specific item.
        /// </summary>
        public void DetermineTrending(int itemId)
        {
            IItemManager itemManager = new ItemManager();
            itemManager.DetermineTrending(itemId);
        }

        /// <summary>
        /// When a new baseline and trendingpercentage is determined,
        /// Alerts will be generated.
        /// </summary>
        public void GenerateAlerts(int itemId)
        {
            ISubscriptionManager subManager = new SubscriptionManager();
            subManager.GenerateAlerts(itemId);
        }
    }
}

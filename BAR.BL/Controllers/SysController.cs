using BAR.BL.Managers;
using System;

namespace BAR.BL.Controllers
{
    /// <summary>
    /// Controller used for backgroundtasks.
    /// </summary>
    public class SysController
    {
        public void DetermineTrending(int itemId)
        {
            IItemManager itemManager = new ItemManager();
            itemManager.DetermineTrending(itemId);
        }

        public void GenerateAlerts(int itemId)
        {
            ISubscriptionManager subManager = new SubscriptionManager();
            subManager.GenerateAlerts(itemId);
        }
    }
}

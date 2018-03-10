using BAR.BL.Managers;
using System;

namespace BAR.BL.Controllers
{
    public class SysController
    {
        public void DetermineTrending(int itemId)
        {
            ItemManager itemManager = new ItemManager();
            itemManager.DetermineTrending(itemId);
        }

        public void GenerateAlerts(int itemId)
        {
            SubsriptionManager subManager = new SubsriptionManager();
            subManager.GenerateAlerts(itemId);
        }
    }
}

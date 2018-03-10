using System;
using BAR.DAL;
using BAR.BL.Domain.Items;

namespace BAR.BL.Managers
{
    /// <summary>
    /// Responsable for managing:
    /// Oraginsations
    /// Persons
    /// Themes
    /// </summary>
    public class ItemManager
    {
        /// <summary>
        /// Adjust the baseline of the given item
        /// Adjust the trendingpercentage of the given item
        /// </summary>
        public void DetermineTrending(int itemId)
        {
            DataManager dataManager = new DataManager();
            int aantalTrending = dataManager.GetAanatalInfo(itemId, DateTime.Now.AddDays(-1));
            int aantalBaseline = dataManager.GetAanatalInfo(itemId, DateTime.Now.AddMonths(-1));

            // bereken de baseline = aantal / aantal dagen since tot vandaag
            // 30 is ongeveer het gemiddelde van 1 maand.
            double baseline = aantalBaseline / 30;

            // Bereken het trendingpercentage = baseline / aantal dagen since tot vandaag
            // 30 is ongeveer het gemiddelde van 1 maand.
            double trendingPer = baseline / 30;

            ItemRepository itemRepo = new ItemRepository();
            itemRepo.UpdateItemTrending(itemId, baseline, trendingPer);
        }

        public double GetTrendingPer(int itemId)
        {
            ItemRepository itemRepo = new ItemRepository();
            Item item = itemRepo.ReadItem(itemId);
            return item.TrendingPercentage;
        }
    }
}

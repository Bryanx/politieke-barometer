using System;
using BAR.DAL;
using BAR.BL.Domain.Items;

namespace BAR.BL.Managers
{
    /// <summary>
    /// Responsable for managing:
    /// Organisations
    /// Persons
    /// Themes
    /// </summary>
    public class ItemManager : IItemManager
    {
        /// <summary>
        /// Adjust the baseline of the given item
        /// Adjust the trendingpercentage of the given item
        /// </summary>
        public void DetermineTrending(int itemId)
        {
            IDataManager dataManager = new DataManager();
            int aantalTrending = dataManager.GetAantalInfo(itemId, DateTime.Now.AddDays(-1));
            int aantalBaseline = dataManager.GetAantalInfo(itemId, DateTime.Now.AddMonths(-1));

            // bereken de baseline = aantal / aantal dagen since tot vandaag
            // 30 is ongeveer het gemiddelde van 1 maand.
            double baseline = aantalBaseline / 30;

            // Bereken het trendingpercentage = baseline / aantal dagen since tot vandaag
            // 30 is ongeveer het gemiddelde van 1 maand.
            double trendingPer = baseline / 30;

            IItemRepository itemRepo = new ItemRepository();
            itemRepo.UpdateItemTrending(itemId, baseline, trendingPer);
            itemRepo.UpdateLastUpdated(itemId, DateTime.Now); //Is not going to work yet. Repos are HC.
        }

        /// <summary>
        /// Gets the trending percentage of a specific item
        /// </summary>
        public double GetTrendingPer(int itemId)
        {
            IItemRepository itemRepo = new ItemRepository();
            Item item = itemRepo.ReadItem(itemId);
            return item.TrendingPercentage;
        }
    }
}

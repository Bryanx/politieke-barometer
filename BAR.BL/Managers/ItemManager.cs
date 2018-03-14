using System;
using BAR.DAL;
using BAR.BL.Domain.Items;
using System.Collections.Generic;

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
    private UnitOfWorkManager uowManager;
    private ItemRepository itemRepo;

    /// <summary>
    /// When unit of work is present, it will effect
    /// initRepo-method. (see documentation of initRepo)
    /// </summary>
    public ItemManager(UnitOfWorkManager uowManager = null)
    {
      this.uowManager = uowManager;
    }

    /// <summary>
    /// Adjust the baseline of the given item
    /// Adjust the trendingpercentage of the given item
    /// </summary>
    public void DetermineTrending(int itemId)
    {
      InitRepo();

      DataManager dataManager = new DataManager();
      int aantalTrending = dataManager.GetNumberInfo(itemId, DateTime.Now.AddDays(-1));
      int aantalBaseline = dataManager.GetNumberInfo(itemId, DateTime.Now.AddMonths(-20));

      // bereken de baseline = aantal / aantal dagen since tot vandaag
      // 30 is ongeveer het gemiddelde van 1 maand.
      double baseline = aantalBaseline / 30;

      // Bereken het trendingpercentage = baseline / aantal dagen since tot vandaag
      // 30 is ongeveer het gemiddelde van 1 maand.
      double trendingPer = Convert.ToDouble(aantalTrending) / baseline;

      itemRepo.UpdateItemTrending(itemId, baseline, trendingPer);
      itemRepo.UpdateLastUpdated(itemId, DateTime.Now); //Is not going to work yet. Repos are HC.
    }

    /// <summary>
    /// Returns an item from a specific itemId.
    /// </summary>
    public Item GetItem(int itemId)
    {
      InitRepo();
      return itemRepo.ReadItem(itemId);
    }

    /// <summary>
    /// Gets the trending percentage of a specific item
    /// </summary>
    public double GetTrendingPer(int itemId)
    {
      InitRepo();
      Item item = itemRepo.ReadItem(itemId);
      return item.TrendingPercentage;
    }

    public List<Item> getAllItems()
    {
      InitRepo();
      return itemRepo.ReadAllItems();
    }

    /// <summary>
    /// Determines if the repo needs a unit of work
    /// if the unitOfWorkManager is present
    /// </summary>
    private void InitRepo()
    {
      if (uowManager == null) itemRepo = new ItemRepository();
      else itemRepo = new ItemRepository(uowManager.UnitOfWork);
    }
  }
}

using BAR.BL.Domain.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.DAL
{
  public interface IItemRepository
  {
    void UpdateItemTrending(int itemId, double baseline, double trendingepr);
    void UpdateLastUpdated(int itemId, DateTime lastUpdated);
    Item ReadItemWithInformations(int itemId);
    Item ReadItem(int itemId);
    List<Item> ReadAllItems();
  }
}

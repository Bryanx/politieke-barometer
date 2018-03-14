using BAR.BL.Domain.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.Managers
{
    public interface IItemManager
    {
        void DetermineTrending(int itemId);
        double GetTrendingPer(int itemId);
		Item GetItem(int itemId);
    }
}

using BAR.BL.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.DAL
{
    /// <summary>
    /// Deze repository werkt op dit moment nog HC.
    /// </summary>
    public class SubscriptionRepository
    {
        public IEnumerable<Subscription> ReadSubscriptions(int itemId)
        {
            //TODO: implement logic
            return null;
        }

        public void UpdateSubscriptions(IEnumerable<Subscription> subs)
        {
            //TODO: implement logic
        }
    }
}

using BAR.BL.Domain.Users;
using System.Collections.Generic;

namespace BAR.BL.Managers
{
    public interface ISubscriptionManager
    {
        void GenerateAlerts(int itemId);
        IEnumerable<Alert> GetAllAlerts(int userId);
    }
}

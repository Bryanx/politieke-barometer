using System.Data.Entity;

namespace BAR.DAL.EF
{
    /// <summary>
    /// Configuration is needed to discard EF
    /// in the UI and introduce loose coupeling.
    /// </summary>
    public class BarometerDbConfiguration : DbConfiguration
    {
        public BarometerDbConfiguration()
        {
            SetDefaultConnectionFactory(new System.Data.Entity.Infrastructure.SqlConnectionFactory());

            SetProviderServices("System.Data.SqlClient", System.Data.Entity.SqlServer.SqlProviderServices.Instance);

            SetDatabaseInitializer<BarometerDbContext>(new BarometerInitializer());
        }
    }
}

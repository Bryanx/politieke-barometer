using System;
using System.Data.Entity;

namespace BAR.DAL.EF
{
    internal class BarometerInitializer : DropCreateDatabaseAlways<BarometerDbContext>
    {
        /// <summary>
        /// Dummy data form the json file will be generated
        /// in this file for the first wave of information
        /// </summary>       
        protected override void Seed(BarometerDbContext context)
        {
            //TODO: implement logic
        }
    }
}

using System;
using BAR.DAL;

namespace BAR.BL.Managers
{
    /// <summary>
    /// Responsable for working with 
    /// data we received from TextGain.
    /// </summary>
    public class DataManager : IDataManager
    {
        /// <summary>
        /// Gets the number of 
        /// </summary
        public int GetAanatalInfo(int itemId, DateTime since)
        {
            IInformationRepository infRepo = new InformationRepository();
            return infRepo.GetAantalInfo(itemId, since);
        }
                   
    }
}



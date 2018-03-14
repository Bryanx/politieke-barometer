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
        /// Gets the number of informations of a specific given item
        /// form since till now.
        /// </summary
        public int GetNumberInfo(int itemId, DateTime since)
        {
            IInformationRepository infRepo = new InformationRepository();
            return infRepo.ReadNumberInfo(itemId, since);
        }		
    }
}



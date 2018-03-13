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
<<<<<<< HEAD
        public int GetAantalInfo(int itemId, DateTime since)
=======
        public int GetNumberInfo(int itemId, DateTime since)
>>>>>>> f34955d8e48a371814e65d354b49033fe6d64af3
        {
            IInformationRepository infRepo = new InformationRepository();
            return infRepo.ReadNumberInfo(itemId, since);
        }

        public int GetAanatalInfo(int itemId, DateTime since) {
            throw new NotImplementedException();
        }
    }
}



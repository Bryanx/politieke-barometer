using System;
using BAR.DAL;

namespace BAR.BL.Managers
{
    public class DataManager
    {

        public int GetAanatalInfo(int itemId, DateTime since)
        {
            InformationRepository infRepo = new InformationRepository();
            return infRepo.getAantalInfo(itemId, since);
        }
                   
    }
}



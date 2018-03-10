using BAR.BL.Domain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.DAL
{
    /// <summary>
    /// At this moment the repository works HC.
    /// </summary>
    public class InformationRepository
    {
        private List<Information> information;

        public InformationRepository()
        {
            information = new List<Information>();
        }

        public int GetAantalInfo(int itemId, DateTime since)
        {
            //TODO: implement logic
            return 0;
        }
    }
}

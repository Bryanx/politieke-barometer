using BAR.BL.Domain.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.Domain.Data
{
    public class Information
    {
        public int InformationId { get; set; }
        public Item Item { get; set; }
        public Source Source { get; set; }
        public DateTime? CreatetionDate { get; set; }
        public ICollection<PropertyValue> PropertieValues { get; set; }
    }

}

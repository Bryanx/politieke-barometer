using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.Domain.Items
{
    public class SocialMediaUrl
    {
        [Key]
        public int SocialMediaId { get; set; }
        public string Url { get; set; }
        public string Source { get; set; }
    }
}

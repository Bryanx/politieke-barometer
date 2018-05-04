using BAR.BL.Domain.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.Domain.Items
{
    public class SocialMediaName
    {
        public int SocialMediaNameId { get; set; }
        public string Username { get; set; }
        public Source Source { get; set; }
    }
}

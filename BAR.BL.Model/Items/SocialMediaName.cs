using BAR.BL.Domain.Data;

namespace BAR.BL.Domain.Items
{
    public class SocialMediaName
    {
        public int SocialMediaNameId { get; set; }
        public string Username { get; set; }
        public Source Source { get; set; }
    }
}

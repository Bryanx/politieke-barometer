using System.Collections.Generic;
using BAR.BL.Domain.Widgets;

namespace BAR.UI.MVC.Models {
    public class AddWidgetDTO {
        public ICollection<int> ItemIds { get; set; }
        public string ItemName { get; set; }
        public string PropertyTag { get; set; }
        public string Title { get; set; }
        public GraphType GraphType { get; set; }
    }
}
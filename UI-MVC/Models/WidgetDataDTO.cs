using System.Collections.Generic;
using BAR.BL.Domain.Widgets;

namespace BAR.UI.MVC.Models {
    public class WidgetDataDTO {
        public int WidgetDataId { get; set; }
        public string KeyValue { get; set; }
        public ICollection<GraphValue> GraphValues { get; set; }
        public int WidgetId { get; set; }
        public string ItemName { get; set; }
    }
}
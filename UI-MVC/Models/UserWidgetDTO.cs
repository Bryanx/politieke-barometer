﻿using BAR.BL.Domain.Widgets;

namespace BAR.UI.MVC.Models {
    public class UserWidgetDTO {
        public int WidgetId { get; set; }
        public string Title { get; set; }
        public int ColumnSpan { get; set; }
        public int RowSpan { get; set; }
        public int ColumnNumber { get; set; }
        public int RowNumber { get; set; }
        public WidgetType WidgetType { get; set; }
        public int DashboardId { get; set; }
    }
}
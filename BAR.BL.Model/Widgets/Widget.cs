using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.Domain.Widgets
{
    public class Widget
    {
        public int WidgetId { get; set; }
        public string Title { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int Xpos { get; set; }
        public int Ypos { get; set; }
    }
}

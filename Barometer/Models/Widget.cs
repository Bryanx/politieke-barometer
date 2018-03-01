using System.Collections.Generic;
using System.Linq;

namespace Barometer.Models {
    public class Widget {
        public int Id { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }

        public Widget(int id, int x, int y, int width, int height) {
            Id = id;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public static IEnumerable<Widget> getWidgets() {
            List<Widget> list = new List<Widget>();
            list.Add(new Widget(1,0,0,6,4));
            list.Add(new Widget(2,0,0,6,4));
            list.Add(new Widget(3,0,0,4,4));
            list.Add(new Widget(4,0,0,4,4));
            list.Add(new Widget(5,0,0,4,4));
            return list.ToList();
        }
    }
}
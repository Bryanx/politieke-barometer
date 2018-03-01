using System.Collections.Generic;
using System.Linq;

namespace Barometer.Models {
    public class Widget {
        public int Id { get; set; }
        public string Title { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public Graph Graph { get; set; }

        public Widget(int id, string title, int x, int y, int width, int height, Graph graph) {
            Id = id;
            Title = title;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.Graph = graph;
        }

        public static IEnumerable<Widget> getWidgets() {
            List<Widget> list = new List<Widget>();
            list.Add(new Widget(1,"Anthony",0,0,6,4, new Graph("donut")));
            list.Add(new Widget(2,"Bryan",0,0,6,4, new Graph("line")));
            list.Add(new Widget(3,"Maarten",0,0,4,4, new Graph("bar")));
            list.Add(new Widget(4,"Yoni",0,0,4,4, new Graph("line")));
            list.Add(new Widget(5,"Remi",0,0,4,4, new Graph("donut")));
            return list.ToList();
        }
    }
}
using System.Collections.Generic;
using System.Linq;

namespace Barometer.Models {
    public class Widget {
    
        public int Id { get; set; }
        public string Title { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Graph Graph { get; set; }

        public Widget(int id, string title, int x, int y, int width, int height, Graph graph) {
            Id = id;
            Title = title;
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
            this.Graph = graph;
        }
    }
}
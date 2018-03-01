namespace Barometer.Models {
    public class Graph {
        public string type { get; set; }

        public Graph(string type) {
            this.type = type.ToLower();
        }
    }
}
using System.Collections.Generic;

namespace BAR.UI.MVC.Models {
    /// <summary>
    /// temp model
    /// </summary>
    public class Graph {
        public string Type { get; set; }
        public string[] DonutLabels { get; set; }
        public int[] DonutValues { get; set; }
        

        public Graph(string type) {
            this.Type = type.ToLower();
        }

        public Graph(string type, string[] donutLabels, int[] donutValues) {
            Type = type;
            DonutLabels = donutLabels;
            DonutValues = donutValues;
        }
    }
}
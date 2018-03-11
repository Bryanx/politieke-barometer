using System.Collections.Generic;
using System.Linq;
using Barometer.Models;

namespace Barometer.DAL {
    public class WidgetManager {
        private static List<Widget> mgr;

        static WidgetManager() {
            mgr = new List<Widget>();
            mgr.Add(new Widget(1,"Anthony",0,0,6,4, new Graph("donut", new string[] {"jarne","bryan","maarten","yoni"}, new int[] {30,15,45,10})));
            mgr.Add(new Widget(2,"Bryan",0,0,6,4, new Graph("line")));
            mgr.Add(new Widget(3,"Maarten",0,0,4,4, new Graph("bar")));
            mgr.Add(new Widget(4,"Yoni",0,0,4,4, new Graph("line")));
            mgr.Add(new Widget(5,"Remi",0,0,4,4, new Graph("donut")));
        }

        public IEnumerable<Widget> Read() {
            return mgr.ToList();
        }

        public Widget Read(int id) {
            return mgr.SingleOrDefault(c => c.Id == id);
        }

        public bool Exists(int id) {
            return mgr.Exists(c => c.Id == id);
        }

        public void Insert(Widget widget) {
            mgr.Add(widget);
        }

        public void Update(Widget widget) {
            Widget widgetFromMgr = mgr.Find(c => c.Id == widget.Id);
            if (widgetFromMgr != null) {
                widgetFromMgr.X = widget.X;
                widgetFromMgr.Y = widget.Y;
                widgetFromMgr.Title = widget.Title;
                widgetFromMgr.Width = widget.Width;
                widgetFromMgr.Height = widget.Height;
//                WidgetFrommgr.Graph = widget.Graph;
            }
        }

        public void Update(int id, string title) {
            Widget widgetFromMgr = mgr.Find(c => c.Id == id);
            if (widgetFromMgr != null) widgetFromMgr
            .Title = title;
        }

        public void Delete(int id) {
            Widget widgetToDelete = Read(id);
            if (widgetToDelete != null) mgr.Remove(widgetToDelete);
        }
    }
}
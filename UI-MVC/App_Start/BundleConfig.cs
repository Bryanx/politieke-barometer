using System.Web;
using System.Web.Optimization;

namespace BAR.UI.MVC {
    public class BundleConfig {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles) {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-2.2.4.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*"));
            
            bundles.Add(new ScriptBundle("~/bundles/flot").Include(
                "~/vendors/Flot/jquery.flot.js",
                "~/vendors/Flot/jquery.flot.pie.js",
                "~/vendors/Flot/jquery.flot.time.js",
                "~/vendors/Flot/jquery.flot.stack.js",
                "~/vendors/Flot/jquery.flot.resize.js",
                "~/vendors/flot.orderbars/js/jquery.flot.orderBars.js",
                "~/vendors/flot-spline/js/jquery.flot.spline.min.js",
                "~/vendors/flot.curvedlines/curvedLines.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/map").Include(
                "~/vendors/jqvmap/dist/jquery.vmap.min.js",
                "~/vendors/jqvmap/dist/maps/jquery.vmap.belgium.js",
                "~/vendors/jqvmap/examples/js/jquery.vmap.sampledata.js")); //TODO: replace smapledata with actual data.
            
            bundles.Add(new ScriptBundle("~/bundles/timeago").Include(
                "~/Scripts/jquery.timeago.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/dates").Include(
                "~/vendors/DateJS/build/date.js",
                "~/vendors/moment/min/moment.min.js",
                "~/vendors/bootstrap-daterangepicker/daterangepicker.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js",
                "~/Scripts/bootstrap-progressbar.min.js",
                "~/Scripts/nprogress.js",
                "~/Scripts/fastclick.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/gridstack").Include(
                "~/Scripts/jquery-ui.js",
                "~/Scripts/underscore.min.js",
                "~/Scripts/gridstack.js",
                "~/Scripts/gridstack.jQueryUI.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/morrisCharts").Include(
                "~/vendors/raphael/raphael.min.js",
                "~/vendors/morris.js/morris.min.js"
                ));
            
            bundles.Add(new ScriptBundle("~/bundles/pnotify").Include(
                "~/vendors/pnotify/dist/pnotify.js",
                "~/vendors/pnotify/dist/pnotify.buttons.js",
                "~/vendors/pnotify/dist/pnotify.nonblock.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
                "~/Content/build/js/custom.js",
                "~/Content/build/js/searchbar.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css"));
        }
    }
}
using System.Web.Optimization;

namespace BAR.UI.MVC {
    public class BundleConfig {

        /// Script fields
        private static readonly string[] Jquery = {
            "~/Scripts/jquery/jquery-3.3.1.min.js",
            "~/Scripts/jquery/jquery.autocomplete.min.js"
        };
        private static readonly string JqueryValidate ="~/Scripts/jquery/jquery.validate*";
        private static readonly string Timeago ="~/Scripts/jquery/jquery.timeago.js";
        private static readonly string Modernizr ="~/Scripts/modernizr-*";
        private static readonly string[] Bootstrap = {
            "~/Scripts/bootstrap.js",
        };
        private static readonly string[] Gridstack = {
            "~/Scripts/jquery/jquery-ui.js",
            "~/Scripts/underscore.min.js",
            "~/Scripts/gridstack/gridstack.js",
            "~/Scripts/gridstack/gridstack.jQueryUI.js",
            "~/Content/build/js/widgets.js"
        };
        private static readonly string[] Jqvmap = {
            "~/Scripts/jqvmap/jquery.vmap.min.js",
            "~/Scripts/jqvmap/jquery.vmap.belgium.js",
            "~/Scripts/jqvmap/jquery.vmap.sampledata.js"
        };
        private static readonly string[] Flot = {
            "~/Scripts/flot/jquery.flot.js",
            "~/Scripts/flot/jquery.flot.pie.js",
            "~/Scripts/flot/jquery.flot.time.js",
            "~/Scripts/flot/jquery.flot.stack.js",
            "~/Scripts/flot/jquery.flot.resize.js",
            "~/Scripts/flot/jquery.flot.orderBars.js",
            "~/Scripts/flot/jquery.flot.spline.min.js",
            "~/Scripts/flot/curvedLines.js"
        };        
        private static readonly string[] Dates = {
            "~/Scripts/dates/date.min.js",
            "~/Scripts/dates/moment.min.js",
            "~/Scripts/dates/daterangepicker.js"
        };
        private static readonly string[] MorrisCharts = {
            "~/Scripts/morrisCharts/raphael.min.js",
            "~/Scripts/morrisCharts/morris.min.js"
        };
        private static readonly string[] DataTables = {
            "~/Scripts/datatables/datatables.net/js/jquery.dataTables.min.js",
            "~/Scripts/datatables/datatables.net-bs/js/dataTables.bootstrap.min.js",
            
        };
        private static readonly string[] CustomScripts = {
            "~/Content/build/js/template.js",
            "~/Content/build/js/searchbar.js",
            "~/Content/build/js/custom.js"
        };
        
        /// Style fields
        private static readonly string[] DefaultCss = {
            "~/Content/build/css/vendor/bootstrap.min.css",
            "~/Content/build/css/vendor/font-awesome.min.css",
            "~/Content/build/css/vendor/nprogress.css",
            "~/Content/build/css/vendor/bootstrap-progressbar-3.3.4.min.css",
            "~/Content/build/css/vendor/jqvmap.min.css"
        };
        private static readonly string[] DatatablesCss = {
            "~/Scripts/datatables/datatables.net-bs/css/dataTables.bootstrap.min.css",
            "~/Scripts/datatables/datatables.net-responsive-bs/css/responsive.bootstrap.min.css"
        };
        private static readonly string CustomCss ="~/Content/build/css/custom.css";

        public static void RegisterBundles(BundleCollection bundles) {
            //Scripts
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(Jquery));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(JqueryValidate));
            bundles.Add(new ScriptBundle("~/bundles/flot").Include(Flot));
            bundles.Add(new ScriptBundle("~/bundles/map").Include(Jqvmap));
            bundles.Add(new ScriptBundle("~/bundles/timeago").Include(Timeago));
            bundles.Add(new ScriptBundle("~/bundles/dates").Include(Dates));
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(Modernizr));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(Bootstrap));
            bundles.Add(new ScriptBundle("~/bundles/gridstack").Include(Gridstack));
            bundles.Add(new ScriptBundle("~/bundles/morrisCharts").Include(MorrisCharts));
            bundles.Add(new ScriptBundle("~/bundles/custom").Include(CustomScripts));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap-wysiwyg").IncludeDirectory("~/Scripts/bootstrap-wysiwyg", "*.js"));
            
            //Script tables
            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(DataTables));
            bundles.Add(new ScriptBundle("~/bundles/datatables-responsive").IncludeDirectory("~/Scripts/datatables/datatables.net-responsive-bs", "*.js", true));
            bundles.Add(new ScriptBundle("~/bundles/datatables-buttons").IncludeDirectory("~/Scripts/datatables/datatables.net-buttons", "*.js", true));
            bundles.Add(new ScriptBundle("~/bundles/datatables-buttons-bs").IncludeDirectory("~/Scripts/datatables/datatables.net-buttons-bs", "*.js", true));
            
            //Styles
            bundles.Add(new StyleBundle("~/Content/css").Include(DefaultCss));
            bundles.Add(new StyleBundle("~/Content/custom").Include(CustomCss));
            bundles.Add(new StyleBundle("~/bundles/datatables-css").Include(DatatablesCss));
            bundles.Add(new StyleBundle("~/bundles/datatables-buttons-css").IncludeDirectory("~/Scripts/datatables/datatables.net-buttons-bs", "*.css", true));
        }
    }
}
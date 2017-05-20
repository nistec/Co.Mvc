using System.Web;
using System.Web.Optimization;

namespace Pro.Mvc
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

            bundles.Add(new ScriptBundle("~/bundles/jqwidgets").Include(
            "~/jqwidgets/jqxcore.js",
            "~/jqwidgets/jqxdata.js",
            "~/jqwidgets/jqxgrid.js",
            "~/jqwidgets/jqxgrid.selection.js",
            "~/jqwidgets/jqxgrid.pager.js",
            "~/jqwidgets/jqxlistbox.js",
            "~/jqwidgets/jqxbuttons.js",
            "~/jqwidgets/jqxscrollbar.js",
            "~/jqwidgets/jqxdatatable.js",
            "~/jqwidgets/jqxtreegrid.js",
            "~/jqwidgets/jqxmenu.js",
            "~/jqwidgets/jqxcalendar.js",
            "~/jqwidgets/jqxgrid.sort.js",
            "~/jqwidgets/jqxgrid.filter.js",
            "~/jqwidgets/jqxdatetimeinput.js",
            "~/jqwidgets/jqxdropdownlist.js",
            "~/jqwidgets/jqxslider.js",
            "~/jqwidgets/jqxeditor.js",
            "~/jqwidgets/jqxinput.js",
            "~/jqwidgets/jqxdraw.js",
            "~/jqwidgets/jqxchart.core.js",
            "~/jqwidgets/jqxchart.rangeselector.js",
            "~/jqwidgets/jqxtree.js",
            "~/jqwidgets/globalization/globalize.js",
            "~/jqwidgets/jqxbulletchart.js",
            "~/jqwidgets/jqxcheckbox.js",
            "~/jqwidgets/jqxradiobutton.js",
            "~/jqwidgets/jqxvalidator.js",
            "~/jqwidgets/jqxpanel.js",
            "~/jqwidgets/jqxpasswordinput.js",
            "~/jqwidgets/jqxnumberinput.js",
            "~/jqwidgets/jqxcombobox.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/jqx").Include(
            "~/jqwidgets/jqxcore.js",
            "~/jqwidgets/jqxdata.js", 
            "~/jqwidgets/jqxbuttons.js",
            "~/jqwidgets/jqxscrollbar.js",
            "~/jqwidgets/jqxlistbox.js",
            "~/jqwidgets/jqxdropdownlist.js",
            "~/jqwidgets/jqxcombobox.js",
            "~/jqwidgets/jqxmenu.js",
            "~/jqwidgets/jqxpanel.js",
            "~/jqwidgets/jqxexpander.js",
            "~/jqwidgets/jqxinput.js",
            "~/jqwidgets/jqxdatetimeinput.js",
            "~/jqwidgets/jqxcalendar.js",
            "~/jqwidgets/jqxcheckbox.js",
            "~/jqwidgets/jqxvalidator.js",
            "~/jqwidgets/jqxnumberinput.js",
            "~/jqwidgets/jqxdropdownbutton.js",
            "~/jqwidgets/jqxwindow.js",
            "~/jqwidgets/jqxtooltip.js",
            "~/jqwidgets/jqxNotification.js",
            "~/jqwidgets/jqxtabs.js"
            ));
 
            bundles.Add(new ScriptBundle("~/bundles/jqxgrid").Include(
            "~/jqwidgets/jqxgrid.js",
            "~/jqwidgets/jqxgrid.sort.js",
            "~/jqwidgets/jqxgrid.selection.js",
            "~/jqwidgets/jqxgrid.storage.js",
            "~/jqwidgets/jqxgrid.pager.js",
            "~/jqwidgets/jqxgrid.edit.js",
            "~/jqwidgets/jqxgrid.filter.js",
            "~/jqwidgets/jqxgrid.columnsresize.js",
            "~/jqwidgets/jqxgrid.columnsreorder.js",
            "~/jqwidgets/jqxgrid.export.js",
            "~/jqwidgets/jqxdatatable.js"
            ));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace CustomWebDeployParameters {
    public class BundleConfig {
        public static void RegisterBundles(BundleCollection bundles) {
            Bundle jsBundle = new Bundle("~/bundles/WebFormsJs", new JsMinify());
            jsBundle.Include("~/Scripts/WebForms/WebForms.js");
            jsBundle.Include("~/Scripts/WebForms/MenuStandards.js");
            jsBundle.Include("~/Scripts/WebForms/Focus.js");
            jsBundle.Include("~/Scripts/WebForms/GridView.js");
            jsBundle.Include("~/Scripts/WebForms/DetailsView.js");
            jsBundle.Include("~/Scripts/WebForms/TreeView.js");
            jsBundle.Include("~/Scripts/WebForms/WebParts.js");
            bundles.Add(jsBundle);

            // Order is very important for these files to work, they have explicit dependencies
            Bundle ajaxBundle = new Bundle("~/bundles/MsAjaxJs", new JsMinify());
            ajaxBundle.Include("~/Scripts/WebForms/MsAjax/MicrosoftAjax.js");
            ajaxBundle.Include("~/Scripts/WebForms/MsAjax/MicrosoftAjaxCore.js");
            ajaxBundle.Include("~/Scripts/WebForms/MsAjax/MicrosoftAjaxSerialization.js");
            ajaxBundle.Include("~/Scripts/WebForms/MsAjax/MicrosoftAjaxNetwork.js");
            ajaxBundle.Include("~/Scripts/WebForms/MsAjax/MicrosoftAjaxWebServices.js");
            ajaxBundle.Include("~/Scripts/WebForms/MsAjax/MicrosoftAjaxApplicationServices.js");
            ajaxBundle.Include("~/Scripts/WebForms/MsAjax/MicrosoftAjaxComponentModel.js");
            ajaxBundle.Include("~/Scripts/WebForms/MsAjax/MicrosoftAjaxGlobalization.js");
            ajaxBundle.Include("~/Scripts/WebForms/MsAjax/MicrosoftAjaxTimer.js");
            ajaxBundle.Include("~/Scripts/WebForms/MsAjax/MicrosoftAjaxHistory.js");
            ajaxBundle.Include("~/Scripts/WebForms/MsAjax/MicrosoftAjaxWebForms.js");
            bundles.Add(ajaxBundle);

            Bundle cssBundle = new Bundle("~/Content/css", new CssMinify());
            cssBundle.Include("~/Content/site.css");
            bundles.Add(cssBundle);

            Bundle jqueryUiCssBundle = new Bundle("~/Content/themes/base/css", new CssMinify());
            jqueryUiCssBundle.Include("~/Content/themes/base/jquery.ui.core.css");
            jqueryUiCssBundle.Include("~/Content/themes/base/jquery.ui.resizable.css");
            jqueryUiCssBundle.Include("~/Content/themes/base/jquery.ui.selectable.css");
            jqueryUiCssBundle.Include("~/Content/themes/base/jquery.ui.accordion.css");
            jqueryUiCssBundle.Include("~/Content/themes/base/jquery.ui.autocomplete.css");
            jqueryUiCssBundle.Include("~/Content/themes/base/jquery.ui.button.css");
            jqueryUiCssBundle.Include("~/Content/themes/base/jquery.ui.dialog.css");
            jqueryUiCssBundle.Include("~/Content/themes/base/jquery.ui.slider.css");
            jqueryUiCssBundle.Include("~/Content/themes/base/jquery.ui.tabs.css");
            jqueryUiCssBundle.Include("~/Content/themes/base/jquery.ui.datepicker.css");
            jqueryUiCssBundle.Include("~/Content/themes/base/jquery.ui.progressbar.css");
            jqueryUiCssBundle.Include("~/Content/themes/base/jquery.ui.theme.css");
            bundles.Add(jqueryUiCssBundle);

        }
    }
}
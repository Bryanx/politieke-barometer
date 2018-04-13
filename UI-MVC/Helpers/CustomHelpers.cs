using System.Collections.Generic;
using System.Web.Mvc;
using MvcSiteMapProvider.Collections.Specialized;

namespace BAR.UI.MVC.Helpers {
    public static class CustomHelpers {
        
        /// <summary>
        /// Returns an img tag with optional parameters.
        /// </summary>
        public static MvcHtmlString ImageHelper(this HtmlHelper helper, string src, string alt = null,
            string title = null, object htmlAttributes = null) {
            var imgTag = new TagBuilder("img");
            imgTag.Attributes.Add("src", src);
            if (alt != null) imgTag.Attributes.Add("alt", alt);
            if (title != null) imgTag.Attributes.Add("title", title);

            if (htmlAttributes != null) {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                imgTag.MergeAttributes(attributes);
            }

            return MvcHtmlString.Create(imgTag.ToString(TagRenderMode.Normal));
        }
    }
}
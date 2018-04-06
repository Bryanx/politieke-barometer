using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using BAR.BL.Domain.Users;

namespace BAR.UI.MVC.Helpers {
    public static class WebGridHelpers {
        public static MvcHtmlString EditableTextBox(this HtmlHelper helper,
            string value, User user, string name) {
            // Text Display
            var span = new TagBuilder("span") {InnerHtml = value};
            span.AddCssClass("cell-value");

            // Input display.
            var formatName = HtmlHelper.GenerateIdFromName(name);

            var uniqueId = String.Format("{0}_{1}", formatName, user.Id);

            var input = helper.TextBox(uniqueId,
                value, new {@class = "hide input-sm"});

            var result = String.Concat(
                span.ToString(TagRenderMode.Normal),
                input.ToHtmlString()
            );

            return MvcHtmlString.Create(result);
        }

        public static MvcHtmlString EditableDateTime(this HtmlHelper helper,
            DateTime value, User user, string name) {
            // Text Display
            var span = new TagBuilder("span") {InnerHtml = value.ToString("yyyy-MM-dd")};
            span.AddCssClass("cell-value");

            // Input display.
            var formatName = HtmlHelper.GenerateIdFromName(name);

            var uniqueId = String.Format("{0}_{1}", formatName, user.Id);

            var input = helper.TextBox(uniqueId, value.ToString("yyyy-MM-dd"),
                new {@type = "date", @class = "hide input-sm"});

            var result = String.Concat(
                span.ToString(TagRenderMode.Normal),
                input.ToHtmlString()
            );
            return MvcHtmlString.Create(result);
        }
    }
}
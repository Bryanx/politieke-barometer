using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BAR.BL.Domain.Items;
using BAR.UI.MVC.Models;

namespace BAR.UI.MVC.Helpers
{
	public static class CustomHelpers
	{
		/// <summary>
		/// Returns an img tag with optional parameters.
		/// </summary>
		public static MvcHtmlString ImageHelper(this HtmlHelper helper, string src, string alt = null,
			string title = null, object htmlAttributes = null)
		{
			var imgTag = new TagBuilder("img");
			imgTag.Attributes.Add("src", src);
			if (alt != null) imgTag.Attributes.Add("alt", alt);
			if (title != null) imgTag.Attributes.Add("title", title);

			if (htmlAttributes != null)
			{
				var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
				imgTag.MergeAttributes(attributes);
			}

			return MvcHtmlString.Create(imgTag.ToString(TagRenderMode.Normal));
		}

		public static MvcHtmlString LoadProfilePicture(this HtmlHelper helper, ItemViewModels.PersonViewModel model,
			string size, object htmlAttributes = null)
		{
			string src = "/Person/Picture?itemId=" + model.Item.ItemId;
			string errorUrl = GetOnErrorUrl(model, size);
			string onerror = "this.onload = null; this.src='" + errorUrl + "';";

			var imgTag = new TagBuilder("img");
			if (model.Item.Picture == null) imgTag.Attributes.Add("src", errorUrl);
			else imgTag.Attributes.Add("src", src);
			imgTag.Attributes.Add("onerror", onerror);

			if (htmlAttributes != null)
			{
				var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
				imgTag.MergeAttributes(attributes);
			}

			return MvcHtmlString.Create(imgTag.ToString(TagRenderMode.Normal));
		}

		private static string GetOnErrorUrl(ItemViewModels.PersonViewModel model, string size)
		{
			try
			{
				SocialMediaName socialMediaName = model.SocialMediaNames.FirstOrDefault(s => s.Source.Name == "Twitter");
				string imageUrl = "https://twitter.com/" + socialMediaName.Username + "/profile_image?size=" + size;

				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(imageUrl);
				using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
				{
					if (response.StatusCode == HttpStatusCode.OK) return response.ResponseUri.AbsoluteUri;
					return "/Content/build/images/picture.jpg";
				}
			}
#pragma warning disable CS0168 // Variable is declared but never used
			catch (Exception e)
#pragma warning restore CS0168 // Variable is declared but never used
			{
				return "/Content/build/images/picture.jpg";
			}
		}
	}
}
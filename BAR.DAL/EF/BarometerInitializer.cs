using BAR.BL.Domain.Core;
using BAR.BL.Domain.Data;
using BAR.BL.Domain.Items;
using BAR.BL.Domain.Users;
using BAR.BL.Domain.Widgets;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;

namespace BAR.DAL.EF
{
	internal class BarometerInitializer : CreateDatabaseIfNotExists<BarometerDbContext>
	{
		/// <summary>
		/// Dummy data from the json file will be generated
		/// in this file for the first wave of information.
		/// </summary>
		protected override void Seed(BarometerDbContext ctx)
		{
			GenerateSubPlatforms(ctx);
			GenerateSources(ctx);
			GenerateProperties(ctx);
			GenerateAreas(ctx);
			GenerateActivityWidgets(ctx);
		}

		/// <summary>
		/// Generates widgets for displaying the activities that happen on the website
		/// </summary>
		private void GenerateActivityWidgets(BarometerDbContext ctx)
		{
			List<PropertyTag> tags1 = new List<PropertyTag>();
			PropertyTag tag1 = new PropertyTag()
			{
				Name = "activity login"
			};
			tags1.Add(tag1);

			Widget loginWidget = new ItemWidget()
			{
				Title = "Login activities",
				RowNumber = 1,
				ColumnNumber = 1,
				RowSpan = 12,
				ColumnSpan = 6,
				WidgetDatas = new List<WidgetData>(),
				WidgetType = WidgetType.GraphType,
				GraphType = GraphType.LineChart,
				PropertyTags = tags1
			};
			ctx.Widgets.Add(loginWidget);

			List<PropertyTag> tags2 = new List<PropertyTag>();
			PropertyTag tag2 = new PropertyTag()
			{
				Name = "activity register"
			};
			tags2.Add(tag2);

			Widget registerWidget = new ItemWidget()
			{
				Title = "Register activities",
				RowNumber = 1,
				ColumnNumber = 1,
				RowSpan = 12,
				ColumnSpan = 6,
				WidgetDatas = new List<WidgetData>(),
				WidgetType = WidgetType.GraphType,
				GraphType = GraphType.LineChart,
				PropertyTags = tags2
			};
			ctx.Widgets.Add(registerWidget);

			List<PropertyTag> tags3 = new List<PropertyTag>();
			PropertyTag tag3 = new PropertyTag()
			{
				Name = "activity visit"
			};
			tags3.Add(tag3);

			Widget visitWidget = new ItemWidget()
			{
				Title = "Visit activities",
				RowNumber = 1,
				ColumnNumber = 1,
				RowSpan = 12,
				ColumnSpan = 6,
				WidgetDatas = new List<WidgetData>(),
				WidgetType = WidgetType.GraphType,
				GraphType = GraphType.LineChart,
				PropertyTags = tags3
			};
			ctx.Widgets.Add(visitWidget);

			ctx.SaveChanges();
		}

		/// <summary>
		/// Generates test data for K3 subplatform
		/// </summary>
		private void GenerateK3(BarometerDbContext ctx)
		{
			Person Marthe = new Person()
			{
				Name = "Marthe De Pillecyn",
				CreationDate = DateTime.Now,
				Baseline = 0,
				TrendingPercentage = 0,
				SubPlatform = ctx.SubPlatforms.Where(sp => sp.Name.Equals("k3")).SingleOrDefault()
			};

			Person Wever = new Person()
			{
				Name = "Bart de Wever",
				CreationDate = DateTime.Now,
				Baseline = 0,
				TrendingPercentage = 0,
				SubPlatform = ctx.SubPlatforms.Where(sp => sp.Name.Equals("politiek")).SingleOrDefault()
			};

			ctx.Items.Add(Wever);
			ctx.Items.Add(Marthe);
			ctx.SaveChanges();
		}

		/// <summary>
		/// Generates some subplatforms.
		/// </summary>
		private void GenerateSubPlatforms(BarometerDbContext ctx)
		{

			SubPlatform subPlatform2 = new SubPlatform
			{
				Name = "politiek",
				CreationDate = DateTime.Now,
				Customization = AddDefaultCustomization(),
				Questions = new List<Question>(),
			};
			ctx.SubPlatforms.Add(subPlatform2);

			SubPlatform subPlatform1 = new SubPlatform
			{
				Name = "k3",
				CreationDate = DateTime.Now,
				Customization = AddDefaultCustomization(),
				Questions = new List<Question>(),
			};
			ctx.SubPlatforms.Add(subPlatform1);

			ctx.SaveChanges();
		}

		/// <summary>
		/// Adds a default customization
		/// </summary>
		private Customization AddDefaultCustomization()
		{
			Customization custom = new Customization()
			{
				//Colors
				PrimaryColor = "#0f8ec4",
				PrimaryDarkerColor = "#0e85b8",
				PrimaryDarkestColor = "#0d77a4",
				SecondaryColor = "#303E4D",
				SecondaryDarkerColor = "#2a3744",
				SecondaryDarkestColor = "#1f2e3f",
				SecondaryLighterColor = "#3c3c3c",
				TertiaryColor = "#278e87",
				BackgroundColor = "#f7f7f7",

				WhiteColor = "#fff",
				LinkColor = "#5A738E",
				TextColor = "#73879C",


				//Navbar and title text
				PersonAlias = "Persoon",
				PersonsAlias = "Personen",
				OrganisationAlias = "Organisatie",
				OrganisationsAlias = "Organisaties",
				ThemeAlias = "Thema",
				ThemesAlias = "Thema's",

				//Privacy
				PrivacyTitle = "Privacy policy",
				PrivacyText = "Copyright " + DateTime.Now.Year,

				//FAQ
				FAQTitle = "Frequently Asked Questions (FAQ)",

				//Contact properties
				StreetAndHousenumber = "Nationalestraat 24",
				Zipcode = "2060",
				City = "Antwerpen",
				Country = "België",
				Email = "contact@politiekebarometer.be",

				//HeaderImage
				HeaderImage = Encoding.ASCII.GetBytes(""),
				LogoImage = Encoding.ASCII.GetBytes(""),
				DarkLogoImage = Encoding.ASCII.GetBytes("")
			};

			return custom;
		}

		/// <summary>
		/// Generates all of the sources we get our information from.
		/// </summary>
		private void GenerateSources(BarometerDbContext ctx)
		{
			Source twitter = new Source
			{
				Name = "Twitter",
				Site = "https://twitter.com/"
			};
			ctx.Sources.Add(twitter);
			Source facebook = new Source
			{
				Name = "Facebook",
				Site = "https://www.facebook.com/"
			};
			ctx.Sources.Add(facebook);
			DataSource textgain = new DataSource
			{
				Name = "Textgain",
				Url = "https://kdg.textgain.com/query",
				DataSourceId = 1,
				Interval = 60,
				SetTime = "0900",
				LastTimeChecked = DateTime.Now

			};
			ctx.DataSources.Add(textgain);
			ctx.SaveChanges();
		}

		/// <summary>
		/// Performance gain if you put all items in a list
		/// and then in the context with addRange();
		/// </summary>
		private void GenerateProperties(BarometerDbContext ctx)
		{
			List<Property> propertiesList = new List<Property>();

			Property gender = new Property
			{
				Name = "Gender"
			};
			Property age = new Property
			{
				Name = "Age"
			};
			Property education = new Property
			{
				Name = "Education"
			};
			Property language = new Property
			{
				Name = "Language"
			};
			Property personality = new Property
			{
				Name = "Personality"
			};
			Property word = new Property
			{
				Name = "Word"
			};
			Property sentiment = new Property
			{
				Name = "Sentiment"
			};
			Property hashtag = new Property
			{
				Name = "Hashtag"
			};
			Property mention = new Property
			{
				Name = "Mention"
			};
			Property url = new Property
			{
				Name = "Url"
			};
			Property date = new Property
			{
				Name = "Date"
			};
			Property geo = new Property
			{
				Name = "Geo"
			};
			Property postId = new Property
			{
				Name = "PostId"
			};
			Property retweet = new Property
			{
				Name = "Retweet"
			};
			Property theme = new Property
			{
				Name = "Theme"
			};

			propertiesList.Add(gender);
			propertiesList.Add(age);
			propertiesList.Add(education);
			propertiesList.Add(language);
			propertiesList.Add(personality);
			propertiesList.Add(word);
			propertiesList.Add(sentiment);
			propertiesList.Add(hashtag);
			propertiesList.Add(mention);
			propertiesList.Add(url);
			propertiesList.Add(date);
			propertiesList.Add(geo);
			propertiesList.Add(postId);
			propertiesList.Add(retweet);
			propertiesList.Add(theme);


			ctx.Properties.AddRange(propertiesList);
			ctx.SaveChanges();
		}

		/// <summary>
		/// Reads all Belgium cities from Json.
		/// </summary>
		private void GenerateAreas(BarometerDbContext ctx)
		{
			string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "zipcode-belgium.json");
			string json = File.ReadAllText(path);
			dynamic deserializedJson = JsonConvert.DeserializeObject(json);

			for (int i = 0; i < deserializedJson.Count; i++)
			{
				Area area = new Area
				{
					Country = "België",
					PostalCode = deserializedJson[i].zip,
					Residence = deserializedJson[i].city
				};
				ctx.Areas.Add(area);
			}
			ctx.SaveChanges();
		}
	}
}


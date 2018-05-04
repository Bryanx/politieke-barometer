using BAR.BL.Domain.Core;
using BAR.BL.Domain.Data;
using BAR.BL.Domain.Items;
using BAR.BL.Domain.Users;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;

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
			//GenerateThemes(ctx);
		}

		/// <summary>
		/// Generates test data themes
		/// </summary>
		/// <param name="ctx"></param>
		private void GenerateThemes(BarometerDbContext ctx)
		{
			Theme t1 = new Theme()
			{
				Name = "Onderwijs",
				CreationDate = DateTime.Now,
				Baseline = 0,
				TrendingPercentage = 0,
				SubPlatform = ctx.SubPlatforms.Where(sp => sp.Name.Equals("politiek")).SingleOrDefault(),
				NumberOfFollowers = 0,
				NumberOfMentions = 0,
				LastUpdated = DateTime.Now,
				LastUpdatedInfo = DateTime.Now,
				ItemType = ItemType.Theme
			};

			Theme t2 = new Theme()
			{
				Name = "Immigranten",
				CreationDate = DateTime.Now,
				Baseline = 0,
				TrendingPercentage = 0,
				SubPlatform = ctx.SubPlatforms.Where(sp => sp.Name.Equals("politiek")).SingleOrDefault(),
				NumberOfFollowers = 0,
				NumberOfMentions = 0,
				LastUpdated = DateTime.Now,
				LastUpdatedInfo = DateTime.Now,
				ItemType = ItemType.Theme
			};

			Theme t3 = new Theme()
			{
				Name = "Pensioenen",
				CreationDate = DateTime.Now,
				Baseline = 0,
				TrendingPercentage = 0,
				SubPlatform = ctx.SubPlatforms.Where(sp => sp.Name.Equals("politiek")).SingleOrDefault(),
				NumberOfFollowers = 0,
				NumberOfMentions = 0,
				LastUpdated = DateTime.Now,
				LastUpdatedInfo = DateTime.Now,
				ItemType = ItemType.Theme
			};

			ctx.Items.Add(t1);
			ctx.Items.Add(t2);
			ctx.Items.Add(t3);
			ctx.SaveChanges();

		}

		/// <summary>
		/// Generates test data for K3 subplatform
		/// </summary>
		/// <param name="ctx"></param>
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
		/// <param name="ctx"></param>
		private void GenerateSubPlatforms(BarometerDbContext ctx)
		{
			SubPlatform subPlatform1 = new SubPlatform
			{
				Name = "politiek",
				CreationDate = DateTime.Now
			};
			ctx.SubPlatforms.Add(subPlatform1);

			SubPlatform subPlatform2 = new SubPlatform
			{
				Name = "k3",
				CreationDate = DateTime.Now
			};
			ctx.SubPlatforms.Add(subPlatform2);

			ctx.SaveChanges();
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

      ctx.Properties.AddRange(propertiesList);
      ctx.SaveChanges();
    }

    /// <summary>
    /// Reads all Belgium cities from Json.
    /// </summary>
    /// <param name="ctx"></param>
    private void GenerateAreas(BarometerDbContext ctx)
    {
      string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "zipcode-belgium.json");
      string json = File.ReadAllText(path);
      dynamic deserializedJson = JsonConvert.DeserializeObject(json);

      for (int i = 0; i < deserializedJson.Count; i++)
      {
        Area area = new Area
        {
          Country    = "België",
          PostalCode = deserializedJson[i].zip,
          Residence  = deserializedJson[i].city
        };
        ctx.Areas.Add(area);
      }
      ctx.SaveChanges();
    }
  }
}


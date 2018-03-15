using BAR.BL.Domain.Data;
using BAR.BL.Domain.Items;
using BAR.BL.Domain.Users;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;

namespace BAR.DAL.EF
{
	internal class BarometerInitializer : DropCreateDatabaseAlways<BarometerDbContext>
	{
		/// <summary>
		/// Dummy data from the json file will be generated
		/// in this file for the first wave of information
		/// </summary>
		protected override void Seed(BarometerDbContext ctx)
		{
			GenerateSources(ctx);
			GenerateProperties(ctx);
			GenerateInformations(ctx);
			GenerateUser(ctx);
		}

		/// <summary>
		/// This method only generates a twitter scource
		/// because it's the only source we are using.
		/// </summary>
		private void GenerateSources(BarometerDbContext ctx)
		{
			Source source = new Source
			{
				Name = "Twitter",
				SourceLine = "twitter.com"
			};
			ctx.Sources.Add(source);
			ctx.SaveChanges();
		}

		/// <summary>
		/// Genertates dummy propeties based on the 
		/// JSON dump file
		/// </summary>
		private void GenerateProperties(BarometerDbContext ctx)
		{
			//Write properties to Set
			Property hashtag = new Property
			{
				Name = "Hashtag"
			};
			Property word = new Property
			{
				Name = "Word"
			};
			Property geo = new Property
			{
				Name = "Geo"
			};
			Property postId = new Property
			{
				Name = "PostId"
			};
			Property userId = new Property
			{
				Name = "UserId"
			};
			Property sentiment = new Property
			{
				Name = "Sentiment"
			};
			Property retweet = new Property
			{
				Name = "Retweet"
			};
			Property url = new Property
			{
				Name = "Url"
			};
			Property mention = new Property
			{
				Name = "Mention"
			};

			ctx.Properties.Add(hashtag);
			ctx.Properties.Add(word);
			ctx.Properties.Add(geo);
			ctx.Properties.Add(postId);
			ctx.Properties.Add(userId);
			ctx.Properties.Add(sentiment);
			ctx.Properties.Add(retweet);
			ctx.Properties.Add(url);
			ctx.Properties.Add(mention);
			ctx.SaveChanges();
		}

		/// <summary>
		/// Genrates information based on the JSON-dump file
		/// </summary>
		private void GenerateInformations(BarometerDbContext ctx)
		{
			string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.json");
			string json = File.ReadAllText(path);
			dynamic deserializedJson = JsonConvert.DeserializeObject(json);

			for (int i = 0; i < 150; i++)
			{
				PropertyValue propertyValue;
				Information information = new Information
				{
					PropertieValues = new List<PropertyValue>()
				};
				//Read hashtags
				for (int j = 0; j < deserializedJson.records[i].hashtags.Count; j++)
				{
					propertyValue = new PropertyValue
					{
						Property = ctx.Properties.Where(x => x.Name.Equals("Hashtag")).SingleOrDefault(),
						Value = deserializedJson.records[i].hashtags[j],
						Confidence = 1
					};
					information.PropertieValues.Add(propertyValue);
				}
				//Read words
				for (int j = 0; j < deserializedJson.records[i].words.Count; j++)
				{
					propertyValue = new PropertyValue
					{
						Property = ctx.Properties.Where(x => x.Name.Equals("Word")).SingleOrDefault(),
						Value = deserializedJson.records[i].words[j],
						Confidence = 1
					};
					information.PropertieValues.Add(propertyValue);
				}
				//Read geo
				propertyValue = new PropertyValue
				{
					Property = ctx.Properties.Where(x => x.Name.Equals("Geo")).SingleOrDefault(),
					Value = deserializedJson.records[i].geo,
					Confidence = 1
				};
				information.PropertieValues.Add(propertyValue);
				//Read postId
				propertyValue = new PropertyValue
				{
					Property = ctx.Properties.Where(x => x.Name.Equals("PostId")).SingleOrDefault(),
					Value = deserializedJson.records[i].id,
					Confidence = 1
				};
				information.PropertieValues.Add(propertyValue);
				//Read userId
				propertyValue = new PropertyValue
				{
					Property = ctx.Properties.Where(x => x.Name.Equals("UserId")).SingleOrDefault(),
					Value = deserializedJson.records[i].user_id,
					Confidence = 1
				};
				information.PropertieValues.Add(propertyValue);
				//Read sentiment
				propertyValue = new PropertyValue
				{
					Property = ctx.Properties.Where(x => x.Name.Equals("Sentiment")).SingleOrDefault(),
					Value = deserializedJson.records[i].sentiment[0],
					Confidence = deserializedJson.records[i].sentiment[1]
				};
				information.PropertieValues.Add(propertyValue);
				//Read retweet
				propertyValue = new PropertyValue
				{
					Property = ctx.Properties.Where(x => x.Name.Equals("Retweet")).SingleOrDefault(),
					Value = deserializedJson.records[i].retweet,
					Confidence = 1
				};
				information.PropertieValues.Add(propertyValue);
				//Read urls
				for (int j = 0; j < deserializedJson.records[i].urls.Count; j++)
				{
					propertyValue = new PropertyValue
					{
						Property = ctx.Properties.Where(x => x.Name.Equals("Url")).SingleOrDefault(),
						Value = deserializedJson.records[i].urls[j],
						Confidence = 1
					};
					information.PropertieValues.Add(propertyValue);
				}
				//Read mentions
				for (int j = 0; j < deserializedJson.records[i].mentions.Count; j++)
				{
					propertyValue = new PropertyValue
					{
						Property = ctx.Properties.Where(x => x.Name.Equals("Mention")).SingleOrDefault(),
						Value = deserializedJson.records[i].mentions[j],
						Confidence = 1
					};
					information.PropertieValues.Add(propertyValue);
				}
				//Add source
				string source = Convert.ToString(deserializedJson.records[i].source);
				information.Source = ctx.Sources.Where(s => s.Name.ToLower().Equals(source)).SingleOrDefault();
				//Add connection to Item (Person)
				string personFullName = String.Format("{0} {1}", deserializedJson.records[i].politician[0], deserializedJson.records[i].politician[1]);
				information.Item = GeneratePoliticians(personFullName, ctx);
				//Add date
				string datum = Convert.ToString(deserializedJson.records[i].date);
				DateTime myInfoDate = DateTime.ParseExact(datum, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
				information.LastUpdated = myInfoDate;

				//Add information object to the DbSet
				ctx.Informations.Add(information);
			}
			ctx.SaveChanges();
		}

		/// <summary>
		/// Generates dummy users for testing purposes.
		/// </summary>
		private void GenerateUser(BarometerDbContext ctx)
		{
			User bryan = new User()
			{
				FirstName = "Bryan",
				Gender = Gender.MAN
			};

			User maarten = new User()
			{
				FirstName = "Maarten",
				Gender = Gender.OTHER
			};

			User remi = new User()
			{
				FirstName = "Remi",
				Gender = Gender.MAN
			};

			User anthony = new User()
			{
				FirstName = "Anthony",
				Gender = Gender.MAN
			};

			User jarne = new User()
			{
				FirstName = "Jarne",
				Gender = Gender.MAN
			};

			User yoni = new User()
			{
				FirstName = "Yoni",
				Gender = Gender.MAN
			};

			ctx.Users.Add(bryan);
			ctx.Users.Add(bryan);
			ctx.Users.Add(anthony);
			ctx.Users.Add(remi);
			ctx.Users.Add(yoni);
			ctx.Users.Add(jarne);
			ctx.SaveChanges();
		}

		/// <summary>
		/// Zal de ID van de politicus teruggeven
		/// Als de politicus nog niet bestaat zal deze aangemaakt worden
		/// </summary>
		private Item GeneratePoliticians(string personFullName, BarometerDbContext ctx)
		{

			Item person = ctx.Items.Where(i => i.Name.Equals(personFullName)).SingleOrDefault();

			if (person == null)
			{
				person = new Person()
				{
					Name = personFullName,
					CreationDate = DateTime.Now,
					Baseline = 0,
					TrendingPercentage = 0
				};
				ctx.Items.Add(person);
				ctx.SaveChanges();
			}

			return person;
		}
	}
}


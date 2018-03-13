using BAR.BL.Domain.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace BAR.DAL
{
    /// <summary>
    /// At this moment the repository works HC.
    /// </summary>
    public class InformationRepository : IInformationRepository
    {
        private List<Information> informations;
        private List<Property> properties;

        public InformationRepository()
        {
            informations = new List<Information>();
            properties = new List<Property>();
            GenerateProperties();
        }

        /// <summary>
        /// Gives back the number of informations for a specific item
        /// from since till now.
        /// </summary>
        public int GetAantalInfo(int itemId, DateTime since)
        {
            return informations.Where(info => info.Item.ItemId == itemId)
                .Where(info => info.LastUpdated <= since).Count();
        }

        /// <summary>
        /// TODO: voor documantation toe (In het engels)
        /// </summary>
        public void GenerateProperties()
        {
            //Make all properties
            Property hashtag = new Property
            {
                PropertyId = 1,
                Name = "Hashtag"
            };
            Property word = new Property
            {
                PropertyId = 2,
                Name = "Word"
            };
            Property date = new Property
            {
                PropertyId = 3,
                Name = "Date"
            };
            Property politician = new Property
            {
                PropertyId = 4,
                Name = "Politician"
            };
            Property geo = new Property
            {
                PropertyId = 5,
                Name = "Geo"
            };
            Property postId = new Property
            {
                PropertyId = 6,
                Name = "TweetId"
            };
            Property userId = new Property
            {
                PropertyId = 7,
                Name = "UserId"
            };
            Property sentiment = new Property
            {
                PropertyId = 8,
                Name = "Sentiment"
            };
            Property retweet = new Property
            {
                PropertyId = 9,
                Name = "Retweet"
            };
            Property url = new Property
            {
                PropertyId = 10,
                Name = "Url"
            };
            Property mention = new Property
            {
                PropertyId = 11,
                Name = "Mention"
            };

            //Add all properties
            properties.Add(hashtag);
            properties.Add(word);
            properties.Add(date);
            properties.Add(politician);
            properties.Add(geo);
            properties.Add(postId);
            properties.Add(userId);
            properties.Add(sentiment);
            properties.Add(retweet);
            properties.Add(url);
            properties.Add(mention);
        }

        /// <summary>
        /// TODO: voeg documentation toe (In het engels)
        /// </summary>
        public void ReadJson()
        {
            Information information = new Information();
            information.InformationId = 1;
            //information.Properties.Add(properties.Where(x => x.Name.Equals("Hashtag"), List<PropertyValue>);
            string json = File.ReadAllText("C:\\Users\\remi\\Google Drive\\KdG\\Leerstof\\2017 - 2018\\Integratieproject\\Repo\\politieke-barometer\\BAR.DAL\\data.json");

            dynamic deserializedJson = JsonConvert.DeserializeObject(json);

            for (int i = 0; i < deserializedJson.records[0].words.Count; i++)
            {
                Console.WriteLine(deserializedJson.records[0].words[i]);
            }

            //for(int i = 0; i < deserializedJson.records.Count; i++)
            //{
            //  for (int j = 0; j < deserializedJson.records[i].Count; j++)
            //  {
            //    for (int k = 0; k < deserializedJson.records[i][j].Count; k++)
            //    {
            //      Console.WriteLine(deserializedJson.records[i][j][k]);
            //    }
            //  }
            //}

            /* SINGLE RESPONSABLILTY!! geen console logica in repo!! */
            Console.ReadKey();
        }
    }
}

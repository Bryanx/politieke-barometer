using BAR.BL.Domain.Data;
using BAR.DAL.EF;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BAR.DAL
{

    /// <summary>
    /// At this moment the repository works HC.
    /// </summary>
    public class InformationRepository : IInformationRepository
    {
        private BarometerDbContext ctx;

        public InformationRepository()
        {
            ctx = new BarometerDbContext();
            GenerateProperties();
            ReadJson();
        }

        /// <summary>
        /// Gets the number of informations of a specific given item
        /// form since till now.
        /// </summary
        public int ReadNumberInfo(int itemId, DateTime since)
        {
            return ctx.Informations.Where(info => info.Item.ItemId == itemId)
                .Where(info => info.LastUpdated <= since).Count();
        }

        /// <summary>
        /// Method used for testing the contents of the informationList
        /// </summary>
        public void PrintInformationList()
        {
            foreach (var item in ctx.Informations.AsEnumerable())
            {
                //TODO: fix code
      
                //Console.WriteLine(String.Format("Post {0}\n--------", item.InformationId));
                //foreach (KeyValuePair<Property, ICollection<PropertyValue>> entry in item.PropertieValues)
                //{
                //    Console.Write(String.Format("{0}: ", entry.Key.Name));
                //    foreach (var property in entry.Value)
                //    {
                //        Console.Write(String.Format("{0} ", property.Value));
                //    }
                //    Console.WriteLine("");
                //}
                //Console.WriteLine("");
            }
            Console.ReadKey();
        }

        /// <summary>
        /// Get all the PropertyValues available for politician
        /// </summary>
        /// <param name="key"></param>
        public void FilterProperty(string key)
        {
            ISet<string> set = new HashSet<string>();

            foreach (var item in ctx.Informations.AsEnumerable())
            {
                //TODO: fix code

                //foreach (KeyValuePair<Property, ICollection<PropertyValue>> entry in item.PropertieValues)
                //{
                //    StringBuilder sb = new StringBuilder();
                //    foreach (var property in entry.Value)
                //    {
                //        if (entry.Key.Name.Equals(key))
                //        {
                //            sb.Append(String.Format("{0} ", property.Value));
                //        }
                //    }
                //    set.Add(sb.ToString());
                //}
            }

            foreach (var item in set)
            {
                Console.WriteLine(item);
            }
            Console.ReadKey();
        }
    }
}


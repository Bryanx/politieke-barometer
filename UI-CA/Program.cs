//using BAR.DAL;
//using BAR.DAL.EF;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using BAR.BL.Managers;
//using BAR.BL.Domain.Items;
//using BAR.BL.Domain.Users;
//using BAR.BL.Controllers;

//namespace BAR.UI.CA
//{
//	/// <summary>
//	/// In this class we test the logic of generating the alerts
//	/// in two waves. The second wave needs to generate new alerts
//	/// </summary>
//	public class Program
//	{
//		/// <summary>
//		/// If true, the program will shut down.
//		/// </summary>
//		private static bool quit = false;

//		/// <summary>
//		/// Start of the program.
//		/// </summary>
//		public static void Main(string[] args)
//		{

//			while (!quit)
//				ShowMenu();
//			//IUserManager userManager = new UserManager();
//			//Console.WriteLine(userManager.GetUser(1).FirstName);
//			//Console.ReadKey();
//			//BL.Domain.Items.Item item = new ItemManager().GetItem(1);

//			//Console.WriteLine(item.Name);

//			//var sm = new SubscriptionManager();
//			//sm.CreateSubscription(1, 1, 50);

//			//Console.WriteLine("GEDAAN");
//			//Console.ReadKey();

//			//wave 1
//			//Are setup is based on the json-dump data.
//			//TODO: implement logic
//			//InformationRepository informationRepository = new InformationRepository();
//			//informationRepository.PrintInformationList();
//			//informationRepository.FilterProperty("Politician");

//			//wave 2
//			// Needs to generate new alerts when HC data is added.
//			//TOCO: implement logic
//		}

//		/// <summary>
//		/// Shows a menu of options that a user can select.
//		/// </summary>
//		private static void ShowMenu()
//		{
//			Console.WriteLine("=================================");
//			Console.WriteLine("=== CONSOLE - BAROMETER =========");
//			Console.WriteLine("=================================");
//			Console.WriteLine("1) Toon alle users");
//			Console.WriteLine("2) Toon alerts van een user");
//			Console.WriteLine("3) Voeg nieuwe tweets toe");
//			Console.WriteLine("4) Toon alle items");
//			Console.WriteLine("5) Bepaal baselines en trending voor alle items");
//			Console.WriteLine("6) Voeg alerts toe voor trending items");
//			Console.WriteLine("7) Subscribe op nieuw item");


//			Console.WriteLine("0) Afsluiten");
//			try
//			{
//				DetectMenuAction();
//			}
//			catch (Exception e)
//			{
//				Console.WriteLine();
//				Console.WriteLine("Er heeft zich een onverwachte fout voorgedaan!");
//				Console.WriteLine();
//			}
//		}

//		/// <summary>
//		/// Determines which code needs to be executed
//		/// when a user selects an option.
//		/// </summary>
//		private static void DetectMenuAction()
//		{
//			bool inValidAction = false;
//			do
//			{
//				Console.Write("Keuze: ");
//				string input = Console.ReadLine();
//				if (Int32.TryParse(input, out int action))
//				{
//					switch (action)
//					{
//						case 1:
//							ShowAllUsers(); break;
//						case 2:
//							ShowUserAlerts(); break;
//						case 3:
//							AddTweets(); break;
//						case 4:
//							ShowAllItems(); break;
//						case 5:
//							CalculateTrending(); break;
//						case 6:
//							AddAlertsForTrendingItems(); break;
//						case 7:
//							SubscribeOnItem(); break;
//						case 0:
//							quit = true;
//							return;
//						default:
//							Console.WriteLine("Geen geldige keuze!");
//							inValidAction = true;
//							break;
//					}
//				}
//			} while (inValidAction);
//		}

//		/// <summary>
//		/// Generates all the alerts for users
//		/// based on new information from the item objects
//		/// </summary>
//		private static void AddAlertsForTrendingItems()
//		{
//			SysController sys = new SysController();
//			IEnumerable<Item> allItems = new ItemManager().getAllItems();
//			int itemSize = allItems.Count();

//			for (int i = 1; i < itemSize; i++)
//			{
//				sys.GenerateAlerts(i);
//			}
//		}

//		/// <summary>
//		/// Shows all alerts for a specific user
//		/// in a showable format.
//		/// </summary>
//		private static void ShowUserAlerts()
//		{
//			Console.Write("UserID: ");
//			int userId = Convert.ToInt32(Console.ReadLine());

//			IEnumerable<Alert> allUserAlerts = new SubscriptionManager().GetAllAlerts(userId);

//			foreach (Alert alert in allUserAlerts)
//			{
//				Console.WriteLine("alertId: " + alert.AlertId + " for " + alert.Subscription.SubscribedUser.FirstName +
//					" that is subscribed to " + alert.Subscription.SubscribedItem.Name);

//			}
//		}

//		/// <summary>
//		/// Subscribes a user to a specific item.
//		/// </summary>
//		private static void SubscribeOnItem()
//		{
//			Console.Write("UserID: ");
//			int userId = Convert.ToInt32(Console.ReadLine());


//			Console.Write("ItemId: ");
//			int itemId = Convert.ToInt32(Console.ReadLine());

//			Console.Write("Threshold (In percentages boven de baseline): ");
//			int treshold = Convert.ToInt32(Console.ReadLine());


//			var sm = new SubscriptionManager();
//			sm.CreateSubscription(userId, itemId, treshold);
//		}

//		/// <summary>
//		/// Calculates a trending percentage and adjusts
//		/// the baseline for a specific item
//		/// </summary>
//		private static void CalculateTrending()
//		{
//			SysController sys = new SysController();
//			IEnumerable<Item> allItems = new ItemManager().getAllItems();
//			int itemSize = allItems.Count();

//			for (int i = 1; i < itemSize; i++)
//			{
//				sys.DetermineTrending(i);
//			}
//		}

//		/// <summary>
//		/// Show all the items that are currently in the system.
//		/// </summary>
//		private static void ShowAllItems()
//		{
//			IEnumerable<Item> allItems = new ItemManager().getAllItems();

//			foreach (Item item in allItems)
//			{
//				Console.WriteLine(item.Name + " baseline: " + item.Baseline + " trendingpercentage: " + item.TrendingPercentage);

//			}
//		}

//		//What is the purpose of this method?
//		private static void AddTweets()
//		{
//			throw new NotImplementedException();
//		}

//		/// <summary>
//		/// Show all the users that are currently in the system.
//		/// </summary>
//		private static void ShowAllUsers()
//		{
//			IEnumerable<User> allUsers = new IdentityUserManager().GetAllUsers();

//			foreach (User user in allUsers)
//			{
//				Console.WriteLine(user.FirstName + " " + user.LastName);

//			}
//		}		
//	}
//}


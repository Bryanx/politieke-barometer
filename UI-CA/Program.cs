using BAR.DAL;
using BAR.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAR.BL.Managers;
using BAR.BL.Domain.Items;
using BAR.BL.Domain.Users;
using BAR.BL.Controllers;

namespace BAR.UI.CA
{
  /// <summary>
  /// In this class we test te logic of generating the alerts
  /// in two waves. The second wave needs to generate new alerts
  /// </summary>
  public class Program
  {
    private static bool quit = false;

    private static void ShowMenu()
    {
      Console.WriteLine("=================================");
      Console.WriteLine("=== CONSOLE - BAROMETER =========");
      Console.WriteLine("=================================");
      Console.WriteLine("1) Toon alle users");
      Console.WriteLine("2) Toon alerts van een user");
      Console.WriteLine("3) Voeg nieuwe tweets toe");
      Console.WriteLine("4) Toon alle items");
      Console.WriteLine("5) Bepaal baselines en trending voor alle items");
      Console.WriteLine("0) Afsluiten");
      try
      {
        DetectMenuAction();
      }
      catch (Exception e)
      {
        Console.WriteLine();
        Console.WriteLine("Er heeft zich een onverwachte fout voorgedaan!");
        Console.WriteLine();
      }
    }

    private static void DetectMenuAction()
    {
      bool inValidAction = false;
      do
      {
        Console.Write("Keuze: ");
        string input = Console.ReadLine();
        int action;
        if (Int32.TryParse(input, out action))
        {
          switch (action)
          {
            case 1:
              ShowAllUsers(); break;
            case 2:
              ShowAlertsForUser(); break;
            case 3:
              AddTweets(); break;
            case 4:
              ShowAllItems(); break;
            case 5:
              CalculateTrending(); break;
            case 0:
              quit = true;
              return;
            default:
              Console.WriteLine("Geen geldige keuze!");
              inValidAction = true;
              break;
          }
        }
      } while (inValidAction);
    }

    private static void CalculateTrending()
    {
      
      SysController sys = new SysController();
      List<Item> allItems = new ItemManager().getAllItems();
      for (int i = 0; i < allItems.Count; i++)
      {
        sys.DetermineTrending(i+1);
      }
    }

    private static void ShowAllItems()
    {
      List<Item> allItems = new ItemManager().getAllItems();
      for (int i=0; i<allItems.Count; i++)
      {
        Console.WriteLine(allItems[i].Name + " baseline: " + allItems[i].Baseline + " trendingpercentage: " + allItems[i].TrendingPercentage);
      }
    }

    private static void AddTweets()
    {
      throw new NotImplementedException();
    }

    private static void ShowAlertsForUser()
    {
      throw new NotImplementedException();
    }

    private static void ShowAllUsers()
    {
      List<User> allUsers = new UserManager().GetAllUsers();
      for (int i = 0; i < allUsers.Count; i++)
      {
        Console.WriteLine(allUsers[i].FirstName + " " + allUsers[i].LastName);
      }
    }

    public static void Main(string[] args)
    {

      while (!quit)
        ShowMenu();
      //IUserManager userManager = new UserManager();
      //Console.WriteLine(userManager.GetUser(1).FirstName);
      //Console.ReadKey();
      //BL.Domain.Items.Item item = new ItemManager().GetItem(1);

      //Console.WriteLine(item.Name);

      //var sm = new SubscriptionManager();
      //sm.CreateSubscription(1, 1, 50);

      //Console.WriteLine("GEDAAN");
      //Console.ReadKey();

      //wave 1
      //Are setup is based on the json-dump data.
      //TODO: implement logic
      //InformationRepository informationRepository = new InformationRepository();
      //informationRepository.PrintInformationList();
      //informationRepository.FilterProperty("Politician");

      //wave 2
    }//Needs to generate new alerts when HC data is added.
     //TOCO: implement logic
  }
}


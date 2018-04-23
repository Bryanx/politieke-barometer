using System.Collections.Generic;

namespace BAR.UI.MVC.Models
{
  public class ItemViewModels
  {
    public class ItemViewModel : BaseViewModel
    {
      public IEnumerable<ItemDTO> Items { get; set; }
      public string Json { get; set; }
    }
    public class PersonViewModel : BaseViewModel
    {
      public ItemDTO Person { get; set; }
      public bool Subscribed { get; set; }

    }
  }
}
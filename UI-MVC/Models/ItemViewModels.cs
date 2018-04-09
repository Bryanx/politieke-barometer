﻿using System.Collections.Generic;

namespace BAR.UI.MVC.Models {
    public class ItemViewModels {
        public class ItemViewModel : BaseViewModel {
            public IEnumerable<ItemDTO> People { get; set; }
        }
        public class PersonViewModel : BaseViewModel {
            public ItemDTO Person { get; set; }
        }
    }
}
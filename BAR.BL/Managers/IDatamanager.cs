﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.Managers
{
    public interface IDataManager
    {
        int GetAanatalInfo(int itemId, DateTime since);
    }
}
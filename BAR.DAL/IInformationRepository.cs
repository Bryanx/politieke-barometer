﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.DAL
{
    public interface IInformationRepository
    {
        int GetAantalInfo(int itemId, DateTime since);
    }
}
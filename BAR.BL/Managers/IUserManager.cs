﻿using BAR.BL.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.Managers
{
    public interface IUserManager
    {
        User GetUser(int userId);
		void InitRepo();
	}
}
using BAR.BL.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using BAR.DAL.EF;

namespace BAR.DAL
{
    public class UserRepository : IUserRepository
    {
        private BarometerDbContext ctx;

        public UserRepository(UnitOfWork uow)
        {
			if (uow == null) ctx = new BarometerDbContext();
			else ctx = uow.Context;
        }

        public User ReadUser(int userId)
        {
            var user = ctx.Users.Find(userId);
            return user;
        }
    }
}

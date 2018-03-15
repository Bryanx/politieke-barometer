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

    /// <summary>
    /// If uow is present then the constructor
    /// will get the context from uow.
    /// </summary>
    public UserRepository(UnitOfWork uow = null)
    {
      if (uow == null) ctx = new BarometerDbContext();
      else ctx = uow.Context;
    }

    public IEnumerable<User> ReadAllUsers()
    {
      return ctx.Users.AsEnumerable();
    }

    /// <summary>
    /// Returns the user from a specific userId.
    /// </summary>
    public User ReadUser(int userId)
    {
      var user = ctx.Users.Find(userId);
      return user;
    }
  }
}

using EdutonPetrpku.Server.Exceptions;
using EdutonPetrpku.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdutonPetrpku.Server.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<AppUser> _userManager;
        public UserRepository(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IQueryable<AppUser> AllUsersExpectAdmin()
        {
            try
            {
                return _userManager.Users.Where(u => u.UserName != "siteadmin").Include(n => n.Nationality);
            }
            catch (SqlException e)
            {
                throw new UserRepositoryException(e);
            }
            catch (Exception e)
            {
                throw new UserRepositoryException(e);
            }
        }
    }
}

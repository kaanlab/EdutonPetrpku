using EdutonPetrpku.Server.Repositories;
using EdutonPetrpku.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdutonPetrpku.Server.Tests.Repositories
{
    public class UserRepositoryFake : IUserRepository
    {


        public IQueryable<AppUser> AllUsersExpectAdmin()
        {
            throw new NotImplementedException();
        }
    }
}

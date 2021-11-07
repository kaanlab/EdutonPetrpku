using EdutonPetrpku.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdutonPetrpku.Server.Repositories
{
    public interface IUserRepository
    {
        IQueryable<AppUser> AllUsersExpectAdmin();
    }
}

using EdutonPetrpku.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdutonPetrpku.Data.Repositories
{
    public interface IUserRepository
    {
        IQueryable<AppUser> AllUsersExpectAdmin();
    }
}

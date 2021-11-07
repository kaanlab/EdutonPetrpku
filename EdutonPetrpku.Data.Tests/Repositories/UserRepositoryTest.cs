using EdutonPetrpku.Data.Repositories;
using EdutonPetrpku.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EdutonPetrpku.Data.Tests.Repositories
{
    public class UserRepositoryTest
    {
        [Fact]
        public void GetUser_ReturnsUser()
        {
            //var dbSetMock = new Mock<DbSet<AppUser>>();
            //var dbContextMock = new Mock<AppDbContext>();
            //dbContextMock.Setup(s => s.Set<AppUser>()).Returns(dbSetMock.Object);

            //var userManagerMock = GetUserManagerMock<AppUser>();
            //userManagerMock.Setup(u => u.Users.Where(u => u.UserName != "siteadmin").Include(n => n.Nationality)).Returns(SomeAppUsers);

            //UserRepository userRepository = new UserRepository(userManagerMock.Object);

            //var user = userRepository.AllUsersExpectAdmin();
            //Assert.NotNull(user);
            //Assert.IsAssignableFrom<ApplicationUser>(user);
        }

        private IQueryable<AppUser> SomeAppUsers()
        {
            List<AppUser> appUsers = new List<AppUser>
            {
                 new AppUser { DisplayName = "Администратор", UserName = "siteadmin", Email = "petrpku@mil.ru", Image = @"upload\empty.jpg", Nationality = null },
                 new AppUser { DisplayName = "Петрозаводское ПКУ", UserName = "petrpku", Email = "petrpku@mil.ru", Image = @"img\1.jpg", Nationality = null }
            };
            return appUsers.AsQueryable();
        }
        private Mock<UserManager<TIDentityUser>> GetUserManagerMock<TIDentityUser>() where TIDentityUser : IdentityUser
        {
            return new Mock<UserManager<TIDentityUser>>(
                    new Mock<IUserStore<TIDentityUser>>().Object,
                    new Mock<IOptions<IdentityOptions>>().Object,
                    new Mock<IPasswordHasher<TIDentityUser>>().Object,
                    new IUserValidator<TIDentityUser>[0],
                    new IPasswordValidator<TIDentityUser>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<IServiceProvider>().Object,
                    new Mock<ILogger<UserManager<TIDentityUser>>>().Object);
        }
    }
}

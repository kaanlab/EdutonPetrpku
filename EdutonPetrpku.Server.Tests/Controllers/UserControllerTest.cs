using EdutonPetrpku.Server.Controllers;
using EdutonPetrpku.Server.Repositories;
using EdutonPetrpku.Server.Tests.Repositories;
using EdutonPetrpku.Shared;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EdutonPetrpku.Server.Tests.Controllers
{
    public class UserControllerTest
    {

        [Fact]
        public void ShoudReturnUsers()
        {
            // Arrange
            var mock = new Mock<IUserRepository>();
            mock.Setup(repo => repo.AllUsersExpectAdmin()).Returns(EmptyListOfAppUsers);
            var controller = new UserController(mock.Object);

            var cut = controller.Summary();
            var r = cut.Result as NotFoundObjectResult;
               r.StatusCode.Should().Be(404);
        }

        private IQueryable<AppUser> EmptyListOfAppUsers()
        {
            return null;
        }
    }
}

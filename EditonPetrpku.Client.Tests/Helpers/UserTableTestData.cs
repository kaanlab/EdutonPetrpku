using EdutonPetrpku.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditonPetrpku.Client.Tests.Helpers
{
    class UserTableTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {
                new List<AppUserViewModel>()
                {
                    new AppUserViewModel
                    {
                      DisplayName = "Администратор",
                        UserName = "siteadmin",
                        Email = "petrpku@mil.ru",
                        Image = @"upload\empty.jpg"

                    },
                    new AppUserViewModel
                    {
                      DisplayName = "Петрозаводское ПКУ",
                        UserName = "petrpku",
                        Email = "petrpku@mil.ru",
                        Image = @"img\1.jpg"


                    }
                }
            };
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}


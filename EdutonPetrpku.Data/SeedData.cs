using EdutonPetrpku.Shared;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdutonPetrpku.Data
{
    public class SeedData
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SeedData(AppDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task AddUsers()
        {
            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole(GlobalVarables.Roles.ADMIN));
                await _roleManager.CreateAsync(new IdentityRole(GlobalVarables.Roles.USER));
            }

            if (!_userManager.Users.Any())
            {
                var admin = new AppUser { DisplayName = "Администратор", UserName = "siteadmin", Email = "petrpku@mil.ru", Image = @"\upload\empty.jpg" };
                await _userManager.CreateAsync(admin, "1Password!");
                await _userManager.AddToRoleAsync(admin, GlobalVarables.Roles.ADMIN);

                //var user = new AppUser { DisplayName = "Петрозаводское ПКУ", UserName = "petrpku", Email = "petrpku@mil.ru", Image = @"/img/1.jpg" };
                //await _userManager.CreateAsync(user, "1Password!");
                //await _userManager.AddToRoleAsync(user, GlobalVarables.Roles.USER);
            }
        }

        //public async Task AddData()
        //{
        //    if (!_context.Nationalities.Any())
        //    {
        //        var nationalities = new List<Nationality>()
        //        {
        //            new Nationality() { Name = "Абазины", Subject = "Карачаево-Черкесская Республика", Population = 43341},
        //            new Nationality() { Name = "Алеуты", Subject = "Камчатский край", Population = 482},
        //            new Nationality() { Name = "Алюторцы", Subject = "Камчатский край", Population = 43341}
        //        };

        //        await _context.Nationalities.AddRangeAsync(nationalities);
        //        await _context.SaveChangesAsync();
        //    }
        //}
    }
}

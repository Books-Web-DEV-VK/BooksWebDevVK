using BooksWeb.Migrations.Data;
using BooksWeb.Models;
using BooksWeb.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksWeb.DataAccess.DbInitializer
{
    public class DBInitializer: IDbInitializer
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationDbContext dbCxt;

        public DBInitializer(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext dbCxt) 
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.dbCxt = dbCxt;
        }
        public void Initialize() 
        {
            // Applying all the pending migrations
            try
            {
                if (dbCxt.Database.GetPendingMigrations().Count() > 0)
                {
                    dbCxt.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
            }

            // Adding all the roles 
            if (!roleManager.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult())
            {
                roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
                roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();  
                roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
                roleManager.CreateAsync(new IdentityRole(SD.Role_Company)).GetAwaiter().GetResult();
            }

            // Adding the super Admin
            var result = userManager.CreateAsync(new ApplicationUser
            {
                Name = "SuperAdmin",
                Email = "super.admin@gmail.com",
                UserName = "super.admin@gmail.com",
                PhoneNumber = "9234567890",
                StreetAddress = "Super Admin Address, RK Beach, Side Pent House",
                City = "Vizag",
                State = "Andhra Pradesh",
                PostalCode = "510987"
            }, "Super.Admin@123").GetAwaiter().GetResult();

            if(result.Succeeded)
            {
                ApplicationUser superAdminUser = dbCxt.ApplicationUsers.FirstOrDefault(user => user.Email == "super.admin@gmail.com");
                var res = userManager.AddToRoleAsync(superAdminUser, SD.Role_Admin).GetAwaiter().GetResult();
            }
        }
    }
}

using BooksWeb.DataAccess.Repository.IRepository;
using BooksWeb.Migrations.Data;
using BooksWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BooksWeb.DataAccess.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _appDbCxt;
        public ApplicationUserRepository(ApplicationDbContext appDbCxt) : base(appDbCxt)
        {
            _appDbCxt = appDbCxt;
        }

        public void Update(ApplicationUser updatableApplicationUser)
        {
            _appDbCxt.ApplicationUsers.Update(updatableApplicationUser);
        }

        public List<ApplicationUser> GetAllUsersWithRoles()
        {
            var roles = _appDbCxt.Roles.ToList();
            var applicationUsers = GetAll(includeProperties: "Company").ToList();
           
            if (roles != null && roles.Any())
            {
                foreach (var applicationUser in applicationUsers)
                {
                    var userRole = _appDbCxt.UserRoles.FirstOrDefault(ur => ur.UserId == applicationUser.Id);
                    if (userRole != null)
                    {
                        applicationUser.Role = roles.FirstOrDefault(r => r.Id == userRole.RoleId)?.Name;
                        applicationUser.CompanyName = applicationUser.Company != null ? applicationUser.Company.Name : string.Empty;
                    }
                }
            }

            return applicationUsers;
        }

        public List<IdentityRole> GetRoles()
        {
            return _appDbCxt.Roles.ToList();
        }
    }
}

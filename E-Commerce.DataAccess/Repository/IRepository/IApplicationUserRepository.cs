using BooksWeb.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BooksWeb.DataAccess.Repository.IRepository
{
    public interface IApplicationUserRepository: IRepository<ApplicationUser>
    {
        void Update(ApplicationUser updatableApplicationUser);
        List<ApplicationUser> GetAllUsersWithRoles();
        List<IdentityRole> GetRoles();
    }
}

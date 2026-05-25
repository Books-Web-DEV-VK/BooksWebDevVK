using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksWeb.Models.ViewModels
{
    public class ApplicationUserRoleVM
    {
        public ApplicationUser applicationUser { get; set; }
        public IEnumerable<IdentityRole> roles { get; set; } = new List<IdentityRole>();
        public IEnumerable<Company> companies { get; set; } = new List<Company>();
    }
}
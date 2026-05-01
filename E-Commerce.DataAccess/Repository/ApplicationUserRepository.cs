using BooksWeb.DataAccess.Repository.IRepository;
using BooksWeb.Migrations.Data;
using BooksWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksWeb.DataAccess.Repository
{
    public class ApplicationUserRepository: Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _appDbCxt;
        public ApplicationUserRepository(ApplicationDbContext appDbCxt): base(appDbCxt)
        {
            _appDbCxt = appDbCxt;
        }
    }
}

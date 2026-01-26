using BooksWeb.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BooksWeb.Models;
using BooksWeb.Migrations.Data;

namespace BooksWeb.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _dbCxt;

        public CompanyRepository(ApplicationDbContext dbCxt): base(dbCxt)
        {
            _dbCxt = dbCxt;
        }

        public void Update(Company entity)
        {
            _dbCxt.Companies.Update(entity);
        }
    }
}

using BooksWeb.DataAccess.Repository.IRepository;
using BooksWeb.Migrations.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BooksWeb.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _appDbCxt;
        private readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext appDbCxt) {
            _appDbCxt = appDbCxt;
            _dbSet = _appDbCxt.Set<T>();
            _appDbCxt.Products.Include(u => u.Category);
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool track = false)
        {
            IQueryable<T> query = track ? _dbSet : _dbSet.AsNoTracking<T>();
            query = query.Where(filter);
            if(!string.IsNullOrEmpty(includeProperties))
            {
                foreach(var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.FirstOrDefault<T>();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, bool track = false)
        {
            IQueryable<T> query = track ? _dbSet : _dbSet.AsNoTracking<T>();
            query = filter==null ? _dbSet : _dbSet.Where(filter);
            if(!string.IsNullOrEmpty(includeProperties))
            {
                foreach(var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            if (query.Any())
              return query.ToList();
            return new List<T>();
        }


        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BooksWeb.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T: class // Reason for this structure : There might some bussinesss logic involved for different kinds of entities ( Models : Category , Products etc ) and we don't want the bussiness logic to be in the IRepository and also to make the code reusable for all king]ds of the entities.
    {
        IEnumerable<T> GetAll();
        T Get(Expression<Func<T,bool>> filter);
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}

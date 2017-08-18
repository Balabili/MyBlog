using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.IRepository
{
    public interface IRepository<T> where T : class
    {
        T GetById(Guid Id);

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);

    }
}

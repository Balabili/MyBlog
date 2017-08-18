using Blog.IRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public EntityContext context;

        public Repository()
        {
            this.context = new EntityContext();
        }

        public virtual void Add(T entity)
        {
            context.Set<T>().Add(entity);
            context.SaveChanges();
        }

        public virtual void Delete(T entity)
        {
            context.Set<T>().Remove(entity);
            context.SaveChanges();
        }

        public virtual T GetById(Guid Id)
        {
            return context.Set<T>().Find(Id);
        }

        public virtual void Update(T entity)
        {          
            context.SaveChanges();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Store.DataAccess.Data;
using Store.DataAccess.Repository.IRepository;

namespace Store.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {

        private readonly ApplicationDbContext _db;
        internal DbSet<T> Dbset;
        public Repository(ApplicationDbContext db)
        {
            _db = db;     
            this.Dbset = _db.Set<T>();
            _db.Products.Include(x => x.Category);//you can include multiple taables
        }
        public void Add(T entity)
        {
            Dbset.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter, string? includeproperties = null)
        {
            IQueryable<T> query = Dbset;
            if (!string.IsNullOrEmpty(includeproperties))
            {
                foreach (var property in includeproperties.Split(new char[]{ ','},StringSplitOptions.RemoveEmptyEntries)) {
                query=query.Include(property);
                }
            }
            query=query.Where(filter);
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(string? includeproperties=null)
        {//incase of multiple tables categoru,covertype
            IQueryable<T> query = Dbset;
            if (!string.IsNullOrEmpty(includeproperties))
            {
                foreach (var property in includeproperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            return query.ToList();
        }

        public void Remove(T entity)
        {
           Dbset.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
          Dbset.RemoveRange(entities); 
        }
    }
}

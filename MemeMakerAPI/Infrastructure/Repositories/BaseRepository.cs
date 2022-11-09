using Domain.Interfaces.Repositories;
using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly MemeMakerDBContext _context;

        public BaseRepository(MemeMakerDBContext context)
        {
            this._context = context;
        }

        public List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public T GetByKeyId(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public bool Insert(T data)
        {
            _context.Set<T>().Add(data);
            return _context.SaveChanges() > 0;
        }

        public bool Update(T updatedData)
        {
            _context.Set<T>().Update(updatedData);
            return _context.SaveChanges() >= 0;
        }

        public bool Delete(int id)
        {
            var item = GetByKeyId(id);
            _context.Set<T>().Remove(item);
            return _context.SaveChanges() > 0;
        }
    }
}

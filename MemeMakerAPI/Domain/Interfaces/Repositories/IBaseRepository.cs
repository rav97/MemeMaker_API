using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        List<T> GetAll();
        T GetByKeyId(int id);
        bool Insert(T data);
        bool Update(T updatedData);
        bool Delete(int id);
    }
}

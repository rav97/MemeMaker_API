using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        #region [ SYNCHRONOUS ]
        List<T> GetAll();
        T GetByKeyId(int id);
        bool Insert(T data);
        bool Update(T updatedData);
        bool Delete(int id);
        #endregion

        #region [ ASYNCHRONOUS ]

        Task<List<T>> GetAllAsync();
        Task<T> GetByKeyIdAsync(int id);
        Task<bool> InsertAsync(T data);
        Task<bool> UpdateAsync(T updatedData);
        Task<bool> DeleteAsync(int id);

        #endregion
    }
}

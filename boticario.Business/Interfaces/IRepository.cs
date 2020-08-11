using System.Collections.Generic;
using System.Threading.Tasks;

namespace boticario.Business.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();

        Task<T> GetById(int id);

        Task<T> Create(T entity);

        Task<bool> Update(T entity);

        Task<bool> DeleteById(int id);

        Task<bool> IsExist(int id);
    }
}

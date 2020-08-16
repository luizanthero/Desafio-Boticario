using System.Collections.Generic;
using System.Threading.Tasks;

namespace boticario.Business.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll(string usuario);

        Task<T> GetById(int id, string usuario);

        Task<T> Create(T entity, string usuario);

        Task<bool> Update(T entity, string usuario);

        Task<bool> DeleteById(int id, string usuario);

        Task<bool> IsExist(int id);
    }
}

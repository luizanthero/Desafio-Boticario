using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace boticario.API.Interfaces
{
    public interface IController<T> where T : class
    {
        Task<ActionResult<IEnumerable<T>>> GetAll();

        Task<ActionResult<T>> GetById(int id);

        Task<ActionResult<T>> Post(T entity);

        Task<IActionResult> Put(int id, T entity);

        Task<IActionResult> Delete(int id);
    }
}

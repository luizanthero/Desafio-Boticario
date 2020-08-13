using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace boticario.API.Interfaces
{
    public interface IController<T> where T : class
    {
        [HttpGet]
        Task<ActionResult<IEnumerable<T>>> GetAll();

        [HttpGet("{id}")]
        Task<ActionResult<T>> GetById(int id);

        [HttpPost]
        Task<ActionResult<T>> Post(T entity);

        [HttpPut("{id}")]
        Task<IActionResult> Put(int id, T entity);

        [HttpDelete("{id}")]
        Task<IActionResult> Delete(int id);
    }
}

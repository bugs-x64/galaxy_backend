using System.Collections.Generic;
using System.Threading.Tasks;
using GalaxyCore.Models;

namespace GalaxyCore.Contracts
{
    public interface ITodosMockProxyService
    {
        Task<IEnumerable<Todo>> GetTodos();
        Task<Todo> GetTodoById(int id);
    }
}

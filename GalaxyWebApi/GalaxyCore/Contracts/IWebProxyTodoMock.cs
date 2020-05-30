using WebService1.BLL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebService1.BLL.Contracts
{
    public interface ITodosMockProxyService
    {
        Task<IEnumerable<Todo>> GetTodos();
        Task<Todo> GetTodoById(int id);
    }
}

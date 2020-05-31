using System.Threading.Tasks;
using GalaxyDto;

namespace GalaxyCore.Contracts
{
    public interface IAuthService
    {
        Task<bool> AuthorizeAsync(string username, string password);
        public Task CreatePasswordAsync(string username, string password);
        Task ChangePasswordAsync(string username, string newPassword);
    }
}
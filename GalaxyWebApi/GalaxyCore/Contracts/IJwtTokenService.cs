using GalaxyDto;

namespace GalaxyCore.Contracts
{
    /// <summary>
    /// Интерфейс сервиса генерации токена.
    /// </summary>
    public interface IJwtTokenService
    {
        TokenDto GenerateToken(string username);
        bool ValidateToken(string token);
    }
}

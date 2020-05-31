using GalaxyDto;

namespace GalaxyCore.Contracts
{
    /// <summary>
    /// ��������� ������� ��������� ������.
    /// </summary>
    public interface IJwtTokenService
    {
        TokenDto GenerateToken(string username);
        bool ValidateToken(string token);
    }
}

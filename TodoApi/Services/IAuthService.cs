using TodoApi.DTOs;

namespace TodoApi.Services
{
    /// <summary>
    /// Interface do serviço de autenticação
    /// Define o contrato que AuthService deve implementar
    /// </summary>
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
        Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);
    }
}
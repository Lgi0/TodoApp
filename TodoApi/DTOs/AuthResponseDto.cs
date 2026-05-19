namespace TodoApi.DTOs
{
    /// <summary>
    /// Resposta de autenticação - Retorna token + dados do usuário
    /// </summary>
    public class AuthResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int ExpiresIn { get; set; }
        public UserDto User { get; set; }
    }
}

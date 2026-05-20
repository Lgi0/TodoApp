using System.ComponentModel.DataAnnotations;

namespace TodoApi.DTOs
{

    /// <summary>
    /// DTO de login - Email e senha para autenticar
    /// </summary>

    public class LoginDto
    {
        [Required(ErrorMessage = "Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Email deve ser um endereço de email válido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Senha é obrigatório.")]
        public string Password { get; set; } = string.Empty;
    }
}

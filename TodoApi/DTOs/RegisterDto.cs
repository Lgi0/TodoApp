using System.ComponentModel.DataAnnotations;

namespace TodoApi.DTOs
{
    /// <summary>
    /// DTO de escrita - O que o cliente envia para registrar
    /// </summary>
    
    public class RegisterDto
    {
        [Required(ErrorMessage = "Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Email deve ser um endereço de email válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha é obrigatório.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password deve ter entre 6 e 100 caracteres.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório.")]
        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }
    }
}

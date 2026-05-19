using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class User
    {
        //key marca como chave primaria
        //mas o c# reconhece o id automaticamente
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "E-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        [StringLength(255)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória.")]
        [StringLength(500)]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório.")]
        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string Role { get; set; } = "User";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        //relacionamento um para muitos, um usuario tem muitas tarefas
        //ICollection = lista de tarefas deste usuário
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
    }
}

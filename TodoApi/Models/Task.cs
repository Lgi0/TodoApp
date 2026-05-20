using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Models
{
    public class Task
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Título é obrigatório.")]
        [StringLength(200, MinimumLength = 3)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public bool IsCompleted { get; set; } = false;

        [Range(0, 5)]
        public int Priority { get; set; } = 3;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime DueDate { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public User User { get; set; } = default!;
    }
}

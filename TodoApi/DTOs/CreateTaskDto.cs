using System.ComponentModel.DataAnnotations;

namespace TodoApi.DTOs
{
    /// <summary>
    /// DTO para criar/atualizar tarefa
    /// </summary>

    public class CreateTaskDto
    {
        [Required(ErrorMessage = "Título é obrigatório.")]
        [StringLength(200, MinimumLength = 3)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Range(0, 5)]
        public int Priority { get; set; } = 3;

        public DateTime? DueDate { get; set; }
    }
}

namespace TodoApi.DTOs
{
    /// <summary>
    /// DTO de leitura - O que enviamos para o cliente
    /// Nunca contém dados sensíveis como senha!
    /// </summary>

    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

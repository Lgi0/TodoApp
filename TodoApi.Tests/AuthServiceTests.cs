using Xunit;
using TodoApi.Data;
using TodoApi.Services;
using TodoApi.DTOs;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using Task = System.Threading.Tasks.Task;
using Microsoft.Extensions.Configuration;

namespace TodoApi.Tests
{
    public class AuthServiceTests
    {
        private readonly IConfiguration _mockConfig;

        public AuthServiceTests()
        {
            // Mock do IConfiguration
            var config = new Dictionary<string, string>
            {
                {"Jwt:SecretKey", "sua-chave-super-secreta-com-mais-de-32-caracteres-aleatorio-12345"},
                {"Jwt:Issuer", "TodoApp"},
                {"Jwt:Audience", "TodoAppUsers"}
            };
            var configBuilder = new ConfigurationBuilder().AddInMemoryCollection(config);
            _mockConfig = configBuilder.Build();
        }

        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task RegisterAsync_WithValidData_ReturnsSuccessResponse()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var authService = new AuthService(context, _mockConfig);
            var dto = new RegisterDto
            {
                Email = "test@example.com",
                Password = "Password123!",
                FirstName = "Test",
                LastName = "User"
            };

            // Act
            var result = await authService.RegisterAsync(dto);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.AccessToken);
            Assert.Equal("Usuário registrado com sucesso!", result.Message);
        }

        [Fact]
        public async Task RegisterAsync_WithExistingEmail_ReturnsFailed()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var authService = new AuthService(context, _mockConfig);

            var user = new User
            {
                Email = "existing@example.com",
                FirstName = "Test",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
                LastName = "User"
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var dto = new RegisterDto
            {
                Email = "existing@example.com",
                Password = "Password123!",
                FirstName = "Test",
                LastName = "User"
            };

            // Act
            var result = await authService.RegisterAsync(dto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Email já registrado", result.Message);
        }

        [Fact]
        public async Task LoginAsync_WithValidCredentials_ReturnsToken()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var authService = new AuthService(context, _mockConfig);

            var password = "Password123!";
            var user = new User
            {
                Email = "test@example.com",
                FirstName = "Test",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                LastName = "User"
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var dto = new LoginDto
            {
                Email = "test@example.com",
                Password = password
            };

            // Act
            var result = await authService.LoginAsync(dto);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.AccessToken);
        }

        [Fact]
        public async Task LoginAsync_WithInvalidPassword_ReturnsFailed()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var authService = new AuthService(context, _mockConfig);

            var user = new TodoApi.Models.User
            {
                Email = "test@example.com",
                FirstName = "Test",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("CorrectPassword!"),
                LastName = "User"
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var dto = new LoginDto
            {
                Email = "test@example.com",
                Password = "WrongPassword!"
            };

            // Act
            var result = await authService.LoginAsync(dto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Email ou senha inválidos", result.Message);
        }
    }
}
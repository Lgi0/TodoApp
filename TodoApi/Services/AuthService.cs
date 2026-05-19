using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TodoApi.Data;
using TodoApi.DTOs;
using TodoApi.Models;
using BCrypt.Net;

namespace TodoApi.Services
{
    /// <summary>
    /// Serviço de autenticação - Lógica de login, register, JWT
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// Registrar novo usuário
        /// </summary>
        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            try
            {
                // 1. Validar se email já existe
                var existingUser = _context.Users.FirstOrDefault(u => u.Email == dto.Email);
                if (existingUser != null)
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Email já registrado"
                    };
                }

                // 2. Hash da senha com BCrypt
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

                // 3. Criar novo usuário
                var user = new User
                {
                    Email = dto.Email,
                    PasswordHash = passwordHash,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    CreatedAt = DateTime.UtcNow
                };

                // 4. Salvar no banco
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // 5. Gerar token e retornar resposta
                return new AuthResponseDto
                {
                    Success = true,
                    Message = "Usuário registrado com sucesso!",
                    AccessToken = GenerateAccessToken(user),
                    RefreshToken = GenerateRefreshToken(),
                    ExpiresIn = 3600,
                    User = new UserDto
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        CreatedAt = user.CreatedAt
                    }
                };
            }
            catch (Exception ex)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = $"Erro ao registrar: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Fazer login
        /// </summary>
        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            try
            {
                // 1. Procurar usuário pelo email
                var user = _context.Users.FirstOrDefault(u => u.Email == dto.Email);

                if (user == null)
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Email ou senha inválidos"
                    };
                }

                // 2. Verificar senha (BCrypt compara automaticamente)
                if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Email ou senha inválidos"
                    };
                }

                // 3. Gerar token e retornar resposta
                return new AuthResponseDto
                {
                    Success = true,
                    Message = "Login realizado com sucesso!",
                    AccessToken = GenerateAccessToken(user),
                    RefreshToken = GenerateRefreshToken(),
                    ExpiresIn = 3600,
                    User = new UserDto
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        CreatedAt = user.CreatedAt
                    }
                };
            }
            catch (Exception ex)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = $"Erro ao fazer login: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Renovar token com refresh token
        /// </summary>
        public Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
        {
            // Implementaremos depois
            throw new NotImplementedException();
        }

        // ===== MÉTODOS PRIVADOS =====

        /// <summary>
        /// Gerar JWT access token
        /// </summary>
        private string GenerateAccessToken(User user)
        {
            // Pega a chave secreta do appsettings.json
            var secretKey = _configuration["Jwt:SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            // Claims = informações sobre o usuário dentro do token
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Role, user.Role)
            };

            // Credenciais para assinar o token
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Criar o token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1), // Expira em 1 hora
                signingCredentials: credentials
            );

            // Converter para string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Gerar refresh token (para renovar access token)
        /// </summary>
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
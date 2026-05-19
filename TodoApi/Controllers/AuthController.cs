using Microsoft.AspNetCore.Mvc;
using TodoApi.DTOs;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Registrar novo usuário
        /// </summary>
        /// <param name="dto">Email, Senha, Nome</param>
        /// <returns>Token JWT e dados do usuário</returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResponseDto
                {
                    Success = false,
                    Message = "Dados inválidos"
                });
            }

            var response = await _authService.RegisterAsync(dto);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        /// <summary>
        /// Fazer login
        /// </summary>
        /// <param name="dto">Email e Senha</param>
        /// <returns>Token JWT e dados do usuário</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResponseDto
                {
                    Success = false,
                    Message = "Dados inválidos"
                });
            }

            var response = await _authService.LoginAsync(dto);

            if (!response.Success)
                return Unauthorized(response);

            return Ok(response);
        }
    }
}
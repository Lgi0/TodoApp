using Microsoft.AspNetCore.Mvc;
using TodoApi.Data;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HealthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("test-db")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> TestDatabase()
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync();
                if (canConnect)
                {
                    return Ok(new { message = "✅ Conectado com sucesso!!" });
                }
                else
                {
                    return BadRequest(new { message = "❌ Banco não conectou" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"❌ Erro ao conectar: {ex.Message}" });
            }
        }
    }
}
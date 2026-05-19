using Microsoft.EntityFrameworkCore;
using TodoApi.Data;

namespace TodoApi
{
    public class TestDbConnection
    {
        public static async Task TestAsync(AppDbContext context)
        {
            try
            {
                var canConnect = await context.Database.CanConnectAsync();

                if (canConnect)
                {
                    Console.WriteLine("Conectado com sucesso!!");

                    var migrations = await context.Database.GetAppliedMigrationsAsync();
                    Console.WriteLine($"Migrations aplicadas: {migrations.Count()}");
                }
                else
                {
                    Console.WriteLine("Sem sucesso.");
                }
            }
            catch (Exception ex)
            {
                {
                    Console.WriteLine($"Erro ao conectar: {ex.Message}");
                }
            }
        }
    }
}


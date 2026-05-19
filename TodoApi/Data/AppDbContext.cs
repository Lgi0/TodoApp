using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using Task = TodoApi.Models.Task;

namespace TodoApi.Data
{
    public class AppDbContext : DbContext
    {
        //DbSet = Tabelas do banco
        public DbSet<User> Users { get; set; } // ← Cria tabela "Users" no BD
        public DbSet<Task> Tasks { get; set; } // ← Cria tabela "Tasks" no BD



        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Email deve ser único (não pode ter 2 usuários com mesmo email)
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // 2. Configurar o relacionamento entre Task e User
            modelBuilder.Entity<Task>()
                .HasOne(t => t.User)                    // Uma Task tem UM User
                .WithMany(u => u.Tasks)                 // Um User tem MUITOS Tasks
                .HasForeignKey(t => t.UserId)           // Foreign key é UserId
                .OnDelete(DeleteBehavior.Cascade);      // Se deletar User, deleta Tasks dele também
        }
    }
}

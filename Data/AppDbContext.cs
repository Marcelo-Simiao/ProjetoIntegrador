namespace ProjetoIntegrador.Data
{
    using Microsoft.EntityFrameworkCore;
    using ProjetoIntegrador.Models;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Funcionario> Funcionarios { get; set; }
    }

}

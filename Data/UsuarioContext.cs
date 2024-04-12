using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class UsuarioContext : DbContext
    {
        public UsuarioContext(DbContextOptions<UsuarioContext> options) : base(options) { }

        public DbSet<Usuarios> Usuarios { get; set; }

        public DbSet<Horario> Horario { get; set; }

    }
}
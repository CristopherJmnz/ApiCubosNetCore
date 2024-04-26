using ApiCubosExamen.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCubosExamen.Data
{
    public class CubosContext:DbContext
    {
        public CubosContext(DbContextOptions<CubosContext>options):base(options)
        {
            
        }

        public DbSet<Cubo> Cubos { get; set; }
        public DbSet<CompraCubo> Compras { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}

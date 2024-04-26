using ApiCubosExamen.Data;
using ApiCubosExamen.Models;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace ApiCubosExamen.Respositories
{
    public class CubosRepository
    {
        private CubosContext context;
        public CubosRepository(CubosContext context)
        {
            this.context = context;
        }

        public async Task<List<Cubo>> GetCubosAsync()
        {
            return await this.context.Cubos.ToListAsync();
        }

        public async Task<List<Cubo>> GetCubosByMarcaAsync(string marca)
        {
            return await this.context.Cubos
                .Where(x=>x.Marca==marca).ToListAsync();
        }

        public async Task CreateUsuarioAsync(Usuario user)
        {
            await this.context.Usuarios.AddAsync(user);
            await this.context.SaveChangesAsync();
        }

        public async Task<List<CompraCubo>> GetPedidosUsuarioAsync(int id)
        {
            return await this.context.Compras
                .Where(x=>x.IdUsuario==id).ToListAsync();
        }

        public async Task CreatePedidoAsync(CompraCubo compra)
        {
            await this.context.Compras.AddAsync(compra);
            await this.context.SaveChangesAsync();
        }

        public async Task<Usuario> FindUsuarioAsync(int id)
        {
            return await this.context.Usuarios
                .FirstOrDefaultAsync(x=>x.IdUsuario==id);
        }

        public async Task<Usuario> LogInAsync
            (string email, string password)
        {
            return await this.context.Usuarios
                .FirstOrDefaultAsync(x => x.Email == email
                && x.Password == password);
        }
    }
}

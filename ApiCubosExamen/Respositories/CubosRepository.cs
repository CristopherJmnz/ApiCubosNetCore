using ApiCubosExamen.Data;
using ApiCubosExamen.Models;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace ApiCubosExamen.Respositories
{
    public class CubosRepository
    {
        private CubosContext context;
        private string BlobUrl;
        public CubosRepository(CubosContext context, IConfiguration config)
        {
            this.BlobUrl = config.GetValue<string>("AzureKeys:BlobUrl");
            this.context = context;
        }

        public async Task<List<Cubo>> GetCubosAsync()
        {
            List<Cubo> cubos = new List<Cubo>();
            foreach (Cubo item in await this.context.Cubos.ToListAsync())
            {
                item.Imagen = BlobUrl + item.Imagen;
                cubos.Add(item);
            }
            return cubos;
        }

        public async Task<List<Cubo>> GetCubosByMarcaAsync(string marca)
        {
            List<Cubo> cubos = new List<Cubo>();
            foreach (Cubo item in await this.context.Cubos
                .Where(x => x.Marca == marca).ToListAsync())
            {
                item.Imagen = BlobUrl + item.Imagen;
                cubos.Add(item);
            }
            return cubos;
        }

        public async Task CreateUsuarioAsync(Usuario user)
        {
            await this.context.Usuarios.AddAsync(user);
            await this.context.SaveChangesAsync();
        }

        public async Task<List<CompraCubo>> GetPedidosUsuarioAsync(int id)
        {
            return await this.context.Compras
                .Where(x => x.IdUsuario == id).ToListAsync();
        }

        public async Task CreatePedidoAsync(CompraCubo compra)
        {
            await this.context.Compras.AddAsync(compra);
            await this.context.SaveChangesAsync();
        }

        public async Task<Usuario> FindUsuarioAsync(int id)
        {
            Usuario user = await this.context.Usuarios
                .FirstOrDefaultAsync(x => x.IdUsuario == id);
            user.Imagen=this.BlobUrl+ user.Imagen;
            return user;
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

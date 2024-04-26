using ApiCubosExamen.Models;
using ApiCubosExamen.Respositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiCubosExamen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CubosController : ControllerBase
    {
        private CubosRepository repo;
        public CubosController(CubosRepository repo)
        {
            this.repo = repo;
        }
        [HttpGet]
        public async Task<ActionResult<List<Cubo>>> Get()
        { 
            return await this.repo.GetCubosAsync();
        }

        [HttpGet("[action]/{marca}")]
        public async Task<ActionResult<List<Cubo>>> CubosMarca(string marca)
        {
            return await this.repo.GetCubosByMarcaAsync(marca);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Usuario usuario)
        {
            await this.repo.CreateUsuarioAsync(usuario);
            return Ok();
        }

        [Authorize]
        [HttpGet("[action]")]
        public async Task<ActionResult<List<CompraCubo>>> PedidosUsuario()
        {
            Usuario user = await this.GetUser();
            return await this.repo.GetPedidosUsuarioAsync(user.IdUsuario);
        }

        [Authorize]
        [HttpGet("[action]")]
        public async Task<ActionResult<Usuario>> PerfilUsuario()
        {
            Usuario user = await this.GetUser();
            return await this.repo.FindUsuarioAsync(user.IdUsuario);
        }

        private async Task<Usuario> GetUser()
        {
            Claim claimUser = HttpContext.User.Claims
                .SingleOrDefault(x => x.Type == "UserData");
            string jsonUser = claimUser.Value;
            Usuario user = JsonConvert.DeserializeObject<Usuario>(jsonUser);
            return user;
        }
    }
}

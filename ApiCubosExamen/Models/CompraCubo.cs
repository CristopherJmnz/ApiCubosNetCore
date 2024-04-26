using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiCubosExamen.Models
{
    [Table("COMPRACUBOS")]
    public class CompraCubo
    {
        [Key]
        [Column("ID_PEDIDO")]
        public int IdPedido { get; set; }
        [Column("ID_CUBO")]
        public int IdCubo { get; set; }
        [Column("ID_USUARIO")]
        public int IdUsuario { get; set; }
        [Column("ID_PEDIDO")]
        public DateTime FechaPedido { get; set; }
    }
}

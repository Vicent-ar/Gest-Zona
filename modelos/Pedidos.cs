using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyecto_admin.modelos
{
    public class Pedidos
    {
        public Pedidos(bool anulado, string articulos, string idpedido, int mesa, bool ticket)
        {
            this.anulado = anulado;
            this.articulos = articulos;
            this.idpedido = idpedido;
            this.mesa = mesa;
            this.ticket = ticket;
          
        }

        public bool anulado { get; set; }
        public string articulos { get; set; }
        public string idpedido { get; set; }
        public int mesa { get; set; }
        public bool ticket { get; set; }
       
    }

    public class RootPedidos
    {
        public List<Pedidos> pedidos { get; set; }
    }

}


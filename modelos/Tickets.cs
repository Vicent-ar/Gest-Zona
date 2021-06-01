using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyecto_admin.modelos
{
    public class Tickets
    {
        public Tickets(string fecha, string hora, string numeroTicket, string pedidosTicket, string articulosTicket, double importe)
        {
            this.fecha = fecha;
            this.hora = hora;
            this.numeroTicket = numeroTicket;
            this.pedidosTicket = pedidosTicket;
            this.articulosTicket = articulosTicket;
            this.importe = importe;
            
        }
        public Tickets(string fecha, string hora, string numeroTicket, string pedidosTicket)
        {
            this.fecha = fecha;
            this.hora = hora;
            this.numeroTicket = numeroTicket;
            this.pedidosTicket = pedidosTicket;
        }
        public Tickets() { }

        public string fecha { get; set; }
        public string hora { get; set; }
        public string numeroTicket { get; set; }
        public string pedidosTicket { get; set; }
        public string articulosTicket { get; set; }
        public double importe { get; set; }
    }

    public class RootTicket
    {
        public List<Tickets> tickets { get; set; }
    }

}

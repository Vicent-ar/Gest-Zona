using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyecto_admin.modelos
{
    public class Reserva
    {
       

        public Reserva(int idReserva, string nombre, string dia, string hora, int comensales, int numeroMesa, bool reservaActiva)
        {
            this.idReserva = idReserva;
            this.nombre = nombre;
            this.dia = dia;
            this.hora = hora;
            this.comensales = comensales;
            this.numeroMesa = numeroMesa;
            this.reservaActiva = reservaActiva;
        }
        public Reserva() { }

        public int idReserva { get; set; }
        public string nombre { get; set; }
        public string dia { get; set; }
        public string hora { get; set; }
        public int comensales { get; set; }
        public int numeroMesa { get; set; }
        public bool reservaActiva { get; set; }
    }

    public class RootReserva
    {
        public List<Reserva> reservas { get; set; }
    }

}

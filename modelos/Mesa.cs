using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyecto_admin.modelos
{
    public class Mesa
    {
        public Mesa(string disponibilidad, string nombreHoraReserva, string idMesa, string localizacion, int nombreMesa, int sillas, bool activa, string controlDia, bool reservadaParaHoy)
        {
            this.disponibilidad = disponibilidad;
            this.nombreHoraReserva = nombreHoraReserva;
            this.idMesa = idMesa;
            this.localizacion = localizacion;
            this.nombreMesa = nombreMesa;
            this.sillas = sillas;
            this.activa = activa;
            this.controlDia = controlDia;
            this.reservadaParaHoy = reservadaParaHoy;

        }
        public Mesa(string disponibilidad, string idMesa, string localizacion, int nombreMesa, int sillas, bool activa, string nombreHoraReserva, bool reservadaParaHoy)
        {
            this.disponibilidad = disponibilidad;
            this.idMesa = idMesa;
            this.localizacion = localizacion;
            this.nombreMesa = nombreMesa;
            this.sillas = sillas;
            this.activa = activa;
            this.nombreHoraReserva = nombreHoraReserva;
            this.reservadaParaHoy = reservadaParaHoy;
        }
        public Mesa() { }

        public string disponibilidad { get; set; }
        public string nombreHoraReserva { get; set; }
        public string idMesa { get; set; }
        public string localizacion { get; set; }
        public int nombreMesa { get; set; }
        public int sillas { get; set; }
        public bool activa { get; set; }
        public string controlDia { get; set; }
        public bool reservadaParaHoy { get; set; }
    }

    public class RootMesa
    {
        public List<Mesa> mesas { get; set; }
    }
}

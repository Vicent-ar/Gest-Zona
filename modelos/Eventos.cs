using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyecto_admin.modelos
{
    class Eventos
    {
        public Eventos()
        {
        }

        public Eventos(string nombreEvento, string direccionEvento, string activoEvento, int idEvento, string fechaEvento, string horaInicioEvento, string horaFinEvento, int zonasEvento,int aforoEvento, string fotoEvento, string fechaEventoFin)
        {
            this.nombreEvento = nombreEvento;
            this.direccionEvento = direccionEvento;
            this.activoEvento = activoEvento;
            this.idEvento = idEvento;
            this.fechaEvento = fechaEvento;
            this.horaInicioEvento = horaInicioEvento;
            this.horaFinEvento = horaFinEvento;
            this.zonasEvento = zonasEvento;
            this.aforoEvento = aforoEvento;
            this.fotoEvento = fotoEvento;
            this.fechaEventoFin = fechaEventoFin;
        }

        public string nombreEvento { get; set; }
        public string direccionEvento { get; set; }
        public string activoEvento { get; set; }
        public int idEvento { get; set; }
        public string fechaEvento { get; set; }
        public string horaInicioEvento { get; set; }
        public string horaFinEvento { get; set; }
        public int zonasEvento { get; set; }
        public int aforoEvento { get; set; }
        public string fotoEvento { get; set; }
        public string fechaEventoFin { get; set; }
        public int visitasEvento { get; set; }
        public int genteEnEvento { get; set; }
        public double porcentageOcupacionEvento { get; set; }
        public double paintingEvento { get; set; }
    }
}

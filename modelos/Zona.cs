using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyecto_admin.modelos
{
    class Zona
    {
        public Zona(string nombreZona, double metrosCuadradosZona, int idZona, int aforoZona, int idEventoZona, 
            double topPositionZona, double leffPositionZona, double heightZona, double widthZona, string zonaActiva,
            string zonaDibujada, string situacionZona, string zonaBloqueada, string inicioZona, string finZona)
        {
            this.nombreZona = nombreZona;
            this.metrosCuadradosZona = metrosCuadradosZona;
            this.idZona = idZona;
            this.aforoZona = aforoZona;
            this.idEventoZona = idEventoZona;
            this.topPositionZona = topPositionZona;
            this.leffPositionZona = leffPositionZona;
            this.heightZona = heightZona;
            this.widthZona = widthZona;
            this.zonaActiva = zonaActiva;
            this.zonaDibujada = zonaDibujada;
            this.situacionZona = situacionZona;
            this.zonaBloqueada = zonaBloqueada;
            this.inicioZona = inicioZona;
            this.finZona = finZona;
        }
        public Zona() { }

        public string nombreZona { get; set; }
        public double metrosCuadradosZona { get; set; }
        public int idZona { get; set; }
        public int aforoZona { get; set; }
        public int idEventoZona { get; set; }
        public double topPositionZona { get; set; }
        public double leffPositionZona { get; set; }
        public double heightZona { get; set; }
        public double widthZona { get; set; }
        public string zonaActiva { get; set; }
        public string zonaDibujada { get; set; }
        public string situacionZona { get; set; }

        public int genteEnZona { get; set; }
        public int visitasZona { get; set; }
        public double porcentajeOcupacionZona { get; set; }
        public double paintingZona { get; set; }
        public string zonaBloqueada { get; set; }
        public string inicioZona { get; set; }
        public string finZona { get; set; }
    }
}

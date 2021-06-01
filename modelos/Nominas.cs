using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyecto_admin.modelos
{
    public class Nomimas
    {
        public Nomimas(string idNomina, int anyo, bool cerrada, double horasExtras, double horasOrdinarias, string idStaff, int mes, double precioExtras, double precioOrdinaria, double total, string nombre)
        {
            this.idNomina = idNomina;
            this.anyo = anyo;
            this.cerrada = cerrada;
            this.horasExtras = horasExtras;
            this.horasOrdinarias = horasOrdinarias;
            this.idStaff = idStaff;
            this.mes = mes;
            this.precioExtras = precioExtras;
            this.precioOrdinaria = precioOrdinaria;
            this.total = total;
            this.nombre = nombre;
        }
        public Nomimas() { }

        public string idNomina { get; set; }
        public int anyo { get; set; }
        public bool cerrada { get; set; }
        public double horasExtras { get; set; }
        public double horasOrdinarias { get; set; }
        public string idStaff { get; set; }
        public int mes { get; set; }
        public double precioExtras { get; set; }
        public double precioOrdinaria { get; set; }
        public double total { get; set; }
        public string nombre { get; set; }
    }

    public class RootNominas
    {
        public List<Nomimas> Nomimas { get; set; }
    }


}

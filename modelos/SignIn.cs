using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyecto_admin
{
    class SignIn
    {

    }
    public class Staff
    {
        public string activo { get; set; }
        public bool alta { get; set; }
        public string dni { get; set; }
        public string domicilio { get; set; }
        public string email { get; set; }
        public string id { get; set; }
        public int intentos { get; set; }
        public string name { get; set; }
        public string numeroCuenta { get; set; }
        public string password { get; set; }
        public string rango { get; set; }
        public int telefono { get; set; }
        public string user { get; set; }

        public Staff() {}

        public Staff(string activo, bool alta, string dni, string domicilio, string email, string id, int intentos, string name, string numeroCuenta, string password, string rango, int telefono, string user)
        {
            this.activo = activo;
            this.alta = alta;
            this.dni = dni;
            this.domicilio = domicilio;
            this.email = email;
            this.id = id;
            this.intentos = intentos;
            this.name = name;
            this.numeroCuenta = numeroCuenta;
            this.password = password;
            this.rango = rango;
            this.telefono = telefono;
            this.user = user;
        }
    }

    public class Nomima
    {
        public int año { get; set; }
        public bool cerrada { get; set; }
        public int horas_extras { get; set; }
        public int horas_ordinarias { get; set; }
        public string id_staff { get; set; }
        public string mes { get; set; }
        public double precio_extras { get; set; }
        public double precio_ordinaria { get; set; }
        public double total { get; set; }
    }

    class RootStaff
    {
        public List<Staff> staff { get; set; }
        public List<Nomima> nomimas { get; set; }
       
        public RootStaff() { }
      
    }
}

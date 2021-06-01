using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyecto_admin.modelos
{
    public class Producto
    {
        public Producto(string id, string designacion, double precio, string gluten, int cantidad, string tipo, bool alta, string caracteristicas, string imagen, bool activo)
        {
            this.id = id;
            this.designacion = designacion;
            this.precio = precio;
            this.gluten = gluten;
            this.cantidad = cantidad;
            this.tipo = tipo;
            this.alta = alta;
            this.caracteristicas = caracteristicas;
            this.imagen = imagen;
            this.activo = activo;
        }
        public Producto(bool activo, bool alta, int cantidad, string caracteristicas, string designacion, string gluten, string id, string imagen, double precio, string tipo)
        {
            this.activo = activo;
            this.alta = alta;
            this.cantidad = cantidad;
            this.caracteristicas = caracteristicas;
            this.designacion = designacion;
            this.gluten = gluten;
            this.id = id;
            this.imagen = imagen;
            this.precio = precio;
            this.tipo = tipo;
        }
        public Producto() { }

        public string id { get; set; }
        public string designacion { get; set; }
        public double precio { get; set; }
        public string gluten { get; set; }
        public int cantidad { get; set; }
        public string tipo { get; set; }
        public bool alta { get; set; }
        public string caracteristicas { get; set; }
        public string imagen { get; set; }
        public bool activo { get; set; }
    }

    public class Root
    {
        public List<Producto> productos { get; set; }
    }

}

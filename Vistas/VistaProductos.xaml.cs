using Newtonsoft.Json;
using proyecto_admin.modelos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace proyecto_admin.Vistas
{
    /// <summary>
    /// Lógica de interacción para VistaProductos.xaml
    /// </summary>
    public partial class Controlador_de_pedido : Window
    {
        public Controlador_de_pedido()
        {
            InitializeComponent();
        }
        List<Producto> TodosProductos = null;
        List<Producto> productos = new List<Producto>();
        int posicionProducto = 0;

        private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            productos.Clear();

            WrapProducto.Children.Clear();

            ListBoxItem tipoProducto = (ListBoxItem)sender as ListBoxItem;

            var url = "http://localhost:3000/productos";

            WebClient wc = new WebClient();

            var datos = wc.DownloadString(url);

            TodosProductos = JsonConvert.DeserializeObject<List<Producto>>(datos);

            foreach (var produc in TodosProductos)
            {
                if (produc.tipo == tipoProducto.Name && produc.alta == true)
                {
                    Producto producto = new Producto(produc.activo, produc.alta, produc.cantidad, produc.caracteristicas, produc.designacion, produc.gluten, produc.id, produc.imagen, produc.precio, produc.tipo);
                    productos.Add(producto);
                }

            }

            for (var x = 0; x < productos.Count(); x++)
            {

                Label labelStock = new Label();
                labelStock.Content = productos[x].cantidad;
                labelStock.FontSize = 12;
                labelStock.IsEnabled = true;

                BitmapImage bi3 = new BitmapImage();
                bi3.BeginInit();
                bi3.UriSource = new Uri(productos[x].imagen, UriKind.Absolute);
                bi3.EndInit();
                Image imgProd = new Image();
                imgProd.Source = bi3;
                imgProd.Width = 120;
                imgProd.Height = 100;

                StackPanel stackContentBotonProducto = new StackPanel();
                TextBlock TextBlockNombreBottonProducto = new TextBlock();
                TextBlockNombreBottonProducto.Text = productos[x].designacion;
                TextBlockNombreBottonProducto.TextWrapping = TextWrapping.Wrap;
                TextBlockNombreBottonProducto.HorizontalAlignment = HorizontalAlignment.Center;

                stackContentBotonProducto.Children.Add(TextBlockNombreBottonProducto);
                stackContentBotonProducto.Children.Add(imgProd);
                stackContentBotonProducto.Children.Add(labelStock);

                Button btnProducto = new Button();
                btnProducto.Click += new RoutedEventHandler(rellenarCamposProducto);
                btnProducto.Content = stackContentBotonProducto;
                btnProducto.Style = null;
                btnProducto.Width = 140;
                btnProducto.Height = 155;
                btnProducto.Background = Brushes.Snow;
                btnProducto.BorderBrush = Brushes.Black;
                btnProducto.BorderThickness = new Thickness(2);
                if (productos[x].cantidad == 0)
                {
                    btnProducto.Background = Brushes.Red;
                }
                btnProducto.ToolTip = productos[x].caracteristicas + "\n Precio: " + productos[x].precio + "\n Con gluten:" + productos[x].gluten;
                btnProducto.Margin = new Thickness(10);

                WrapProducto.Children.Add(btnProducto);
            }
        }

        public void rellenarCamposProducto(object sender, RoutedEventArgs e)
        {
            Button btnProducto = (Button)sender as Button;

            StackPanel stackContentBoton = (StackPanel)btnProducto.Content;

            UIElementCollection HijosDelStackPanel = stackContentBoton.Children;

            TextBlock TextBlockNombreProducto = (TextBlock)HijosDelStackPanel[0];

            Label LabelStockProducto = (Label)HijosDelStackPanel[2];

            Stock.Content = LabelStockProducto.Content;
            NumProducto.Maximum = Convert.ToInt32(LabelStockProducto.Content);
            NombreProdcuto.Content = TextBlockNombreProducto.Text;
            especificaciones.Text = "";
            btnAñadir.Content = "Añadir";
            NumProducto.Value = 1;
            btnAñadir.IsEnabled = true;
        }

        private void ButtonAñadirEditar_Click(object sender, RoutedEventArgs e)
        {

            if ((String)btnAñadir.Content == "Añadir")
            {
                if (especificaciones.Text.Length > 0)
                {
                    //Creo un objeto y se lo añado al ListBox mediante un template que le e puesto
                    TodoItem items = new TodoItem() { Title = NombreProdcuto.Content + " x" + NumProducto.Value, Especificaciones = "  - " + especificaciones.Text };
                    MenuPedido.Items.Add(items);
                }
                else
                {
                    //Añado un listItem al listBox con el producto y su cantidad
                    ListBoxItem listItem = new ListBoxItem();
                    listItem.FontSize = 20;
                    listItem.Height = 50;
                    listItem.Content = NombreProdcuto.Content + " x" + NumProducto.Value;
                    MenuPedido.Items.Add(listItem);
                }
            }
            else
            {
                //Aquí entra cuando hay que editar un producto
                MenuPedido.Items.RemoveAt(posicionProducto);
                String nombre = "";
                String nProduc = NombreProdcuto.Content.ToString();
                for (var x = 0; x < (nProduc.Length - 1); x++)
                {
                    nombre += nProduc[x];
                }
                if (especificaciones.Text.Length > 0)
                {
                    TodoItem items = new TodoItem() { Title = nombre + " x" + NumProducto.Value, Especificaciones = "  - " + especificaciones.Text };
                    MenuPedido.Items.Insert(posicionProducto, items);
                }
                else
                {
                    ListBoxItem listItem = new ListBoxItem();
                    listItem.FontSize = 20;
                    listItem.Height = 50;
                    listItem.Content = nombre + " x" + NumProducto.Value;
                    MenuPedido.Items.Insert(posicionProducto, listItem);
                }
            }

            btnAñadir.IsEnabled = false;
            NombreProdcuto.Content = "";
            NumProducto.Value = 1;
            Stock.Content = "";
            especificaciones.Text = "";
        }


        //Esta Clase es el objeto que creo para hacer el binding al template del listBox
        public class TodoItem
        {
            public string Title { get; set; }
            public string Especificaciones { get; set; }
        }

        private void btnEliminarProducto_Click(object sender, RoutedEventArgs e)
        {
            if (MenuPedido.SelectedIndex == -1)
            {
                MessageBox.Show("Selecciona un Producto!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Estas seguro que quieres eliminar el producto", "Confirmacion", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        MenuPedido.Items.RemoveAt(MenuPedido.SelectedIndex);
                        break;
                }
            }
        }

        private void btnEditarProducto_Click(object sender, RoutedEventArgs e)
        {

            posicionProducto = MenuPedido.SelectedIndex;
            especificaciones.Text = "";

            if (posicionProducto != -1)
            {
                try
                {
                    ListBoxItem listBoxItem = (ListBoxItem)MenuPedido.SelectedItem;
                    string todoElProducto = listBoxItem.Content.ToString();

                    char[] delimiterChars = { 'x' };
                    string[] splitDelProducto = todoElProducto.Split(delimiterChars);
                    int intento = 0;
                    for (int xy = 0; xy < splitDelProducto.Length; xy++)
                    {
                        try
                        {
                            int numPro = Convert.ToInt32(splitDelProducto[xy]);
                            NumProducto.Value = numPro;
                            break;
                        }
                        catch
                        {
                            intento++;
                        }
                    }
                    if (intento > 1)
                    {
                        for (int x = 0; x < intento; x++)
                        {
                            if (x < intento - 1)
                            {
                                NombreProdcuto.Content += splitDelProducto[x].ToString() + "x";
                            }
                            else
                            {
                                NombreProdcuto.Content += splitDelProducto[x].ToString();
                            }

                        }
                    }
                    else
                    {
                        NombreProdcuto.Content = splitDelProducto[0].ToString();
                    }
                    btnAñadir.Content = "Editar";
                    btnAñadir.IsEnabled = true;
                }
                catch
                {
                    //Aquí estra cuando el producto no se puede pasar a un ListBoxItem por lo que es un producto con especificaciones de tipo TodoItem
                    TodoItem todoitem = (TodoItem)MenuPedido.SelectedItem;

                    string NombreCantidad = todoitem.Title.ToString();
                    char[] delimiterChars = { 'x' };
                    string[] splitNombreCantidad = NombreCantidad.Split(delimiterChars);
                    int intento = 0;
                    string Espacificaciones = todoitem.Especificaciones.ToString();
                    char[] barra = { '-' };
                    string[] valorDespuesBarraEspacificaciones = Espacificaciones.Trim().Split(barra);
                    especificaciones.Text = valorDespuesBarraEspacificaciones[1].Trim().ToString();
                    for (int xy = 0; xy < splitNombreCantidad.Length; xy++)
                    {
                        try
                        {
                            int numPro = Convert.ToInt32(splitNombreCantidad[xy]);
                            NumProducto.Value = numPro;
                            break;
                        }
                        catch
                        {
                            intento++;
                        }
                    }
                    if (intento > 1)
                    {
                        for (int x = 0; x < intento; x++)
                        {
                            if (x < intento - 1)
                            {
                                NombreProdcuto.Content += splitNombreCantidad[x].ToString() + "x";
                            }
                            else
                            {
                                NombreProdcuto.Content += splitNombreCantidad[x].ToString();
                            }

                        }
                    }
                    else
                    {
                        NombreProdcuto.Content = splitNombreCantidad[0].ToString();
                    }
                    btnAñadir.Content = "Editar";
                    btnAñadir.IsEnabled = true;
                }
                for (int x = 0; x < TodosProductos.Count; x++)
                {
                    if (NombreProdcuto.Content.ToString().Trim() == TodosProductos[x].designacion)
                    {
                        Stock.Content = TodosProductos[x].cantidad;
                        NumProducto.Maximum = TodosProductos[x].cantidad;
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecciona un Producto!!");
            }
        }

        private void Button_GuardarPedido(object sender, RoutedEventArgs e)
        {
            List<string> ids = new List<string>();
            String pedido = "";
            String nombre = "";
            int NumeroDeproducto = 0;
            string id = "";
            string precio = "";
            String especificaciones = "";

            for (var x = 0; x < MenuPedido.Items.Count; x++)
            {
                especificaciones = "";
                try
                {
                    ListBoxItem NombreCantidad = (ListBoxItem)MenuPedido.Items[x] as ListBoxItem;
                    string todoNombreCantidad = NombreCantidad.Content.ToString();

                    char[] delimiterChars = { 'x' };
                    string[] splitNombreCantidad = todoNombreCantidad.Split(delimiterChars);
                    int intento = 0;
                    for (int xy = 0; xy < splitNombreCantidad.Length; xy++)
                    {
                        try
                        {
                            int numPro = Convert.ToInt32(splitNombreCantidad[xy]);
                            NumeroDeproducto = numPro;
                            break;
                        }
                        catch
                        {
                            intento++;
                        }
                    }
                    //Si entra en este if significa que el nombre del producto contiene una x y la posicion de la cantidad es distinta
                    if (intento > 1)
                    {
                        for (int y = 0; y < intento; y++)
                        {
                            if (y < intento - 1)
                            {
                                nombre += splitNombreCantidad[x].ToString() + "x";
                            }
                            else
                            {
                                nombre += splitNombreCantidad[x].ToString();
                            }
                        }
                    }
                    else
                    {
                        nombre = splitNombreCantidad[0].ToString();
                    }

                    for (int z = 0; z < TodosProductos.Count; z++)
                    {
                        if (TodosProductos[z].designacion.ToString() == nombre.Trim())
                        {
                            id = TodosProductos[z].id;
                            precio = Convert.ToString(TodosProductos[z].precio);
                            ids.Add(id);
                            TodosProductos[z].cantidad = TodosProductos[z].cantidad - NumeroDeproducto;
                            break;
                        }
                    }
                }
                catch
                {
                    TodoItem todoitem = (TodoItem)MenuPedido.Items[x];
                    string NombreCantidad = todoitem.Title.ToString();
                    char[] delimiterChars = { 'x' };
                    string[] splitNombreCantidad = NombreCantidad.Split(delimiterChars);
                    int intento = 0;
                    string todoEspacificaciones = todoitem.Especificaciones.ToString();
                    char[] barra = { '-' };
                    string[] valorDespuesBarra = todoEspacificaciones.Trim().Split(barra);
                    especificaciones += valorDespuesBarra[1].Trim().ToString();
                    for (int xy = 0; xy < splitNombreCantidad.Length; xy++)
                    {
                        try
                        {
                            int numPro = Convert.ToInt32(splitNombreCantidad[xy]);
                            NumeroDeproducto = numPro;
                            break;
                        }
                        catch
                        {
                            intento++;
                        }
                    }
                    if (intento > 1)
                    {
                        for (int y = 0; y < intento; y++)
                        {
                            if (y < intento - 1)
                            {
                                nombre += splitNombreCantidad[x].ToString() + "x";
                            }
                            else
                            {
                                nombre += splitNombreCantidad[x].ToString();
                            }
                        }
                    }
                    else
                    {
                        nombre = splitNombreCantidad[0].ToString();
                    }

                    for (int z = 0; z < TodosProductos.Count; z++)
                    {
                        if (TodosProductos[z].designacion.ToString() == nombre.Trim())
                        {
                            id = TodosProductos[z].id;
                            precio = Convert.ToString(TodosProductos[z].precio);
                            ids.Add(id);
                            TodosProductos[z].cantidad = TodosProductos[z].cantidad - NumeroDeproducto;
                            break;
                        }
                    }
                }
                if (especificaciones == "")
                {
                    pedido += id + "/" + nombre.Trim() + "/" + precio + "/" + NumeroDeproducto + "/sin observaciones/false/";

                }
                else
                {
                    pedido += id + "/" + nombre.Trim() + "/" + precio + "/" + NumeroDeproducto + "/" + especificaciones + "/false/";
                }
            }
            //MessageBox.Show(pedido);
            //Aqui envio los datos del pedido a Node

            if (pedido == "")
            {
                MessageBox.Show("Tienes que hacer un pedido valido!");
            }
            else
            {
                var url = "http://localhost:3000/pedidos";

                WebClient wc = new WebClient();

                var datos = wc.DownloadString(url);

                List<Pedidos> TodosPedidos = JsonConvert.DeserializeObject<List<Pedidos>>(datos);

                int NumeroPedidos = TodosPedidos.Count();

                int idPedido = 10000 + (NumeroPedidos + 1);

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(" http://localhost:3000/pedidos/" + NumeroPedidos);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                string json = "";
                string nummesa = NumeroMesa.Content.ToString();
                char[] espacio = { ' ' };
                string[] mesa = nummesa.Split(espacio);
                string numeroMesa = "";
                if (mesa[0] == "Barra")
                {
                    numeroMesa = "0";
                }
                else
                {
                    numeroMesa = mesa[1];
                }
                var data = new
                {
                    idpedido = Convert.ToString(idPedido),
                    mesa = numeroMesa,
                    articulos = pedido,
                    anulado = false,
                    ticket = false,
                };

                json = JsonConvert.SerializeObject(data);
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    streamReader.Close();
                }

                //resto el stock de los productos pedidos
                for (int x = 0; x < ids.Count; x++)
                {
                    for (int y = 0; y < TodosProductos.Count; y++)
                    {
                        if (ids[x] == TodosProductos[y].id)
                        {
                            modificarStock(TodosProductos[y], y);
                        }
                    }
                }
                if (mesa[0].Trim() != "Barra")
                {
                    OcuparMesa(mesa[1].ToString().Trim());
                }
                MessageBox.Show("Pedido guardado correctamente!!", "Informacio", MessageBoxButton.OK, MessageBoxImage.Information);
                VistaPedidos vistaPedidos = new VistaPedidos();
                vistaPedidos.Show();
                this.Close();
            }
        }

        public void modificarStock(Producto producto, int posicion)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(" http://localhost:3000/productos/" + posicion);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            string json = "";
            var data = new
            {
                id = producto.id,
                designacion = producto.designacion,
                precio = producto.precio,
                gluten = producto.gluten,
                cantidad = producto.cantidad,
                tipo = producto.tipo,
                alta = producto.alta,
                caracteristicas = producto.caracteristicas,
                imagen = producto.imagen,
                activo = true
            };

            json = JsonConvert.SerializeObject(data);
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                streamReader.Close();
            }
        }

        public void OcuparMesa(string mesa)
        {
            var url = "http://localhost:3000/mesas";

            WebClient wc = new WebClient();

            var datos = wc.DownloadString(url);

            List<Mesa> TodasMesas = JsonConvert.DeserializeObject<List<Mesa>>(datos);
            Mesa MesaOcupar = null;
            int pos = 0;
            for (int x = 0; x < TodasMesas.Count; x++)
            {
                if (mesa.Trim() == TodasMesas[x].idMesa)
                {
                    MesaOcupar = TodasMesas[x];
                    break;
                }
                pos++;
            }

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(" http://localhost:3000/mesas/" + pos);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            string json = "";
            var data = new
            {
                disponibilidad = "ocupada",
                idMesa = MesaOcupar.idMesa,
                localizacion = MesaOcupar.localizacion,
                nombreMesa = MesaOcupar.nombreMesa,
                sillas = MesaOcupar.sillas,
            };

            json = JsonConvert.SerializeObject(data);
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                streamReader.Close();
            }
            //MessageBox.Show(mesa+" "+pos+" Ocupada");
        }

        private void Button_CancelarPedido(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Estas seguro que quieres salir \n Se perderan todos los cambio", "Confirmacion", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    VistaPedidos vistaPedidos = new VistaPedidos();
                    vistaPedidos.Show();
                    this.Close();
                    break;
            }
        }
    }
}
using Newtonsoft.Json;
using proyecto_admin.modelos;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Lógica de interacción para VistaPedidos.xaml
    /// </summary>
    
    public partial class VistaPedidos : Window
    {
        System.Windows.Forms.Timer t1 = new System.Windows.Forms.Timer();
        public VistaPedidos()
        {
            InitializeComponent();
            cargarMesas();
            cargarPedidos();
            recarga();


        }
        private void recarga()
        {

            t1.Interval = 10000;
            t1.Enabled = true;
            t1.Start();
            t1.Tick += new EventHandler(TimerEventProcessor);
        }

        private void TimerEventProcessor(object sender, EventArgs e)
        {

            cargarMesas();
            cargarPedidos();

        }

        List<Mesa> TodasMesas = null;
        List<Pedidos> TodosPedidos = null;
        List<Pedidos> TodosPedidosFiltrados = new List<Pedidos>();
        string mesaSeleccionada = "";

        private void cargarMesas()
        {
            WrapMesas2.Children.Clear();
            var url = "http://localhost:3000/mesas";

            WebClient wc = new WebClient();

            var datos = wc.DownloadString(url);

            TodasMesas = JsonConvert.DeserializeObject<List<Mesa>>(datos);

            for (var x = 0; x < TodasMesas.Count(); x++)
            {
                if (TodasMesas[x].activa == true)
                {
                    Button btnMesa = new Button();
                    

                    if (TodasMesas[x].disponibilidad == "ocupada")
                    {
                        btnMesa.Background = Brushes.Red;

                    }
                    else if (TodasMesas[x].reservadaParaHoy == true)
                    {
                        btnMesa.Background = Brushes.Yellow;
                        string reserva = "";
                        string todalaReserva = TodasMesas[x].nombreHoraReserva;
                        string[] camposReserva = todalaReserva.Split('/');
                        int nombre = 0;
                        int personas = 1;
                        int hora = 2;
                        for (int y = 0; y < camposReserva.Length / 3; y++)
                        {
                            reserva += "Nombre: " + camposReserva[nombre] + "\n";
                            reserva += "Persona: " + camposReserva[personas] + "\n";
                            reserva += "Hora: " + camposReserva[hora] + "\n";
                            nombre = nombre + 3;
                            personas = personas + 3;
                            hora = hora + 3;
                        }
                        btnMesa.ToolTip = reserva;
                    }
                    else if (TodasMesas[x].disponibilidad == "libre")
                    {
                        
                        btnMesa.Background = Brushes.LightGreen;
                    }



                    btnMesa.Click += new RoutedEventHandler(ObtenerMesa);
                    if (TodasMesas[x].nombreMesa == 0)
                    {
                        btnMesa.Content = "Barra ";
                    }
                    else
                    {
                        btnMesa.Content = "Mesa " + TodasMesas[x].nombreMesa;
                    }
                    btnMesa.Width = 90;
                    btnMesa.Height = 90;
                    btnMesa.FontWeight = FontWeights.Bold;
                    btnMesa.FontSize = 20;
                    btnMesa.Margin = new Thickness(5);

                    WrapMesas2.Children.Add(btnMesa);
                }
            }
        }

        public void cargarPedidos()
        {
            TodosPedidosFiltrados.Clear();

            ListBoxPedidos.Items.Clear();
            var url = "http://localhost:3000/pedidos";

            WebClient wc = new WebClient();

            var datos = wc.DownloadString(url);

            TodosPedidos = JsonConvert.DeserializeObject<List<Pedidos>>(datos);

            foreach (var pedidos in TodosPedidos)
            {
                if (pedidos.anulado == false && pedidos.ticket == false)
                {
                    Pedidos pedido = new Pedidos(pedidos.anulado, pedidos.articulos, pedidos.idpedido, pedidos.mesa, pedidos.ticket);
                    TodosPedidosFiltrados.Add(pedido);
                }
            }
            for (int x = TodosPedidosFiltrados.Count - 1; x >= 0; x--)
            {
                int posicionCaracteristicas = 4;
                int posicionNombre = 1;
                int posicionCantidad = 3;
                string todo = TodosPedidosFiltrados[x].articulos.ToString();
                char[] delimiterChars = { '/' };
                string[] Articulos = todo.Split(delimiterChars);
                int cantidadProductos = Articulos.Count() / 6;
                ListBoxItem ListBoxItem = new ListBoxItem();
                ListBoxItem.BorderBrush = Brushes.Black;
                ListBoxItem.BorderThickness = new Thickness(3);
                ListBoxItem.Margin = new Thickness(5);
                ListBoxItem.FontSize = 20;
                ListBoxItem.FontWeight = FontWeights.Bold;
                StackPanel stack = new StackPanel();
                Label Mesa = new Label();
                Mesa.Foreground = Brushes.White;
                Mesa.Content = "Mesa " + TodosPedidosFiltrados[x].mesa;
                Mesa.HorizontalContentAlignment = HorizontalAlignment.Center;
                Label labelid = new Label();
                labelid.Foreground = Brushes.Wheat;
                labelid.Visibility = Visibility.Collapsed;
                labelid.Content = TodosPedidosFiltrados[x].idpedido;
                stack.Children.Add(Mesa);
                stack.Children.Add(labelid);
                for (int y = 0; y < cantidadProductos; y++)
                {
                    if (Articulos[posicionCaracteristicas] == "sin observaciones")
                    {
                        Label label = new Label();
                        label.Content = "- " + Articulos[posicionNombre].ToString() + " x" + Articulos[posicionCantidad].ToString();
                        label.Style = null;
                        label.Foreground = Brushes.Yellow;
                        label.HorizontalContentAlignment = HorizontalAlignment.Left;
                        label.Opacity = 3;
                        stack.Children.Add(label);
                    }
                    else
                    {
                        StackPanel stack2 = new StackPanel();
                        Label NombreyCantidad = new Label();
                        NombreyCantidad.Style = null;
                        NombreyCantidad.Foreground = Brushes.Yellow;
                        NombreyCantidad.HorizontalContentAlignment = HorizontalAlignment.Left;
                        Label Espacificaciones = new Label();
                        Espacificaciones.Style = null;
                        Espacificaciones.Foreground = Brushes.Yellow;
                        Espacificaciones.HorizontalContentAlignment = HorizontalAlignment.Left;
                        NombreyCantidad.Content = "- " + Articulos[posicionNombre].ToString() + " x" + Articulos[posicionCantidad].ToString();
                        Espacificaciones.Content = "    - " + Articulos[posicionCaracteristicas].ToString();
                        stack2.Children.Add(NombreyCantidad);
                        stack2.Children.Add(Espacificaciones);
                        stack.Children.Add(stack2);
                    }
                    posicionNombre = posicionNombre + 6;
                    posicionCaracteristicas = posicionCaracteristicas + 6;
                    posicionCantidad = posicionCantidad + 6;
                }
                ListBoxItem.Content = stack;
                ListBoxPedidos.Items.Add(ListBoxItem);
            }
        }

        public void ObtenerMesa(object sender, RoutedEventArgs e)
        {

            Button btn = (Button)sender;

            if (btn.Background == Brushes.Red)
            {

                MessageBox.Show("Esa mesa ya esta ocupada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                String nombremesa = btn.Content.ToString();
                char[] delimiterChars = { ' ' };
                string[] Articulos = nombremesa.Split(delimiterChars);
                if (Articulos[0] == "Barra")
                {
                    mesaSeleccionada = "0";
                    LabelNumeroMesa.Content = "0";
                }
                else
                {
                    mesaSeleccionada = Articulos[1];
                    LabelNumeroMesa.Content = mesaSeleccionada;
                }
            }
        }

        private void Button_ClickNuevoPedido(object sender, RoutedEventArgs e)
        {
            if (mesaSeleccionada == "")
            {
                MessageBox.Show("Selecciona una mesa!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Controlador_de_pedido VistaProductos = new Controlador_de_pedido();

                Label label = (Label)VistaProductos.FindName("NumeroMesa") as Label;
                if (mesaSeleccionada == "0")
                {
                    label.Content = "Barra";
                }
                else
                {
                    label.Content = "Mesa " + mesaSeleccionada;
                }
                t1.Stop();
                VistaProductos.Show();
                this.Close();
            }
            mesaSeleccionada = "";
        }

        private void Button_ClickEliminarPedido(object sender, RoutedEventArgs e)
        {
            if (ListBoxPedidos.SelectedIndex == -1)
            {
                MessageBox.Show("Selecciona un el pedido que quieres eliminar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                ListBoxItem listboxitem = (ListBoxItem)ListBoxPedidos.Items[ListBoxPedidos.SelectedIndex];
                StackPanel stackItem = (StackPanel)listboxitem.Content;
                Label labelMesa = (Label)stackItem.Children[0];
                char[] delimiterChars = { ' ' };
                string[] splitMesa = labelMesa.Content.ToString().Split(delimiterChars);
                MessageBoxResult result = MessageBox.Show("Estas seguro que quieres eliminar el pedido de la " + labelMesa.Content, "Confirmacion", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        devolverStockyBorrar(listboxitem, true);
                        if (Convert.ToString(splitMesa[1]).Trim() != "0")
                        {
                            desocuparMesa(Convert.ToString(splitMesa[1]));
                        }
                        cargarMesas();
                        break;
                }
            }
        }

        public void desocuparMesa(string mesa)
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
                disponibilidad = "libre",
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
        }
        public void devolverStockyBorrar(ListBoxItem listboxitem, bool eliminar)
        {
            string pedido = "";
            List<Producto> productos = new List<Producto>();
            var url = "http://localhost:3000/productos";
            WebClient wc = new WebClient();
            var datos = wc.DownloadString(url);
            productos = JsonConvert.DeserializeObject<List<Producto>>(datos);
            StackPanel stackPedido = (StackPanel)listboxitem.Content;
            Label id = (Label)stackPedido.Children[1];
            for (int x = 0; x < TodosPedidos.Count; x++)
            {
                if (TodosPedidos[x].idpedido == id.Content.ToString())
                {
                    pedido = TodosPedidos[x].articulos;
                }
            }

            //MessageBox.Show(pedido);
            int posicionNombre = 1;
            int posicionCantidad = 3;
            char[] delimiterChars = { '/' };
            string[] Articulos = pedido.Split(delimiterChars);

            List<string> ids = new List<string>();
            List<string> nombre = new List<string>();
            List<int> cantidad = new List<int>();
            for (int x = 0; x < Articulos.Length / 6; x++)
            {
                nombre.Add(Articulos[posicionNombre].Trim());
                cantidad.Add(Convert.ToInt32(Articulos[posicionCantidad]));
                posicionNombre = posicionNombre + 6;
                posicionCantidad = posicionCantidad + 6;
            }

            for (int x = 0; x < productos.Count; x++)
            {
                if (nombre.Contains(productos[x].designacion.Trim()))
                {
                    int pos = 0;
                    for (int y = 0; y < nombre.Count; y++)
                    {
                        if (nombre[y].ToString() == productos[x].designacion.Trim())
                        {
                            break;
                        }
                        pos++;
                    }
                    productos[x].cantidad = productos[x].cantidad + cantidad[pos];
                    ids.Add(productos[x].id);
                    //MessageBox.Show(Convert.ToString(productos[x].cantidad));
                }
            }

            for (int x = 0; x < ids.Count; x++)
            {
                for (int y = 0; y < productos.Count; y++)
                {
                    if (ids[x] == productos[y].id)
                    {
                        modificarStock(productos[y], y);
                    }
                }
            }

            if (eliminar == true)
            {
                int posPedido = 0;
                for (int x = 0; x < TodosPedidos.Count; x++)
                {
                    if (id.Content.ToString() == TodosPedidos[x].idpedido)
                    {
                        cancelarPedido(TodosPedidos[x], posPedido);
                    }
                    posPedido++;
                }
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
        public void cancelarPedido(Pedidos pedido, int posicion)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:3000/pedidos/" + posicion);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            string json = "";
            var data = new
            {
                idpedido = pedido.idpedido,
                mesa = pedido.mesa,
                articulos = pedido.articulos,
                anulado = true,
                ticket = pedido.ticket,
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
            cargarPedidos();
        }

        private void Button_ClickEditarPedido(object sender, RoutedEventArgs e)
        {
            if (ListBoxPedidos.SelectedIndex == -1)
            {
                MessageBox.Show("Selecciona un Pedido!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (ListBoxPedidos.SelectedItems.Count > 1)
            {
                MessageBox.Show("Selecciona solo un pedido!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {

                ListBoxItem listboxitem = (ListBoxItem)ListBoxPedidos.Items[ListBoxPedidos.SelectedIndex];
                StackPanel stackPedido = (StackPanel)listboxitem.Content;
                Label idPedido = (Label)stackPedido.Children[1];
                //MessageBox.Show(Convert.ToString(idPedido.Content));

                Pedidos pedido = null;

                for (int x = 0; x < TodosPedidos.Count; x++)
                {
                    if (Convert.ToString(idPedido.Content) == TodosPedidos[x].idpedido)
                    {
                        pedido = TodosPedidos[x];
                        break;
                    }
                }
                Application.Current.Properties["idpedio"] = pedido.idpedido;
                Application.Current.Properties["articulos"] = pedido.articulos;
                EditarPedidos EditarPedido = new EditarPedidos();
                Label labelNumeroMesa = (Label)EditarPedido.FindName("NumeroMesa") as Label;
                labelNumeroMesa.Content = "Mesa " + pedido.mesa;
                devolverStockyBorrar(listboxitem, false);
                t1.Stop();

                EditarPedido.Show();
                this.Close();
            }
        }

        private void Button_BuscarPedido(object sender, RoutedEventArgs e)
        {
            EventoBuscarPedido();
        }

        private void EventoBuscarPedido()
        {
            if (BuscarPedido.Text == "")
            {
                cargarPedidos();
            }
            else
            {
                bool encontrado = false;
                List<Pedidos> BusquedaPedido = new List<Pedidos>();
                try
                {
                    for (int x = 0; x < TodosPedidos.Count(); x++)
                    {
                        if (Convert.ToInt32(BuscarPedido.Text.Trim()) == TodosPedidos[x].mesa && TodosPedidos[x].ticket == false && TodosPedidos[x].anulado == false)
                        {
                            encontrado = true;
                            BusquedaPedido.Add(TodosPedidos[x]);
                        }
                    }
                    if (encontrado == true)
                    {
                        ListBoxPedidos.Items.Clear();
                        for (int x = 0; x < BusquedaPedido.Count(); x++)
                        {
                            int posicionCaracteristicas = 4;
                            int posicionNombre = 1;
                            int posicionCantidad = 3;
                            string todo = BusquedaPedido[x].articulos.ToString();
                            char[] delimiterChars = { '/' };
                            string[] Articulos = todo.Split(delimiterChars);
                            int cantidadProductos = Articulos.Count() / 6;
                            ListBoxItem ListBoxItem = new ListBoxItem();
                            ListBoxItem.BorderBrush = Brushes.Black;
                            ListBoxItem.BorderThickness = new Thickness(3);
                            ListBoxItem.Margin = new Thickness(5);
                            ListBoxItem.FontSize = 20;
                            ListBoxItem.FontWeight = FontWeights.Bold;
                            StackPanel stack = new StackPanel();
                            Label Mesa = new Label();
                            Mesa.Content = "Mesa " + BusquedaPedido[x].mesa;
                            Mesa.HorizontalContentAlignment = HorizontalAlignment.Center;
                            Label labelid = new Label();
                            labelid.Visibility = Visibility.Collapsed;
                            labelid.Content = BusquedaPedido[x].idpedido;
                            stack.Children.Add(Mesa);
                            stack.Children.Add(labelid);
                            for (int y = 0; y < cantidadProductos; y++)
                            {
                                if (Articulos[posicionCaracteristicas] == "sin observaciones")
                                {
                                    Label label = new Label();
                                    label.Content = "- " + Articulos[posicionNombre].ToString() + " x" + Articulos[posicionCantidad].ToString();
                                    stack.Children.Add(label);
                                }
                                else
                                {
                                    StackPanel stack2 = new StackPanel();
                                    Label NombreyCantidad = new Label();
                                    Label Espacificaciones = new Label();
                                    NombreyCantidad.Content = "- " + Articulos[posicionNombre].ToString() + " x" + Articulos[posicionCantidad].ToString();
                                    Espacificaciones.Content = "    - " + Articulos[posicionCaracteristicas].ToString();
                                    stack2.Children.Add(NombreyCantidad);
                                    stack2.Children.Add(Espacificaciones);
                                    stack.Children.Add(stack2);
                                }
                                posicionNombre = posicionNombre + 6;
                                posicionCaracteristicas = posicionCaracteristicas + 6;
                                posicionCantidad = posicionCantidad + 6;
                            }
                            ListBoxItem.Content = stack;
                            ListBoxPedidos.Items.Add(ListBoxItem);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Mesa no encontrada", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch
                {
                    MessageBox.Show("Solo puedes escribir numeros!!", "error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Button_CobrarPedido(object sender, RoutedEventArgs e)
        {
            string idpedidos = "";
            string Mesas = "";
            if (ListBoxPedidos.SelectedIndex == -1)
            {
                MessageBox.Show("Tienes que seleccionar un pedido un Pedido!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                for (int x = 0; x < ListBoxPedidos.SelectedItems.Count; x++)
                {
                    ListBoxItem pedido = (ListBoxItem)ListBoxPedidos.SelectedItems[x];
                    StackPanel contenidoItem = (StackPanel)pedido.Content;
                    Label id = (Label)contenidoItem.Children[1];
                    Label mesas = (Label)contenidoItem.Children[0];
                    idpedidos += id.Content + "/";
                    Mesas += mesas.Content + " ";
                }
                MessageBoxResult result = MessageBox.Show("Quieres cobrar los pedidos de: " + Mesas, "Confirmacion", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        generarTicketLiberarMesaEditarPedidoSacarTicket(idpedidos);
                        break;
                }
            }
        }

        public void generarTicketLiberarMesaEditarPedidoSacarTicket(string idspedidos)
        {
            //Hago el tickect
            var url = "http://localhost:3000/tickets";
            WebClient wc = new WebClient();
            var datos = wc.DownloadString(url);
            List<Tickets> TodosTickets = JsonConvert.DeserializeObject<List<Tickets>>(datos);
            string numeroticket = "10000" + (TodosTickets.Count + 1);
            string productos = "";
            string Nowhora = DateTime.Now.ToString("hh:mm");
            string Date = DateTime.Now.ToString("dd-MM-yyyy");

            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:3000/tickets/" + TodosTickets.Count);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            string json = "";
            var data = new
            {
                fecha = Date,
                hora = Nowhora,
                numeroTicket = numeroticket,
                pedidosTicket = idspedidos,
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

            //Pongo en el pedido que tiene un ticket
            char[] delimiterChars = { '/' };
            string[] ids = idspedidos.Split(delimiterChars);

            for (int x = 0; x < ids.Length - 1; x++)
            {
                Pedidos pedidoEditar = null;
                int pos = 0;
                //Saco la posicion del pedido y el pedido
                for (int y = 0; y < TodosPedidos.Count(); y++)
                {
                    if (ids[x] == TodosPedidos[y].idpedido)
                    {
                        pedidoEditar = TodosPedidos[y];
                        //Pongo la mesa libre
                        desocuparMesa(Convert.ToString(TodosPedidos[y].mesa));
                        //Aprovecho y guardo todos los productos en una variable para poder imprimir ticket
                        productos += TodosPedidos[y].articulos;
                        break;
                    }
                    pos++;
                }
                //Actualizo el pedido
                var httpWebRequest2 = (HttpWebRequest)WebRequest.Create("http://localhost:3000/pedidos/" + pos);
                httpWebRequest2.ContentType = "application/json";
                httpWebRequest2.Method = "POST";
                string json2 = "";
                var data2 = new
                {
                    idpedido = pedidoEditar.idpedido,
                    mesa = pedidoEditar.mesa,
                    articulos = pedidoEditar.articulos,
                    anulado = false,
                    ticket = true,
                };

                json2 = JsonConvert.SerializeObject(data2);
                using (var streamWriter = new StreamWriter(httpWebRequest2.GetRequestStream()))
                {
                    streamWriter.Write(json2);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                var httpResponse2 = (HttpWebResponse)httpWebRequest2.GetResponse();
                using (var streamReader = new StreamReader(httpResponse2.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    streamReader.Close();
                }
            }
            MessageBoxResult result2 = MessageBox.Show("Quieres imprimir el ticket?", "Confirmacion", MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (result2)
            {
                case MessageBoxResult.Yes:
                    CrearEstructuraTicket(productos, Int32.Parse(numeroticket), Nowhora, Date);
                    break;
                case MessageBoxResult.No:
                    MessageBox.Show("Ticket Guardado", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
            }
            cargarMesas();
            cargarPedidos();
        }

        private void CrearEstructuraTicket(string articulos, int numeroticket, string Nowhora, string Date)
        {
            List<string> nombre = new List<string>();
            List<double> precio = new List<double>();
            List<int> quantity = new List<int>();
            List<double> subtotal = new List<double>();
            CultureInfo culture = new CultureInfo("en-US");
            double imporTicket = 0f;
            string[] cantidades = articulos.Split('/');
            for (int x = 2; x < cantidades.Length; x = x + 6)
            {
                imporTicket = imporTicket + (double.Parse(cantidades[x]) * double.Parse(cantidades[x + 1]));
            }
            MessageBox.Show(Convert.ToString(imporTicket));
            string encabezado = "RESTAURANTE CARONTE CIF B-36124534\n" + "    CALLE LA ALMADRABA 12, BAJO    \n" +
                "        TEL: 961361257-680121212        \n";
            string lineaFija = "TICKET Nº: " + numeroticket + " FECHA: " + Date + " " + Nowhora + "\n";
            string pie = "TOTAL".PadRight(38, '-') + imporTicket + "€\nGRACIAS POR SU VISITA";

            string articles = articulos;
            string[] articlesSplit = articles.Split('/');

            for (int f = 1; f < articlesSplit.Length; f = f + 6)
            {
                nombre.Add(articlesSplit[f].PadRight(25, '-'));
                double pvp = Double.Parse(articlesSplit[f + 1]);
                precio.Add(pvp);
                int quant = Int32.Parse(articlesSplit[f + 2]);
                quantity.Add(quant);
                double sub = pvp * quant;
                subtotal.Add(sub);

            }
            String s = String.Format("{0,-25} {1,-8} {2, -8} {3, -8}\n\n", "Artículo", "Cantidad", "Precio", "Subtotal");
            for (int index = 0; index < nombre.Count; index++)
                s += String.Format("{0,-25} {1,-8} {2, -8} {3, -8} \n",
                                   nombre[index], quantity[index], precio[index], subtotal[index]);

            string ticketImprimir = encabezado + lineaFija + "\n" + s + pie;
            imprimir(ticketImprimir);
        }

        private void imprimir(string ticket)
        {

            //C:\Users\flval\Desktop\proyecto\tickets
            string path = @"C:\Users\flval\Desktop\proyecto\tickets\nuevoTicket.txt";
            StreamWriter file = new StreamWriter(path, false);
            string createText = ticket + Environment.NewLine;
            file.WriteLine(createText);
            file.Flush();
            file.Close();
            MessageBox.Show("SE HA CREADO EL ARCHIVO PARA IMPRIMIR", "Atención", MessageBoxButton.OK, MessageBoxImage.Information);
            System.Diagnostics.Process.Start(@"C:\Users\flval\Desktop\proyecto\tickets\nuevoTicket.txt");
        }

        private void BuscarPedidoKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) EventoBuscarPedido();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mn = new MainWindow();
            t1.Stop();
            mn.Show();
            this.Close();
        }
    }
}

using proyecto_admin.modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using Label = System.Windows.Forms.Label;
using MessageBox = System.Windows.MessageBox;
using System.Data;
using System.Net;
using Newtonsoft.Json;
using System.Windows.Threading;
using proyecto_admin.Vistas.AyudaGestEvento;

namespace proyecto_admin.Vistas
{
    /// <summary>
    /// Lógica de interacción para GestionEventos.xaml
    /// </summary>
    public partial class GestionEventos : Window
    {
        double topPosition;
        double leftPosition;
        double WidthDesing;
        double HeightDesing;
        Rectangle rec;
        bool Move = false;
        bool positionEnabled = false;
        bool zonaGuardada = true;
        string pers = " personas";
        string zone = "Zona ";
        int counterZona = 1;
        int counterName1 = 0;
        int positionZones = -1;
        double mtsZonaDouble = 0f;
        int aforoZonaInteger = 0;
        int aforoEvento = 0;
        int counter = 0;
        bool nuevaZona = true;
        double metrosEvento = 0;
        string img1 = "";
        string imageToPost = "";
        int webcounter = 0;
        int idZonacreada = 0;
        int countervisit = 0;
        int counterPepople = 0;
        Point point;
        int counterZonasNuevas = 0;
        TextBlock textBlockNombreZona;
        TextBlock textMetrosCuadradosZona;
        TextBlock textAforoZona;
        Zona zona;
        int selectedIndex = 0;
        Eventos eventos;
        int selectedZone = -1;
        List<Eventos> eventosBBDD = new List<Eventos>();
        List<Zona> zonasEvento = new List<Zona>();
        List<Zona> zonasBBDD = new List<Zona>();
        List<Zona> zonaModificada = new List<Zona>();
        List<Zona> todasLasZonas = new List<Zona>();
        List<Registrado> regTodasPersonasBBDD = new List<Registrado>();
        List<Registrado> regPersonasEnZonaBBDD = new List<Registrado>();
        List<Registrado> salidaForzadaPersonas = new List<Registrado>();
        System.Windows.Forms.Timer t1 = new System.Windows.Forms.Timer();
        public GestionEventos()
        {
            InitializeComponent();
            zona = new Zona();
            DataZonas.DataContext = zona;
            eventos = new Eventos();
            DataEventos.DataContext = eventos;
            LienzoClave.IsEnabled = false;
            txtNombreEvento.IsEnabled = false;
            txtDireccionEvento.IsEnabled = false;
            datePickerEvento.IsEnabled = false;
            comboHoraInicio.IsEnabled = false;
            comboMinutosInicio.IsEnabled = false;
            datePickerEventoFin.IsEnabled = false;
            comboHoraFin.IsEnabled = false;
            comboMinutosFin.IsEnabled = false;
            btnEventoAmp.IsEnabled = false;
            txtNambreZona.IsEnabled = false;
            btnActivarZona.IsEnabled = false;
            comboSituacionZona.IsEnabled = false;
            txtMtsZona.IsEnabled = false;
            txtMts2.IsEnabled = false;
            comboAforoPorcentaje.IsEnabled = false;
            btnEstimarAforoZona.IsEnabled = false;
            txtAforoIncluidoZona.IsEnabled = false;
            datePickerZona.IsEnabled = false;
            comboHoraZonaInicio.IsEnabled = false;
            comboMinutosZonaInicio.IsEnabled = false;
            datePickerZonaFin.IsEnabled = false;
            comboHoraZonaFin.IsEnabled = false;
            comboMinutosZonaFin.IsEnabled = false;
            btnInsertPlano.IsEnabled = false;
            btnModificarZona.IsEnabled = false;
            btnCrearNuevaZona.IsEnabled = false;
            btnEliminarZonaCreada.IsEnabled = false;
            btnModificarEvento.IsEnabled = false;
            btnGuardarZonaCreada.IsEnabled = false;
            btnModificarEvento.IsEnabled = false;
            nuevaZona = false;
            cargarArray();
            

        }
        private void recarga()
        {

            t1.Interval = 50000;
            t1.Enabled = true;
            t1.Start();
            t1.Tick += new EventHandler(TimerEventProcessor);
        }

        private void TimerEventProcessor(object sender, EventArgs e)
        {
            
            limpiarDatos();
            cargarArray();

        }
        private void limpiarDatos()
        {
            zonasEvento.Clear();
            eventosBBDD.Clear();
            zonasBBDD.Clear();
            regTodasPersonasBBDD.Clear();
            regPersonasEnZonaBBDD.Clear();
            todasLasZonas.Clear();
            DataEventos.ItemsSource = null;

        }
        private void cargarArray()
        {
            
            webcounter = 0;
            int acumulaGenteEvento = 0;
            int acumulaVisitasEvento = 0;
            double acumulaPorcentajeOcupacionEvento = 0f;
            
            var url = "http://localhost:4000/Eventos";
            WebClient wc = new WebClient();
            var datos = wc.DownloadString(url);
            if (datos == "null")
            {
                webcounter = 1;
            }
            if (webcounter == 0)
            {
                List<Eventos> eventosGet = JsonConvert.DeserializeObject<List<Eventos>>(datos);
                Eventos st;

                for (int g = 0; g < eventosGet.Count; g++)
                {

                    if (eventosGet[g].activoEvento == "Si")
                    {
                        string fechaFin = eventosGet[g].fechaEventoFin;
                        string horaFin = eventosGet[g].horaFinEvento;
                        string[] fechaFinSplit = fechaFin.Split('/');
                        string[] horaFinSplit = horaFin.Split(':');
                        int yearFin = Convert.ToInt32(fechaFinSplit[2]);
                        int monthFin = Convert.ToInt32(fechaFinSplit[1]);
                        int dayFin = Convert.ToInt32(fechaFinSplit[0]);
                        int hourFin = Convert.ToInt32(horaFinSplit[0]);
                        int minFin = Convert.ToInt32(horaFinSplit[1]);
                        DateTime date1 = new DateTime(yearFin, monthFin, dayFin, hourFin, minFin, 0);
                        DateTime date2 = DateTime.Now;

                        int result = DateTime.Compare(date1, date2);

                        if (result < 0)
                        {

                            AltaEvento(g, "No");
                            limpiarDatos();
                            cargarArray();

                        }
                        else
                        {
                            st = new Eventos(eventosGet[g].nombreEvento, eventosGet[g].direccionEvento, eventosGet[g].activoEvento,
                            eventosGet[g].idEvento, eventosGet[g].fechaEvento, eventosGet[g].horaInicioEvento, eventosGet[g].horaFinEvento,
                            eventosGet[g].zonasEvento, eventosGet[g].aforoEvento, eventosGet[g].fotoEvento, eventosGet[g].fechaEventoFin);
                            eventosBBDD.Add(st);
                        }
                    }
                }

                var url2 = "http://localhost:4000/Zonas";
                WebClient wc2 = new WebClient();

                var datos2 = wc2.DownloadString(url2);
                var datosTodasZonas = wc2.DownloadString(url2);

                List<Zona> zonasGet = JsonConvert.DeserializeObject<List<Zona>>(datos2);
                Zona st2;
                Zona todasZona;
                for (int r = 0; r < zonasGet.Count; r++)
                {
                    todasZona = new Zona(zonasGet[r].nombreZona, zonasGet[r].metrosCuadradosZona, zonasGet[r].idZona, zonasGet[r].aforoZona, zonasGet[r].idEventoZona,
                            zonasGet[r].topPositionZona, zonasGet[r].leffPositionZona, zonasGet[r].heightZona, zonasGet[r].widthZona, zonasGet[r].zonaActiva,
                            zonasGet[r].zonaDibujada, zonasGet[r].situacionZona, zonasGet[r].zonaBloqueada, zonasGet[r].inicioZona, zonasGet[r].finZona);
                    todasLasZonas.Add(todasZona);
                }

                for (int t = 0; t < eventosBBDD.Count; t++)
                {
                    for (int z = 0; z < zonasGet.Count; z++)
                    {
                        if (eventosBBDD[t].idEvento == zonasGet[z].idEventoZona)
                        {
                            st2 = new Zona(zonasGet[z].nombreZona, zonasGet[z].metrosCuadradosZona, zonasGet[z].idZona, zonasGet[z].aforoZona, zonasGet[z].idEventoZona,
                            zonasGet[z].topPositionZona, zonasGet[z].leffPositionZona, zonasGet[z].heightZona, zonasGet[z].widthZona, zonasGet[z].zonaActiva,
                            zonasGet[z].zonaDibujada, zonasGet[z].situacionZona, zonasGet[z].zonaBloqueada, zonasGet[z].inicioZona, zonasGet[z].finZona);
                            zonasBBDD.Add(st2);

                        }
                    }
                }

                var url3 = "http://localhost:4000/registrados";
                WebClient wc3 = new WebClient();
                var datos3 = wc3.DownloadString(url3);
                var values = JsonConvert.DeserializeObject<Dictionary<string, Registrado>>(datos3);
                Registrado reg;
                DateTime dateTimeNow = DateTime.Now;
                for (int x = 0; x < zonasBBDD.Count; x++)
                {
                    string[] splFinZona = zonasBBDD[x].finZona.Split(' ');
                    string[] splFechaFin = splFinZona[0].Split('/');
                    string[] splHoraFin = splFinZona[1].Split(':');
                    DateTime zonaFin = new DateTime(Convert.ToInt32(splFechaFin[0]), Convert.ToInt32(splFechaFin[1]), Convert.ToInt32(splFechaFin[2]),
                        Convert.ToInt32(splHoraFin[0]), Convert.ToInt32(splHoraFin[1]), Convert.ToInt32(splHoraFin[2]));
                   
                    int result2 = DateTime.Compare(zonaFin, dateTimeNow);
                    if (result2 < 0)
                    {
                        zonasBBDD[x].zonaActiva = "No";
                        ActivaZona(zonasBBDD[x].idZona - 1000, "No");
                        
                    }
                    
                    foreach (KeyValuePair<string, Registrado> entry in values)
                    {
                        if (zonasBBDD[x].idZona == entry.Value.IdZona)
                        {
                            //Todas las personas

                            //string idPersona, int age, string firstname, int idZona, string identityCard,
                            //string lastname, string timeIn, string timeOut
                            reg = new Registrado(entry.Key, entry.Value.Age, entry.Value.Firstname, entry.Value.IdZona, entry.Value.IdentityCard,
                                entry.Value.Lastname, entry.Value.TimeIn, entry.Value.TimeOut);
                            regTodasPersonasBBDD.Add(reg);
                            countervisit++;
                            zonasBBDD[x].visitasZona = countervisit;
                            //Personas activas en zona
                            if (entry.Value.TimeOut == "")
                            {
                                regPersonasEnZonaBBDD.Add(reg);
                                counterPepople++;
                                zonasBBDD[x].genteEnZona = counterPepople;
                                zonasBBDD[x].porcentajeOcupacionZona = (zonasBBDD[x].genteEnZona) * 100 / zonasBBDD[x].aforoZona;
                                if (zonasBBDD[x].porcentajeOcupacionZona > 0 && zonasBBDD[x].porcentajeOcupacionZona < 26)
                                {
                                    zonasBBDD[x].paintingZona = 25;
                                }
                                if (zonasBBDD[x].porcentajeOcupacionZona > 25 && zonasBBDD[x].porcentajeOcupacionZona < 50)
                                {
                                    zonasBBDD[x].paintingZona = 35;
                                }
                                if (zonasBBDD[x].porcentajeOcupacionZona > 49 && zonasBBDD[x].porcentajeOcupacionZona < 66)
                                {
                                    zonasBBDD[x].paintingZona = 55;
                                }
                                if (zonasBBDD[x].porcentajeOcupacionZona > 65 && zonasBBDD[x].porcentajeOcupacionZona < 90)
                                {
                                    zonasBBDD[x].paintingZona = 75;
                                }
                                if (zonasBBDD[x].porcentajeOcupacionZona > 89)
                                {
                                    zonasBBDD[x].paintingZona = 90;
                                }
                                if (counterPepople >= zonasBBDD[x].aforoZona)
                                {
                                    //put zona bloqueada
                                    putZonaBloqueada(zonasBBDD[x].idZona - 1000, "Si");
                                }
                                if (counterPepople < zonasBBDD[x].aforoZona )
                                {
                                    //put zona liberada
                                    putZonaBloqueada(zonasBBDD[x].idZona - 1000, "No");
                                }
                            }
                        }
                    }
                    counterPepople = 0;
                    countervisit = 0;
                }
                for (int k = 0; k < eventosBBDD.Count; k++)
                {
                    for (int x = 0; x < zonasBBDD.Count; x++)
                    {
                        if (eventosBBDD[k].idEvento == zonasBBDD[x].idEventoZona)
                        {
                            acumulaGenteEvento = acumulaGenteEvento + zonasBBDD[x].genteEnZona;
                            acumulaVisitasEvento = acumulaVisitasEvento + zonasBBDD[x].visitasZona;
                            acumulaPorcentajeOcupacionEvento = acumulaPorcentajeOcupacionEvento + zonasBBDD[x].porcentajeOcupacionZona;
                        }
                    }
                    
                    eventosBBDD[k].genteEnEvento = acumulaGenteEvento;
                    eventosBBDD[k].visitasEvento = acumulaVisitasEvento;
                    eventosBBDD[k].porcentageOcupacionEvento = (eventosBBDD[k].genteEnEvento) * 100 / eventosBBDD[k].aforoEvento;
                    if (eventosBBDD[k].porcentageOcupacionEvento > 0 && eventosBBDD[k].porcentageOcupacionEvento < 26)
                    {
                        eventosBBDD[k].paintingEvento = 25;
                    }
                    if (eventosBBDD[k].porcentageOcupacionEvento > 25 && eventosBBDD[k].porcentageOcupacionEvento < 50)
                    {
                        eventosBBDD[k].paintingEvento = 35;
                    }
                    if (eventosBBDD[k].porcentageOcupacionEvento > 49 && eventosBBDD[k].porcentageOcupacionEvento < 66)
                    {
                        eventosBBDD[k].paintingEvento = 55;
                    }
                    if (eventosBBDD[k].porcentageOcupacionEvento > 65 && eventosBBDD[k].porcentageOcupacionEvento < 90)
                    {
                        eventosBBDD[k].paintingEvento = 75;
                    }
                    if (eventosBBDD[k].porcentageOcupacionEvento > 89 && eventosBBDD[k].porcentageOcupacionEvento < 97)
                    {
                        eventosBBDD[k].paintingEvento = 90;
                    }
                    acumulaGenteEvento = 0;
                    acumulaVisitasEvento = 0;
                    acumulaPorcentajeOcupacionEvento = 0;
                }
                DataEventos.ItemsSource = eventosBBDD;
            }
        }

        private void putZonaBloqueada(int v1, string zonaBloqueada1)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(" http://localhost:4000/Zonas-bloqueo/" + v1);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "PUT";
            string json = "";
            var data = new
            {
                zonaBloqueada = zonaBloqueada1
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

        private void LienzoClave_MouseMove(object sender, MouseEventArgs e)
        {

            if (positionEnabled == true)
            {
                if (Move)
                {
                    Point p = e.GetPosition(LienzoClave);
                    WidthDesing = p.X - leftPosition;
                    HeightDesing = p.Y - topPosition;
                    if (WidthDesing < 0 || HeightDesing < 0)
                    { }
                    else
                    {
                        SolidColorBrush mySolidColorBrush = new SolidColorBrush(Colors.Black);
                        mySolidColorBrush.Opacity = 0.01;
                        rec = new Rectangle();
                        rec.StrokeThickness = 3;
                        rec.Fill = mySolidColorBrush;
                        rec.StrokeThickness = 4;
                        rec.Width = Convert.ToInt32(WidthDesing);
                        rec.Height = Convert.ToInt32(HeightDesing);
                        Canvas.SetLeft(rec, leftPosition);
                        Canvas.SetTop(rec, topPosition);
                        textBlockNombreZona = new TextBlock();
                        Canvas.SetLeft(textBlockNombreZona, leftPosition + 5);
                        Canvas.SetTop(textBlockNombreZona, topPosition + 5);
                        textBlockNombreZona.FontSize = 10;
                        textBlockNombreZona.Foreground = new SolidColorBrush(Colors.White);
                        if (txtNambreZona.Text == "" || txtNambreZona.Text == "Opcional")
                        {
                            textBlockNombreZona.Text = zone;
                        }
                        else
                        {
                            for (int i = 0; i < zonasEvento.Count; i++)
                            {
                                if (txtNambreZona.Text.Equals(zonasEvento[i].nombreZona))
                                {
                                    counterName1++;
                                }
                            }
                            if (counterName1 > 0)
                            {
                                MessageBox.Show("El nombre introducido ya está en uso. Por favor, ingresa otro nombre.", "ATENCIÓN", MessageBoxButton.OK, MessageBoxImage.Error);
                                txtNambreZona.Text = "";
                                zone = "";
                                counterZona--;
                            }
                            else
                            {
                                textBlockNombreZona.Text = txtNambreZona.Text;
                            }
                        }
                        textMetrosCuadradosZona = new TextBlock();
                        Canvas.SetLeft(textMetrosCuadradosZona, leftPosition + 5);
                        Canvas.SetTop(textMetrosCuadradosZona, topPosition + 20);
                        textMetrosCuadradosZona.FontSize = 10;
                        textMetrosCuadradosZona.Foreground = new SolidColorBrush(Colors.White);
                        textMetrosCuadradosZona.Text = txtMtsZona.Text + " metros2";
                        textAforoZona = new TextBlock();
                        Canvas.SetLeft(textAforoZona, leftPosition + 5);
                        Canvas.SetTop(textAforoZona, topPosition + 35);
                        textAforoZona.FontSize = 10;
                        textAforoZona.Foreground = new SolidColorBrush(Colors.White);
                        textAforoZona.Text = txtAforoIncluidoZona.Text + pers;
                    }
                    if (counterName1 == 0)
                    {
                        try
                        {
                            LienzoClave.Children.Add(rec);
                            LienzoClave.Children.Add(textBlockNombreZona);
                            LienzoClave.Children.Add(textMetrosCuadradosZona);
                            LienzoClave.Children.Add(textAforoZona);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                    }
                    else
                    {
                        Move = false;
                    }
                }
            }
        }
        private void LienzoClave_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Move = false;
            zone = "Zona ";
            positionEnabled = false;
            zonaGuardada = false;
            
        }
        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {

            Ayuda1 ayuda1 = new Ayuda1();
            ayuda1.Show();
        }

        private void txtNambreZona_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            txtNambreZona.Text = "";
        }

        private void btnActivarZona_Click(object sender, RoutedEventArgs e)
        {
            
                selectedIndex = DataZonas.SelectedIndex;
            
            
           
            if (datePickerZona.IsEnabled == false && datePickerZonaFin.IsEnabled == false)
            {
                MessageBox.Show("No se puede activar una zona con fecha fin anterior a la fecha actual. Crea una nueva zona con las nuevas fechas.", "Atención", MessageBoxButton.OK, MessageBoxImage.Error);
            } else if (datePickerZona.IsEnabled == false)
            {
                DialogResult result2 = (DialogResult)MessageBox.Show("¡ATENCIÓN LEE ESTA INFORMACIÓN CON DETENIMIENTO. EN ESTE PUNTO TUS ACTOS TIENEN CONSECUENCIAS PARA LOS USUARIOS ACTIVOS DEL EVENTO!:\n\n -Si elegiste una zona ACTIVA para DESACTIVAR, LA ZONA SE CERRARÁ Y LOS USUARIOS DEBERÁN ABANDONAR LA ZONA.\n\n-Si por el contrario" +
                    " decidiste ACTIVAR UNA ZONA DESACTIVADA, DEBERÁS asegurarte de que la Zona está totalmente vacia antes de hacerlo. " +
                                           "\n\nComprueba y pulsa SI para continuar o NO para abortar la acción.", "ATENCIÓN", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result2 == System.Windows.Forms.DialogResult.Yes)
                {
                    if (positionZones > -1)
                    {
                        
                        if (zonasEvento[selectedIndex].zonaActiva == "Si")
                        {
                            zonasEvento[selectedIndex].zonaActiva = "No";

                            ActivaZona(zonasEvento[selectedIndex].idZona - 1000, "No");
                            var url3 = "http://localhost:4000/registrados";
                            WebClient wc3 = new WebClient();
                            var datos3 = wc3.DownloadString(url3);
                            var values = JsonConvert.DeserializeObject<Dictionary<string, Registrado>>(datos3);
                            Registrado reg;
                            
                            foreach (KeyValuePair<string, Registrado> entry in values)
                            {
                                if (zonasEvento[selectedIndex].idZona == entry.Value.IdZona && entry.Value.TimeOut == "") 
                                {
                                    //Todas las personas

                                    //string idPersona, int age, string firstname, int idZona, string identityCard,
                                    //string lastname, string timeIn, string timeOut
                                    reg = new Registrado(entry.Key, entry.Value.Age, entry.Value.Firstname, entry.Value.IdZona, entry.Value.IdentityCard,
                                        entry.Value.Lastname, entry.Value.TimeIn, entry.Value.TimeOut);
                                    salidaForzadaPersonas.Add(reg);
                                }
                            }
                            for (int j = 0; j < salidaForzadaPersonas.Count; j++)
                            {
                                
                                string[] dateNow = DateTime.Now.ToString().Split(' ');
                                string[] dateNowSpl = dateNow[0].Split('/');
                                string fechSalida = dateNowSpl[2] + "/" + dateNowSpl[1] + "/" + dateNowSpl[0] + " " + dateNow[1];
                                SalidaForzadaPersona(salidaForzadaPersonas[j].idPersona, fechSalida);                                
                            }
                            salidaForzadaPersonas.Clear();
                            resetItems();
                            MessageBox.Show("La zona ha sido modificada.\nPulsa sobre el evento para refrescar los datos.", "INFORMACIÓN", MessageBoxButton.OK, MessageBoxImage.Information);

                        }
                        else
                        {
                            DateTime zonafin = DateTime.Parse(zonasEvento[selectedIndex].finZona);
                            DateTime ahora = DateTime.Now;
                            int result = DateTime.Compare(zonafin, ahora);
                            if (result < 0)
                            {
                                MessageBox.Show("La zona no ha sido activada porque su fecha de fin es anterior a la actual.\nModifica la fecha de fin y pulsa el botón modificar evento antes de activar.", "INFORMACIÓN", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                            {
                                ActivaZona(zonasEvento[selectedIndex].idZona - 1000, "Si");
                                resetItems();
                                MessageBox.Show("La zona ha sido modificada.\nPulsa sobre el evento para refrescar los datos.", "INFORMACIÓN", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            
                        }
                    }
                    else
                    {
                        MessageBox.Show("Debes seleccionar una Zona de la lista, para activarla o inactivarla", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                { }
            }
            else
            {
                DialogResult result2 = (DialogResult)MessageBox.Show("¡ATENCIÓN LEE ESTA INFORMACIÓN CON DETENIMIENTO. EN ESTE PUNTO TUS ACTOS TIENEN CONSECUENCIAS PARA LOS USUARIOS ACTIVOS DEL EVENTO!:\n\n -Si elegiste una zona ACTIVA para DESACTIVAR, LA ZONA SE CERRARÁ Y LOS USUARIOS DEBERÁN ABANDONAR LA ZONA.\n\n-Si por el contrario" +
                    " decidiste ACTIVAR UNA ZONA DESACTIVADA, DEBERÁS asegurarte de que la Zona está totalmente vacia antes de hacerlo. " +
                                           "\n\nComprueba y pulsa SI para continuar o NO para abortar la acción.", "ATENCIÓN", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result2 == System.Windows.Forms.DialogResult.Yes)
                {
                    if (positionZones > -1)
                    {

                        if (zonasEvento[selectedIndex].zonaActiva == "Si")
                        {
                            zonasEvento[selectedIndex].zonaActiva = "No";

                            ActivaZona(zonasEvento[selectedIndex].idZona - 1000, "No");
                            for (int j = 0; j < regPersonasEnZonaBBDD.Count; j++)
                            {
                                if (zonasEvento[selectedIndex].idZona == regPersonasEnZonaBBDD[j].IdZona)
                                {
                                    string[] dateNow = DateTime.Now.ToString().Split(' ');
                                    string[] dateNowSpl = dateNow[0].Split('/');
                                    string fechSalida = dateNowSpl[2] + "/" + dateNowSpl[1] + "/" + dateNowSpl[0] + " " + dateNow[1];
                                    SalidaForzadaPersona(regPersonasEnZonaBBDD[j].idPersona, fechSalida);

                                }
                            }

                            resetItems();
                            MessageBox.Show("La zona ha sido modificada.\nPulsa sobre el evento para refrescar los datos.", "INFORMACIÓN", MessageBoxButton.OK, MessageBoxImage.Information);

                        }
                        else
                        {
                            ActivaZona(zonasEvento[selectedIndex].idZona - 1000, "Si");
                            resetItems();
                            MessageBox.Show("La zona ha sido modificada.\nPulsa sobre el evento para refrescar los datos.", "INFORMACIÓN", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Debes seleccionar una Zona de la lista, para activarla o inactivarla", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }


        }

        private void resetItems()
        {
            LienzoClave.IsEnabled = false;
            txtNombreEvento.IsEnabled = false;
            txtNombreEvento.Text = "";
            txtMts2.IsEnabled = false;
            txtMts2.Text = "";
            txtDireccionEvento.IsEnabled = false;
            txtDireccionEvento.Text = "";
            datePickerEvento.IsEnabled = false;
            datePickerEvento.SelectedDate = DateTime.Now;
            comboHoraInicio.IsEnabled = false;
            comboHoraInicio.Text = "Hora";
            comboMinutosInicio.IsEnabled = false;
            comboMinutosInicio.Text = "Minutos";
            datePickerEventoFin.IsEnabled = false;
            datePickerEventoFin.SelectedDate = DateTime.Now;
            comboHoraFin.IsEnabled = false;
            comboHoraFin.Text = "Hora";
            comboMinutosFin.IsEnabled = false;
            comboMinutosFin.Text = "Minutos";
            btnEventoAmp.IsEnabled = false;
            txtNambreZona.IsEnabled = false;
            txtNambreZona.Text = "Opcional";
            btnActivarZona.IsEnabled = false;
            comboSituacionZona.IsEnabled = false;
            comboSituacionZona.Text = "";
            txtMtsZona.IsEnabled = false;
            txtMtsZona.Text = "";
            txtVisitasZona.IsEnabled = false;
            txtVisitasZona.Text = "";
            txtZonaActiva.IsEnabled = false;
            txtZonaActiva.Text = "";
            txtOcupacionPorcentajeZona.IsEnabled = false;
            txtOcupacionPorcentajeZona.Text = "";
            txtOcupacionPersonasZona.Text = "";
            txtOcupacionPersonasZona.IsEnabled = false;
            txtOcupacionPorcentajEvento.IsEnabled = false;
            txtOcupacionPorcentajEvento.Text = "";
            txtOcupacionPaxEvento.IsEnabled = false;
            txtOcupacionPaxEvento.Text = "";
            txtOcupacionPorcentajeZona.Background = new SolidColorBrush(Colors.White);
            txtOcupacionPersonasZona.Background = new SolidColorBrush(Colors.White);
            txtOcupacionPaxEvento.Background = new SolidColorBrush(Colors.White);
            txtOcupacionPorcentajEvento.Background = new SolidColorBrush(Colors.White);
            comboAforoPorcentaje.IsEnabled = false;
            comboAforoPorcentaje.Text = "";
            btnEstimarAforoZona.IsEnabled = false;
            txtAforoIncluidoZona.IsEnabled = false;
            txtAforoIncluidoZona.Text = "";
            datePickerZona.IsEnabled = false;
            datePickerZona.SelectedDate = DateTime.Now;
            comboHoraZonaInicio.IsEnabled = false;
            comboHoraZonaInicio.Text = "Hora";
            comboMinutosZonaInicio.IsEnabled = false;
            comboMinutosZonaInicio.Text = "Minutos";
            datePickerZonaFin.IsEnabled = false;
            datePickerZonaFin.SelectedDate = DateTime.Now;
            comboHoraZonaFin.IsEnabled = false;
            comboHoraZonaFin.Text = "Hora";
            comboMinutosZonaFin.IsEnabled = false;
            comboMinutosZonaFin.Text = "Minutos";
            btnInsertPlano.IsEnabled = false;
            btnModificarZona.IsEnabled = false;
            btnCrearNuevaZona.IsEnabled = false;
            btnEliminarZonaCreada.IsEnabled = false;
            btnModificarEvento.IsEnabled = false;
            if (counterZonasNuevas > 0)
            {
                btnGuardarZonaCreada.IsEnabled = false;
                btnEliminarZonaCreada.IsEnabled = true;
            }
            else
            {
                btnGuardarZonaCreada.IsEnabled = true;
                btnEliminarZonaCreada.IsEnabled = false;
            }
            
            btnModificarEvento.IsEnabled = false;
            LienzoClave.Children.Clear();
            LienzoClave.Background = new SolidColorBrush(Colors.White);
            LienzoClave.Opacity = 0.5;
            zonasEvento.Clear();
            DataZonas.ItemsSource = null;
            DataZonas.ItemsSource = zonasEvento;

            limpiarDatos();
            cargarArray();
        }

        private void SalidaForzadaPersona(string idPersona, string timeOut1)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(" http://localhost:4000/registrados/" + idPersona);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "PUT";
            string json = "";
            var data = new
            {
                timeOut = timeOut1
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

        private void DataZonas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            positionZones = DataZonas.SelectedIndex;
            
            txtZonaActiva.IsEnabled = true;
            txtVisitasZona.IsEnabled = true;
            txtOcupacionPersonasZona.IsEnabled = true;
            txtOcupacionPorcentajeZona.IsEnabled = true;
            txtNambreZona.IsEnabled = true;
            btnActivarZona.IsEnabled = true;
            comboSituacionZona.IsEnabled = true;
            txtMtsZona.IsEnabled = true;
            comboAforoPorcentaje.IsEnabled = true;
            btnEstimarAforoZona.IsEnabled = true;
            txtAforoIncluidoZona.IsEnabled = true;
            datePickerZona.IsEnabled = true;
            comboHoraZonaInicio.IsEnabled = true;
            comboMinutosZonaInicio.IsEnabled = true;
            datePickerZonaFin.IsEnabled = true;
            comboHoraZonaFin.IsEnabled = true;
            comboMinutosZonaFin.IsEnabled = true;
            btnInsertPlano.IsEnabled = true;
            btnModificarZona.IsEnabled = true;
            btnCrearNuevaZona.IsEnabled = true;
            btnModificarEvento.IsEnabled = true;
            
            btnGuardarZonaCreada.IsEnabled = false;
            if (positionZones > -1)
            {
               
                txtNambreZona.Text = zonasEvento[positionZones].nombreZona;
                txtZonaActiva.Text = zonasEvento[positionZones].zonaActiva;
                txtMtsZona.Text = (zonasEvento[positionZones].metrosCuadradosZona).ToString();
                txtAforoIncluidoZona.Text = zonasEvento[positionZones].aforoZona.ToString();
                txtIdzona.Text = zonasEvento[positionZones].idZona.ToString();

                txtOcupacionPersonasZona.Text = zonasEvento[positionZones].genteEnZona.ToString();
                decimal percent = Convert.ToDecimal(zonasEvento[positionZones].porcentajeOcupacionZona);
                txtOcupacionPorcentajeZona.Text = percent.ToString("N2") + "%";
                txtVisitasZona.Text = zonasEvento[positionZones].visitasZona.ToString();
                SolidColorBrush mySolidColorBrush10 = new SolidColorBrush();
                if (percent == 0)
                {
                    mySolidColorBrush10.Color = Colors.White;
                    txtOcupacionPersonasZona.Background = mySolidColorBrush10;
                    txtOcupacionPorcentajeZona.Background = mySolidColorBrush10;

                }
                if (percent > 0 && percent < 26)
                {
                    mySolidColorBrush10.Color = Color.FromRgb(15, 236, 15);
                    txtOcupacionPersonasZona.Background = mySolidColorBrush10;
                    txtOcupacionPorcentajeZona.Background = mySolidColorBrush10;
                }
                if (percent > 25 && percent < 50)
                {
                    mySolidColorBrush10.Color = Color.FromRgb(7, 129, 7);
                    txtOcupacionPersonasZona.Background = mySolidColorBrush10;
                    txtOcupacionPorcentajeZona.Background = mySolidColorBrush10;
                }
                if (percent > 49 && percent < 66)
                {
                    mySolidColorBrush10.Color = Color.FromRgb(224, 245, 12);
                    txtOcupacionPersonasZona.Background = mySolidColorBrush10;
                    txtOcupacionPorcentajeZona.Background = mySolidColorBrush10;
                }
                if (percent > 65 && percent < 90)
                {
                    mySolidColorBrush10.Color = Color.FromRgb(245, 128, 12);
                    txtOcupacionPersonasZona.Background = mySolidColorBrush10;
                    txtOcupacionPorcentajeZona.Background = mySolidColorBrush10;
                }
                if (percent > 89)
                {
                    mySolidColorBrush10.Color = Color.FromRgb(245, 12, 12);
                    txtOcupacionPersonasZona.Background = mySolidColorBrush10;
                    txtOcupacionPorcentajeZona.Background = mySolidColorBrush10;
                }
                string[] splInicioZona = zonasEvento[positionZones].inicioZona.Split(' ');
                string[] splFechaInicio = splInicioZona[0].Split('/');
                string[] splHoraInicio = splInicioZona[1].Split(':');
                DateTime zonaInit = new DateTime(Convert.ToInt32(splFechaInicio[0]), Convert.ToInt32(splFechaInicio[1]), Convert.ToInt32(splFechaInicio[2]),
                    Convert.ToInt32(splHoraInicio[0]), Convert.ToInt32(splHoraInicio[1]), Convert.ToInt32(splHoraInicio[2]));
                DateTime dateTimeNow = DateTime.Now;
                datePickerZona.SelectedDate = new DateTime(Convert.ToInt32(splFechaInicio[0]), Convert.ToInt32(splFechaInicio[1]), Convert.ToInt32(splFechaInicio[2]));
                comboHoraZonaInicio.Text = splHoraInicio[0];
                comboMinutosZonaInicio.Text = splHoraInicio[1];
                int result = DateTime.Compare(zonaInit, dateTimeNow);
                if (result < 0 && zonasEvento[positionZones].visitasZona > 0)
                {
                    datePickerZona.IsEnabled = false;
                    comboHoraZonaInicio.IsEnabled = false;
                    comboMinutosZonaInicio.IsEnabled = false;
                }
                if (result < 0)
                {
                    datePickerZona.IsEnabled = false;
                    comboHoraZonaInicio.IsEnabled = false;
                    comboMinutosZonaInicio.IsEnabled = false;
                }
                string[] splFinZona = zonasEvento[positionZones].finZona.Split(' ');
                string[] splFechaFin = splFinZona[0].Split('/');
                string[] splHoraFin = splFinZona[1].Split(':');
                DateTime zonaFin = new DateTime(Convert.ToInt32(splFechaFin[0]), Convert.ToInt32(splFechaFin[1]), Convert.ToInt32(splFechaFin[2]),
                    Convert.ToInt32(splHoraFin[0]), Convert.ToInt32(splHoraFin[1]), Convert.ToInt32(splHoraFin[2]));
                datePickerZonaFin.SelectedDate = new DateTime(Convert.ToInt32(splFechaFin[0]), Convert.ToInt32(splFechaFin[1]), Convert.ToInt32(splFechaFin[2]));
                comboHoraZonaFin.Text = splHoraFin[0];
                comboMinutosZonaFin.Text = splHoraFin[1];
                int result2 = DateTime.Compare(zonaFin, dateTimeNow);
                if (result2 < 0 && zonasEvento[positionZones].visitasZona > 0)
                {
                    txtNambreZona.IsEnabled = false;
                    comboSituacionZona.IsEnabled = false;
                    txtMtsZona.IsEnabled = false;
                    txtZonaActiva.IsEnabled = false;
                    comboAforoPorcentaje.IsEnabled = false;
                    btnEstimarAforoZona.IsEnabled = false;
                    txtAforoIncluidoZona.IsEnabled = false;
                    datePickerZona.IsEnabled = false;
                    comboHoraZonaInicio.IsEnabled = false;
                    comboMinutosZonaInicio.IsEnabled = false;
                    datePickerZonaFin.IsEnabled = false;
                    comboHoraZonaFin.IsEnabled = false;
                    comboMinutosZonaFin.IsEnabled = false;
                    btnModificarZona.IsEnabled = true;
                    btnGuardarZonaCreada.IsEnabled = false;

                }
            }
            
        }

        private void btnEstimarAforoZona_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double mts = Convert.ToDouble(txtMtsZona.Text) / 4;
                int mtsint = Convert.ToInt32(mts);
                int percentatje = Convert.ToInt32(comboAforoPorcentaje.Text);
                mtsint = mtsint * percentatje / 100;
                if (mtsint > 0)
                {
                    txtAforoEstimado.Text = mtsint.ToString();
                }
                else
                {
                    MessageBox.Show("El campo metros cuadrados no puede ser negativo.", "ATENCIÓN", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("El campo metros cuadrados no es correcto.", "ATENCIÓN", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine(ex);
            }
        }

        private void DataEventos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if (DataEventos.SelectedIndex > -1)
            {
                
                DataZonas.IsEnabled = true;
                
                cargarEvento();
            }
        }

        private void cargarEvento()
        {

            img1 = "";
            DataZonas.ItemsSource = null;
            zonasEvento.Clear();
            zonaModificada.Clear();
            aforoEvento = 0;
            metrosEvento = 0;
            DataZonas.ItemsSource = null;
            LienzoClave.Children.Clear();
            LienzoClave.IsEnabled = true;
            txtNombreEvento.IsEnabled = true;
            txtDireccionEvento.IsEnabled = true;
            datePickerEvento.IsEnabled = true;
            comboHoraInicio.IsEnabled = true;
            comboMinutosInicio.IsEnabled = true;
            datePickerEventoFin.IsEnabled = true;
            comboHoraFin.IsEnabled = true;
            comboMinutosFin.IsEnabled = true;
            btnEventoAmp.IsEnabled = true;
            btnInsertPlano.IsEnabled = true;
            btnModificarEvento.IsEnabled = true;
            txtMts2.IsEnabled = true;
            txtOcupacionPaxEvento.IsEnabled = true;
            txtOcupacionPorcentajEvento.IsEnabled = true;
            btnModificarEvento.IsEnabled = true;

            selectedZone = DataEventos.SelectedIndex;
            txtOcupacionPaxEvento.Text = eventosBBDD[selectedZone].genteEnEvento.ToString();
            txtIdEvento.Text = eventosBBDD[selectedZone].idEvento.ToString();
            decimal percent = Convert.ToDecimal(eventosBBDD[selectedZone].porcentageOcupacionEvento);
            txtOcupacionPorcentajEvento.Text = percent.ToString("N2") + "%";
            txtVisitasEvento.Text = eventosBBDD[selectedZone].visitasEvento.ToString();
            SolidColorBrush mySolidColorBrush10 = new SolidColorBrush();
            if (eventosBBDD[selectedZone].fotoEvento == "")
            {
                positionEnabled = false;
            }
            else
            {
                positionEnabled = true;
            }
            if (eventosBBDD[selectedZone].porcentageOcupacionEvento == 0)
            {
                mySolidColorBrush10.Color = Colors.White;
                txtOcupacionPaxEvento.Background = mySolidColorBrush10;
                txtOcupacionPorcentajEvento.Background = mySolidColorBrush10;
            }
            if (eventosBBDD[selectedZone].porcentageOcupacionEvento > 0 && eventosBBDD[selectedZone].porcentageOcupacionEvento < 26)
            {
                mySolidColorBrush10.Color = Color.FromRgb(15, 236, 15);
                txtOcupacionPaxEvento.Background = mySolidColorBrush10;
                txtOcupacionPorcentajEvento.Background = mySolidColorBrush10;
            }
            if (eventosBBDD[selectedZone].porcentageOcupacionEvento > 25 && eventosBBDD[selectedZone].porcentageOcupacionEvento < 50)
            {
                mySolidColorBrush10.Color = Color.FromRgb(7, 129, 7);
                txtOcupacionPaxEvento.Background = mySolidColorBrush10;
                txtOcupacionPorcentajEvento.Background = mySolidColorBrush10;
            }
            if (eventosBBDD[selectedZone].porcentageOcupacionEvento > 49 && eventosBBDD[selectedZone].porcentageOcupacionEvento < 66)
            {
                mySolidColorBrush10.Color = Color.FromRgb(224, 245, 12);
                txtOcupacionPaxEvento.Background = mySolidColorBrush10;
                txtOcupacionPorcentajEvento.Background = mySolidColorBrush10;
            }
            if (eventosBBDD[selectedZone].porcentageOcupacionEvento > 65 && eventosBBDD[selectedZone].porcentageOcupacionEvento < 90)
            {
                mySolidColorBrush10.Color = Color.FromRgb(245, 128, 12);
                txtOcupacionPaxEvento.Background = mySolidColorBrush10;
                txtOcupacionPorcentajEvento.Background = mySolidColorBrush10;
            }
            if (eventosBBDD[selectedZone].porcentageOcupacionEvento > 89 && eventosBBDD[selectedZone].porcentageOcupacionEvento < 97)
            {
                mySolidColorBrush10.Color = Color.FromRgb(245, 12, 12);
                txtOcupacionPaxEvento.Background = mySolidColorBrush10;
                txtOcupacionPorcentajEvento.Background = mySolidColorBrush10;
            }
            string date = eventosBBDD[selectedZone].fechaEvento;
            string[] dateSplit = date.Split('/');
            txtNombreEvento.Text = eventosBBDD[selectedZone].nombreEvento;
            txtDireccionEvento.Text = eventosBBDD[selectedZone].direccionEvento;
            comboEventoActivo.Text = eventosBBDD[selectedZone].activoEvento;
            string[] horaInicioEvento = eventosBBDD[selectedZone].horaInicioEvento.Split(':');
            comboHoraInicio.Text = horaInicioEvento[0];
            comboMinutosInicio.Text = horaInicioEvento[1];
            DateTime dateTimeInitEvento = new DateTime(Convert.ToInt32(dateSplit[2]), Convert.ToInt32(dateSplit[1]), Convert.ToInt32(dateSplit[0]), Convert.ToInt32(horaInicioEvento[0]), Convert.ToInt32(horaInicioEvento[1]), 0);
            DateTime dateTimeNow = DateTime.Now;
            int result = DateTime.Compare(dateTimeInitEvento, dateTimeNow);
            if (result <= 0)
            {
                datePickerEvento.IsEnabled = false;

                comboHoraInicio.IsEnabled = false;

                comboMinutosInicio.IsEnabled = false;
            }
            string[] horaFinEvento = eventosBBDD[selectedZone].horaFinEvento.Split(':');
            comboHoraFin.Text = horaFinEvento[0];
            comboMinutosFin.Text = horaFinEvento[1];

            datePickerEvento.SelectedDate = new DateTime(Convert.ToInt32(dateSplit[2]), Convert.ToInt32(dateSplit[1]), Convert.ToInt32(dateSplit[0]));
            string date2 = eventosBBDD[selectedZone].fechaEventoFin;
            string[] dateSplit2 = date2.Split('/');
            datePickerEventoFin.SelectedDate = new DateTime(Convert.ToInt32(dateSplit2[2]), Convert.ToInt32(dateSplit2[1]), Convert.ToInt32(dateSplit2[0]));
            if (eventosBBDD[selectedZone].fotoEvento != "")
            {
                BitmapImage bi3 = new BitmapImage();
                bi3.BeginInit();
                bi3.UriSource = new Uri(eventosBBDD[selectedZone].fotoEvento);
                bi3.EndInit();
                ImageBrush imageBrush = new ImageBrush();
                imageBrush.ImageSource = new BitmapImage(bi3.UriSource);
                LienzoClave.Background = imageBrush;
                LienzoClave.Opacity = 1;
                positionEnabled = true;
            }
            else
            {
                //Background="White" Opacity="0.5"
                LienzoClave.Background = new SolidColorBrush(Colors.White);
                LienzoClave.Opacity = 0.5;
            }

            for (int b = 0; b < zonasBBDD.Count; b++)
            {
                if (eventosBBDD[selectedZone].idEvento == zonasBBDD[b].idEventoZona)
                {
                    zonasEvento.Add(zonasBBDD[b]);
                }
            }
            for (int c = 0; c < zonasEvento.Count; c++)
            {

                SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                textBlockNombreZona = new TextBlock();
                if (zonasEvento[c].porcentajeOcupacionZona == 0)
                {
                    mySolidColorBrush.Color = Colors.Black;
                    textBlockNombreZona.Foreground = new SolidColorBrush(Colors.White);
                }
                if (zonasEvento[c].porcentajeOcupacionZona > 0 && zonasEvento[c].porcentajeOcupacionZona < 26)
                {
                    mySolidColorBrush.Color = Color.FromRgb(15, 236, 15);
                    textBlockNombreZona.Foreground = new SolidColorBrush(Colors.Black);
                }
                if (zonasEvento[c].porcentajeOcupacionZona > 25 && zonasEvento[c].porcentajeOcupacionZona < 50)
                {
                    mySolidColorBrush.Color = Color.FromRgb(7, 129, 7);
                    textBlockNombreZona.Foreground = new SolidColorBrush(Colors.White);
                }
                if (zonasEvento[c].porcentajeOcupacionZona > 49 && zonasEvento[c].porcentajeOcupacionZona < 66)
                {
                    mySolidColorBrush.Color = Color.FromRgb(224, 245, 12);
                    textBlockNombreZona.Foreground = new SolidColorBrush(Colors.Black);
                }
                if (zonasEvento[c].porcentajeOcupacionZona > 65 && zonasEvento[c].porcentajeOcupacionZona < 90)
                {
                    mySolidColorBrush.Color = Color.FromRgb(245, 128, 12);
                    textBlockNombreZona.Foreground = new SolidColorBrush(Colors.White);
                }
                if (zonasEvento[c].porcentajeOcupacionZona > 89 )
                {
                    mySolidColorBrush.Color = Color.FromRgb(245, 12, 12);
                    textBlockNombreZona.Foreground = new SolidColorBrush(Colors.White);
                }
                rec = new Rectangle();
                rec.StrokeThickness = 3;
                rec.Fill = mySolidColorBrush;
                rec.StrokeThickness = 4;
                rec.Width = (zonasEvento[c].widthZona);
                rec.Height = zonasEvento[c].heightZona;
                Canvas.SetLeft(rec, zonasEvento[c].leffPositionZona);
                Canvas.SetTop(rec, zonasEvento[c].topPositionZona);

                Canvas.SetLeft(textBlockNombreZona, zonasEvento[c].leffPositionZona + 5);
                Canvas.SetTop(textBlockNombreZona, zonasEvento[c].topPositionZona + 5);
                textBlockNombreZona.FontSize = 10;

                textBlockNombreZona.Text = zonasEvento[c].nombreZona;
                //string nombreZona, double metrosCuadradosZona, int idZona, int aforoZona, int idEventoZona, double topPositionZona, double leffPositionZona, double heightZona, double widthZona, bool zonaActiva, string zonaDibujada
                textMetrosCuadradosZona = new TextBlock();
                Canvas.SetLeft(textMetrosCuadradosZona, zonasEvento[c].leffPositionZona + 5);
                Canvas.SetTop(textMetrosCuadradosZona, zonasEvento[c].topPositionZona + 20);
                textMetrosCuadradosZona.FontSize = 10;
                textMetrosCuadradosZona.Foreground = new SolidColorBrush(Colors.White);
                textMetrosCuadradosZona.Text = zonasEvento[c].metrosCuadradosZona.ToString() + " metros2";
                textAforoZona = new TextBlock();
                Canvas.SetLeft(textAforoZona, zonasEvento[c].leffPositionZona + 5);
                Canvas.SetTop(textAforoZona, zonasEvento[c].topPositionZona + 35);
                textAforoZona.FontSize = 10;
                textAforoZona.Foreground = new SolidColorBrush(Colors.White);
                textAforoZona.Text = zonasEvento[c].aforoZona + pers;
                LienzoClave.Children.Add(rec);
                LienzoClave.Children.Add(textBlockNombreZona);
                LienzoClave.Children.Add(textMetrosCuadradosZona);
                LienzoClave.Children.Add(textAforoZona);
                aforoEvento = aforoEvento + zonasEvento[c].aforoZona;
                metrosEvento = metrosEvento + zonasEvento[c].metrosCuadradosZona;
            }
            counterZona = zonasEvento.Count;
            txtMts2.Text = metrosEvento.ToString();
            txtNumeroZonas.Text = (counterZona).ToString();
            txtAforo.Text = aforoEvento.ToString();
            DataZonas.ItemsSource = zonasEvento;
        }

        private async void UploadImg(string imagen2, string name2)
        {
            var result = "";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(" http://localhost:4000/Image/");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "PUT";
            string json = "";
            var data = new
            {
                imagen = imagen2,
                name = name2
            };
            json = JsonConvert.SerializeObject(data);
            using (var streamWriter = new StreamWriter(await httpWebRequest.GetRequestStreamAsync()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
                result = JsonConvert.SerializeObject(streamWriter);
            }
            var httpResponse = (HttpWebResponse)await httpWebRequest.GetResponseAsync();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
                streamReader.Close();
            }
            if (result == "")
            {
                MessageBox.Show("mal vamos");
            }
            else
            {

                imageToPost = result;
            }
        }

        private void uploadEvento(int count, string nombreEvento1, string direccionEvento1, string activoEvento1, int idEvento1,
            string fechaEvento1, string horaInicioEvento1, string horaFinEvento1, int zonasEvento1,
            int aforoEvento1, string fotoEvento1, string fechaEventoFin1)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(" http://localhost:4000/Eventos/" + count);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            string json = "";
            var data = new
            {
                nombreEvento = nombreEvento1,
                direccionEvento = direccionEvento1,
                activoEvento = activoEvento1,
                idEvento = idEvento1,
                fechaEvento = fechaEvento1,
                horaInicioEvento = horaInicioEvento1,
                horaFinEvento = horaFinEvento1,
                zonasEvento = zonasEvento1,
                aforoEvento = aforoEvento1,
                fotoEvento = fotoEvento1,
                fechaEventoFin = fechaEventoFin1
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
        private void AltaEvento(int count, string activoEvento1)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(" http://localhost:4000/Eventos/" + count);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "PUT";
            string json = "";
            var data = new
            {
                activoEvento = activoEvento1
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
        private void ActivaZona(int count, string zonaActiva1)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(" http://localhost:4000/Zonas/" + count);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "PUT";
            string json = "";
            var data = new
            {
                zonaActiva = zonaActiva1
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

        private void UpdateZonas(int count, string nombreZona1, double metrosCuadradosZona1,
            int idZona1, int aforoZona1, int idEventoZona1, double topPositionZona1,
            double leffPositionZona1, double heightZona1, double widthZona1, string zonaActiva1,
            string zonaDibujada1, string situacionZona1, string zonaBloqueada2,string inicioZona1, string finZona1)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(" http://localhost:4000/Zonas/" + count);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            string json = "";
            var data = new
            {
                nombreZona = nombreZona1,
                metrosCuadradosZona = metrosCuadradosZona1,
                idZona = idZona1,
                aforoZona = aforoZona1,
                idEventoZona = idEventoZona1,
                topPositionZona = topPositionZona1,
                leffPositionZona = leffPositionZona1,
                heightZona = heightZona1,
                widthZona = widthZona1,
                zonaActiva = zonaActiva1,
                zonaDibujada = zonaDibujada1,
                situacionZona = situacionZona1,
                zonaBloqueada = zonaBloqueada2,
                inicioZona = inicioZona1,
                finZona = finZona1
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

        private void btnEventoAmp_Click(object sender, RoutedEventArgs e)
        {
            if (txtNombreEvento.Text == "" || txtDireccionEvento.Text == "" || comboHoraInicio.Text == "Hora" || comboMinutosInicio.Text == "Minutos" ||
                comboHoraFin.Text == "Hora" || comboMinutosFin.Text == "Minutos")
            {
                MessageBox.Show("DEBES RELLENAR LOS CAMPOS OBLIGATORIOS=> NOMBRE EVENTO, DIRECCIÓN EVENTO Y FECHA Y HORAS DE EVENTO PARA MOSTRARTE LA INFORMACIÓN AMPLIADA.", "ATENCIÓN", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                InfoEvento infoEvento = new InfoEvento();
                infoEvento.tblockNombreEvento.Text = txtNombreEvento.Text;
                infoEvento.tblockdirec.Text = txtDireccionEvento.Text;
                string dteinit = datePickerEvento.SelectedDate.ToString();
                string[] dteinitSpt = dteinit.Split(' ');
                infoEvento.tblockiniEvento.Text = dteinitSpt[0] + "  " + comboHoraInicio.Text + ":" + comboMinutosInicio.Text + ":00";
                string dtefin = datePickerEventoFin.SelectedDate.ToString();
                string[] dtefintSpt = dtefin.Split(' ');
                infoEvento.tblockfinEvet.Text = dtefintSpt[0] + "  " + comboHoraFin.Text + ":" + comboMinutosFin.Text + ":00";
                infoEvento.tblockActivo.Text = comboEventoActivo.Text;
                infoEvento.tblockMetros.Text = txtMts2.Text;
                infoEvento.tblockAforo.Text = txtAforo.Text;
                infoEvento.tblockZonas.Text = txtNumeroZonas.Text;
                infoEvento.canvasito.Background = LienzoClave.Background;
                DateTime fecharegistro = DateTime.Parse(dteinitSpt[0] + "  " + comboHoraInicio.Text.ToString() + ":" + comboMinutosInicio.Text.ToString() + ":00"); //obtenemos este valor de una bbdd        
                DateTime ahoritaMismo = DateTime.Now;
                DateTime fechaFin = DateTime.Parse(dtefintSpt[0] + "  " + comboHoraFin.Text.ToString() + ":" + comboMinutosFin.Text.ToString() + ":00");
                string horas = "";
                int compareDates = DateTime.Compare(fecharegistro, ahoritaMismo);
                int comparesFin = DateTime.Compare(fechaFin, ahoritaMismo);
                if (comparesFin < 0)
                {
                    horas = "Evento Pasado";
                    infoEvento.txtCuentaAtras.Text = "";
                }
                else if (compareDates < 0)
                {
                    horas = (DateTime.Now - fecharegistro).ToString(@"dd\d\ h\hmm\m\ ");
                    infoEvento.txtCuentaAtras.Text = "Tiempo desde inicio:";
                }
                else
                {
                    horas = (DateTime.Now - fecharegistro).ToString(@"dd\d\ h\hmm\m\ ");
                    infoEvento.txtCuentaAtras.Text = "Tiempo hasta inicio";
                }


                infoEvento.tbkclock.Text = horas;
                infoEvento.Show();
            }
        }
      
        private void btnCreacionEventos_Click(object sender, RoutedEventArgs e)
        {
            Plano plano = new Plano();
            plano.Show();
            this.Close();
        }

        private void btnInsertPlano_Click(object sender, RoutedEventArgs e)
        {
            
            txtMtsZona.Focus();
            img1 = "";
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
                try
                {
                    img1 = openFileDialog.FileName;

                    ImageBrush imageBrush = new ImageBrush();
                    imageBrush.ImageSource = new BitmapImage(new Uri(img1, UriKind.Absolute));
                    LienzoClave.Background = imageBrush;
                    LienzoClave.Opacity = 1;
                    positionEnabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Debes utiliza imágenes PNG O JPG", "ATENCIÓN", MessageBoxButton.OK, MessageBoxImage.Information);
                    Console.WriteLine(ex);
                }
        }

        private void LienzoClave_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (nuevaZona)
            {
                
                counterName1 = 0;
                if (zonaGuardada)
                {
                    if (positionEnabled)
                    {
                        if (txtMtsZona.Text != "" && txtAforoIncluidoZona.Text != "")
                        {
                            try
                            {
                                mtsZonaDouble = Convert.ToDouble(txtMtsZona.Text);
                                aforoZonaInteger = Convert.ToInt32(txtAforoIncluidoZona.Text);
                                point = e.GetPosition(LienzoClave);
                                topPosition = point.Y;
                                leftPosition = point.X;
                                Move = true;
                                zone = zone + counterZona;
                                counterZona++;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Los campos de metros cuadrados o/y aforo no son correctos.", "ATENCIÓN", MessageBoxButton.OK, MessageBoxImage.Error);
                                Console.WriteLine(ex);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Los campos de metros cuadrados y aforo de zona son obligatorios.", "ATENCIÓN", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Si deseas dibujar zonas tienes que importar primero una imagen como plano.", "ATENCIÓN", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Debes guardar la zona creada antes de crear la siguiente. Si no es de tu agrado la zona que creaste puedes eliminarla con el botoón Eliminar Última Zona ", "ATENCIÓN", MessageBoxButton.OK, MessageBoxImage.Information);
                    Move = false;
                }
            }
        }

        private void btnModificarZona_Click(object sender, RoutedEventArgs e)
        {
            
            
            try
            {
                string[] fechainiZona = datePickerZona.SelectedDate.ToString().Split(' ');
                string[] fechaZonaIni = fechainiZona[0].Split('/');
                string[] fechafinZona = datePickerZonaFin.SelectedDate.ToString().Split(' ');
                string[] fechaZonaFin = fechafinZona[0].Split('/');

                string[] eventoIni = datePickerEvento.SelectedDate.ToString().Split(' ');
                string[] fechaEventoIni = eventoIni[0].Split('/');
                string[] eventoFin = datePickerEventoFin.SelectedDate.ToString().Split(' ');
                string[] fechaEventoFin = eventoFin[0].Split('/');

                DateTime initZona = new DateTime(Convert.ToInt32(fechaZonaIni[2]), Convert.ToInt32(fechaZonaIni[1]), Convert.ToInt32(fechaZonaIni[0]),
                    Convert.ToInt32(comboHoraZonaInicio.Text), Convert.ToInt32(comboMinutosZonaInicio.Text), 0);
                DateTime finiZona = new DateTime(Convert.ToInt32(fechaZonaFin[2]), Convert.ToInt32(fechaZonaFin[1]), Convert.ToInt32(fechaZonaFin[0]),
                Convert.ToInt32(comboHoraZonaFin.Text), Convert.ToInt32(comboMinutosZonaFin.Text), 0);
                DateTime ahoritaMismo = DateTime.Now;
                DateTime initEvento = new DateTime(Convert.ToInt32(fechaEventoIni[2]), Convert.ToInt32(fechaEventoIni[1]), Convert.ToInt32(fechaEventoIni[0]),
                    Convert.ToInt32(comboHoraInicio.Text), Convert.ToInt32(comboMinutosInicio.Text), 0);
                DateTime finEvento = new DateTime(Convert.ToInt32(fechaEventoFin[2]), Convert.ToInt32(fechaEventoFin[1]), Convert.ToInt32(fechaEventoFin[0]),
                   Convert.ToInt32(comboHoraFin.Text), Convert.ToInt32(comboMinutosFin.Text), 0);
                int zonaIniAntesAhora = DateTime.Compare(initZona, ahoritaMismo);
                int zonaFinAntesAhora = DateTime.Compare(finiZona, ahoritaMismo);
                int zonaIniDespZonaFin = DateTime.Compare(finiZona, initZona);
                int zonaIniAntesEventoIni = DateTime.Compare(initZona, initEvento);
                int zonaFinDespEventoFin = DateTime.Compare(finEvento, finiZona);
                double mtsCuad = Convert.ToDouble(txtMtsZona.Text);
                int aforo = Convert.ToInt32(txtAforoIncluidoZona.Text);

                if (aforo < 0 || mtsCuad < 0)
                {
                    MessageBox.Show("Comprueba los datos introducidos como Aforo y Metros cuadrados.\nHay alguno de ellos incorrecto. ", "Atención", MessageBoxButton.OK, MessageBoxImage.Error);
                    
                }
                else
                {
                    if(datePickerZona.IsEnabled == false && datePickerZonaFin.IsEnabled == false)
                    {
                        MessageBox.Show("NO SE PUEDEN MODIFICAR ZONAS VENCIDAS ", "Atención", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if (Convert.ToInt32(txtOcupacionPersonasZona.Text) > 0 && datePickerZona.IsEnabled == true && datePickerZonaFin.IsEnabled == true)
                    {
                        if (zonaIniAntesAhora < 0 || zonaFinAntesAhora < 0)
                        {
                            MessageBox.Show("Tanto la fecha inicio como la fecha fin de la Zona deben ser posteriores al momento actual.\nPor favor " +
                                "corrija las fechas.", "Atención", MessageBoxButton.OK, MessageBoxImage.Error);
                        }else if (zonaIniAntesEventoIni<0)
                        {
                            MessageBox.Show("La fecha inicio de la Zona no puede ser anterior al inicio del Evento.\nPor favor " +
                                "corrija la fecha incorrecta.", "Atención", MessageBoxButton.OK, MessageBoxImage.Error);
                        }else if (zonaFinDespEventoFin<0)
                        {
                            MessageBox.Show("La fecha fin de la Zona no puede ser posterior al fin del Evento.\nPor favor " +
                               "corrija la fecha incorrecta.", "Atención", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else if (zonaIniDespZonaFin < 0)
                        {
                            MessageBox.Show("La fecha inicio de la Zona no puede ser posterior a la fecha fin de la Zona.\nPor favor " +
                            "corrija las fechas.", "Atención", MessageBoxButton.OK, MessageBoxImage.Error);
                        }else if(Convert.ToInt32(txtAforoIncluidoZona.Text)<Convert.ToInt32(txtOcupacionPersonasZona.Text)){
                            MessageBox.Show("El Aforo decidido no puede ser menor a la cifra de ocupación actual.\nPor favor " +
                            "corrija el dato de Aforo.", "Atención", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else if (txtNambreZona.Text == "" || txtNambreZona.Text == "Opcional")
                        {
                            
                            modificarZonaUpd("Zona " + (zonasEvento.Count + 1));
                            resetItems();
                        }
                        else
                        {
                            
                            modificarZonaUpd(txtNambreZona.Text);
                            resetItems();
                        }
                    //si visitas =0  y datapiker inicio y final enable=> fecha de antes y fecha despues de ahora aforo y metros positivos
                    }
                    else if(Convert.ToInt32(txtOcupacionPersonasZona.Text) == 0 && datePickerZona.IsEnabled == true && datePickerZonaFin.IsEnabled == true)
                    {
                        if (zonaIniAntesAhora < 0 || zonaFinAntesAhora < 0)
                        {
                            MessageBox.Show("Tanto la fecha inicio como la fecha fin de la Zona deben ser posteriores al momneto actual.\nPor favor " +
                                "corrija las fechas.", "Atención", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else if (zonaIniDespZonaFin < 0)
                        {
                            MessageBox.Show("La fecha inicio de la Zona no puede ser posterior a la fecha fin de la Zona.\nPor favor " +
                            "corrija las fechas.", "Atención", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else if (txtNambreZona.Text == "" || txtNambreZona.Text == "Opcional")
                        {
                            modificarZonaUpd("Zona " + (zonasEvento.Count + 1));
                            resetItems();
                        }
                        else
                        {
                            
                            modificarZonaUpd(txtNambreZona.Text);
                            resetItems();
                        }
                    }else if (Convert.ToInt32(txtOcupacionPersonasZona.Text) > 0 && datePickerZona.IsEnabled == false && datePickerZonaFin.IsEnabled == true)
                    {
                        if (zonaFinAntesAhora < 0)
                        {
                            MessageBox.Show("La fecha fin de la Zona debe ser posteriores al momneto actual.\nPor favor " +
                                "corrija la fecha.", "Atención", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else if (zonaIniDespZonaFin < 0)
                        {
                            MessageBox.Show("La fecha inicio de la Zona no puede ser posterior a la fecha fin de la Zona.\nPor favor " +
                            "corrija las fechas.", "Atención", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else if (Convert.ToInt32(txtAforoIncluidoZona.Text) < Convert.ToInt32(txtOcupacionPersonasZona.Text))
                        {
                            MessageBox.Show("El Aforo decidido no puede ser menor a la cifra de ocupación actual.\nPor favor " +
                            "corrija el dato de Aforo.", "Atención", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else if (txtNambreZona.Text == "" || txtNambreZona.Text == "Opcional")
                        {
                           
                            modificarZonaUpd("Zona " + (zonasEvento.Count + 1));
                            resetItems();
                        }
                        else
                        {
                            
                            modificarZonaUpd(txtNambreZona.Text);
                            resetItems();
                        }
                        //si visitas =0 y datapker final  enable=> fecha fin despues ahora, metros y aforo positivo
                    }
                    else if (Convert.ToInt32(txtOcupacionPersonasZona.Text) == 0 && datePickerZona.IsEnabled == false && datePickerZonaFin.IsEnabled == true)
                    {
                        if (zonaFinAntesAhora < 0)
                        {
                            MessageBox.Show("La fecha fin de la Zona debe ser posteriores al momneto actual.\nPor favor " +
                                "corrija la fecha.", "Atención", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else if (zonaIniDespZonaFin < 0)
                        {
                            MessageBox.Show("La fecha inicio de la Zona no puede ser posterior a la fecha fin de la Zona.\nPor favor " +
                            "corrija las fechas.", "Atención", MessageBoxButton.OK, MessageBoxImage.Error);
                        }                      
                        else if (txtNambreZona.Text == "" || txtNambreZona.Text == "Opcional")
                        {
                           
                            modificarZonaUpd("Zona " + (zonasEvento.Count + 1));
                            resetItems();
                        }
                        else
                        {
                            
                            modificarZonaUpd(txtNambreZona.Text);
                            resetItems();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Comprueba los datos introducidos como Aforo, Metros Cuadrados, Fechas y horas de inicio y fin.\nTODOS SON OBLIGATORIOS Y DEBEN TENER EL FORMATO CORRECTO ", "Atención", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine(ex);
            }


           
            
            //si visitas =0 y datapker final  enable=> fecha fin despues ahora, metros y aforo positivo
        }

        private void modificarZonaUpd(string v)
        {
            int posZona = Convert.ToInt32(txtIdzona.Text) - 1000;
            string[] zonaIni = datePickerZona.SelectedDate.ToString().Split(' ');
            string[] fechaIni = zonaIni[0].Split('/');
            string fechIni = fechaIni[2] + "/"+fechaIni[1]+"/"+fechaIni[0] + " " + comboHoraZonaInicio.Text + ":" + comboMinutosZonaInicio.Text + ":00";
            string[] zonaFin = datePickerZonaFin.SelectedDate.ToString().Split(' ');
            string[] fechaFin =zonaFin[0].Split('/');
            string fechFin = fechaFin[2] + "/" + fechaFin[1] + "/" + fechaFin[0]+" "+comboHoraZonaFin.Text+":"+comboMinutosZonaFin.Text+":00";
           
            UpdateZonas(posZona, v, Convert.ToDouble(txtMtsZona.Text), Convert.ToInt32(txtIdzona.Text),
                                Convert.ToInt32(txtAforoIncluidoZona.Text), zonasEvento[positionZones].idEventoZona, zonasEvento[positionZones].topPositionZona,
                                zonasEvento[positionZones].leffPositionZona, zonasEvento[positionZones].heightZona, zonasEvento[positionZones].widthZona,
                                txtZonaActiva.Text, zonasEvento[positionZones].zonaDibujada, comboSituacionZona.Text, zonasEvento[positionZones].zonaBloqueada,fechIni,fechFin);
             
             MessageBox.Show("La zona ha sido correctamente modificada. Seleccione de nuevo el evento para observar los cambios", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnCrearNuevaZona_Cilck(object sender, RoutedEventArgs e)
        {
            
            zonaGuardada = true;
            txtNambreZona.IsEnabled = true;
            comboSituacionZona.IsEnabled = true;
            txtMtsZona.IsEnabled = true;
            comboAforoPorcentaje.IsEnabled = true;
            btnEstimarAforoZona.IsEnabled = true;
            txtAforoIncluidoZona.IsEnabled = true;
            txtZonaActiva.IsEnabled = true;
            txtVisitasZona.IsEnabled = true;
            datePickerZona.IsEnabled = true;
            
            comboHoraZonaInicio.IsEnabled = true;
            comboMinutosZonaInicio.IsEnabled = true;
            txtOcupacionPorcentajeZona.IsEnabled = true;
            datePickerZonaFin.IsEnabled = true;
            
            comboHoraZonaFin.IsEnabled = true;
            comboMinutosZonaFin.IsEnabled = true;
            txtOcupacionPersonasZona.IsEnabled = true;
            btnModificarZona.IsEnabled = false;
            btnActivarZona.IsEnabled = false;
            btnGuardarZonaCreada.IsEnabled = true;
            txtNambreZona.IsEnabled = true;
            txtNambreZona.IsEnabled = true;
            topPosition = 0;
            leftPosition=0;
            WidthDesing=0;
            HeightDesing=0;


            try
            {
                string[] fechainiZona = datePickerZona.SelectedDate.ToString().Split(' ');
                string[] fechaZonaIni = fechainiZona[0].Split('/');
                string[] fechafinZona = datePickerZonaFin.SelectedDate.ToString().Split(' ');
                string[] fechaZonaFin = fechafinZona[0].Split('/');

                string[] eventoIni = datePickerEvento.SelectedDate.ToString().Split(' ');
                string[] fechaEventoIni = eventoIni[0].Split('/');
                string[] eventoFin = datePickerEventoFin.SelectedDate.ToString().Split(' ');
                string[] fechaEventoFin = eventoFin[0].Split('/');

                DateTime initZona = new DateTime(Convert.ToInt32(fechaZonaIni[2]), Convert.ToInt32(fechaZonaIni[1]), Convert.ToInt32(fechaZonaIni[0]),
                    Convert.ToInt32(comboHoraZonaInicio.Text), Convert.ToInt32(comboMinutosZonaInicio.Text), 0);
                
                DateTime finiZona = new DateTime(Convert.ToInt32(fechaZonaFin[2]), Convert.ToInt32(fechaZonaFin[1]), Convert.ToInt32(fechaZonaFin[0]),
                Convert.ToInt32(comboHoraZonaFin.Text), Convert.ToInt32(comboMinutosZonaFin.Text), 0);
               
                DateTime ahoritaMismo = DateTime.Now;
                DateTime initEvento = new DateTime(Convert.ToInt32(fechaEventoIni[2]), Convert.ToInt32(fechaEventoIni[1]), Convert.ToInt32(fechaEventoIni[0]),
                    Convert.ToInt32(comboHoraInicio.Text), Convert.ToInt32(comboMinutosInicio.Text), 0);
                
                DateTime finEvento = new DateTime(Convert.ToInt32(fechaEventoFin[2]), Convert.ToInt32(fechaEventoFin[1]), Convert.ToInt32(fechaEventoFin[0]),
                   Convert.ToInt32(comboHoraFin.Text), Convert.ToInt32(comboMinutosFin.Text), 0);
               
                int zonaIniAntesAhora = DateTime.Compare(initZona, ahoritaMismo);
                int zonaFinAntesAhora = DateTime.Compare(finiZona, ahoritaMismo);
                int zonaIniDespZonaFin = DateTime.Compare(finiZona, initZona);
                int zonaIniAntesEventoIni = DateTime.Compare(initZona, initEvento);
                int zonaFinDespEventoFin = DateTime.Compare(finEvento, finiZona);

                double mtsCuad = Convert.ToDouble(txtMtsZona.Text);
                int aforo = Convert.ToInt32(txtAforoIncluidoZona.Text);

                if (aforo < 0 || mtsCuad < 0)
                {
                    MessageBox.Show("Comprueba los datos introducidos como Aforo y Metros cuadrados.\nHay alguno de ellos incorrecto. ", "Atención", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                        if (zonaIniAntesAhora < 0 || zonaFinAntesAhora < 0)
                        {
                            MessageBox.Show("Tanto la fecha inicio como la fecha fin de la Zona deben ser posteriores al momento actual.\nPor favor " +
                                "corrija las fechas.", "Atención", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else if (zonaIniAntesEventoIni < 0)
                        {
                            MessageBox.Show("La fecha inicio de la Zona no puede ser anterior al inicio del Evento.\nPor favor " +
                                "corrija la fecha incorrecta.", "Atención", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else if (zonaFinDespEventoFin < 0)
                        {
                            MessageBox.Show("La fecha fin de la Zona no puede ser posterior al fin del Evento.\nPor favor " +
                               "corrija la fecha incorrecta.", "Atención", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else if (zonaIniDespZonaFin < 0)
                        {
                            MessageBox.Show("La fecha inicio de la Zona no puede ser posterior a la fecha fin de la Zona.\nPor favor " +
                            "corrija las fechas.", "Atención", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                        else if (txtNambreZona.Text == "" || txtNambreZona.Text == "Opcional")
                        {
                            MessageBox.Show("Puedes crear la nueva zona. Hazlo como anteiormente, o bien dibujándola o bien sin dibujo.\nRecurda GUARDAR LA ZONA para no perder el trabajo.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                            counter++;
                            nuevaZona = true;
                        }
                        else
                        {
                            MessageBox.Show("Puedes crear la nueva zona. Hazlo como anteiormente, o bien dibujándola o bien sin dibujo.\nRecurda GUARDAR LA ZONA para no perder el trabajo.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                            counter++;
                            nuevaZona = true;
                        }                  
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("Comprueba los datos introducidos como Aforo, Metros Cuadrados, Fechas y horas de inicio y fin.\nTODOS SON OBLIGATORIOS Y DEBEN TENER EL FORMATO CORRECTO.\n\nCUANDO LOS TENGAS CORRECTAMENTE VUELVE A PULSAR EL BOTON 'CREAR NUEVA ZONA' ", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                Console.WriteLine(ex);
            }        
        }

        private void btnGuardarZonaCreada_Click(object sender, RoutedEventArgs e)
        {
            
            if (counter >0)
            {
                nuevaZona = false;
                string nameZona = "";
                counter = 0;
                string dibujada = "";
                string[] fechainiZona = datePickerZona.SelectedDate.ToString().Split(' ');
                string[] fechaZonaIni = fechainiZona[0].Split('/');
                string[] fechafinZona = datePickerZonaFin.SelectedDate.ToString().Split(' ');
                string[] fechaZonaFin = fechafinZona[0].Split('/');
                
                string dateZoneInit = (fechaZonaIni[2]) + "/" + (fechaZonaIni[1]) + "/" + (fechaZonaIni[0]) + " " +
                   (comboHoraZonaInicio.Text) + ":" + (comboMinutosZonaInicio.Text) + ":00";
                string dateZoneFin = (fechaZonaFin[2]) + "/" + (fechaZonaFin[1]) + "/" + (fechaZonaFin[0]) + " " +
                   (comboHoraZonaFin.Text) + ":" + (comboMinutosZonaFin.Text) + ":00";

                if (txtNambreZona.Text == "" || txtNambreZona.Text == "Opcional")
                {
                    nameZona ="Zona "+ (zonasEvento.Count+1);
                }
                else
                {
                    nameZona = txtNambreZona.Text;
                }if (WidthDesing > 0 && HeightDesing > 0)
                {
                    dibujada = "Si";
                }
                else
                {
                    dibujada = "No";
                }
                DataZonas.ItemsSource = null;
                idZonacreada = Convert.ToInt32(todasLasZonas.Count + 1000);
                UpdateZonas(todasLasZonas.Count,nameZona, Convert.ToDouble(txtMtsZona.Text), Convert.ToInt32(todasLasZonas.Count+1000),
                                   Convert.ToInt32(txtAforoIncluidoZona.Text),Convert.ToInt32(txtIdEvento.Text), topPosition,leftPosition, HeightDesing, WidthDesing,
                                   "No", dibujada, comboSituacionZona.Text, "No", dateZoneInit, dateZoneFin);
                
                MessageBox.Show("La zona ha sido correctamente creada. Seleccione de nuevo el evento para observar los cambios", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                counter = 0;
                counterZonasNuevas++;
                resetItems();
            }
            
        }

        private void btnEliminarZonaCreada_Click(object sender, RoutedEventArgs e)
        {
            if (counterZonasNuevas > 0&&idZonacreada>999)
            {
                if (zonasEvento[zonasEvento.Count - 1].visitasZona > 0|| zonasEvento[zonasEvento.Count - 1].zonaActiva=="Si")
                {
                    MessageBox.Show("No se puede eliminar la zona por estar activa o por tener visitas", "Atención", MessageBoxButton.OK, MessageBoxImage.Error);
                    counterZonasNuevas++;
                    btnCrearNuevaZona.IsEnabled = true;
                    btnGuardarZonaCreada.IsEnabled = false;
                }
                else if(idZonacreada!= zonasEvento[zonasEvento.Count - 1].idZona)
                {
                    MessageBox.Show("La zona a eliminar (última zona creada) no corresponde a este evento. Por favor cambie de evento");
                }
                else
                {
                   deleteZona(idZonacreada - 1000);

                    MessageBox.Show("La zona se ha eliminado correctamente. Recarga el evento para observar los cambios","Información",MessageBoxButton.OK,MessageBoxImage.Information);
                    idZonacreada = 0;
                    resetItems();
                }
            }
            else
            {
                MessageBox.Show("Solo se pueden eliminar zonas creadas en este entorno", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void deleteZona(int v)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(" http://localhost:4000/delete-zonas/" + v);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            string json = "";
            var data = new
            {
                
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

        private void btnModificarEvento_Click(object sender, RoutedEventArgs e)
        {
            if (DataEventos.SelectedIndex > -1)
            {
                try
                {
                    string fotoEvento = "";
                    string[] eventoIni = datePickerEvento.SelectedDate.ToString().Split(' ');
                    string[] fechaEventoIni = eventoIni[0].Split('/');
                    string[] eventoFin = datePickerEventoFin.SelectedDate.ToString().Split(' ');
                    string[] fechaEventoFin = eventoFin[0].Split('/');
                    string fechaInsertini = fechaEventoIni[0] + "/" + fechaEventoIni[1] + "/" + fechaEventoIni[2];
                    string fechaInsertfin = fechaEventoFin[0] + "/" + fechaEventoFin[1] + "/" + fechaEventoFin[2];
                    DateTime ahoritaMismo = DateTime.Now;
                    DateTime initEvento = new DateTime(Convert.ToInt32(fechaEventoIni[2]), Convert.ToInt32(fechaEventoIni[1]), Convert.ToInt32(fechaEventoIni[0]),
                        Convert.ToInt32(comboHoraInicio.Text), Convert.ToInt32(comboMinutosInicio.Text), 0);

                    DateTime finEvento = new DateTime(Convert.ToInt32(fechaEventoFin[2]), Convert.ToInt32(fechaEventoFin[1]), Convert.ToInt32(fechaEventoFin[0]),
                       Convert.ToInt32(comboHoraFin.Text), Convert.ToInt32(comboMinutosFin.Text), 0);
                    int finEventoAntesInicio = DateTime.Compare(finEvento, initEvento);
                    int inicioEventoAntesAhora = DateTime.Compare(initEvento, ahoritaMismo);
                    int finEventoAntesAhora = DateTime.Compare(finEvento, ahoritaMismo);
                    //Si la fecha desde y hasta están libres. No permitido conflicto fechas
                    if (datePickerEvento.IsEnabled && datePickerEventoFin.IsEnabled)
                    {
                        if (finEventoAntesInicio < 0)
                        {
                            MessageBox.Show("La fecha de fin del evento no puede ser anterior a la de inicio", "Atención", MessageBoxButton.OK, MessageBoxImage.Error);
                        }else if (inicioEventoAntesAhora < 0)
                        {
                            MessageBox.Show("La fecha de inicio del evento no puede ser anterior a la actual", "Atención", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            if (img1 == "")
                            {
                                for (int y=0; y < eventosBBDD.Count; y++)
                                {
                                    if (Convert.ToInt32(txtIdEvento.Text) == eventosBBDD[y].idEvento)
                                    {
                                        fotoEvento = eventosBBDD[y].fotoEvento;
                                    }
                                }
                                MessageBox.Show("El Evento se ha modificado satisfactoriamente. Seleccione de nuevo el evento para observar los cambios", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                                uploadEvento(Convert.ToInt32(txtIdEvento.Text), txtNombreEvento.Text, txtDireccionEvento.Text, "Si", Convert.ToInt32(txtIdEvento.Text),
                                fechaInsertini, comboHoraInicio.Text+":"+comboMinutosInicio.Text+":00", comboHoraFin.Text + ":" + comboMinutosFin.Text + ":00", Convert.ToInt32(txtNumeroZonas.Text),
                                Convert.ToInt32(txtAforo.Text), fotoEvento, fechaInsertfin);
                                img1 = "";
                                resetItems();

                            }
                            else
                            {
                                UploadImg(img1, img1);
                                MessageBox.Show("El Evento se ha modificado satisfactoriamente. Seleccione de nuevo el evento para observar los cambios", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                                uploadEvento(Convert.ToInt32(txtIdEvento.Text), txtNombreEvento.Text, txtDireccionEvento.Text, "Si", Convert.ToInt32(txtIdEvento.Text),
                                fechaInsertini, comboHoraInicio.Text + ":" + comboMinutosInicio.Text + ":00", comboHoraFin.Text + ":" + comboMinutosFin.Text + ":00", Convert.ToInt32(txtNumeroZonas.Text),
                                Convert.ToInt32(txtAforo.Text), imageToPost, fechaInsertfin);
                                img1 = "";
                                resetItems();
                                //upload imagen primero y luego evento
                            }
                        }

                    }
                    else if (datePickerEventoFin.IsEnabled)
                    {
                        if (finEventoAntesInicio < 0)
                        {
                            MessageBox.Show("La fecha de fin del evento no puede ser anterior a la de inicio", "Atención", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else if (finEventoAntesAhora < 0)
                        {
                            MessageBox.Show("La fecha de fin del evento no puede ser anterior a la actual", "Atención", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            if (img1 == "")
                            {
                                for (int y = 0; y < eventosBBDD.Count; y++)
                                {
                                    if (Convert.ToInt32(txtIdEvento.Text) == eventosBBDD[y].idEvento)
                                    {
                                        fotoEvento = eventosBBDD[y].fotoEvento;
                                    }
                                }
                                MessageBox.Show("El Evento se ha modificado satisfactoriamente. Seleccione de nuevo el evento para observar los cambios", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                                uploadEvento(Convert.ToInt32(txtIdEvento.Text), txtNombreEvento.Text, txtDireccionEvento.Text, "Si", Convert.ToInt32(txtIdEvento.Text),
                                fechaInsertini, comboHoraInicio.Text + ":" + comboMinutosInicio.Text + ":00", comboHoraFin.Text + ":" + comboMinutosFin.Text + ":00", Convert.ToInt32(txtNumeroZonas.Text),
                                Convert.ToInt32(txtAforo.Text), fotoEvento, fechaInsertfin);
                                img1 = "";
                                resetItems();
                            }
                            else
                            {
                                UploadImg(img1, img1);
                                MessageBox.Show("El Evento se ha modificado satisfactoriamente. Seleccione de nuevo el evento para observar los cambios", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                                uploadEvento(Convert.ToInt32(txtIdEvento.Text), txtNombreEvento.Text, txtDireccionEvento.Text, "Si", Convert.ToInt32(txtIdEvento.Text),
                                fechaInsertini, comboHoraInicio.Text + ":" + comboMinutosInicio.Text + ":00", comboHoraFin.Text + ":" + comboMinutosFin.Text + ":00", Convert.ToInt32(txtNumeroZonas.Text),
                                Convert.ToInt32(txtAforo.Text), imageToPost, fechaInsertfin);
                                img1 = "";
                                resetItems();
                                //upload imagen primero y luego evento
                            }
                        }
                    }
                    
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Los datos fecha y hora son obligatorios", "Atención", MessageBoxButton.OK, MessageBoxImage.Error);
                    Console.WriteLine(ex);
                }
                
            }
            else
            {
                MessageBox.Show("Debes elegir un evento para modificarlo", "Información", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDesactivarEvento_Click(object sender, RoutedEventArgs e)
        {
            int idEvento = Convert.ToInt32(txtIdEvento.Text);
            var url3 = "http://localhost:4000/registrados";
            WebClient wc3 = new WebClient();
            var datos3 = wc3.DownloadString(url3);
            var values = JsonConvert.DeserializeObject<Dictionary<string, Registrado>>(datos3);
            Registrado reg;
            DateTime dateTimeNow = DateTime.Now;
            for (int x=0; x<zonasEvento.Count; x++)
            {
                ActivaZona(zonasEvento[x].idZona - 1000, "No");
               
                foreach (KeyValuePair<string, Registrado> entry in values)
                {
                    if (zonasEvento[x].idZona == entry.Value.IdZona && entry.Value.TimeOut == "")
                    {
                        
                        reg = new Registrado(entry.Key, entry.Value.Age, entry.Value.Firstname, entry.Value.IdZona, entry.Value.IdentityCard,
                            entry.Value.Lastname, entry.Value.TimeIn, entry.Value.TimeOut);
                        salidaForzadaPersonas.Add(reg);
                    }
                }
                for (int j = 0; j < salidaForzadaPersonas.Count; j++)
                {

                    string[] dateNow = DateTime.Now.ToString().Split(' ');
                    string[] dateNowSpl = dateNow[0].Split('/');
                    string fechSalida = dateNowSpl[2] + "/" + dateNowSpl[1] + "/" + dateNowSpl[0] + " " + dateNow[1];
                    SalidaForzadaPersona(salidaForzadaPersonas[j].idPersona, fechSalida);
                }
                salidaForzadaPersonas.Clear();
               
                
            }
            AltaEvento(idEvento, "No");
            resetItems();
            MessageBox.Show("El Evento se ha desactivado correctamente. Si deseas volverlo a activar ingresa en la pantallade Creación de " +
                "Eventos","Información",MessageBoxButton.OK, MessageBoxImage.Information );
        }

        private void btnMonitorEventos_Click(object sender, RoutedEventArgs e)
        {
            MonitorEventos monitorEventos = new MonitorEventos();
            monitorEventos.Show();
            this.Close();
        }
    }
}

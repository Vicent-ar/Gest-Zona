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
using proyecto_admin.modelos;
using System.Data;
using System.Net;
using Newtonsoft.Json;
using System.Windows.Threading;

namespace proyecto_admin.Vistas
{
    /// <summary>
    /// Lógica de interacción para Plano.xaml
    /// </summary>
    public partial class Plano : Window
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
        int counterPeople = 0;
        int positionZones = -1;
        double mtsZonaDouble = 0f;
        int aforoZonaInteger = 0;
        int aforoEvento = 0;
        int counter = 0;
        double metrosEvento = 0;
        string img1 = "";
        string imageToPost = "";
        int webcounter = 0;
        Point point;
        TextBlock textBlockNombreZona;
        TextBlock textMetrosCuadradosZona;
        TextBlock textAforoZona;
        Zona zona;
        Eventos eventos;
        int selectedZone = -1;
        List<Eventos> eventosBBDD = new List<Eventos>();
        List<Zona> zonasEvento = new List<Zona>();
        List<Zona> zonasBBDD = new List<Zona>();
        List<Zona> zonaModificada = new List<Zona>();
        System.Windows.Forms.Timer t1 = new System.Windows.Forms.Timer();
        public Plano()
        {
            InitializeComponent();
            zona = new Zona();
            DataZonas.DataContext = zona;
            eventos = new Eventos();
            DataEventos.DataContext = eventos;

            cargarArray();
            recarga();
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

            eventosBBDD.Clear();
            zonasBBDD.Clear();
            DataEventos.ItemsSource = null;
        }
        private void cargarArray()
        {
            counter = 0;
            webcounter = 0;
            limpiarDatos();
            var url = "http://localhost:4000/Eventos";
            WebClient wc = new WebClient();
            var datos = wc.DownloadString(url);

            var url2 = "http://localhost:4000/Zonas";
            WebClient wc2 = new WebClient();
            var datos2 = wc2.DownloadString(url2);
            

            var url3 = "http://localhost:4000/registrados";
            WebClient wc3 = new WebClient();
            var datos3 = wc3.DownloadString(url3);
            var values = JsonConvert.DeserializeObject<Dictionary<string, Registrado>>(datos3);
            if (datos == "null")
            {
                webcounter = 1;
            }
            if (webcounter == 0)
            {
                List<Eventos> eventosGet = JsonConvert.DeserializeObject<List<Eventos>>(datos);
                List<Zona> zonasGetOut = JsonConvert.DeserializeObject<List<Zona>>(datos2);
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
                            st = new Eventos(eventosGet[g].nombreEvento, eventosGet[g].direccionEvento, "No",
                            eventosGet[g].idEvento, eventosGet[g].fechaEvento, eventosGet[g].horaInicioEvento, eventosGet[g].horaFinEvento,
                            eventosGet[g].zonasEvento, eventosGet[g].aforoEvento, eventosGet[g].fotoEvento, eventosGet[g].fechaEventoFin);
                            eventosBBDD.Add(st);
                            for(int r=0;r<zonasGetOut.Count; r++)
                            {
                                if (eventosGet[g].idEvento == zonasGetOut[r].idEventoZona)
                                {
                                    foreach (KeyValuePair<string, Registrado> entry in values)
                                    {
                                        if (zonasGetOut[r].idZona == entry.Value.IdZona && entry.Value.TimeOut == "") 
                                        {
                                            SalidaForzadaPersona(entry.Key, zonasGetOut[r].finZona);
                                        }
                                    }
                                    ActivaZona(r, "No");
                                }
                            }
                            AltaEvento(g, "No");
                        }
                        else
                        {
                            st = new Eventos(eventosGet[g].nombreEvento, eventosGet[g].direccionEvento, eventosGet[g].activoEvento,
                            eventosGet[g].idEvento, eventosGet[g].fechaEvento, eventosGet[g].horaInicioEvento, eventosGet[g].horaFinEvento,
                            eventosGet[g].zonasEvento, eventosGet[g].aforoEvento, eventosGet[g].fotoEvento, eventosGet[g].fechaEventoFin);
                            eventosBBDD.Add(st);
                        }
                    }
                    else
                    {
                        //string nombreEvento, string direccionEvento, string activoEvento, 
                        //int idEvento, string fechaEvento, string horaInicioEvento, string horaFinEvento, 
                        //int zonasEvento,int aforoEvento, string fotoEvento
                        st = new Eventos(eventosGet[g].nombreEvento, eventosGet[g].direccionEvento, eventosGet[g].activoEvento,
                            eventosGet[g].idEvento, eventosGet[g].fechaEvento, eventosGet[g].horaInicioEvento, eventosGet[g].horaFinEvento,
                            eventosGet[g].zonasEvento, eventosGet[g].aforoEvento, eventosGet[g].fotoEvento, eventosGet[g].fechaEventoFin);
                        eventosBBDD.Add(st);
                    }
                }
                DataEventos.ItemsSource = eventosBBDD;
                var url4= "http://localhost:4000/Zonas";
                WebClient wc4 = new WebClient();
                var datos4 = wc4.DownloadString(url4);
                List<Zona> zonasGet = JsonConvert.DeserializeObject<List<Zona>>(datos4);

                Zona st2;
                for (int z = 0; z < zonasGet.Count; z++)
                {
                    //string nombreZona, double metrosCuadradosZona, int idZona, int aforoZona, int idEventoZona, 
                    // double topPositionZona, double leffPositionZona, double heightZona, double widthZona, string zonaActiva,
                    //string zonaDibujada, string situacionZona
                    st2 = new Zona(zonasGet[z].nombreZona, zonasGet[z].metrosCuadradosZona, zonasGet[z].idZona, zonasGet[z].aforoZona, zonasGet[z].idEventoZona,
                        zonasGet[z].topPositionZona, zonasGet[z].leffPositionZona, zonasGet[z].heightZona, zonasGet[z].widthZona, zonasGet[z].zonaActiva,
                        zonasGet[z].zonaDibujada, zonasGet[z].situacionZona, zonasGet[z].zonaBloqueada, zonasGet[z].inicioZona, zonasGet[z].finZona);

                    zonasBBDD.Add(st2);
                    
                        string fechaFin = zonasGet[z].finZona;
                    
                    string[] fechaFinSplit = fechaFin.Split(' ');
                    string[] mesFinSp = fechaFinSplit[0].Split('/');
                    string[] horaFinSplit = fechaFinSplit[1].Split(':');
                    int yearFin = Convert.ToInt32(mesFinSp[0]);
                    int monthFin = Convert.ToInt32(mesFinSp[1]);
                    int dayFin = Convert.ToInt32(mesFinSp[2]);
                    int hourFin = Convert.ToInt32(horaFinSplit[0]);
                    int minFin = Convert.ToInt32(horaFinSplit[1]);
                    DateTime date1 = new DateTime(yearFin, monthFin, dayFin, hourFin, minFin, 0);
                    DateTime date2 = DateTime.Now;
                    int result = DateTime.Compare(date1, date2);
                    if (result<0)
                    {
                        foreach (KeyValuePair<string, Registrado> entry in values)
                        {
                            if (zonasGet[z].idZona == entry.Value.IdZona && entry.Value.TimeOut == ""&&zonasGet[z].zonaActiva=="No")
                            {
                                SalidaForzadaPersona(entry.Key, zonasGet[z].finZona);
                            }
                        }
                    }                 
                }
                for (int z=0; z < zonasBBDD.Count; z++)
                {
                    foreach (KeyValuePair<string, Registrado> entry in values)
                    {
                        if (zonasBBDD[z].idZona == entry.Value.IdZona&&entry.Value.TimeOut=="")
                        {
                            counterPeople++;
                            zonasBBDD[z].genteEnZona = counterPeople;
                            
                        }
                    }
                   
                    counterPeople = 0;
                }
                counterPeople = 0;
                for (int y=0; y<zonasBBDD.Count; y++)
                {
                    if (zonasBBDD[y].genteEnZona >= zonasBBDD[y].aforoZona )
                    {
                        putZonaBloqueada(zonasBBDD[y].idZona - 1000, "Si");
                    }
                    if (zonasBBDD[y].genteEnZona < zonasBBDD[y].aforoZona)
                    {
                        //put zona liberada
                        putZonaBloqueada(zonasBBDD[y].idZona - 1000, "No");
                    }
                }
            }
        }
        private void putZonaBloqueada(int p, string zonaBloqueada1)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(" http://localhost:4000/Zonas-bloqueo/" + p);
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
        private void btnInsertPlano_Click(object sender, RoutedEventArgs e)
        {
            counter = 0;
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

            InfoCreacionEventos info = new InfoCreacionEventos();
            info.Show();
        }
        private void txtNambreZona_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            txtNambreZona.Text = "";
        }
        private void btnGuardarZona_Click(object sender, RoutedEventArgs e)
        {
            counter = 0;
            DataZonas.ItemsSource = null;
            positionEnabled = true;
            zonaGuardada = true;
            string nombreZona = "";
            int counterName2 = 0;
            if (txtNombreEvento.Text == "" || txtDireccionEvento.Text == "" || comboHoraInicio.Text == "Hora" || comboMinutosInicio.Text == "Minutos" ||
                comboHoraFin.Text == "Hora" || comboMinutosFin.Text == "Minutos")
            {
                openHelpCreatingZone();
            }
            else
            {
                if (HeightDesing == 0 || WidthDesing == 0)
                {

                    if (txtMtsZona.Text != "" && txtAforoIncluidoZona.Text != "")
                    {
                        try
                        {
                            mtsZonaDouble = Convert.ToDouble(txtMtsZona.Text);
                            aforoZonaInteger = Convert.ToInt32(txtAforoIncluidoZona.Text);

                            if (txtNambreZona.Text == "" || txtNambreZona.Text == "Opcional")
                            {
                                nombreZona = "Zona " + (zonasEvento.Count+1);
                                
                                aforoEvento = aforoZonaInteger + aforoEvento;
                                metrosEvento = metrosEvento + mtsZonaDouble;
                                txtMts2.Text = metrosEvento.ToString();
                                txtAforo.Text = aforoEvento.ToString();
                                txtNumeroZonas.Text = (zonasEvento.Count+1).ToString();
                                string horaInicioEvento = datePickerEvento.SelectedDate.ToString();
                                string[] splHoraInicioEvento = horaInicioEvento.Split(' ');
                                string[] splFechaInicio = splHoraInicioEvento[0].Split('/');
                                DateTime inicioEvento = new DateTime(Convert.ToInt32(splFechaInicio[2]), Convert.ToInt32(splFechaInicio[1]), Convert.ToInt32(splFechaInicio[0]),
                                    Convert.ToInt32(comboHoraInicio.Text), Convert.ToInt32(comboMinutosInicio.Text), 0);
                                string horaFinEvento = datePickerEventoFin.SelectedDate.ToString();
                                string[] splHoraFinEvento = horaFinEvento.Split(' ');
                                string[] splFechaFin = splHoraFinEvento[0].Split('/');
                                DateTime finEvento = new DateTime(Convert.ToInt32(splFechaFin[2]), Convert.ToInt32(splFechaFin[1]), Convert.ToInt32(splFechaFin[0]),
                                    Convert.ToInt32(comboHoraFin.Text), Convert.ToInt32(comboMinutosFin.Text), 0);
                                string horaInicioZona = datePickerZona.SelectedDate.ToString();
                                string[] splHoraInicioZona = horaInicioZona.Split(' ');
                                string[] splFechaInicioZona = splHoraInicioZona[0].Split('/');
                                DateTime inicioZona = new DateTime(Convert.ToInt32(splFechaInicioZona[2]), Convert.ToInt32(splFechaInicioZona[1]), Convert.ToInt32(splFechaInicioZona[0]),
                                    Convert.ToInt32(comboHoraZonaInicio.Text), Convert.ToInt32(comboMinutosZonaInicio.Text), 0);
                                string horaFinZona = datePickerZonaFin.SelectedDate.ToString();
                                string[] splHoraFinZona = horaFinZona.Split(' ');
                                string[] splFechaFinZona = splHoraFinZona[0].Split('/');
                                DateTime finZona = new DateTime(Convert.ToInt32(splFechaFinZona[2]), Convert.ToInt32(splFechaFinZona[1]), Convert.ToInt32(splFechaFinZona[0]),
                                    Convert.ToInt32(comboHoraZonaFin.Text), Convert.ToInt32(comboMinutosZonaFin.Text), 0);
                                int compareInicioZonaFinZona = DateTime.Compare(finZona, inicioZona);
                                int compareInicioZonaInicioEvento = DateTime.Compare(inicioZona, inicioEvento);
                                int compareFinZonaFinEvento = DateTime.Compare(finEvento, finZona);

                                if (compareInicioZonaFinZona <= 0)
                                {
                                    MessageBox.Show("La fecha FIN de Zona no puede ser anterior NI LA MISMA a la fecha INICIO de Zona", "Atención", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                                else if (compareInicioZonaInicioEvento < 0)
                                {
                                    MessageBox.Show("La fecha INICIO de Zona no puede ser anterior a la fecha INICIO del Evento", "Atención", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                                else if (compareFinZonaFinEvento < 0)
                                {
                                    MessageBox.Show("La fecha FIN de Zona no puede ser posterior a la fecha FIN del Evento", "Atención", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                                else
                                {
                                    string FechaInicioZona = Convert.ToInt32(splFechaInicioZona[2])+"/"+ Convert.ToInt32(splFechaInicioZona[1]) + "/"+ Convert.ToInt32(splFechaInicioZona[0])+" " + comboHoraZonaInicio.Text + ":" + comboMinutosZonaInicio.Text + ":00";
                                    string FechaFinZona = Convert.ToInt32(splFechaFinZona[2]) + "/" + Convert.ToInt32(splFechaFinZona[1]) + "/" + Convert.ToInt32(splFechaFinZona[0]) + " " + comboHoraZonaFin.Text + ":" + comboMinutosZonaFin.Text + ":00";
                                    zona = new Zona(nombreZona, mtsZonaDouble, 0, aforoZonaInteger, 0, 0, 0, 0, 0, "Si", "No", comboSituacionZona.Text, "No", FechaInicioZona, FechaFinZona);
                                    zonasEvento.Add(zona);
                                    txtMtsZona.Text = "";
                                    txtAforoIncluidoZona.Text = "";
                                    txtAforoEstimado.Text = "";
                                }
                            }
                            else
                            {
                                for (int i = 0; i < zonasEvento.Count; i++)
                                {
                                    if (txtNambreZona.Text.Equals(zonasEvento[i].nombreZona))
                                    {
                                        counterName2++;
                                    }
                                }
                                if (counterName2 > 0)
                                {
                                    MessageBox.Show("El nombre introducido ya está en uso. Por favor, ingresa otro nombre.", "ATENCIÓN", MessageBoxButton.OK, MessageBoxImage.Error);
                                    txtNambreZona.Text = "";
                                }
                                else
                                {
                                    aforoEvento = aforoZonaInteger + aforoEvento;
                                    txtAforo.Text = aforoEvento.ToString();
                                    metrosEvento = metrosEvento + mtsZonaDouble;
                                    txtMts2.Text = metrosEvento.ToString();
                                    txtNumeroZonas.Text = zonasEvento.Count.ToString();
                                    nombreZona = txtNambreZona.Text;
                                    string horaInicioEvento = datePickerEvento.SelectedDate.ToString();
                                    string[] splHoraInicioEvento = horaInicioEvento.Split(' ');
                                    string[] splFechaInicio = splHoraInicioEvento[0].Split('/');
                                    DateTime inicioEvento = new DateTime(Convert.ToInt32(splFechaInicio[2]), Convert.ToInt32(splFechaInicio[1]), Convert.ToInt32(splFechaInicio[0]),
                                        Convert.ToInt32(comboHoraInicio.Text), Convert.ToInt32(comboMinutosInicio.Text), 0);
                                    string horaFinEvento = datePickerEventoFin.SelectedDate.ToString();
                                    string[] splHoraFinEvento = horaFinEvento.Split(' ');
                                    string[] splFechaFin = splHoraFinEvento[0].Split('/');
                                    DateTime finEvento = new DateTime(Convert.ToInt32(splFechaFin[2]), Convert.ToInt32(splFechaFin[1]), Convert.ToInt32(splFechaFin[0]),
                                        Convert.ToInt32(comboHoraFin.Text), Convert.ToInt32(comboMinutosFin.Text), 0);
                                    string horaInicioZona = datePickerZona.SelectedDate.ToString();
                                    string[] splHoraInicioZona = horaInicioZona.Split(' ');
                                    string[] splFechaInicioZona = splHoraInicioZona[0].Split('/');
                                    DateTime inicioZona = new DateTime(Convert.ToInt32(splFechaInicioZona[2]), Convert.ToInt32(splFechaInicioZona[1]), Convert.ToInt32(splFechaInicioZona[0]),
                                        Convert.ToInt32(comboHoraZonaInicio.Text), Convert.ToInt32(comboMinutosZonaInicio.Text), 0);
                                    string horaFinZona = datePickerZonaFin.SelectedDate.ToString();
                                    string[] splHoraFinZona = horaFinZona.Split(' ');
                                    string[] splFechaFinZona = splHoraFinZona[0].Split('/');
                                    DateTime finZona = new DateTime(Convert.ToInt32(splFechaFinZona[2]), Convert.ToInt32(splFechaFinZona[1]), Convert.ToInt32(splFechaFinZona[0]),
                                        Convert.ToInt32(comboHoraZonaFin.Text), Convert.ToInt32(comboMinutosZonaFin.Text), 0);
                                    int compareInicioZonaFinZona = DateTime.Compare(finZona, inicioZona);
                                    int compareInicioZonaInicioEvento = DateTime.Compare(inicioZona, inicioEvento);
                                    int compareFinZonaFinEvento = DateTime.Compare(finEvento, finZona);

                                    if (compareInicioZonaFinZona <= 0)
                                    {
                                        MessageBox.Show("La fecha FIN de Zona no puede ser anterior NI LA MISMA a la fecha INICIO de Zona", "Atención", MessageBoxButton.OK, MessageBoxImage.Error);
                                    }
                                    else if (compareInicioZonaInicioEvento < 0)
                                    {
                                        MessageBox.Show("La fecha INICIO de Zona no puede ser anterior a la fecha INICIO del Evento", "Atención", MessageBoxButton.OK, MessageBoxImage.Error);
                                    }
                                    else if (compareFinZonaFinEvento < 0)
                                    {
                                        MessageBox.Show("La fecha FIN de Zona no puede ser posterior a la fecha FIN del Evento", "Atención", MessageBoxButton.OK, MessageBoxImage.Error);
                                    }
                                    else
                                    {
                                        string FechaInicioZona = Convert.ToInt32(splFechaInicioZona[2]) + "/" + Convert.ToInt32(splFechaInicioZona[1]) + "/" + Convert.ToInt32(splFechaInicioZona[0]) + " " + comboHoraZonaInicio.Text + ":" + comboMinutosZonaInicio.Text + ":00";
                                        string FechaFinZona = Convert.ToInt32(splFechaFinZona[2]) + "/" + Convert.ToInt32(splFechaFinZona[1]) + "/" + Convert.ToInt32(splFechaFinZona[0]) + " " + comboHoraZonaFin.Text + ":" + comboMinutosZonaFin.Text + ":00";
                                        zona = new Zona(nombreZona, mtsZonaDouble, 0, aforoZonaInteger, 0, 0, 0, 0, 0, "Si", "No", comboSituacionZona.Text, "No", FechaInicioZona, FechaFinZona);
                                        zonasEvento.Add(zona);
                                        counterName2 = 0;
                                        txtMtsZona.Text = "";
                                        txtAforoIncluidoZona.Text = "";
                                        txtZonaActiva.Text = "";
                                        txtNambreZona.Text = "Opcional";
                                        txtAforoEstimado.Text = "";
                                    }
                                    
                                }
                            }
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
                    if (counterName1 == 0)
                    {
                        aforoEvento = aforoZonaInteger + aforoEvento;
                        txtAforo.Text = aforoEvento.ToString();
                        txtNumeroZonas.Text = zonasEvento.Count.ToString();
                        metrosEvento = metrosEvento + mtsZonaDouble;
                        txtMts2.Text = metrosEvento.ToString();
                        string horaInicioEvento = datePickerEvento.SelectedDate.ToString();
                        string[] splHoraInicioEvento = horaInicioEvento.Split(' ');
                        string[] splFechaInicio = splHoraInicioEvento[0].Split('/');
                        DateTime inicioEvento = new DateTime(Convert.ToInt32(splFechaInicio[2]), Convert.ToInt32(splFechaInicio[1]), Convert.ToInt32(splFechaInicio[0]),
                            Convert.ToInt32(comboHoraInicio.Text), Convert.ToInt32(comboMinutosInicio.Text), 0);
                        string horaFinEvento = datePickerEventoFin.SelectedDate.ToString();
                        string[] splHoraFinEvento = horaFinEvento.Split(' ');
                        string[] splFechaFin = splHoraFinEvento[0].Split('/');
                        DateTime finEvento = new DateTime(Convert.ToInt32(splFechaFin[2]), Convert.ToInt32(splFechaFin[1]), Convert.ToInt32(splFechaFin[0]),
                            Convert.ToInt32(comboHoraFin.Text), Convert.ToInt32(comboMinutosFin.Text), 0);
                        string horaInicioZona = datePickerZona.SelectedDate.ToString();
                        string[] splHoraInicioZona = horaInicioZona.Split(' ');
                        string[] splFechaInicioZona = splHoraInicioZona[0].Split('/');
                        DateTime inicioZona = new DateTime(Convert.ToInt32(splFechaInicioZona[2]), Convert.ToInt32(splFechaInicioZona[1]), Convert.ToInt32(splFechaInicioZona[0]),
                            Convert.ToInt32(comboHoraZonaInicio.Text), Convert.ToInt32(comboMinutosZonaInicio.Text), 0);
                        string horaFinZona = datePickerZonaFin.SelectedDate.ToString();
                        string[] splHoraFinZona = horaFinZona.Split(' ');
                        string[] splFechaFinZona = splHoraFinZona[0].Split('/');
                        DateTime finZona = new DateTime(Convert.ToInt32(splFechaFinZona[2]), Convert.ToInt32(splFechaFinZona[1]), Convert.ToInt32(splFechaFinZona[0]),
                            Convert.ToInt32(comboHoraZonaFin.Text), Convert.ToInt32(comboMinutosZonaFin.Text), 0);
                        int compareInicioZonaFinZona = DateTime.Compare(finZona, inicioZona);
                        int compareInicioZonaInicioEvento = DateTime.Compare(inicioZona, inicioEvento);
                        int compareFinZonaFinEvento = DateTime.Compare(finEvento, finZona);

                        if (compareInicioZonaFinZona <= 0)
                        {
                            MessageBox.Show("La fecha FIN de Zona no puede ser anterior NI LA MISMA a la fecha INICIO de Zona", "Atención", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else if (compareInicioZonaInicioEvento < 0)
                        {
                            MessageBox.Show("La fecha INICIO de Zona no puede ser anterior a la fecha INICIO del Evento", "Atención", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else if (compareFinZonaFinEvento < 0)
                        {
                            MessageBox.Show("La fecha FIN de Zona no puede ser posterior a la fecha FIN del Evento", "Atención", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            string FechaInicioZona = Convert.ToInt32(splFechaInicioZona[2]) + "/" + Convert.ToInt32(splFechaInicioZona[1]) + "/" + Convert.ToInt32(splFechaInicioZona[0]) + " " + comboHoraZonaInicio.Text + ":" + comboMinutosZonaInicio.Text + ":00";
                            string FechaFinZona = Convert.ToInt32(splFechaFinZona[2]) + "/" + Convert.ToInt32(splFechaFinZona[1]) + "/" + Convert.ToInt32(splFechaFinZona[0]) + " " + comboHoraZonaFin.Text + ":" + comboMinutosZonaFin.Text + ":00";
                            zona = new Zona(textBlockNombreZona.Text, mtsZonaDouble, 0, aforoZonaInteger, 0, topPosition, leftPosition, HeightDesing, WidthDesing, "Si", "Si", comboSituacionZona.Text, "No", FechaInicioZona, FechaFinZona);
                            zonasEvento.Add(zona);
                            txtMtsZona.Text = "";
                            txtAforoIncluidoZona.Text = "";
                            txtZonaActiva.Text = "";
                            txtNambreZona.Text = "Opcional";
                        }
                        
                    }
                }
            }

            //string nombreZona, double metrosCuadradosZona, int idZona, int aforoZona, int idEventoZona, double topPositionZona, double leffPositionZona, double heightZona, double widthZona, bool zonaActiva, string zonaDibujada
            DataZonas.ItemsSource = zonasEvento;
            //al final
            HeightDesing = 0;
            WidthDesing = 0;
            mtsZonaDouble = 0;
            aforoZonaInteger = 0;
        }
        private void btnActivarZona_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = DataZonas.SelectedIndex;
            counter = 0;
            if (positionZones > -1)
            {
                if (zonasEvento[selectedIndex].zonaActiva == "Si")
                {
                    zonasEvento[selectedIndex].zonaActiva = "No";
                    txtNambreZona.Text = zonasEvento[positionZones].nombreZona;
                    txtZonaActiva.Text = zonasEvento[positionZones].zonaActiva;
                    txtMtsZona.Text = (zonasEvento[positionZones].metrosCuadradosZona).ToString();
                    txtAforoIncluidoZona.Text = zonasEvento[positionZones].aforoZona.ToString();
                }
                else
                {
                    zonasEvento[selectedIndex].zonaActiva = "Si";
                    txtNambreZona.Text = zonasEvento[positionZones].nombreZona;
                    txtZonaActiva.Text = zonasEvento[positionZones].zonaActiva;
                    txtMtsZona.Text = (zonasEvento[positionZones].metrosCuadradosZona).ToString();
                    txtAforoIncluidoZona.Text = zonasEvento[positionZones].aforoZona.ToString();
                }
                DataZonas.ItemsSource = null;
                DataZonas.ItemsSource = zonasEvento;
            }
            else
            {
                MessageBox.Show("Debes seleccionar una Zona de la lista, para activarla o inactivarla", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void DataZonas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            positionZones = DataZonas.SelectedIndex;
            counter = 0;
            if (positionZones > -1)
            {
                txtNambreZona.Text = zonasEvento[positionZones].nombreZona;
                txtZonaActiva.Text = zonasEvento[positionZones].zonaActiva;
                txtMtsZona.Text = (zonasEvento[positionZones].metrosCuadradosZona).ToString();
                txtAforoIncluidoZona.Text = zonasEvento[positionZones].aforoZona.ToString();
                string fechaInicioZona = zonasEvento[positionZones].inicioZona;
                //MessageBox.Show(fechaInicioZona);
                string[] splitFechaHoraInicio = fechaInicioZona.Split(' ');

                string[] splitFechaInicio = splitFechaHoraInicio[0].Split('/');
                datePickerZona.SelectedDate = new DateTime(Convert.ToInt32(splitFechaInicio[0]), Convert.ToInt32(splitFechaInicio[1]), Convert.ToInt32(splitFechaInicio[2]));
                string fechaFinZona = zonasEvento[positionZones].finZona;
                string[] splitFechaHoraFin = fechaFinZona.Split(' ');
                string[] splitFechaFin = splitFechaHoraFin[0].Split('/');
                datePickerZonaFin.SelectedDate = new DateTime(Convert.ToInt32(splitFechaFin[0]), Convert.ToInt32(splitFechaFin[1]), Convert.ToInt32(splitFechaFin[2]));
                string[] splitHozaZonaInicio = splitFechaHoraInicio[1].Split(':');
                comboHoraZonaInicio.Text = splitHozaZonaInicio[0];
                comboMinutosZonaInicio.Text = splitHozaZonaInicio[1];
                string[] splitHoraZonaFin = splitFechaHoraFin[1].Split(':');
                comboHoraZonaFin.Text = splitHoraZonaFin[0];
                comboMinutosZonaFin.Text = splitHoraZonaFin[1];
            }
        }
        private void btnEliminarZona_Click(object sender, RoutedEventArgs e)
        {
            counter = 0;
            zonaModificada.Clear();
            aforoEvento = 0;
            metrosEvento = 0;
            DataZonas.ItemsSource = null;
            LienzoClave.Children.Clear();
            if (zonasEvento.Count == 0)
            {
                counterZona = 1;
            }

            for (int i = 0; i < zonasEvento.Count - 1; i++)
            {
                zonaModificada.Add(zonasEvento[i]);
                SolidColorBrush mySolidColorBrush = new SolidColorBrush(Colors.Black);
                rec = new Rectangle();
                rec.StrokeThickness = 3;
                rec.Fill = mySolidColorBrush;
                rec.StrokeThickness = 4;
                rec.Width = (zonaModificada[i].widthZona);
                rec.Height = zonaModificada[i].heightZona;
                Canvas.SetLeft(rec, zonaModificada[i].leffPositionZona);
                Canvas.SetTop(rec, zonaModificada[i].topPositionZona);
                textBlockNombreZona = new TextBlock();
                Canvas.SetLeft(textBlockNombreZona, zonaModificada[i].leffPositionZona + 5);
                Canvas.SetTop(textBlockNombreZona, zonaModificada[i].topPositionZona + 5);
                textBlockNombreZona.FontSize = 10;
                textBlockNombreZona.Foreground = new SolidColorBrush(Colors.White);
                textBlockNombreZona.Text = zonaModificada[i].nombreZona;
                //string nombreZona, double metrosCuadradosZona, int idZona, int aforoZona, int idEventoZona, double topPositionZona, double leffPositionZona, double heightZona, double widthZona, bool zonaActiva, string zonaDibujada
                textMetrosCuadradosZona = new TextBlock();
                Canvas.SetLeft(textMetrosCuadradosZona, zonaModificada[i].leffPositionZona + 5);
                Canvas.SetTop(textMetrosCuadradosZona, zonaModificada[i].topPositionZona + 20);
                textMetrosCuadradosZona.FontSize = 10;
                textMetrosCuadradosZona.Foreground = new SolidColorBrush(Colors.White);
                textMetrosCuadradosZona.Text = zonaModificada[i].metrosCuadradosZona.ToString() + " metros2";
                textAforoZona = new TextBlock();
                Canvas.SetLeft(textAforoZona, zonaModificada[i].leffPositionZona + 5);
                Canvas.SetTop(textAforoZona, zonaModificada[i].topPositionZona + 35);
                textAforoZona.FontSize = 10;
                textAforoZona.Foreground = new SolidColorBrush(Colors.White);
                textAforoZona.Text = zonaModificada[i].aforoZona + pers;
                LienzoClave.Children.Add(rec);
                LienzoClave.Children.Add(textBlockNombreZona);
                LienzoClave.Children.Add(textMetrosCuadradosZona);
                LienzoClave.Children.Add(textAforoZona);
                aforoEvento = aforoEvento + zonaModificada[i].aforoZona;
                metrosEvento = metrosEvento + zonaModificada[i].metrosCuadradosZona;
            }
            zonasEvento.Clear();
            counterZona = zonaModificada.Count + 1;
            for (int j = 0; j < zonaModificada.Count; j++)
            {
                zonasEvento.Add(zonaModificada[j]);
            }
            txtMts2.Text = metrosEvento.ToString();
            txtNumeroZonas.Text = (counterZona - 1).ToString();
            txtAforo.Text = aforoEvento.ToString();
            DataZonas.ItemsSource = zonasEvento;
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
            counter = 0;
            if (DataEventos.SelectedIndex > -1)
            {
                if (zonasEvento.Count > 0)
                {
                    DialogResult result = (DialogResult)MessageBox.Show("¡ATENCIÓN!: Si continuas cargarás un nuevo plano con sus zonas pero eliminarás las zonas que has creado. ¿Estás seguro de continuar?", "ATENCIÓN", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        cargarEvento();
                    }
                    else { }
                }
                else
                {
                    cargarEvento();
                }
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

            selectedZone = DataEventos.SelectedIndex;

            txtNombreEvento.Text = eventosBBDD[selectedZone].nombreEvento;
            txtDireccionEvento.Text = eventosBBDD[selectedZone].direccionEvento;
            comboEventoActivo.Text = eventosBBDD[selectedZone].activoEvento;
            string[] horaInicioEvento = eventosBBDD[selectedZone].horaInicioEvento.Split(':');
            comboHoraInicio.Text = horaInicioEvento[0];
            comboMinutosInicio.Text = horaInicioEvento[1];
            string[] horaFinEvento = eventosBBDD[selectedZone].horaFinEvento.Split(':');
            comboHoraFin.Text = horaFinEvento[0];
            comboMinutosFin.Text = horaFinEvento[1];
            string date = eventosBBDD[selectedZone].fechaEvento;
            string[] dateSplit = date.Split('/');
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
                SolidColorBrush mySolidColorBrush = new SolidColorBrush(Colors.Black);
                rec = new Rectangle();
                rec.StrokeThickness = 3;
                rec.Fill = mySolidColorBrush;
                rec.StrokeThickness = 4;
                rec.Width = (zonasEvento[c].widthZona);
                rec.Height = zonasEvento[c].heightZona;
                Canvas.SetLeft(rec, zonasEvento[c].leffPositionZona);
                Canvas.SetTop(rec, zonasEvento[c].topPositionZona);
                textBlockNombreZona = new TextBlock();
                Canvas.SetLeft(textBlockNombreZona, zonasEvento[c].leffPositionZona + 5);
                Canvas.SetTop(textBlockNombreZona, zonasEvento[c].topPositionZona + 5);
                textBlockNombreZona.FontSize = 10;
                textBlockNombreZona.Foreground = new SolidColorBrush(Colors.White);
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

        private void btnGuardarIm_Click(object sender, RoutedEventArgs e)
        {
            postEvento();
        }

        private void postEvento()
        {
            if (txtNombreEvento.Text == "" || txtDireccionEvento.Text == "" || comboHoraInicio.Text == "Hora" ||
                comboMinutosInicio.Text == "Minutos" || comboHoraFin.Text == "Hora" || comboHoraFin.Text == "Minutos")
            {
                MessageBox.Show("Los campos Nombre Evento, Dirección, Fecha, Hora y Minutos de Inicio y Hora y Minutos Fin de Evento deben estar rellenos.", "ATENCIÓN", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (datePickerEvento.SelectedDate < DateTime.Now.Date || datePickerEventoFin.SelectedDate < DateTime.Now.Date)
                {
                    MessageBox.Show("La Fecha de inicio y Fin del Evento no puede ser anterior a la de hoy.", "ATENCIÓN", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    string dateinit = datePickerEvento.SelectedDate.ToString();
                    string[] dateinitsp1 = dateinit.Split(' ');
                    string dateinit1 = dateinitsp1[0];
                    string[] dateinitsp2 = dateinit1.Split('/');
                    int anyoinicio = Convert.ToInt32(dateinitsp2[2]);
                    int mesinicio = Convert.ToInt32(dateinitsp2[1]);
                    int diainicio = Convert.ToInt32(dateinitsp2[0]);
                    int horainicio = Convert.ToInt32(comboHoraInicio.Text);
                    int minutosinicio = Convert.ToInt32(comboMinutosInicio.Text);
                    string dateinitF = datePickerEventoFin.SelectedDate.ToString();
                    string[] dateinitsp1F = dateinitF.Split(' ');
                    string dateinit1F = dateinitsp1F[0];
                    string[] dateinitsp2F = dateinit1F.Split('/');
                    int anyoinicioF = Convert.ToInt32(dateinitsp2F[2]);
                    int mesinicioF = Convert.ToInt32(dateinitsp2F[1]);
                    int diainicioF = Convert.ToInt32(dateinitsp2F[0]);
                    int horafin = Convert.ToInt32(comboHoraFin.Text);
                    int minutosfin = Convert.ToInt32(comboMinutosFin.Text);
                    DateTime date1 = new DateTime(anyoinicio, mesinicio, diainicio, horainicio, minutosinicio, 0);
                    DateTime date2 = new DateTime(anyoinicioF, mesinicioF, diainicioF, horafin, minutosfin, 0);
                    int result = DateTime.Compare(date1, date2);
                    if (result < 0)
                    {
                        if (zonasEvento.Count > 0)
                        {
                            if (img1 == "")
                            {
                                if (selectedZone > -1)
                                {
                                    if (eventosBBDD[selectedZone].fotoEvento == "")
                                    {
                                        DialogResult result2 = (DialogResult)MessageBox.Show("¡ATENCIÓN!: Si continuas darás de alta el evento SIN FOTO y con " + zonasEvento.Count.ToString() + " zonas. " +
                                            "Comprueba si son los datos deseados y pulsa SI para continuar. ASEGÚRATE DE HABER GUARDADO LA ÚLTIMA ZONA CREADA.", "ATENCIÓN", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                                        if (result2 == System.Windows.Forms.DialogResult.Yes)
                                        {
                                            string inicioEvento = datePickerEvento.SelectedDate.ToString();
                                            string[] inicioEventoSplit = inicioEvento.Split(' ');
                                            string finEvento = datePickerEventoFin.SelectedDate.ToString();
                                            string[] finEventoSplit = finEvento.Split(' ');
                                            uploadEvento(eventosBBDD.Count, txtNombreEvento.Text, txtDireccionEvento.Text, "No", eventosBBDD.Count,
                                            inicioEventoSplit[0], comboHoraInicio.Text + ":" + comboMinutosInicio.Text, comboHoraFin.Text + ":" + comboMinutosFin.Text,
                                            zonasEvento.Count, Convert.ToInt32(txtAforo.Text), "", finEventoSplit[0]);
                                            for (int j = 0; j < zonasEvento.Count; j++)
                                            {
                                                UpdateZonas(zonasBBDD.Count + j, zonasEvento[j].nombreZona, (zonasEvento[j].metrosCuadradosZona), zonasBBDD.Count + 1000 + j,
                                                  (zonasEvento[j].aforoZona), eventosBBDD.Count, zonasEvento[j].topPositionZona,
                                                  zonasEvento[j].leffPositionZona, zonasEvento[j].heightZona, zonasEvento[j].widthZona, zonasEvento[j].zonaActiva,
                                                  zonasEvento[j].zonaDibujada, zonasEvento[j].situacionZona, zonasEvento[j].inicioZona, zonasEvento[j].finZona);
                                            }
                                            limpiarDatos();
                                            cargarArray();
                                        }
                                        else { }
                                    }
                                    else
                                    {
                                        DialogResult result3 = (DialogResult)MessageBox.Show("¡ATENCIÓN!: Si continuas darás de alta el evento CON LA FOTO SELECCIONADA y con " + zonasEvento.Count.ToString() + " zonas. " +
                                            "Comprueba si son los datos deseados y pulsa SI para continuar. ASEGÚRATE DE HABER GUARDADO LA ÚLTIMA ZONA CREADA.", "ATENCIÓN", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                                        if (result3 == System.Windows.Forms.DialogResult.Yes)
                                        {
                                            string inicioEvento = datePickerEvento.SelectedDate.ToString();
                                            string[] inicioEventoSplit = inicioEvento.Split(' ');
                                            string finEvento = datePickerEventoFin.SelectedDate.ToString();
                                            string[] finEventoSplit = finEvento.Split(' ');
                                            uploadEvento(eventosBBDD.Count, txtNombreEvento.Text, txtDireccionEvento.Text, "No", eventosBBDD.Count,
                                            inicioEventoSplit[0], comboHoraInicio.Text + ":" + comboMinutosInicio.Text, comboHoraFin.Text + ":" + comboMinutosFin.Text,
                                            zonasEvento.Count, Convert.ToInt32(txtAforo.Text), eventosBBDD[selectedZone].fotoEvento, finEventoSplit[0]);
                                            for (int j = 0; j < zonasEvento.Count; j++)
                                            {
                                                UpdateZonas(zonasBBDD.Count + j, zonasEvento[j].nombreZona, (zonasEvento[j].metrosCuadradosZona), zonasBBDD.Count + 1000 + j,
                                                  (zonasEvento[j].aforoZona), eventosBBDD.Count, zonasEvento[j].topPositionZona,
                                                  zonasEvento[j].leffPositionZona, zonasEvento[j].heightZona, zonasEvento[j].widthZona, zonasEvento[j].zonaActiva,
                                                  zonasEvento[j].zonaDibujada, zonasEvento[j].situacionZona, zonasEvento[j].inicioZona, zonasEvento[j].finZona);
                                            }
                                            limpiarDatos();
                                            cargarArray();
                                        }
                                        else { }
                                    }
                                }
                                else
                                {
                                    DialogResult result4 = (DialogResult)MessageBox.Show("¡ATENCIÓN!: Si continuas darás de alta el evento SIN FOTO y con " + zonasEvento.Count.ToString() + " zonas. " +
                                             "Comprueba si son los datos deseados y pulsa SI para continuar", "ATENCIÓN", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                                    if (result4 == System.Windows.Forms.DialogResult.Yes)
                                    {
                                        string inicioEvento = datePickerEvento.SelectedDate.ToString();
                                        string[] inicioEventoSplit = inicioEvento.Split(' ');
                                        string finEvento = datePickerEventoFin.SelectedDate.ToString();
                                        string[] finEventoSplit = finEvento.Split(' ');
                                        uploadEvento(eventosBBDD.Count, txtNombreEvento.Text, txtDireccionEvento.Text, "No", eventosBBDD.Count,
                                        inicioEventoSplit[0], comboHoraInicio.Text + ":" + comboMinutosInicio.Text, comboHoraFin.Text + ":" + comboMinutosFin.Text,
                                        zonasEvento.Count, Convert.ToInt32(txtAforo.Text), "", finEventoSplit[0]);
                                        for (int j = 0; j < zonasEvento.Count; j++)
                                        {
                                            UpdateZonas(zonasBBDD.Count + j, zonasEvento[j].nombreZona, (zonasEvento[j].metrosCuadradosZona), zonasBBDD.Count + 1000 + j,
                                              (zonasEvento[j].aforoZona), eventosBBDD.Count, zonasEvento[j].topPositionZona,
                                              zonasEvento[j].leffPositionZona, zonasEvento[j].heightZona, zonasEvento[j].widthZona, zonasEvento[j].zonaActiva,
                                              zonasEvento[j].zonaDibujada, zonasEvento[j].situacionZona, zonasEvento[j].inicioZona, zonasEvento[j].finZona);
                                        }
                                        limpiarDatos();
                                        cargarArray();
                                    }
                                    else { }
                                }
                            }
                            else
                            {
                                //update primero de la imagen y
                                UploadImg(img1, img1);
                                DialogResult result5 = (DialogResult)MessageBox.Show("¡ATENCIÓN!: Si continuas darás de alta el Evento con la FOTO escogida del archivo y con " + zonasEvento.Count.ToString() + " zonas. " +
                                             "Comprueba si son los datos deseados y pulsa SI para continuar", "ATENCIÓN", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                                if (result5 == System.Windows.Forms.DialogResult.Yes)
                                {
                                    string inicioEvento = datePickerEvento.SelectedDate.ToString();
                                    string[] inicioEventoSplit = inicioEvento.Split(' ');
                                    string finEvento = datePickerEventoFin.SelectedDate.ToString();
                                    string[] finEventoSplit = finEvento.Split(' ');
                                    uploadEvento(eventosBBDD.Count, txtNombreEvento.Text, txtDireccionEvento.Text, "No", eventosBBDD.Count,
                                    inicioEventoSplit[0], comboHoraInicio.Text + ":" + comboMinutosInicio.Text, comboHoraFin.Text + ":" + comboMinutosFin.Text,
                                    zonasEvento.Count, Convert.ToInt32(txtAforo.Text), imageToPost, finEventoSplit[0]);
                                    for (int j = 0; j < zonasEvento.Count; j++)
                                    {
                                        UpdateZonas(zonasBBDD.Count + j, zonasEvento[j].nombreZona, (zonasEvento[j].metrosCuadradosZona), zonasBBDD.Count + 1000 + j,
                                          (zonasEvento[j].aforoZona), eventosBBDD.Count, zonasEvento[j].topPositionZona,
                                          zonasEvento[j].leffPositionZona, zonasEvento[j].heightZona, zonasEvento[j].widthZona, zonasEvento[j].zonaActiva,
                                          zonasEvento[j].zonaDibujada, zonasEvento[j].situacionZona, zonasEvento[j].inicioZona, zonasEvento[j].finZona);
                                    }
                                    limpiarDatos();
                                    cargarArray();
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Para dar de alta el Evento hace falta crear como mínimo una Zona.", "ATENCIÓN", MessageBoxButton.OK, MessageBoxImage.Error);

                        }
                    }
                    else if (result == 0)
                        MessageBox.Show("La Fecha de inicio y Fin del Evento son la misma. No se creará el evento.", "ATENCIÓN", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                        MessageBox.Show("La Fecha de fin es anterior a la de Fin del Evento. No se creará el evento.", "ATENCIÓN", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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

        private void UpdateZonas(int count, string nombreZona1, double metrosCuadradosZona1,
            int idZona1, int aforoZona1, int idEventoZona1, double topPositionZona1,
            double leffPositionZona1, double heightZona1, double widthZona1, string zonaActiva1,
            string zonaDibujada1, string situacionZona1, string inicioZona1, string finZona1)
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
                zonaBloqueada = "No",
                inicioZona= inicioZona1,
                finZona= finZona1

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

        private void btnActivarEvento_Click(object sender, RoutedEventArgs e)
        {

            if (DataEventos.SelectedIndex < 0)
            {
                MessageBox.Show("Debes GUARDAR EL EVENTO para ACTIVARLO o elegir uno de los Eventos Guardados", "ATENCIÓN", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {

                if (counter == 1)
                {
                    string dateinit = eventosBBDD[DataEventos.SelectedIndex].fechaEvento;

                    string[] dateinitsp2 = dateinit.Split('/');
                    int anyoinicio = Convert.ToInt32(dateinitsp2[2]);
                    int mesinicio = Convert.ToInt32(dateinitsp2[1]);
                    int diainicio = Convert.ToInt32(dateinitsp2[0]);
                    string horarioInicio = (eventosBBDD[DataEventos.SelectedIndex].horaInicioEvento);
                    string[] horarioInicioSpl = horarioInicio.Split(':');
                    DateTime datec = new DateTime(anyoinicio, mesinicio, diainicio, Convert.ToInt32(horarioInicioSpl[0]), Convert.ToInt32(horarioInicioSpl[1]), 0);
                    DateTime dated = DateTime.Now;
                    int result = DateTime.Compare(datec, dated);
                    if (result < 0)
                    {
                        MessageBox.Show("La fecha de evento no puede ser anterior a la actual. Recuerda GUARDAR EL EVENTO si cambiaste la fecha y seleccionarlo de nuevo.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        if (eventosBBDD[DataEventos.SelectedIndex].activoEvento == "Si")
                        {
                            MessageBox.Show("El Evento seleccionado ya se encuentra dado de alta. Escoge un evento de la lista que no esté activo", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            int coincidencias = 0;
                            string nameEvento = eventosBBDD[DataEventos.SelectedIndex].nombreEvento;
                            string fechaEvento = eventosBBDD[DataEventos.SelectedIndex].fechaEvento;
                            string horaInic = eventosBBDD[DataEventos.SelectedIndex].horaInicioEvento;
                            string fechaFinEvento = eventosBBDD[DataEventos.SelectedIndex].fechaEventoFin;
                            string horaFinEvento = eventosBBDD[DataEventos.SelectedIndex].horaFinEvento;
                            string[] fechaIniEven = fechaEvento.Split('/');
                            string[] hrIniEven = horaInic.Split(':');
                            int anyoIniEv = Convert.ToInt32(fechaIniEven[2]);
                            int mesIniEv = Convert.ToInt32(fechaIniEven[1]);
                            int diaIniEv = Convert.ToInt32(fechaIniEven[0]);
                            int horIniEv = Convert.ToInt32(hrIniEven[0]);
                            int minIniEv = Convert.ToInt32(hrIniEven[1]);
                            string[] fechaFinEven = fechaFinEvento.Split('/');
                            string[] hrFinEven = horaFinEvento.Split(':');
                            int anyoFinEv = Convert.ToInt32(fechaFinEven[2]);
                            int mesFinEv = Convert.ToInt32(fechaFinEven[1]);
                            int diaFinEv = Convert.ToInt32(fechaFinEven[0]);
                            int horFinEv = Convert.ToInt32(hrFinEven[0]);
                            int minFinEv = Convert.ToInt32(hrFinEven[1]);

                            DateTime dateIniEv = new DateTime(anyoIniEv, mesIniEv, diaIniEv, horIniEv, minIniEv, 0);
                            DateTime dateFinEve = new DateTime(anyoFinEv, mesFinEv, diaFinEv, horFinEv, minFinEv, 0);

                            for (int j = 0; j < eventosBBDD.Count; j++)
                            {
                                if (eventosBBDD[j].nombreEvento == nameEvento && eventosBBDD[j].activoEvento == "Si")
                                {
                                    //ver solapados
                                    string fechaEventoBBDD = eventosBBDD[j].fechaEvento;
                                    string horaInicBBDD = eventosBBDD[j].horaInicioEvento;
                                    string fechaFinEventoBBDD = eventosBBDD[j].fechaEventoFin;
                                    string horaFinEventoBBDD = eventosBBDD[j].horaFinEvento;
                                    string[] fechaIniEvenBBDD = fechaEventoBBDD.Split('/');
                                    string[] hrIniEvenBBDD = horaInicBBDD.Split(':');
                                    int anyoIniEvBBDD = Convert.ToInt32(fechaIniEvenBBDD[2]);
                                    int mesIniEvBBDD = Convert.ToInt32(fechaIniEvenBBDD[1]);
                                    int diaIniEvBBDD = Convert.ToInt32(fechaIniEvenBBDD[0]);
                                    int horIniEvBBDD = Convert.ToInt32(hrIniEvenBBDD[0]);
                                    int minIniEvBBDD = Convert.ToInt32(hrIniEvenBBDD[1]);
                                    string[] fechaFinEvenBBDD = fechaFinEventoBBDD.Split('/');
                                    string[] hrFinEvenBBDD = horaFinEventoBBDD.Split(':');
                                    int anyoFinEvBBDD = Convert.ToInt32(fechaFinEvenBBDD[2]);
                                    int mesFinEvBBDD = Convert.ToInt32(fechaFinEvenBBDD[1]);
                                    int diaFinEvBBDD = Convert.ToInt32(fechaFinEvenBBDD[0]);
                                    int horFinEvBBDD = Convert.ToInt32(hrFinEvenBBDD[0]);
                                    int minFinEvBBDD = Convert.ToInt32(hrFinEvenBBDD[1]);
                                    DateTime dateIniEvBBDD = new DateTime(anyoIniEvBBDD, mesIniEvBBDD, diaIniEvBBDD, horIniEvBBDD, minIniEvBBDD, 0);
                                    DateTime dateFinEveBBDD = new DateTime(anyoFinEvBBDD, mesFinEvBBDD, diaFinEvBBDD, horFinEvBBDD, minFinEvBBDD, 0);

                                    int result1 = DateTime.Compare(dateIniEv, dateIniEvBBDD);
                                    int result2 = DateTime.Compare(dateIniEv, dateFinEveBBDD);
                                    int result3 = DateTime.Compare(dateFinEve, dateIniEvBBDD);
                                    int result4 = DateTime.Compare(dateFinEve, dateFinEveBBDD);
                                    int result5 = DateTime.Compare(dateIniEvBBDD, dateIniEv);
                                    int result6 = DateTime.Compare(dateIniEvBBDD, dateFinEve);
                                    int result7 = DateTime.Compare(dateFinEveBBDD, dateIniEv);
                                    int result8 = DateTime.Compare(dateFinEveBBDD, dateFinEve);
                                    if (result1 >= 0 && result2 <= 0)
                                    {
                                        coincidencias++;
                                    }
                                    if (result3 >= 0 && result4 <= 0)
                                    {
                                        coincidencias++;
                                    }
                                    if (result5 >= 0 && result6 <= 0)
                                    {
                                        coincidencias++;
                                    }
                                    if (result7 >= 0 && result8 <= 0)
                                    {
                                        coincidencias++;
                                    }
                                }
                            }
                            if (coincidencias > 0)
                            {
                                MessageBox.Show("El evento que intentas activar tiene EL MISMO NOMBRE que uno de los Eventos Activos y SE SOLAPA EN FECHAS. Cambia las fechas para que no se solapen o renombra el evento", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            else
                            {
                                AltaEvento(eventosBBDD[DataEventos.SelectedIndex].idEvento, "Si");
                                MessageBox.Show("El Evento se ha activado correctamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                                limpiarDatos();
                                cargarArray();
                                counter = 1;
                            }
                        }

                    }
                }
                if (counter == 0)
                {
                    MessageBox.Show("¡ATENCIÓN!: Has seleccionado el Evento nº " + DataEventos.SelectedIndex + " de la LISTA DE EVENTOS para activar. SI HAS MODIFICADO LOS DATOS DESPUÉS DE ELEGIRLO DEBES GUARDAR EL EVENTO Y LUEGO ACTIVARLO. COMPRUEBA QUE LOS DATOS SON LOS CORRECTOS Y QUE LA FECHA ES POSTERIOR A LA DE HOY. " +
                     " Si estás de acuerdo con los mismos pulsa una ves más el botón ACTIVAR EVENTO. En caso contrario haz las modificaciones que desees, GUARDA EL EVENTO pulsando el botón, SELECCIÓNALO EN LA LISTA Y ACTÍVALO.", "ATENCIÓN", MessageBoxButton.OK, MessageBoxImage.Warning);
                    counter++;
                }
            }
        }

        private void btnNuevoEvento_Click(object sender, RoutedEventArgs e)
        {
            counter = 0;
            LienzoClave.Children.Clear();
            LienzoClave.Background = new SolidColorBrush(Colors.White);
            LienzoClave.Opacity = 0.5;
            txtNombreEvento.Text = "";
            comboEventoActivo.Text = "No";
            txtDireccionEvento.Text = "";
            txtNumeroZonas.Text = "";
            datePickerEvento.SelectedDate = DateTime.Now.Date;
            datePickerEventoFin.SelectedDate = DateTime.Now.Date;
            comboHoraInicio.Text = "Hora";
            comboMinutosInicio.Text = "Minutos";
            comboHoraFin.Text = "Hora";
            comboMinutosFin.Text = "Minutos";
            txtMts2.Text = "";
            txtAforo.Text = "";
            txtNambreZona.Text = "Opcional";
            txtZonaActiva.Text = "";
            comboSituacionZona.Text = "Interior";
            txtMtsZona.Text = "";
            txtAforoIncluidoZona.Text = "";
            txtAforoEstimado.Text = "";
            DataZonas.ItemsSource = null;
            zonasEvento.Clear();
            DataZonas.ItemsSource = zonasEvento;
            counterZona = 1;
            counterName1 = 0;
            positionZones = -1;
            mtsZonaDouble = 0f;
            aforoZonaInteger = 0;
            aforoEvento = 0;
            metrosEvento = 0;
            img1 = "";
            imageToPost = "";
            webcounter = 0;
            selectedZone = -1;
            DataZonas.SelectedIndex = -1;
            DataEventos.SelectedIndex = -1;
            positionEnabled = false;
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
                }else if (compareDates < 0)
                {
                    horas =(DateTime.Now - fecharegistro).ToString(@"dd\d\ h\hmm\m\ ");
                    infoEvento.txtCuentaAtras.Text = "Tiempo desde inicio:";
                }
                else
                {
                    horas = (DateTime.Now - fecharegistro).ToString(@"dd\d\ h\hmm\m\ ");
                    infoEvento.txtCuentaAtras.Text = "Tiempo hasta inicio:";
                }
                

                infoEvento.tbkclock.Text = horas;
                infoEvento.Show();
            }

        }

        private void btnGestionEventos_Click(object sender, RoutedEventArgs e)
        {
            GestionEventos gestionEventos = new GestionEventos();
            gestionEventos.Show();
            this.Close();
        }
        private void openHelpCreatingZone()
        {
            infoGuardarZona info = new infoGuardarZona();
            info.Show();
        }

        private void btnMonitorEventos_Click(object sender, RoutedEventArgs e)
        {
            MonitorEventos monitorEventos = new MonitorEventos();
            monitorEventos.Show();
            this.Close();
        }
    }
}

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
    /// Lógica de interacción para MonitorEventos.xaml
    /// </summary>
    public partial class MonitorEventos : Window
    {
        System.Windows.Forms.Timer t1 = new System.Windows.Forms.Timer();
        Registrados register;
        int counter = 0;
        int webcounter = 0;
        int counterPeople = 0;
        int ocupacionEvento = 0;
        double mtsEvento = 0;
        int aforoEvento = 0;
        int counterZonas = 0;
        int counterSearch = 0;
        double porcentageOcupacionEvento = 0;
        double ocupacionZona = 0;
        double porcenZona = 0;
        List<Eventos> eventosBBDD = new List<Eventos>();
        List<Zona> zonasBBDD = new List<Zona>();
        List<Registrado> regitradosPasados = new List<Registrado>();
        List<Registrado> regitradosActuales = new List<Registrado>();
        List<Registrado> registradosTodos = new List<Registrado>();
        
        

        public MonitorEventos()
        {
            InitializeComponent();
            
            register = new Registrados();
            DataGenteFiltroActivo.DataContext = register;
            DataGenteFiltroPasado.DataContext = register;
            cargarElementos();
            recarga();
        }

        private void recarga()
        {
            t1.Interval = 5000;
            t1.Enabled = true;
            t1.Start();
            t1.Tick += new EventHandler(TimerEventProcessor);
        }

        private void TimerEventProcessor(object sender, EventArgs e)
        {
            limpiarElementos();
            cargarElementos();
            
        }

        private void limpiarElementos()
        {
            eventosBBDD.Clear();
            zonasBBDD.Clear();
            registradosTodos.Clear();
            regitradosActuales.Clear();
            regitradosPasados.Clear();
            if (counterSearch == 0)
            {
                DataGenteFiltroPasado.ItemsSource = null;
                DataGenteFiltroActivo.ItemsSource = null;
            }
            
            LienzoClave.Children.Clear();
            mtsEvento = 0;
            counter = 0;
            aforoEvento = 0;
            ocupacionEvento = 0;
            porcentageOcupacionEvento = 0;
            counterZonas = 0;
            ocupacionZona = 0;
            porcenZona = 0;
        }

        private void cargarElementos()
        {
            counter = 0;
            webcounter = 0;
            limpiarElementos();
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
                            for (int r = 0; r < zonasGetOut.Count; r++)
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
               
                var url4 = "http://localhost:4000/Zonas";
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
                    if (result < 0)
                    {
                        foreach (KeyValuePair<string, Registrado> entry in values)
                        {
                            if (zonasGet[z].idZona == entry.Value.IdZona && entry.Value.TimeOut == "" && zonasGet[z].zonaActiva == "No")
                            {
                                SalidaForzadaPersona(entry.Key, zonasGet[z].finZona);
                            }
                        }
                    }
                }
               
                for (int z = 0; z < zonasBBDD.Count; z++)
                {
                    foreach (KeyValuePair<string, Registrado> entry in values)
                    {
                        
                        if (zonasBBDD[z].idZona == entry.Value.IdZona && entry.Value.TimeOut == "")
                        {
                            counterPeople++;
                            zonasBBDD[z].genteEnZona = counterPeople;
                            
                        }
                    }

                    counterPeople = 0;
                }
                counterPeople = 0;
                for (int y = 0; y < zonasBBDD.Count; y++)
                {
                    if (zonasBBDD[y].genteEnZona >= zonasBBDD[y].aforoZona)
                    {
                        putZonaBloqueada(zonasBBDD[y].idZona - 1000, "Si");
                    }
                    if (zonasBBDD[y].genteEnZona < zonasBBDD[y].aforoZona)
                    {
                        //put zona liberada
                        putZonaBloqueada(zonasBBDD[y].idZona - 1000, "No");
                    }
                }
                //string idPersona, int age, string firstname, int idZona, string identityCard,
                //string lastname, string timeIn, string timeOu
                Registrado reg;
                foreach (KeyValuePair<string, Registrado> entry in values)
                {
                    reg = new Registrado(entry.Key, entry.Value.Age, entry.Value.Firstname, entry.Value.IdZona, entry.Value.IdentityCard,
                        entry.Value.Lastname, entry.Value.TimeIn, entry.Value.TimeOut);
                    for (int b = 0; b < zonasBBDD.Count; b++)
                    {
                        if (entry.Value.IdZona == zonasBBDD[b].idZona)
                        {
                            reg.NombreZonaPerson = zonasBBDD[b].nombreZona;
                            for(int t=0; t<eventosBBDD.Count; t++)
                            {
                                if(zonasBBDD[b].idEventoZona==eventosBBDD[t].idEvento&& eventosBBDD[t].activoEvento == "Si")
                                {
                                    reg.EventoActivoPerson = "Si";
                                    reg.NombreEventoPerson = eventosBBDD[t].nombreEvento;
                                }
                                if (zonasBBDD[b].idEventoZona == eventosBBDD[t].idEvento && eventosBBDD[t].activoEvento == "No")
                                {
                                    reg.EventoActivoPerson = "No";
                                    reg.NombreEventoPerson = eventosBBDD[t].nombreEvento;
                                }
                            }
                        }                     
                    }
                    registradosTodos.Add(reg);
                }
                for (int g = 0; g < registradosTodos.Count; g++)
                {
                    if (registradosTodos[g].EventoActivoPerson == "Si")
                    {
                        regitradosActuales.Add(registradosTodos[g]);
                       
                    }
                    if (registradosTodos[g].EventoActivoPerson == "No")
                    {
                        regitradosPasados.Add(registradosTodos[g]);
                       
                    }
                }
                if (counterSearch == 0)
                {
                    DataGenteFiltroActivo.ItemsSource = regitradosActuales;
                    DataGenteFiltroPasado.ItemsSource = regitradosPasados;
                }
              
               
                for (int t=0; t < eventosBBDD.Count;t++)
                {
                    if (eventosBBDD[t].activoEvento == "Si")
                    {
                        
                        SolidColorBrush mySolidColorBrushZ = new SolidColorBrush(Colors.BlueViolet);
                        StackPanel stackZActV = new StackPanel();
                        stackZActV.Orientation = Orientation.Vertical;
                        for (int g=0; g<zonasBBDD.Count; g++)
                        {
                            if (eventosBBDD[t].idEvento == zonasBBDD[g].idEventoZona)
                            {
                                mtsEvento = mtsEvento + zonasBBDD[g].metrosCuadradosZona;
                                counter++;
                                aforoEvento = aforoEvento + zonasBBDD[g].aforoZona;
                                ocupacionEvento = ocupacionEvento + zonasBBDD[g].genteEnZona;
                                if (zonasBBDD[g].zonaActiva == "Si")
                                {
                                    counterZonas++;
                                    
                                    StackPanel stackZActH = new StackPanel();
                                    stackZActH.Orientation = Orientation.Horizontal;  
                                    Label ZonasAct = new Label();
                                    ocupacionZona = zonasBBDD[g].genteEnZona;
                                    porcenZona = ocupacionZona * 100 / zonasBBDD[g].aforoZona;
                                    Double formatPorcent =Math.Round(porcenZona, 2);
                                    ZonasAct.Content = zonasBBDD[g].nombreZona+ " -- %Ocup: "+ formatPorcent+ "% -- Inicio/Fin: "+zonasBBDD[g].inicioZona+"--"+zonasBBDD[g].finZona;
                                    ZonasAct.FontSize = 14;
                                    Thickness marginZ = ZonasAct.Margin;
                                    marginZ.Left = 10;
                                    ZonasAct.Margin = marginZ;
                                    Thickness marginZt = ZonasAct.Margin;
                                    marginZt.Top = -10;
                                    ZonasAct.Margin = marginZt;
                                    ZonasAct.Foreground = mySolidColorBrushZ;
                                    ZonasAct.Height = 30;
                                    ZonasAct.HorizontalContentAlignment = HorizontalAlignment.Left;

                                    stackZActH.Children.Add(ZonasAct);

                                    stackZActV.Children.Add(stackZActH);
                                    StackPanel stackSplit = new StackPanel();
                                    stackSplit.Orientation = Orientation.Horizontal;
                                    /*Thickness marginBotom = stackSplit.Margin;
                                    marginBotom.Bottom = 5;
                                    stackSplit.Margin = marginBotom;*/
                                    Label Zero = new Label();
                                    Thickness marginZero = Zero.Margin;
                                    marginZero.Top = 5;
                                    Zero.Content = 0;
                                    Zero.FontSize = 12;
                                    Zero.Margin = marginZero;
                                    Thickness marginZeroz = Zero.Margin;
                                    marginZeroz.Left = 80;
                                    Zero.Margin = marginZeroz;
                                    Zero.HorizontalContentAlignment = HorizontalAlignment.Right;
                                    Label ocupZ = new Label();
                                    Thickness marginOcp = ocupZ.Margin;
                                    marginOcp.Top = 5;
                                    ocupZ.Content =zonasBBDD[g].aforoZona+ "   Aforo Máximo";
                                    ocupZ.FontSize = 12;
                                    ocupZ.Margin = marginOcp;
                                    ocupZ.HorizontalContentAlignment = HorizontalAlignment.Left;
                                    if (porcenZona >= 0 && porcenZona < 26)
                                    {
                                        SolidColorBrush mySolidColorBrush100 = new SolidColorBrush();
                                        Slider sliZona1 = new Slider();
                                        sliZona1.Foreground = Brushes.Black;
                                        sliZona1.Width = 360;
                                        sliZona1.Height = 45;
                                        sliZona1.IsSnapToTickEnabled = true;
                                        sliZona1.TickPlacement = System.Windows.Controls.Primitives.TickPlacement.Both;
                                        sliZona1.TickFrequency = 1;
                                        sliZona1.BorderBrush = Brushes.Red;
                                        sliZona1.BorderThickness = new Thickness(2);
                                        sliZona1.Minimum = 0;
                                        sliZona1.Maximum = zonasBBDD[g].aforoZona;
                                        sliZona1.Value = zonasBBDD[g].genteEnZona;
                                        sliZona1.IsEnabled = false;
                                        mySolidColorBrush100.Color = Color.FromRgb(15, 236, 15);
                                        sliZona1.Background = mySolidColorBrush100;
                                        stackSplit.Children.Add(Zero);
                                        stackSplit.Children.Add(sliZona1);
                                        stackSplit.Children.Add(ocupZ);
                                        stackZActV.Children.Add(stackSplit);
                                    }
                                    else
                                    if (porcenZona > 25 && porcenZona < 50)
                                    {
                                        SolidColorBrush mySolidColorBrush101 = new SolidColorBrush();
                                        Slider sliZona = new Slider();
                                        sliZona.Foreground = Brushes.Black;
                                        sliZona.Width = 360;
                                        sliZona.Height = 45;
                                        sliZona.TickPlacement = System.Windows.Controls.Primitives.TickPlacement.Both;
                                        sliZona.BorderBrush = Brushes.Red;
                                        sliZona.BorderThickness = new Thickness(2);
                                        sliZona.TickFrequency = 1;
                                        sliZona.Minimum = 0;
                                        sliZona.BorderBrush = Brushes.Red;
                                        sliZona.BorderThickness = new Thickness(2);
                                        sliZona.Maximum = zonasBBDD[g].aforoZona;
                                        sliZona.Value = zonasBBDD[g].genteEnZona;
                                        sliZona.IsEnabled = false;
                                        mySolidColorBrush101.Color = Color.FromRgb(7, 129, 7);
                                        sliZona.IsSnapToTickEnabled = true;
                                        sliZona.Background = mySolidColorBrush101;
                                        stackSplit.Children.Add(Zero);
                                        stackSplit.Children.Add(sliZona);
                                        stackSplit.Children.Add(ocupZ);
                                        stackZActV.Children.Add(stackSplit);
                                    }
                                    else
                                    if (porcenZona > 49 && porcenZona < 66)
                                    {
                                        SolidColorBrush mySolidColorBrush102 = new SolidColorBrush();
                                        Slider sliZona2 = new Slider();
                                        sliZona2.Foreground = Brushes.Black;
                                        sliZona2.Width = 360;
                                        sliZona2.Height = 45;
                                        sliZona2.TickPlacement = System.Windows.Controls.Primitives.TickPlacement.Both;
                                        sliZona2.BorderBrush = Brushes.Red;
                                        sliZona2.BorderThickness = new Thickness(2);
                                        sliZona2.TickFrequency = 1;
                                        sliZona2.Minimum = 0;
                                        sliZona2.Maximum = zonasBBDD[g].aforoZona;
                                        sliZona2.Value = zonasBBDD[g].genteEnZona;
                                        sliZona2.IsEnabled = false;
                                        mySolidColorBrush102.Color = Color.FromRgb(224, 245, 12);
                                        sliZona2.IsSnapToTickEnabled = true;
                                        sliZona2.Background = mySolidColorBrush102;
                                        stackSplit.Children.Add(Zero);
                                        stackSplit.Children.Add(sliZona2);
                                        stackSplit.Children.Add(ocupZ);
                                        stackZActV.Children.Add(stackSplit);
                                    }
                                    else
                                    if (porcenZona > 65 && porcenZona < 90)
                                    {
                                        SolidColorBrush mySolidColorBrush103 = new SolidColorBrush();
                                        Slider sliZona3 = new Slider();
                                        sliZona3.Foreground = Brushes.Black;
                                        sliZona3.Width = 360;
                                        sliZona3.Height = 45;
                                        sliZona3.TickPlacement = System.Windows.Controls.Primitives.TickPlacement.Both;
                                        sliZona3.BorderBrush = Brushes.Red;
                                        sliZona3.BorderThickness = new Thickness(2);
                                        sliZona3.TickFrequency = 1;
                                        sliZona3.Minimum = 0;
                                        sliZona3.Maximum = zonasBBDD[g].aforoZona;
                                        sliZona3.Value = zonasBBDD[g].genteEnZona;
                                        sliZona3.IsEnabled = false;
                                        mySolidColorBrush103.Color = Color.FromRgb(245, 128, 12);
                                        sliZona3.IsSnapToTickEnabled = true;
                                        sliZona3.Background = mySolidColorBrush103;
                                        stackSplit.Children.Add(Zero);
                                        stackSplit.Children.Add(sliZona3);
                                        stackSplit.Children.Add(ocupZ);
                                        stackZActV.Children.Add(stackSplit);
                                    }
                                    else
                                    if (porcenZona > 89)
                                    {
                                        SolidColorBrush mySolidColorBrush104 = new SolidColorBrush();
                                        Slider sliZona4 = new Slider();
                                        sliZona4.Foreground = Brushes.Black;
                                        sliZona4.Width = 360;
                                        sliZona4.Height = 45;
                                        sliZona4.TickPlacement = System.Windows.Controls.Primitives.TickPlacement.Both;
                                        sliZona4.BorderBrush = Brushes.Red;
                                        sliZona4.BorderThickness = new Thickness(2);
                                        sliZona4.TickFrequency = 1;
                                        sliZona4.Minimum = 0;
                                        sliZona4.Maximum = zonasBBDD[g].aforoZona;
                                        sliZona4.Value = zonasBBDD[g].genteEnZona;
                                        sliZona4.IsEnabled = false;
                                        mySolidColorBrush104.Color = Color.FromRgb(245, 12, 12);
                                        sliZona4.IsSnapToTickEnabled = true;
                                        sliZona4.Background = mySolidColorBrush104;
                                        stackSplit.Children.Add(Zero);
                                        stackSplit.Children.Add(sliZona4);
                                        stackSplit.Children.Add(ocupZ);
                                        stackZActV.Children.Add(stackSplit);
                                    }
                                    Label okupas = new Label();
                                    okupas.Content= "Ocupación (PAX): " + zonasBBDD[g].genteEnZona;
                                    okupas.FontSize = 14;
                                    okupas.HorizontalAlignment = HorizontalAlignment.Center;
                                    Thickness merginOkupas = okupas.Margin;
                                    merginOkupas.Top = -25;
                                    okupas.Margin = merginOkupas;
                                    stackZActV.Children.Add(okupas);


                                    porcenZona = 0;
                                }
                            }
                        }
                        LienzoClave.Background = Brushes.White;
                        ocupacionZona = 0;
                        porcentageOcupacionEvento = (ocupacionEvento * 100 / aforoEvento);
                        Double porcentRound = Math.Round(porcentageOcupacionEvento, 2); 
                         SolidColorBrush mySolidColorBrush = new SolidColorBrush(Colors.Blue);
                        Label porcent = new Label();
                        porcent.Content = "% Ocup: ";
                        porcent.FontSize = 18;
                        Thickness margin = porcent.Margin;
                        porcent.Foreground = mySolidColorBrush;
                        porcent.Height = 30;
                        porcent.HorizontalContentAlignment = HorizontalAlignment.Left;
                        margin.Top = -10;
                        porcent.Margin = margin;
                        Label prc = new Label();
                        prc.Content = porcentRound+"%";
                        prc.FontSize = 14;
                        prc.Height = 30;
                        Thickness margin26 = prc.Margin;
                        margin26.Left = -15;
                        prc.Margin = margin26;
                        Thickness margin30 = prc.Margin;
                        margin30.Top = -1;
                        prc.Margin = margin30;
                        prc.HorizontalContentAlignment = HorizontalAlignment.Left;

                        Label Aforo = new Label();
                        Aforo.Content = "Aforo: ";
                        Aforo.FontSize = 18;
                        
                        Aforo.Foreground = mySolidColorBrush;
                        Aforo.Height = 30;
                        Aforo.HorizontalContentAlignment = HorizontalAlignment.Left;
                        margin.Top = -10;
                        Aforo.Margin = margin;
                        Label Afr = new Label();
                        Afr.Content = aforoEvento;
                        Afr.FontSize = 14;
                        Afr.Height = 30;
                       
                        margin26.Left = -15;
                        Afr.Margin = margin26;
                        
                        margin30.Top = -1;
                        Afr.Margin = margin30;
                        Afr.HorizontalContentAlignment = HorizontalAlignment.Left;
                        Label Ocupacion = new Label();
                        Ocupacion.Content = "Ocupacion: ";
                        Ocupacion.FontSize = 18;

                        Ocupacion.Foreground = mySolidColorBrush;
                        Ocupacion.Height = 30;
                        Ocupacion.HorizontalContentAlignment = HorizontalAlignment.Left;
                        margin.Top = -10;
                        Ocupacion.Margin = margin;
                        Label Ocp = new Label();
                        Ocp.Content = ocupacionEvento;
                        Ocp.FontSize = 14;
                        Ocp.Height = 30;
                        
                        margin26.Left = -15;
                        Ocp.Margin = margin26;
                        
                        margin30.Top = -1;
                        Ocp.Margin = margin30;
                        Ocp.HorizontalContentAlignment = HorizontalAlignment.Left;

                        StackPanel stackZona = new StackPanel();
                        stackZona.Orientation = Orientation.Horizontal;
                        Label numeroZonas = new Label();
                        numeroZonas.Content = "Número Zonas: ";
                        numeroZonas.FontSize = 18;
                        numeroZonas.Margin = margin;
                        numeroZonas.Foreground = mySolidColorBrush;
                        numeroZonas.Height = 30;
                        numeroZonas.HorizontalContentAlignment = HorizontalAlignment.Left;
                        Label nZon = new Label();
                        nZon.Content = counter;
                        nZon.FontSize = 14;
                        nZon.Height = 30;
                        Thickness margin2 = nZon.Margin;
                        margin2.Left = -15;
                        nZon.Margin = margin2;
                        Thickness margin3 = nZon.Margin;
                        margin3.Top = -1;
                        nZon.Margin = margin3;
                        nZon.HorizontalContentAlignment = HorizontalAlignment.Left;
                        Label zonasActivas = new Label();
                        zonasActivas.Content = "Activas: ";
                        zonasActivas.FontSize = 18;
                        zonasActivas.Margin = margin;
                        zonasActivas.Foreground = mySolidColorBrush;
                        zonasActivas.Height = 30;
                        zonasActivas.HorizontalContentAlignment = HorizontalAlignment.Left;
                        Label zAct = new Label();
                        zAct.Content = counterZonas;
                        zAct.FontSize = 14;
                        zAct.Height = 30;
                        
                        margin2.Left = -15;
                        zAct.Margin = margin2;
                        
                        margin3.Top = -1;
                        zAct.Margin = margin3;
                        zAct.HorizontalContentAlignment = HorizontalAlignment.Left;

                        StackPanel stackEvento = new StackPanel();
                        stackEvento.Orientation = Orientation.Vertical;
                        stackEvento.HorizontalAlignment = HorizontalAlignment.Left;
                        stackEvento.VerticalAlignment = VerticalAlignment.Top;
                        StackPanel stakInicio = new StackPanel();
                        stakInicio.Orientation = Orientation.Horizontal;
                        Label labelGenericEvent = new Label();
                        Thickness margin266 = labelGenericEvent.Margin;
                        margin266.Left = 5;
                        
                        
                        labelGenericEvent.Content = "Evento nº: "+eventosBBDD[t].idEvento;
                        labelGenericEvent.FontSize = 22;
                        SolidColorBrush mySolidColorBrush2 = new SolidColorBrush(Colors.Red);
                        Border borderTitle = new Border();
                        borderTitle.Width = 230;
                        borderTitle.Height = 65;
                        borderTitle.BorderBrush = Brushes.Black;
                        borderTitle.BorderThickness = new Thickness(2);
                        Thickness margin267 = borderTitle.Margin;
                        margin267.Top = 15;
                        borderTitle.Margin = margin267;
                        

                        borderTitle.Background = Brushes.Gray;
                        labelGenericEvent.Foreground = mySolidColorBrush2;
                        labelGenericEvent.Height = 40;
                        labelGenericEvent.HorizontalContentAlignment = HorizontalAlignment.Left;
                        Border borderCanvas = new Border();
                        borderCanvas.Width = 70;
                        borderCanvas.Height = 50;
                        borderCanvas.BorderBrush = Brushes.Black;
                        borderCanvas.BorderThickness = new Thickness(2);
                        Canvas canvasMiniatura = new Canvas();
                        canvasMiniatura.Height = 50;
                        canvasMiniatura.Width = 70;
                        
                        if (eventosBBDD[t].fotoEvento != "")
                        {
                            BitmapImage bi3 = new BitmapImage();
                            bi3.BeginInit();
                            bi3.UriSource = new Uri(eventosBBDD[t].fotoEvento);
                            bi3.EndInit();
                            ImageBrush imageBrush = new ImageBrush();
                            imageBrush.ImageSource = new BitmapImage(bi3.UriSource);
                            canvasMiniatura.Background = imageBrush;
                            canvasMiniatura.Opacity = 2;
                            
                        }
                        
                        borderCanvas.Child = canvasMiniatura;
                        borderTitle.Child = stakInicio;
                        Label labelGenericData = new Label();                       
                        labelGenericData.Content = "Nombre-Direccción: ";
                        labelGenericData.FontSize = 18;
                       
                        labelGenericData.Foreground = mySolidColorBrush;
                        labelGenericData.Height = 30;
                        labelGenericData.HorizontalContentAlignment = HorizontalAlignment.Left;
                        Label labelNombreEvento = new Label();
                        labelNombreEvento.FontSize = 14;
                        labelNombreEvento.Content = eventosBBDD[t].nombreEvento + "--" + eventosBBDD[t].direccionEvento;
                        labelNombreEvento.Height = 30;
                        labelNombreEvento.HorizontalContentAlignment = HorizontalAlignment.Left;
                        
                        margin.Top = -10;
                        labelNombreEvento.Margin = margin;
                        Label labelGenericData2 = new Label();
                        labelGenericData2.Content = "Fecha Inicio-Fin: ";
                        labelGenericData2.FontSize = 18;                       
                        labelGenericData2.Margin = margin;
                        labelGenericData2.Foreground = mySolidColorBrush;
                        labelGenericData2.Height = 30;
                        labelGenericData2.HorizontalContentAlignment = HorizontalAlignment.Left;
                        Label labelDateEvent = new Label();
                        labelDateEvent.Content = eventosBBDD[t].horaInicioEvento + " " + eventosBBDD[t].fechaEvento + "--" + eventosBBDD[t].horaFinEvento + " " + eventosBBDD[t].fechaEventoFin;
                        labelDateEvent.FontSize = 14;
                        labelDateEvent.Height = 30;
                        labelDateEvent.Margin = margin;
                        labelDateEvent.HorizontalContentAlignment = HorizontalAlignment.Left;
                        StackPanel stackAforo = new StackPanel();
                        stackAforo.Orientation = Orientation.Horizontal;
                        Label metros2 = new Label();
                        metros2.Content = "Metros Cuadrados: ";
                        metros2.FontSize = 18;
                        metros2.Margin = margin;
                        metros2.Foreground = mySolidColorBrush;
                        metros2.Height = 30;
                        metros2.HorizontalContentAlignment = HorizontalAlignment.Left;
                        Label mts2 = new Label();
                        mts2.Content = mtsEvento;
                        mts2.FontSize = 14;
                        mts2.Height = 30;
                        //Thickness margin2 = mts2.Margin;
                        margin2.Left = -15;
                        mts2.Margin = margin2;
                        //Thickness margin3 = mts2.Margin;
                        margin3.Top = -1;
                        mts2.Margin = margin3;
                        mts2.HorizontalContentAlignment = HorizontalAlignment.Left;
                        Label zonaInactiva = new Label();
                        zonaInactiva.Content = "Inactivas: ";
                        zonaInactiva.FontSize = 18;
                        zonaInactiva.Margin = margin;
                        zonaInactiva.Foreground = mySolidColorBrush;
                        zonaInactiva.Height = 30;
                        zonaInactiva.HorizontalContentAlignment = HorizontalAlignment.Left;
                        Label zIna = new Label();
                        zIna.Content = counter-counterZonas;
                        zIna.FontSize = 14;
                        zIna.Height = 30;
                        //Thickness margin2 = mts2.Margin;
                        margin2.Left = -15;
                        zIna.Margin = margin2;
                        //Thickness margin3 = mts2.Margin;
                        margin3.Top = -1;
                        zIna.Margin = margin3;
                        zIna.HorizontalContentAlignment = HorizontalAlignment.Left;
                       

                        if (porcentRound >= 0 && porcentRound < 26)
                        {
                            SolidColorBrush mySolidColorBrush10 = new SolidColorBrush();
                            mySolidColorBrush10.Color = Color.FromRgb(15, 236, 15);
                            Ocp.Background = mySolidColorBrush10;
                            prc.Background = mySolidColorBrush10;
                        }
                        if (porcentRound > 25 && porcentRound < 50)
                        {
                            SolidColorBrush mySolidColorBrush1101 = new SolidColorBrush();
                            mySolidColorBrush1101.Color = Color.FromRgb(7, 129, 7);
                            Ocp.Background = mySolidColorBrush1101;
                            prc.Background = mySolidColorBrush1101;
                        }
                        if (porcentRound > 49 && porcentRound < 66)
                        {
                            SolidColorBrush mySolidColorBrush1102 = new SolidColorBrush();
                            mySolidColorBrush1102.Color = Color.FromRgb(224, 245, 12);
                            Ocp.Background = mySolidColorBrush1102;
                            prc.Background = mySolidColorBrush1102;
                        }
                        if (porcentRound > 65 && porcentRound < 90)
                        {
                            SolidColorBrush mySolidColorBrush1103 = new SolidColorBrush();
                            mySolidColorBrush1103.Color = Color.FromRgb(245, 128, 12);
                            Ocp.Background = mySolidColorBrush1103;
                            prc.Background = mySolidColorBrush1103;
                        }
                        if (porcentRound > 89 )
                        {
                            SolidColorBrush mySolidColorBrush1104 = new SolidColorBrush();
                            mySolidColorBrush1104.Color = Color.FromRgb(245, 12, 12);
                            Ocp.Background = mySolidColorBrush1104;
                            prc.Background = mySolidColorBrush1104;
                        }
                        Border EventInit = new Border();
                        EventInit.BorderThickness = new Thickness(2);
                        EventInit.BorderBrush = Brushes.Red;
                        EventInit.Width = 605;
                        Thickness marginI = EventInit.Margin;
                        marginI.Left = 0;
                        EventInit.Margin = marginI;
                        Border EventFin = new Border();
                        EventFin.BorderThickness = new Thickness(2);
                        EventFin.BorderBrush = Brushes.Red;
                        EventFin.Width = 605;
                        StackPanel ZonasActivas = new StackPanel();
                        ZonasActivas.Orientation = Orientation.Vertical;
                        Label ZonasAct2 = new Label();
                        ZonasAct2.Content = "Zonas Activas";
                        ZonasAct2.Foreground = Brushes.Red;
                        ZonasAct2.FontSize = 18;
                        ZonasActivas.HorizontalAlignment = HorizontalAlignment.Center;                      
                        ZonasActivas.Children.Add(ZonasAct2 );
                        marginI.Left = 0;
                        EventFin.Margin = marginI;

                        stackAforo.Children.Add(metros2);
                        stackAforo.Children.Add(mts2);
                        stackAforo.Children.Add(Aforo);
                        stackAforo.Children.Add(Afr);
                        stackAforo.Children.Add(Ocupacion);
                        stackAforo.Children.Add(Ocp);
                        stackAforo.Children.Add(porcent);
                        stackAforo.Children.Add(prc);
                        stackZona.Children.Add(numeroZonas);
                        stackZona.Children.Add(nZon);
                        stackZona.Children.Add(zonasActivas);
                        stackZona.Children.Add(zAct);
                        stackZona.Children.Add(zonaInactiva);
                        stackZona.Children.Add(zIna);
                        
         
                        stakInicio.Children.Add(labelGenericEvent);
                        stakInicio.Children.Add(borderCanvas);
                        stackEvento.Children.Add(borderTitle);
                        stackEvento.Children.Add(EventInit);
                        stackEvento.Children.Add(labelGenericData);
                        stackEvento.Children.Add(labelNombreEvento);
                        stackEvento.Children.Add(labelGenericData2);
                        stackEvento.Children.Add(labelDateEvent);
                        stackEvento.Children.Add(stackAforo);
                        stackEvento.Children.Add(stackZona);
                        if(counterZonas>0)
                        stackEvento.Children.Add(ZonasActivas);
                        stackEvento.Children.Add(stackZActV);
                        stackEvento.Children.Add(EventFin);

                        LienzoClave.Children.Add(stackEvento);                      
                    }
                    mtsEvento = 0;
                    counter = 0;
                    aforoEvento = 0;
                    ocupacionEvento = 0;
                    porcentageOcupacionEvento = 0;
                    counterZonas = 0;
                }
            }
        }

        private void TxtSearch_SelectionChanged(object sender, RoutedEventArgs e)
        {
            //String busqueda = registradosTodos[g].Firstname + registradosTodos[g].Lastname + registradosTodos[g].Age + registradosTodos[g].IdentityCard + registradosTodos[g].TimeIn + registradosTodos[g].TimeOut +
            //registradosTodos[g].NombreEventoPerson + registradosTodos[g].NombreZonaPerson;
            //stAltas.Add(busqueda);
            List<Registrado> selectAltas = new List<Registrado>();
            List<Registrado> SelectBajas = new List<Registrado>();
            List<string> stAltas = new List<string>();
            List<string> stBajas = new List<string>();
            List<Registrado> Alta = new List<Registrado>();
            List<Registrado> Baja = new List<Registrado>();
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
            List<Eventos> eventosGet = JsonConvert.DeserializeObject<List<Eventos>>(datos);
            List<Zona> zonasGet = JsonConvert.DeserializeObject<List<Zona>>(datos2);
            selectAltas.Clear();
            SelectBajas.Clear();
            stAltas.Clear();
            stBajas.Clear();
            Alta.Clear();
            Baja.Clear();
            Registrado reg;
            for (int z = 0; z < eventosGet.Count; z++)
            {
                
                for (int s = 0; s < zonasGet.Count; s++)
                {
                    foreach (KeyValuePair<string, Registrado> entry in values)
                    {
                       
                        if (eventosGet[z].activoEvento == "Si" && zonasGet[s].idEventoZona == eventosGet[z].idEvento
                            && zonasGet[s].idZona == entry.Value.IdZona)
                        {
                            
                            reg = new Registrado(entry.Key, entry.Value.Age, entry.Value.Firstname, entry.Value.IdZona, entry.Value.IdentityCard,
                                entry.Value.Lastname, entry.Value.TimeIn, entry.Value.TimeOut);
                            reg.NombreZonaPerson = zonasGet[s].nombreZona;
                            reg.NombreEventoPerson = eventosGet[z].nombreEvento;
                            string busqueda = entry.Value.Age + entry.Value.Firstname.ToLower() + entry.Value.IdentityCard.ToLower() +
                                entry.Value.Lastname.ToLower() + entry.Value.TimeIn + entry.Value.TimeOut + zonasGet[s].nombreZona.ToLower() + eventosGet[z].nombreEvento.ToLower();
                            selectAltas.Add(reg);

                            stAltas.Add(busqueda);
                        }
                        if (eventosGet[z].activoEvento == "No" && zonasGet[s].idEventoZona == eventosGet[z].idEvento
                            && zonasGet[s].idZona == entry.Value.IdZona)
                        {

                            reg = new Registrado(entry.Key, entry.Value.Age, entry.Value.Firstname, entry.Value.IdZona, entry.Value.IdentityCard,
                                entry.Value.Lastname, entry.Value.TimeIn, entry.Value.TimeOut);
                            reg.NombreZonaPerson = zonasGet[s].nombreZona;
                            reg.NombreEventoPerson = eventosGet[z].nombreEvento;
                            string busqueda = entry.Value.Age + entry.Value.Firstname.ToLower() + entry.Value.IdentityCard.ToLower() +
                               entry.Value.Lastname.ToLower() + entry.Value.TimeIn + entry.Value.TimeOut + zonasGet[s].nombreZona.ToLower() + eventosGet[z].nombreEvento.ToLower();
                            SelectBajas.Add(reg);
                            stBajas.Add(busqueda);
                        }
                    }
                }
            }


            string d = TxtSearch.Text;
            string x = d.ToLower();
            if (d.Length > 0)
            {
                
                DataGenteFiltroActivo.ItemsSource = null;
                DataGenteFiltroPasado.ItemsSource = null;
                counterSearch = 1;
                for(int b=0;b< stAltas.Count; b++)
                {
                    if (stAltas[b].Contains(x) )
                    {
                        Alta.Add(selectAltas[b]);
                    }
                }
                for (int q = 0; q < stBajas.Count; q++)
                {
                    if (stBajas[q].Contains(x))
                    {
                        Baja.Add(SelectBajas[q]);
                    }
                }
                DataGenteFiltroActivo.ItemsSource = Alta;
                DataGenteFiltroPasado.ItemsSource = Baja;
            }
            else
            {
                counterSearch = 0;
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

        private void btnGestionEventos_Click(object sender, RoutedEventArgs e)
        {
            GestionEventos gestionEventos = new GestionEventos();
            gestionEventos.Show();
            this.Close();
        }

        private void btnPlano_Click(object sender, RoutedEventArgs e)
        {
            Plano plano = new Plano();
            plano.Show();
            this.Close();
        }
    }
}

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
    /// Lógica de interacción para GestionNominas.xaml
    /// </summary>
    public partial class GestionNominas : Window
    {
        Nomimas nomimN;
        List<Staff> staffs = new List<Staff>();
        List<Nomimas> nomimas = new List<Nomimas>();
        List<Nomimas> editNominas = new List<Nomimas>();
        List<Nomimas> cerrNominas = new List<Nomimas>();
        List<Nomimas> selectEditables = new List<Nomimas>();
        List<Nomimas> selectCerradas = new List<Nomimas>();
        List<string> editStrings = new List<string>();
        List<string> cerrStrings = new List<string>();

        int position = -1;
        int position2 = -1;
        int position3 = -1;
        public GestionNominas()
        {
            InitializeComponent();
            nomimN = new Nomimas();
            this.DataContext = nomimN;
           
            cargarDataItems();
        }

        private void cargarDataItems()
        {
            var url = " http://localhost:3000/staff";
            var url2 = " http://localhost:3000/nominas";

            WebClient wc = new WebClient();
            WebClient wc2 = new WebClient();

            var datos = wc.DownloadString(url);
            var datos2 = wc2.DownloadString(url2);

            List<Staff> staf = JsonConvert.DeserializeObject<List<Staff>>(datos);
            List<Nomimas> noms = JsonConvert.DeserializeObject<List<Nomimas>>(datos2);
           

            Staff sta;
            Nomimas non;

            for (int j=0; j<staf.Count; j++)
            {   // string activo, bool alta, string dni, string domicilio, 
                //string email, string id, int intentos, string name, string numeroCuenta,
                // string password, string rango, int telefono, string user
                sta = new Staff(staf[j].activo, staf[j].alta, staf[j].dni, staf[j].domicilio,
                    staf[j].email, staf[j].id, staf[j].intentos, staf[j].name, staf[j].numeroCuenta,
                    staf[j].password, staf[j].rango, staf[j].telefono, staf[j].numeroCuenta);
                staffs.Add(sta);
                
            }
            for (int g=0; g<noms.Count; g++)
            {   // int anyo, bool cerrada, double horasExtras, double horasOrdinarias, string idStaff,
                // int mes, double precioExtras, double precioOrdinaria, double total, string nombre
                non = new Nomimas(noms[g].idNomina,noms[g].anyo, noms[g].cerrada, noms[g].horasExtras, noms[g].horasOrdinarias, noms[g].idStaff,
                   noms[g].mes, noms[g].precioExtras, noms[g].precioOrdinaria, noms[g].total, noms[g].nombre);
                nomimas.Add(non);
            }
            
            for (int t=0; t<nomimas.Count; t++)
            {
                for (int h=0; h<staffs.Count; h++)
                {
                   
                    if (nomimas[t].idStaff.Equals(staffs[h].id))
                    {
                        nomimas[t].nombre = staffs[h].name;
                    }
                }
            }
            for(int y=0;y<nomimas.Count; y++)
            {
                if (nomimas[y].cerrada == true)
                {
                    cerrNominas.Add(nomimas[y]);
                }
                else
                {
                    editNominas.Add(nomimas[y]);
                }
            }
          
            for (int h = 0; h < staffs.Count; h++)
            {
                comboName.Items.Add(staffs[h].name);
            }
            for (int p=0; p < nomimas.Count; p++)
            {
                if (nomimas[p].cerrada == false)
                {
                    editStrings.Add(nomimas[p].anyo.ToString() + nomimas[p].horasExtras.ToString() + nomimas[p].horasOrdinarias + 
                   nomimas[p].mes + nomimas[p].precioExtras + nomimas[p].precioOrdinaria + nomimas[p].total+ nomimas[p].nombre.ToLower());
                  
                }
                else
                {
                    cerrStrings.Add(nomimas[p].anyo.ToString() + nomimas[p].horasExtras.ToString() + nomimas[p].horasOrdinarias + 
                   nomimas[p].mes + nomimas[p].precioExtras + nomimas[p].precioOrdinaria + nomimas[p].total + nomimas[p].nombre.ToLower());
                }
            }
            DataNominasEditables.ItemsSource = editNominas;
            DataNominasCerradas.ItemsSource = cerrNominas;
        }

        private void DataNominasEditable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            position = DataNominasEditables.SelectedIndex;
            
            if (selectEditables.Count == 0 & selectCerradas.Count == 0)
            {
                if (position > -1)
                { 
                    comboName.Text=(editNominas[position].nombre.ToString());
                    comboAnyo.Text=(editNominas[position].anyo.ToString());
                    comboMes.Text = (editNominas[position].mes.ToString());
                    txtHoraExtra.Text = editNominas[position].horasExtras.ToString();
                    txtHoraOrd.Text = editNominas[position].horasOrdinarias.ToString();
                    txtPrecioHoraExtra.Text = editNominas[position].precioExtras.ToString();
                    txtPrecioHora.Text = editNominas[position].precioOrdinaria.ToString();
                    txtTotal.Text = editNominas[position].total.ToString();

                    position2 = -1;
                     DataNominasCerradas.SelectedIndex = -1;
                }
            }
            else
            {
                if (position > -1)
                {
                    comboName.Text = (selectEditables[position].nombre.ToString());
                    comboAnyo.Text = (selectEditables[position].anyo.ToString());
                    comboMes.Text = (selectEditables[position].mes.ToString());
                    txtHoraExtra.Text = selectEditables[position].horasExtras.ToString();
                    txtHoraOrd.Text = selectEditables[position].horasOrdinarias.ToString();
                    txtPrecioHoraExtra.Text = selectEditables[position].precioExtras.ToString();
                    txtPrecioHora.Text = selectEditables[position].precioOrdinaria.ToString();
                    txtTotal.Text = selectEditables[position].total.ToString();

                    position2 = -1;
                    DataNominasCerradas.SelectedIndex = -1;
                    position3 = position;
                }
            }
        }
        private void DataBajas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            position2 = DataNominasCerradas.SelectedIndex;

            if (selectEditables.Count == 0 & selectCerradas.Count == 0)
            {
                if (position2 > -1)
                {
                    comboName.Text = (cerrNominas[position2].nombre.ToString());
                    comboAnyo.Text = (cerrNominas[position2].anyo.ToString());
                    comboMes.Text = (cerrNominas[position2].mes.ToString());
                    txtHoraExtra.Text = cerrNominas[position2].horasExtras.ToString();
                    txtHoraOrd.Text = cerrNominas[position2].horasOrdinarias.ToString();
                    txtPrecioHoraExtra.Text = cerrNominas[position2].precioExtras.ToString();
                    txtPrecioHora.Text = cerrNominas[position2].precioOrdinaria.ToString();
                    
                    txtTotal.Text = cerrNominas[position2].total.ToString();

                    position = -1;
                    DataNominasEditables.SelectedIndex = -1;
                }
            }
            else
            {
                if (position2 > -1)
                {
                    comboName.Text = (selectCerradas[position2].nombre.ToString());
                    comboAnyo.Text = (selectCerradas[position2].anyo.ToString());
                    comboMes.Text = (selectCerradas[position2].mes.ToString());
                    txtHoraExtra.Text = selectCerradas[position2].horasExtras.ToString();
                    txtHoraOrd.Text = selectCerradas[position2].horasOrdinarias.ToString();
                    txtPrecioHoraExtra.Text = selectCerradas[position2].precioExtras.ToString();
                    txtPrecioHora.Text = selectCerradas[position2].precioOrdinaria.ToString();
                    txtTotal.Text = selectCerradas[position2].total.ToString();

                    position = -1;
                    DataNominasEditables.SelectedIndex = -1;
                }
            }
        }

        private void TxtSearch_SelectionChanged(object sender, RoutedEventArgs e)
        {
            selectEditables.Clear();
            selectCerradas.Clear();

            string d = TxtSearch.Text;
            string x = d.ToLower();
            if (d.Length > 0)
            {
                DataNominasEditables.ItemsSource = null;
                DataNominasCerradas.ItemsSource = null;

                for (int n = 0; n < editStrings.Count; n++)
                {
                    if (editStrings[n].Contains(x) & editNominas[n].cerrada == false)
                    {
                        selectEditables.Add(editNominas[n]);
                    }
                }
                for (int n = 0; n < cerrStrings.Count; n++)
                {
                    if (cerrStrings[n].Contains(x) & cerrNominas[n].cerrada == true)
                    {
                        selectCerradas.Add(cerrNominas[n]);
                    }
                }
                DataNominasEditables.ItemsSource = selectEditables;
                DataNominasCerradas.ItemsSource = selectCerradas;
            }
            else
            {
                DataNominasEditables.ItemsSource = editNominas;
                DataNominasCerradas.ItemsSource = cerrNominas;
            }
        }

        private void btnAltaNomina(object sender, RoutedEventArgs e)
        {
            int counter =-1;
           
            if (comboMes.Text==""||comboAnyo.Text==""||comboName.Text==""||txtHoraExtra.Text==""
                || txtHoraOrd.Text == "" || txtPrecioHora.Text == "" || txtPrecioHoraExtra.Text == "")
            {
                MessageBox.Show("POR FAVOR, RELLENA TODOS LOS CAMPOS EXCEPTO EL DEL TOTAL", "Atención", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
               try
                {
                    double hora = Convert.ToDouble(txtHoraOrd.Text);
                    double horaExtra = Convert.ToDouble(txtHoraExtra.Text);
                    double precioHora = Convert.ToDouble(txtPrecioHora.Text);
                    double precioHoraExtra = Convert.ToDouble(txtPrecioHoraExtra.Text);
                    System.DateTime now = DateTime.Today;
                    DateTime date1 = new DateTime(now.Year, now.Month, 15);
                    DateTime date2 = new DateTime(Int32.Parse(comboAnyo.Text), Int32.Parse(comboMes.Text), 15);
                    int result = DateTime.Compare(date1, date2);
                    double total = 0f;
                    total = ((hora * precioHora) + (horaExtra * precioHoraExtra));
                    txtTotal.Text = total.ToString();

                if (result <= 0)
                {
                        for (int n = 0; n < nomimas.Count; n++)
                        {
                            if (nomimas[n].mes.Equals(comboMes.Text) && nomimas[n].anyo.Equals(comboAnyo.Text) && nomimas[n].nombre.Equals(comboName.Text))
                            {
                                counter = n;

                            }

                        }
                            if (counter > -1)
                            {
                                var Result = MessageBox.Show("ATENCION: YA HAY UNA NÓMINA CREADA PARA ESE USUARIO EN ESE MES\n" +
                                     "¿ESTÁ SEGURO DE DARLA DE ALTA?", "Atención", MessageBoxButton.YesNo, MessageBoxImage.Question);
                                if (Result == MessageBoxResult.Yes)
                                {
                                    
                                    AltaEditNomina(nomimas.Count, (nomimas.Count + 101).ToString(), Int32.Parse(comboAnyo.Text), false, Convert.ToDouble(txtHoraExtra.Text),
                                    Convert.ToDouble(txtHoraOrd.Text), nomimas[counter].idStaff, Int32.Parse(comboMes.Text), Convert.ToDouble(txtPrecioHoraExtra.Text),
                                    Convert.ToDouble(txtPrecioHora.Text), Convert.ToDouble(txtTotal.Text));
                                    limpiarArrays();
                                    cargarDataItems();

                                }

                            }
                            else
                            {
                                int idST = 0;
                                for (int h=0; h < staffs.Count; h++)
                                {
                                    if (comboName.Text.Equals(staffs[h].name)){
                                        idST = h;
                                    }
                                }
                                AltaEditNomina(nomimas.Count, (nomimas.Count + 101).ToString(), Int32.Parse(comboAnyo.Text), false, Convert.ToDouble(txtHoraExtra.Text),
                                        Convert.ToDouble(txtHoraOrd.Text), nomimas[idST].idStaff, Int32.Parse(comboMes.Text), Convert.ToDouble(txtPrecioHoraExtra.Text),
                                        Convert.ToDouble(txtPrecioHora.Text), Convert.ToDouble(txtTotal.Text));
                                limpiarArrays();
                                cargarDataItems();

                            }

                        
                }
                else
                    MessageBox.Show("EL MES DE LA NÓMINA DEBE SER MAYOR O IGUAL AL MES ACTUAL ", "Atención", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
                catch(Exception ex)
                {
                    MessageBox.Show("ALGUNO DE LOS CAMPOS NUMÉRICOS NO ES CORRECTO");
                    Console.WriteLine(ex);
                }
            }      
        }

        private void AltaEditNomina( int count, string idNomina1, int anyo1, bool cerrada1, double horasExtras1, double horasOrdinarias1, string idStaff1,
            int mes1, double precioExtras1, double precioOrdinaria1, double total1)
        {
   
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(" http://localhost:3000/nominas/" + count);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            string json = "";
            var data = new
            {
                idNomina=idNomina1,
                anyo = anyo1,
                cerrada = cerrada1,
                horasExtras = horasExtras1,
                horasOrdinarias = horasOrdinarias1,
                idStaff = idStaff1,
                mes = mes1,
                precioExtras = precioExtras1,
                precioOrdinaria = precioOrdinaria1,
                total = total1
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
        private void limpiarArrays()
        {
       
            staffs.Clear();
            nomimas.Clear();
            editNominas.Clear();
            cerrNominas.Clear();
            selectCerradas.Clear();
            selectEditables.Clear();
            editStrings.Clear();
            cerrStrings.Clear();

            DataNominasCerradas.ItemsSource = null;
            DataNominasEditables.ItemsSource = null;
        }

        private void btnEdidionNomina(object sender, RoutedEventArgs e)
        {

            string idNom ="";
            int select = -1;
            int t = 0;
            if (selectEditables.Count > 0&&DataNominasEditables.SelectedIndex>-1)
            {
                
                idNom = (selectEditables[DataNominasEditables.SelectedIndex].idNomina);
                for (t=0; t<nomimas.Count; t++)
                {
                    if (idNom.Equals(nomimas[t].idNomina))
                    {
                        select = t;
                    }
                }
                try
                {
                    double hora = Convert.ToDouble(txtHoraOrd.Text);
                    double horaExtra = Convert.ToDouble(txtHoraExtra.Text);
                    double precioHora = Convert.ToDouble(txtPrecioHora.Text);
                    double precioHoraExtra = Convert.ToDouble(txtPrecioHoraExtra.Text);
                    System.DateTime now = DateTime.Today;
                    DateTime date1 = new DateTime(now.Year, now.Month, 15);
                    DateTime date2 = new DateTime(Int32.Parse(comboAnyo.Text), Int32.Parse(comboMes.Text), 15);
                    int result = DateTime.Compare(date1, date2);
                    double total = 0f;
                    total = ((hora * precioHora) + (horaExtra * precioHoraExtra));
                    txtTotal.Text = total.ToString();

                    if (result <= 0)
                    {
                        var Result = MessageBox.Show("ATENCION: SE VA A MODIFICAR UNA NÓMINA\n" +
                                     "¿ESTÁ SEGURO DE LOS DATOS INTRODUCIDOS?", "Atención", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (Result == MessageBoxResult.Yes)
                        {
                            AltaEditNomina(select, idNom, Int32.Parse(comboAnyo.Text), false, horaExtra,
                                            hora, nomimas[select].idStaff, Int32.Parse(comboMes.Text), precioHoraExtra,
                                            precioHora, total);
                            limpiarArrays();
                            cargarDataItems();
                        }
                    }
                    else
                    {
                        MessageBox.Show("EL MES DE LA NÓMINA DEBE SER MAYOR O IGUAL AL MES ACTUAL ", "Atención", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }catch(Exception ex)
                {
                    MessageBox.Show("ALGUNO DE LOS CAMPOS NUMÉRICOS NO ES CORRECTO");
                    Console.WriteLine(ex);
                }
                }
            else if(DataNominasEditables.SelectedIndex>-1)
            {
                
                idNom = (editNominas[DataNominasEditables.SelectedIndex].idNomina);
                for (t = 0; t < nomimas.Count; t++)
                {
                    if (idNom.Equals(nomimas[t].idNomina))
                    {
                        select = t;
                    }
                }
                try
                {
                    double hora = Convert.ToDouble(txtHoraOrd.Text);
                    double horaExtra = Convert.ToDouble(txtHoraExtra.Text);
                    double precioHora = Convert.ToDouble(txtPrecioHora.Text);
                    double precioHoraExtra = Convert.ToDouble(txtPrecioHoraExtra.Text);
                    System.DateTime now = DateTime.Today;
                    DateTime date1 = new DateTime(now.Year, now.Month, 15);
                    DateTime date2 = new DateTime(Int32.Parse(comboAnyo.Text), Int32.Parse(comboMes.Text), 15);
                    int result = DateTime.Compare(date1, date2);
                    double total = 0f;
                    total = ((hora * precioHora) + (horaExtra * precioHoraExtra));
                    txtTotal.Text = total.ToString();

                    if (result <= 0)
                    {
                        var Result = MessageBox.Show("ATENCION: SE VA A MODIFICAR UNA NÓMINA\n" +
                                    "¿ESTÁ SEGURO DE LOS DATOS INTRODUCIDOS?", "Atención", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (Result == MessageBoxResult.Yes)
                        {
                            AltaEditNomina(select, idNom, Int32.Parse(comboAnyo.Text), false, horaExtra,
                                            hora, nomimas[select].idStaff, Int32.Parse(comboMes.Text), precioHoraExtra,
                                            precioHora, total);
                            TxtSearch.Text = "";
                            limpiarArrays();
                            cargarDataItems();
                        }
                    }
                    else
                    {
                        MessageBox.Show("EL MES DE LA NÓMINA DEBE SER MAYOR O IGUAL AL MES ACTUAL ", "Atención", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ALGUNO DE LOS CAMPOS NUMÉRICOS NO ES CORRECTO");
                    Console.WriteLine(ex);
                }
            
        }
            else
            {
                MessageBox.Show("ATENCIÓN: No se puede editar una nómina cerrada" ,"Atencion", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void btnCerrarNonima(object sender, RoutedEventArgs e)
        {
            string idNom = "";
            int select = -1;
            int t = 0;
            if (selectEditables.Count > 0 && DataNominasEditables.SelectedIndex > -1)
            {

                idNom = (selectEditables[DataNominasEditables.SelectedIndex].idNomina);
                for (t = 0; t < nomimas.Count; t++)
                {
                    if (idNom.Equals(nomimas[t].idNomina))
                    {
                        select = t;
                    }
                }
                
                double hora = Convert.ToDouble(txtHoraOrd.Text);
                double horaExtra = Convert.ToDouble(txtHoraExtra.Text);
                double precioHora = Convert.ToDouble(txtPrecioHora.Text);
                double precioHoraExtra = Convert.ToDouble(txtPrecioHoraExtra.Text);
                
                
        
                if (comboName.Text==(nomimas[select].nombre) && comboAnyo.Text==(nomimas[select].anyo.ToString())&&
                    comboMes.Text==(nomimas[select].mes.ToString())&& hora == (nomimas[select].horasOrdinarias)&&
                    precioHora == (nomimas[select].precioOrdinaria) && horaExtra == (nomimas[select].horasExtras)&&
                     precioHoraExtra == (nomimas[select].precioExtras))
                {
                    MessageBox.Show(horaExtra.ToString());
                    AltaEditNomina(select, nomimas[select].idNomina, nomimas[select].anyo, true, nomimas[select].horasExtras,
                                           nomimas[select].horasOrdinarias, nomimas[select].idStaff, nomimas[select].mes, nomimas[select].precioExtras,
                                           nomimas[select].precioOrdinaria, nomimas[select].total);
                    TxtSearch.Text = "";
                    limpiarArrays();
                    cargarDataItems();

                }
                else
                {
                    MessageBox.Show("NO SE PUEDEN EDITAR CAMPOS. SI DESEA EDITAR UTILICE\nEL BOTÓN DE EDICIÓN ", "Atencion", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (DataNominasEditables.SelectedIndex > -1)
            {

                idNom = (editNominas[DataNominasEditables.SelectedIndex].idNomina);
                for (t = 0; t < nomimas.Count; t++)
                {
                    if (idNom.Equals(nomimas[t].idNomina))
                    {
                        select = t;
                    }
                }
                double hora = Convert.ToDouble(txtHoraOrd.Text);
                double horaExtra = Convert.ToDouble(txtHoraExtra.Text);
                double precioHora = Convert.ToDouble(txtPrecioHora.Text);
                double precioHoraExtra = Convert.ToDouble(txtPrecioHoraExtra.Text);
                if (comboName.Text == (nomimas[select].nombre) && comboAnyo.Text == (nomimas[select].anyo.ToString()) &&
                    comboMes.Text == (nomimas[select].mes.ToString()) && hora == (nomimas[select].horasOrdinarias) &&
                    precioHora == (nomimas[select].precioOrdinaria) && horaExtra == (nomimas[select].horasExtras) &&
                     precioHoraExtra == (nomimas[select].precioExtras))
                {
                   
                    AltaEditNomina(select, nomimas[select].idNomina, nomimas[select].anyo, true, nomimas[select].horasExtras,
                                           nomimas[select].horasOrdinarias, nomimas[select].idStaff, nomimas[select].mes, nomimas[select].precioExtras,
                                           nomimas[select].precioOrdinaria, nomimas[select].total);
                    TxtSearch.Text = "";
                    limpiarArrays();
                    cargarDataItems();

                }
                else
                {
                    MessageBox.Show("NO SE PUEDEN EDITAR CAMPOS. SI DESEA EDITAR UTILICE\nEL BOTÓN DE EDICIÓN ", "Atencion", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            else
            {
                MessageBox.Show("Esa nómina ya está cerrada", "Atencion", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btnGestTiket_click(object sender, RoutedEventArgs e)
        {
            GestionTickets gtTikets = new GestionTickets();
            gtTikets.Show();
            this.Close();
        }

        private void btnGestPersonal_click(object sender, RoutedEventArgs e)
        {
            Gestion_personal gtPersonal = new Gestion_personal();
            gtPersonal.Show();
            this.Close();
        }

        private void btnGestArti_click(object sender, RoutedEventArgs e)
        {
            Articulos gtArt = new Articulos();
            gtArt.Show();
            this.Close();
        }

        private void btnAppBarra_click(object sender, RoutedEventArgs e)
        {
            VistaPedidos win = new VistaPedidos();
            win.Show();
            this.Close();
        }

        private void btnGestMesas_click(object sender, RoutedEventArgs e)
        {
            GestionMesas gtMesas = new GestionMesas();
            gtMesas.Show();
            this.Close();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mn = new MainWindow();
            mn.Show();
            this.Close();
        }
    }
}

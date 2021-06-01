using Newtonsoft.Json;
using proyecto_admin.Vistas;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Input;

namespace proyecto_admin
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    { 
        
        string name = "";
        string pasword = "";
       
        List<Staff> staff = new List<Staff>();
        
        int intentos = 0;

        public MainWindow()
        {
            InitializeComponent();
            cargarDatos();
            txtUser.Focus();
        }   
        private void btnClickSignIn(object sender, RoutedEventArgs e)
        {
            checkNamePassword();
            
        }

        private void checkNamePassword()
        {
            name = txtUser.Text;
            pasword = txtPasword.Password;
            int counterName = 0;
            int n = 0;
            if (name == "" || pasword == "")
            {
                MessageBox.Show("LOS CAMPOS USUARIO Y CONTRASEÑA SON OBLIGATORIOS"
                           , "Atención", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                for (int pos = 0; pos < staff.Count; pos++)
                {

                    if (name.Equals(staff[pos].user))
                    {

                        counterName++;
                        n = pos;
                    }
                }
                if (counterName == 0)
                {
                    intentos++;
                    //metodo intentos no user
                    warningNoUser(intentos);
                }
                if (counterName > 0)
                {
                    if (name.Equals(staff[n].user) && staff[n].alta == false)
                    {
                        intentos++;
                        warningNoUser(intentos);
                    }
                    if (name.Equals(staff[n].user) && staff[n].alta == true && staff[n].intentos >= 3)
                    {
                        MessageBox.Show("ESTE USUARIO HA SIDO BLOQUEADO. POR FAVOR, PÓNGASE EN\n" +
                          "CONTACTO CON SU ADMINISTRADOR"
                           , "Atención", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        this.Close();
                    }
                    if (name.Equals(staff[n].user) && staff[n].alta == true && staff[n].intentos < 3 && !pasword.Equals(staff[n].password))
                    {

                        if (staff[n].rango == "admin")
                        {
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("CONTRASEÑA INCORRECTA. QUEDAN " + (3 - (staff[n].intentos + 1)) + " INTENTOS."
                              , "Atención", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            //post para aumentar intentos del user en bbdd
                            postStaff(n, staff[n].activo, staff[n].intentos + 1, staff[n].dni, staff[n].rango, staff[n].alta,
                                staff[n].domicilio, staff[n].email, staff[n].name, staff[n].numeroCuenta, staff[n].password,
                                staff[n].telefono, staff[n].user, staff[n].id);
                            staff.Clear();
                            cargarDatos();
                        }
                    }
                    if (name.Equals(staff[n].user) && staff[n].alta == true && staff[n].intentos < 3 && pasword.Equals(staff[n].password))
                    {
                        MessageBox.Show("Login realizado correctamente"
                          , "Atención", MessageBoxButton.OK, MessageBoxImage.Information);
                        if (staff[n].rango == "admin")
                        {
                            Window1 win = new Window1();
                            win.Show();
                            this.Close();
                        }
                        else
                        {
                            VistaPedidos win2 = new VistaPedidos();
                            win2.Show();
                            this.Close();
                        }
                    }
                }
            }
        }

        private void postStaff(int count, string activo1, int intentos1, string dni1, string rango1, bool v1, string domicilio1,
           string email1, string name1, string numeroCuenta1, string password1, int tel1, string user1, string id1)
        {
            if (intentos1 > 2)
            {
                activo1 = "NO";
            }
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(" http://localhost:3000/Staff/" + count);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            string json = "";
            var data = new
            {
                intentos = intentos1,
                activo = activo1,
                dni = dni1,
                rango = rango1,
                alta = v1,
                domicilio = domicilio1,
                email = email1,
                name = name1,
                numeroCuenta = numeroCuenta1,
                password = password1,
                telefono = tel1,
                user = user1,
                id = id1
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

        private void warningNoUser(int intentos)
        {
            if (intentos < 4)
            {
                MessageBox.Show("EL USUARIO O LA CONTRASEÑA INTRODUCIDOS NO SON CORRECTOS.\n" +
               "QUEDAN " + (3 - intentos) + " INTENTOS"
                , "Atención", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                txtPasword.Clear();
            }
            else
                this.Close();
        }

        private void cargarDatos()
        {
           
            var url = " http://localhost:3000/staff";

            WebClient wc = new WebClient();

            var datos = wc.DownloadString(url);

            List<Staff> sts = JsonConvert.DeserializeObject<List<Staff>>(datos);

            Staff st;
            //bool alta, string password, int intentos, string user, string rango
            foreach (var person in sts)
            {
                st = new Staff(person.activo, person.alta, person.dni, person.domicilio, person.email,
                   person.id, person.intentos, person.name, person.numeroCuenta, person.password,
                   person.rango, person.telefono, person.user);
                staff.Add(st);
            }
        }

        private void PasswordKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                checkNamePassword();
            }

        }
    }
}

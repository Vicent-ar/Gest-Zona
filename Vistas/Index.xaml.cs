using proyecto_admin.Vistas;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace proyecto_admin
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void btnGestPers(object sender, RoutedEventArgs e)
        {
            Gestion_personal gestion_Personal = new Gestion_personal();
            gestion_Personal.Show();
            this.Close();
        }

        private void btnBarra(object sender, RoutedEventArgs e)
        {
            VistaPedidos window2 = new VistaPedidos();
            window2.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Articulos art = new Articulos();
            art.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            GestionTickets gest = new GestionTickets();
            gest.Show();
            this.Close();
        }

        private void btnGestNominas(object sender, RoutedEventArgs e)
        {
            GestionNominas nom = new GestionNominas();
            nom.Show();
            this.Close();
        }

        private void btnMesas(object sender, RoutedEventArgs e)
        {
            GestionMesas gtMesas = new GestionMesas();
            gtMesas.Show();
            this.Close();

        }
    }
}

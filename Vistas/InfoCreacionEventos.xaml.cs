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

namespace proyecto_admin.Vistas
{
    /// <summary>
    /// Lógica de interacción para InfoCreacionEventos.xaml
    /// </summary>
    public partial class InfoCreacionEventos : Window
    {
        public InfoCreacionEventos()
        {
            InitializeComponent();
            WindowStartupLocation=
            System.Windows.WindowStartupLocation.CenterScreen;
        }

        private void btnInfo2_Click(object sender, RoutedEventArgs e)
        {
            InfoCreacionZonas zonas = new InfoCreacionZonas();
            zonas.ShowDialog();
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

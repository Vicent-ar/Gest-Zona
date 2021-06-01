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

namespace proyecto_admin.Vistas.AyudaGestEvento
{
    /// <summary>
    /// Lógica de interacción para Ayuda5.xaml
    /// </summary>
    public partial class Ayuda5 : Window
    {
        public Ayuda5()
        {
            InitializeComponent();
        }

        private void btnDrcha_Click(object sender, RoutedEventArgs e)
        {
            
            Ayuda4 ayuda4 = new Ayuda4();
            ayuda4.Show();
            this.Close();
        }

        private void btnIzda_Click(object sender, RoutedEventArgs e)
        {
            Ayuda6 ayuda6 = new Ayuda6();
            ayuda6.Show();
            this.Close();
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

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
    /// Lógica de interacción para Ayuda7.xaml
    /// </summary>
    public partial class Ayuda7 : Window
    {
        public Ayuda7()
        {
            InitializeComponent();
        }

        private void btnDrcha_Click(object sender, RoutedEventArgs e)
        {
            Ayuda6 ayuda6 = new Ayuda6();
            ayuda6.Show();
            this.Close();
        }

        private void btnIzda_Click(object sender, RoutedEventArgs e)
        {
            
            Ayuda8 ayuda8 = new Ayuda8();
            ayuda8.Show();
            this.Close();
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

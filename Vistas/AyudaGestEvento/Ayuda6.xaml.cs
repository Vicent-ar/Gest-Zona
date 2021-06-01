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
    /// Lógica de interacción para Ayuda6.xaml
    /// </summary>
    public partial class Ayuda6 : Window
    {
        public Ayuda6()
        {
            InitializeComponent();
        }

        private void btnDrcha_Click(object sender, RoutedEventArgs e)
        {
            Ayuda5 ayuda5 = new Ayuda5();
            ayuda5.Show();
            this.Close();
        }

        private void btnIzda_Click(object sender, RoutedEventArgs e)
        {
            
            Ayuda7 ayuda7 = new Ayuda7();
            ayuda7.Show();
            this.Close();
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

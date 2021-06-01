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
    /// Lógica de interacción para Ayuda1.xaml
    /// </summary>
    public partial class Ayuda1 : Window
    {
        public Ayuda1()
        {
            InitializeComponent();
        }

        private void btnIzda_Click(object sender, RoutedEventArgs e)
        {
            Ayuda2 ayuda2 = new Ayuda2();
            ayuda2.Show();
            this.Close();
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

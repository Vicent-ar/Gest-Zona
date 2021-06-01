using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace proyecto_admin.Vistas
{
    /// <summary>
    /// Lógica de interacción para InfoCreacionZonas.xaml
    /// </summary>
    public partial class InfoCreacionZonas : Window
    {
        public InfoCreacionZonas()
        {
            InitializeComponent();
            WindowStartupLocation =
            System.Windows.WindowStartupLocation.CenterScreen;
        }

        private void btnCancelInfo_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
        }
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e) { Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri)); e.Handled = true; }

    }
}

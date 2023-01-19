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

namespace Dragger_WPF
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
       
        public  string CodiResponsable;
        public  string NomResponsable;
      


        public Window1()
        {
            InitializeComponent();
           

        }
        private void Sortir(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
        private void tornar(object sender, RoutedEventArgs e)
        {
            CodiResponsable = codiF.Text;
            NomResponsable = nomF.Text;
            DialogResult = true;
  
            Close();
        }
        
    }
}

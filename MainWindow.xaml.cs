﻿using Dragger_WPF;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dragger_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void kanbanView(object sender, RoutedEventArgs e)
        {
            kanbanBoard.Width = new GridLength(1, GridUnitType.Star);
            responsable.Width = new GridLength(0, GridUnitType.Star);
            addButton.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            kanbanBoard.Width = new GridLength(0, GridUnitType.Star);
            responsable.Width = new GridLength(1, GridUnitType.Star);
            addButton.Visibility = Visibility.Collapsed;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
          
        }

        void Editar(object sender, RoutedEventArgs e)
        {
            Window1 formulari = new Window1();
           
            bool resultat = (bool)formulari.ShowDialog();

        }

        void Eliminar(object sender, RoutedEventArgs e)
        {
          
            
        }
    }
}

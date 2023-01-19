using Dragger_WPF.Entity;
using Dragger_WPF.Persistence;
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
using Dragger_WPF.Service;
using System.Reflection;

namespace Dragger_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Card> cards = new List<Card>();

        public MainWindow()
        {
            InitializeComponent();
            DbContext.Up();
        }

        private void kanbanView(object sender, RoutedEventArgs e)
        {
            if (addButton.Visibility == Visibility.Collapsed)
            {
                kanbanBoard.Width = new GridLength(1, GridUnitType.Star);
                responsable.Width = new GridLength(0, GridUnitType.Star);
                addButton.Visibility = Visibility.Visible;
                addButtonRes.Visibility = Visibility.Collapsed;
            }

        }

        private void responsableView(object sender, RoutedEventArgs e)
        {
            if (addButtonRes.Visibility == Visibility.Collapsed)
            {
                kanbanBoard.Width = new GridLength(0, GridUnitType.Star);
                responsable.Width = new GridLength(1, GridUnitType.Star);
                addButton.Visibility = Visibility.Collapsed;
                addButtonRes.Visibility = Visibility.Visible;
            }

        }

        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            Card card = new Card();

            card._id_persona = 3;
            card._id_card = 05;
            card._description = "ALT DESCR";
            card._creationDate = new DateOnly(2022, 12, 1);
            card._color = "red";
            card._priority = 3;
            card._goalDate = new DateOnly(2023, 1, 1);
            card._position = 1;

            cards.Add(card);

            Border border = new Border();
            border.Width = 540;
            border.Height = 300;
            border.CornerRadius = new CornerRadius(8);
            border.Margin = new Thickness(20);

            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = Colors.White;
            brush.Opacity = 0.2;
            border.Background = brush;

            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Vertical;
            stackPanel.VerticalAlignment = VerticalAlignment.Center;
            stackPanel.HorizontalAlignment = HorizontalAlignment.Center;

            Label label1 = new Label();
            label1.Content = card._id_card;
            label1.FontSize = 25;
            stackPanel.Children.Add(label1);

            Label label2 = new Label();
            label2.Content = card._id_persona;
            label2.FontSize = 25;
            stackPanel.Children.Add(label2);

            Label label3 = new Label();
            label3.Content = card._description;
            label3.FontSize = 25;
            stackPanel.Children.Add(label3);

            Label label4 = new Label();
            label4.Content = card._creationDate;
            label4.FontSize = 25;
            stackPanel.Children.Add(label4);

            Label label5 = new Label();
            label5.Content = card._goalDate;
            label5.FontSize = 25;
            stackPanel.Children.Add(label5);

            // Add the remaining labels in a similar manner

            border.Child = stackPanel;

            //DEPENDENCY PROPERTY

            if (card._position == 1)
            {
                stackTODO.Children.Add(border);
            }
            else if (card._position == 2)
            {
                stackDOING.Children.Add(border);
            }
            else if (card._position == 3)
            {
                stackDONE.Children.Add(border);
            }
            else
                MessageBox.Show("SOMETHING WENT WRONG", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void addButtonRes_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Responsable");
        }


        private void SaveCards()
        {
            foreach (Card card in cards) {
                DbContext.InsertCard(card);
            }
        }


        //private void ReadCards()
        //{
        //    List<Card> redCards = (List<Card>)CardService.GetAll();
        //    foreach (Card card in redCards)
        //    {
        //        if (card._position == 1)
        //        {
        //            stackTODO.Children.Add();
        //        }
        //        else if (card._position == 2)
        //        {
        //            stackDOING.Children.Add();
        //        }
        //        else if (card._position == 3)
        //        {
        //            stackDONE.Children.Add();
        //        }
        //        else
        //            MessageBox.Show("SOMETHING WENT WRONG", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveCards();          
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

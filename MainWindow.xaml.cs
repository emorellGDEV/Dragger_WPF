using Dragger_WPF.Entity;
using Dragger_WPF.Persistence;
using Dragger_WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Dragger_WPF.Service;
using RandomNameGeneratorLibrary;
using System.Reflection;
using Dragger_WPF.UserControls;

namespace Dragger_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Card> cards = new List<Card>();
        List<Person> persons = new List<Person>();

        public MainWindow()
        {
            InitializeComponent();
            DbContext.Up();
            ReadCards();
            ReadPersons();
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
            DateTime now = DateTime.Now;
            String nowFormat = now.ToString("dd/MM/yyyy");
            card._creationDate = Convert.ToDateTime(nowFormat);
            card._id_card = DbContext.SelectMaxCard() + 1;
            card._id_persona = 0;
            card._goalDate = Convert.ToDateTime(nowFormat);
            card._position = 1;
            card._color = string.Empty;
            card._priority= 0;
            card._description = "Notes";

            CardUserControl cardUser = new CardUserControl(card);
            stackTODO.Children.Add(cardUser);
            DbContext.InsertCard(card);
        }

        private void addButtonRes_Click(object sender, RoutedEventArgs e)
        {
            var rName = new PersonNameGenerator();
            Random r = new Random();
            Person person = new Person();
            person._id_person = (int)r.NextInt64(0, 1000);
            person._name = rName.GenerateRandomFirstName();
            persons.Add(person);

            Border border = new Border();
            border.CornerRadius = new CornerRadius(10);
            border.VerticalAlignment = VerticalAlignment.Top;
            border.BorderThickness = new Thickness(2);
            border.Width = 330;
            border.Height = 150;
            border.Margin = new Thickness(30, 30, 0, 0);
            border.Background = new SolidColorBrush(Colors.White);
            border.Opacity = 0.5;

            Grid grid = new Grid();
            RowDefinition row1 = new RowDefinition();
            RowDefinition row2 = new RowDefinition();
            grid.RowDefinitions.Add(row1);
            grid.RowDefinitions.Add(row2);

            TextBlock codi = new TextBlock();
            codi.Text = person._id_person.ToString();
            Grid.SetRow(codi, 0);
            codi.HorizontalAlignment = HorizontalAlignment.Left;
            codi.Margin = new Thickness(20, 10, 0, 0);
            grid.Children.Add(codi);



            TextBlock nom = new TextBlock();
            nom.Text = person._name;
            Grid.SetRow(nom, 0);
            nom.HorizontalAlignment = HorizontalAlignment.Right;
            nom.Margin = new Thickness(0, 10, 20, 0);
            grid.Children.Add(nom);

            Button add = new Button();
            add.Content = "change";
            Grid.SetRow(add, 1);
            add.HorizontalAlignment = HorizontalAlignment.Right;
            add.Margin = new Thickness(0, 0, 60, 0);
            grid.Children.Add(add);


            Button delete = new Button();
            delete.Content = "change";
            grid.Children.Add(delete);
            delete.HorizontalAlignment = HorizontalAlignment.Right;
            delete.Margin = new Thickness(0, 0, 10, 0);
            Grid.SetRow(delete, 1);

            border.Child = grid;

            wrapResponsable.Children.Add(border);



            //INSERT PERSON 
            DbContext.InsertPerson(person);


        }


        private void SaveCards()
        {
            foreach (Card card in cards)
            {
                DbContext.InsertCard(card);
            }
        }

        private void SavePersons()
        {
            foreach (Person person in persons)
            {
                DbContext.InsertPerson(person);
            }
        }

        private void ReadPersons()
        {
            List<Person> list = (List<Person>)PersonService.GetAll();
            
            foreach (Person person in list)
            {
                /*
                PersonUserControl redPerson = new PersonUserControl();
                wrapResponsable.Children.Add(redPerson.container);
                */

                persons.Add(person);
            }
        }


        private void ReadCards()
        {
            List<Card> redCards = (List<Card>)CardService.GetAll();
            foreach (Card red in redCards)
            {
                CardUserControl redUser = new CardUserControl(red);

                // Add the remaining labels in a similar manner
                if (red._position == 1)
                {
                    stackTODO.Children.Remove(redUser);
                }
                else if (red._position == 2)
                {
                    stackDOING.Children.Remove(redUser);
                }
                else if (red._position == 3)
                {
                    stackDONE.Children.Remove(redUser);
                }


                if (red._position == 1)
                {
                    stackTODO.Children.Add(redUser);
                }
                else if (red._position == 2)
                {
                    stackDOING.Children.Add(redUser);
                }
                else if (red._position == 3)
                {
                    stackDONE.Children.Add(redUser);
                }
                else
                    MessageBox.Show("SOMETHING WENT WRONG", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //SaveCards();
            //SavePersons();
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

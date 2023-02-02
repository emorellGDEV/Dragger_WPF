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
            int maxPerson = DbContext.SelectMaxPerson();
            if (maxPerson == 0)
            {
                MessageBox.Show("Crea un responsable!");
            }
            else
            {
                Card card = new Card();
                DateTime now = DateTime.Now;
                String nowFormat = now.ToString("dd/MM/yyyy");
                card._creationDate = Convert.ToDateTime(nowFormat);
                card._id_card = DbContext.SelectMaxCard() + 1;
                card._id_persona = maxPerson;
                card._goalDate = Convert.ToDateTime(nowFormat);
                card._position = 1;
                card._color = string.Empty;
                card._priority = 1;
                card._description = "Notes";

                CardUserControl cardUser = new CardUserControl(card);
                stackTODO.Children.Add(cardUser);
                DbContext.InsertCard(card);
            }
        }

        private void addButtonRes_Click(object sender, RoutedEventArgs e)
        {
            Person person = new Person();
            person._id_person = DbContext.SelectMaxPerson() + 1;
            person._name = "Nom";

            PersonUserControl personUser = new PersonUserControl(person);
            wrapResponsable.Children.Add(personUser);

            DbContext.InsertPerson(person);
        }

        private void ReadPersons()
        {
            List<Person> list = (List<Person>)PersonService.GetAll();

            foreach (Person person in list)
            {
                PersonUserControl redPerson = new PersonUserControl(person);
                wrapResponsable.Children.Add(redPerson);

                persons.Add(person);
            }
        }


        private void ReadCards()
        {
            List<Card> redCards = (List<Card>)CardService.GetAll();
            foreach (Card red in redCards)
            {
                CardUserControl redUser = new CardUserControl(red);
                redUser.checkPriority();

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

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
using System.Windows.Shapes;
using MongoDB.Driver.Linq;
using MongoDB.Bson;
using MongoDB.Driver;

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
            ReadCards();
            ReadPersons();
        }

        //Mostra el panel kanban i amaga el de responsables.
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

        //Mostra el panel de responsables i amaga el kanban
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

        //Afegeix una targeta a la columna TODO
        private async void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            int maxPerson = await DbContext.SelectMaxPerson();
            int maxCard = await DbContext.SelectMaxCard();
            //Comprova que almenys hi ha 1 responsable per la tasca
            if (maxPerson <= 0)
            {
                MessageBox.Show("No tens responsables, crea un i torna a intentar-ho.");
            }
            else
            {//Si hi ha responsables disponibles, instancia una tasca.
                var randomName = new PlaceNameGenerator();

                //Proporciona valors "dafult" a la tasca.
                Card card = new Card();
                DateTime now = DateTime.Now;
                String nowFormat = now.ToString("dd/MM/yyyy");
                card.creationDate = Convert.ToDateTime(nowFormat);
                card.id_card = maxCard + 1;
                card.fk_id_responsable = maxPerson;
                card.goalDate = Convert.ToDateTime(nowFormat);
                card.position = 1;
                card.priority = 1;
                card.description = randomName.GenerateRandomPlaceName();

                CardUserControl cardUser = new CardUserControl(card);
                //Inserta la tasca a l'stack panel i a la bdd.
                stackTODO.Children.Add(cardUser);
                DbContext.InsertCard(card);
            }

        }

        private async void addButtonRes_Click(object sender, RoutedEventArgs e)
        {
            //Funció similar a l'anterior, adaptada als responsables.
            var randomName = new PersonNameGenerator();
            Person person = new Person();
            person.id_person = await DbContext.SelectMaxPerson() + 1;
            person.name = randomName.GenerateRandomFirstName();

            PersonUserControl personUser = new PersonUserControl(person);
            wrapResponsable.Children.Add(personUser);

            DbContext.InsertPerson(person);
        }

        private void ReadPersons()
        {
            //Legeix els responsables, aquesta funció es llegeix al executar-se el programa.
            List<Person> list = (List<Person>)PersonService.GetAll();
            foreach (Person person in list)
            {
                PersonUserControl redPerson = new PersonUserControl(person);
                wrapResponsable.Children.Add(redPerson);
            }
        }


        private void ReadCards()
        {
            //Legeix les tasques, aquesta funció es llegeix al executar-se el programa.
            var redCards = (List<Card>)CardService.GetAll();

            //En cas de les tasques, ens em de fixar en el parametre "position" de cada tasca. Després, la col·loca 
            //en el seu stackpanel corresponent.
            foreach (Card red in redCards)
            {
                CardUserControl redUser = new CardUserControl(red);
                redUser.checkPriority();

                //Per evitar errors, els el·limina de l'stack panel.
                if (red.position == 1)
                {
                    stackTODO.Children.Remove(redUser);
                }
                else if (red.position == 2)
                {
                    stackDOING.Children.Remove(redUser);
                }
                else if (red.position == 3)
                {
                    stackDONE.Children.Remove(redUser);
                }

                //I després els posiciona.
                if (red.position == 1)
                {
                    stackTODO.Children.Add(redUser);
                }
                else if (red.position == 2)
                {
                    stackDOING.Children.Add(redUser);
                }
                else if (red.position == 3)
                {
                    stackDONE.Children.Add(redUser);
                }
                else
                    MessageBox.Show("SOMETHING WENT WRONG", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Les següents funcions es fan carreg del draganddrop, totes criden a la funció changePosition de l'objecte card.
        //Aquesta funció canvia el parametre posicio del objecte i de la bdd.

        //Drop en la columna DONE.
        void Done_drop(object sender, DragEventArgs e)
        {

            var obj = e.Data.GetData(typeof(CardUserControl)) as CardUserControl;

            ((StackPanel)obj.Parent).Children.Remove(obj);

            stackDONE.Children.Add(obj);
            obj.changePosition(3);
        }

        //Drop en la columna DOING.
        void Doing_drop(object sender, DragEventArgs e)
        {

            var obj = e.Data.GetData(typeof(CardUserControl)) as CardUserControl;

            ((StackPanel)obj.Parent).Children.Remove(obj);

            stackDOING.Children.Add(obj);
            obj.changePosition(2);

        }

        //Drop en la columna TODO.
        void Do_drop(object sender, DragEventArgs e)
        {

            var obj = e.Data.GetData(typeof(CardUserControl)) as CardUserControl;

            ((StackPanel)obj.Parent).Children.Remove(obj);

            stackTODO.Children.Add(obj);

            obj.changePosition(1);
        }


    }
}

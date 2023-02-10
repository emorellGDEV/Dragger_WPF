using Dragger_WPF.Entity;
using Dragger_WPF.Persistence;
using Dragger_WPF.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace Dragger_WPF.UserControls
{
    /// <summary>
    /// Lógica de interacción para PersonUserControl.xaml
    /// </summary>
    public partial class PersonUserControl : UserControl
    {
        //Els metodes d'aquest usercontrol son iguals o casi iguals als del cardusercontrol, adaptat a l'objecte Person.
        Person person;

        bool editing = false;
        public PersonUserControl(Person newPerson)
        {
            person = newPerson;
            InitializeComponent();

            codiResp.Text = person.id_person.ToString();
            nomResp.Text = person.name;
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            editing = true;
            if (editing)
            {
                border.Background = Brushes.PaleVioletRed;
                border.Opacity = 0.5;

                txtnom.Text = nomResp.Text;

                txtnom.Visibility = Visibility.Visible;

                nomResp.Visibility = Visibility.Hidden;
            }
        }
        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return && editing)
            {
                border.Background = Brushes.White;
                border.Opacity = 0.5;
                editing = false;
                nomResp.Visibility = Visibility.Visible;
                txtnom.Visibility = Visibility.Collapsed;

                if (txtnom.Text != nomResp.Text.ToString())
                    nomResp.Text = txtnom.Text;

                person.name = nomResp.Text;
                person.id_person = Convert.ToInt32(codiResp.Text);

                DbContext.UpdatePerson(person);
            }
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            List<int> ids = new List<int>();
            if (MessageBox.Show("Vols borrar aquest usuari?", "ALERTA!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
            }
            else
            {
                List<Card> cards = (List<Card>)CardService.GetAll();
                foreach (Card card in cards)
                {
                    ids.Add(card.fk_id_responsable);
                }
                if (ids.Contains(Convert.ToInt32(codiResp.Text)))
                {
                    MessageBox.Show("Aquesta persona te tasques assignades!");
                }
                else
                {
                    ((Panel)this.Parent).Children.Remove(this);
                    DbContext.DeletePerson(person);
                }
            }
        }
    }
}

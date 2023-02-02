using Dragger_WPF.Entity;
using Dragger_WPF.Persistence;
using Dragger_WPF.Service;
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
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace Dragger_WPF.UserControls
{
    /// <summary>
    /// Lógica de interacción para PersonUserControl.xaml
    /// </summary>
    public partial class PersonUserControl : UserControl
    {
        Person person;

        bool editing = false;
        public PersonUserControl(Person newPerson)
        {
            person = newPerson;
            InitializeComponent();

            codiResp.Text = person._id_person.ToString();
            nomResp.Text = person._name;
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            editing = true;
            if (editing)
            {

                txtCodiResp.Text = codiResp.Text;
                txtnom.Text = nomResp.Text;

                txtCodiResp.Visibility = Visibility.Visible;
                txtnom.Visibility = Visibility.Visible;

                nomResp.Visibility = Visibility.Hidden;
                codiResp.Visibility = Visibility.Hidden;
            }
        }
        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return && editing)
            {
                editing = false;
                nomResp.Visibility = Visibility.Visible;
                codiResp.Visibility = Visibility.Visible;
                txtCodiResp.Visibility = Visibility.Collapsed;
                txtnom.Visibility = Visibility.Collapsed;

                if (txtCodiResp.Text != codiResp.Text.ToString())
                    codiResp.Text = txtCodiResp.Text;

                if (txtnom.Text != nomResp.Text.ToString())
                    nomResp.Text = txtnom.Text;

                person._name = nomResp.Text;
                person._id_person = Convert.ToInt32(codiResp.Text);

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
                    ids.Add(card._id_persona);
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

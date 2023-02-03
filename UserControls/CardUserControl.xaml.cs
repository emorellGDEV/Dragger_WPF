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

namespace Dragger_WPF.UserControls
{
    /// <summary>
    /// Lógica de interacción para CardUserControl.xaml
    /// </summary>
    public partial class CardUserControl : UserControl
    {
        


        bool editing = false;
        Card card { get; set; }

        public CardUserControl(Card cardd)
        {
            card = cardd;
            InitializeComponent();

            lidCard.Content = card._id_card;
            lidPer.Content = card._id_persona;
            ldescription.Content = card._description;
            String fCreation = card._creationDate.ToString("dd/MM/yyyy");
            String fGoal = card._goalDate.ToString("dd/MM/yyyy");
            lcDate.Content = fCreation;
            lgDate.Content = fGoal;

            tidPer.ItemsSource = PersonService.GetAll();
            tidPer.DisplayMemberPath = "_name";
            tidPer.SelectedValuePath = "_id_person";
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            tidPer.SelectedValue = card._id_persona;
            editing = true;
            if (editing)
            {

                tdescription.Text = ldescription.Content.ToString();
                tgDate.Text = lgDate.Content.ToString();

                tdescription.Visibility = Visibility.Visible;
                tgDate.Visibility = Visibility.Visible;
                tidPer.Visibility = Visibility.Visible;
            }
        }
        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return && editing)
            {
                editing = false;
                tdescription.Visibility = Visibility.Collapsed;
                tgDate.Visibility = Visibility.Collapsed;
                tidPer.Visibility = Visibility.Collapsed;

                if (tdescription.Text != ldescription.Content.ToString())
                    ldescription.Content = tdescription.Text;

                if (tgDate.Text != lgDate.Content.ToString())
                    lgDate.Content = tgDate.Text;

                lidPer.Content = tidPer.SelectedValue.ToString();

                card._goalDate = Convert.ToDateTime(lgDate.Content);
                card._id_persona = Convert.ToInt32(lidPer.Content);
                card._description = Convert.ToString(ldescription.Content);

                DbContext.UpdateCard(card);
            }
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("ALERTA!", "Vols borrar aquesta tasca?", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No){
            }
            else {
                ((Panel)this.Parent).Children.Remove(this);
                DbContext.DeleteCard(card);
            }
        }

      
        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(this, this,  DragDropEffects.Move);
            }
        }





    }
}

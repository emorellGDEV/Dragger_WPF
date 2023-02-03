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

        int priorityIndex;

        public CardUserControl(Card cardd)
        {
            card = cardd;
            InitializeComponent();
            checkPriority();
            lidCard.Content = card.id_card;
            lidPer.Content = card.fk_id_responsable;
            ldescription.Content = card.description;
            String fCreation = card.creationDate.ToString("dd/MM/yyyy");
            String fGoal = card.goalDate.ToString("dd/MM/yyyy");
            lcDate.Content = fCreation;
            lgDate.Content = fGoal;
            priorityIndex = card.priority;

            tidPer.ItemsSource = (System.Collections.IEnumerable)PersonService.GetAll();
            tidPer.DisplayMemberPath = "name";
            tidPer.SelectedValuePath = "id_person";
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            tidPer.SelectedValue = card.fk_id_responsable;
            tidPer.ItemsSource = (System.Collections.IEnumerable)PersonService.GetAll();
            tidPer.DisplayMemberPath = "name";
            tidPer.SelectedValuePath = "id_person";
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

                if ((int)tidPer.SelectedValue != card.fk_id_responsable)
                    lidPer.Content = tidPer.SelectedValue.ToString();

                card.goalDate = Convert.ToDateTime(lgDate.Content);
                card.fk_id_responsable = Convert.ToInt32(lidPer.Content);
                card.description = Convert.ToString(ldescription.Content);

                DbContext.UpdateCard(card);
            }
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Vols borrar aquesta tasca?", "ALERTA!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
            }
            else
            {
                ((Panel)this.Parent).Children.Remove(this);
                DbContext.DeleteCard(card);
            }
        }

        private void priorityButton_Click(object sender, RoutedEventArgs e)
        {
            priority.Background = null;

            checkPriority();
            card.priority = priorityIndex;
            DbContext.UpdateCard(card);
        }

        public void checkPriority()
        {

            switch (priorityIndex)
            {
                case 1:
                    {
                        priority.Background = Brushes.Yellow;
                        priorityIndex = 2;
                        break;
                    }
                case 2:
                    {
                        priority.Background = Brushes.Red;
                        priorityIndex = 3;
                        break;
                    }
                case 3:
                    {
                        priorityIndex = 1;
                        priority.Background = Brushes.Green;
                        break;
                    }
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

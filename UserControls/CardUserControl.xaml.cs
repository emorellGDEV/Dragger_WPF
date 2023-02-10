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

        //Carrega l'informació de la "card" passsada per la funció que crea les tasques.
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
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            //Omplim la llista de responsables, actualitzada, al selector de responsable.
            tidPer.SelectedValue = card.fk_id_responsable;
            tidPer.ItemsSource = PersonService.GetAll();
            lidPer.Visibility = Visibility.Hidden;
            ldescription.Visibility = Visibility.Hidden;
            lgDate.Visibility = Visibility.Hidden;
            tidPer.DisplayMemberPath = "name";
            tidPer.SelectedValuePath = "id_person";
            editing = true;
            if (editing)
            {
                //Ens mostra els elements editables del UserControl
                tdescription.Visibility = Visibility.Visible;
                tgDate.Visibility = Visibility.Visible;
                tidPer.Visibility = Visibility.Visible;
                BorderB.Background = Brushes.PaleVioletRed;
                BorderB.Opacity = 0.5;
                tdescription.Text = ldescription.Content.ToString();
                tgDate.Text = lgDate.Content.ToString();
                tidPer.SelectedValue = card.fk_id_responsable;

            }
        }
        private async void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            //En detectar la tecla Enter, guarda els canvis realitzats.
            if (e.Key == Key.Return && editing)
            {
                BorderB.Background = Brushes.White;
                BorderB.Opacity = 0.5;
                editing = false;
                tdescription.Visibility = Visibility.Collapsed;
                tgDate.Visibility = Visibility.Collapsed;
                tidPer.Visibility = Visibility.Hidden;

                //Si no s'ha modificat el camp, no s'actualitza.
                if (tdescription.Text != ldescription.Content.ToString())
                    ldescription.Content = tdescription.Text;

                if (tgDate.Text != lgDate.Content.ToString())
                    lgDate.Content = tgDate.Text;


                lidPer.Content = tidPer.SelectedValue.ToString();

                //Aplica aquests canvis a l'objecte card.
                card.fk_id_responsable = Convert.ToInt32(lidPer.Content);
                card.goalDate = Convert.ToDateTime(lgDate.Content);
                card.description = Convert.ToString(ldescription.Content);

                lidPer.Visibility = Visibility.Visible;
                ldescription.Visibility = Visibility.Visible;
                lgDate.Visibility = Visibility.Visible;

                //Actualitza en la bdd els canvis realitzats mitjançant la funció UpdateCard
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
                //El·limina la tasca del stackpanel i la borra de la bdd.
                ((Panel)this.Parent).Children.Remove(this);
                DbContext.DeleteCard(card);
            }
        }

        //Canvia la prioritat de la tasca
        private void priorityButton_Click(object sender, RoutedEventArgs e)
        {
            priority.Background = null;

            checkPriority();
            card.priority = priorityIndex;
            DbContext.UpdateCard(card);
        }

        //Aplica els canvis de colors de la tasca llegint l'index de prioritat i aplicant el color corresponent.
        public void checkPriority()
        {

            switch (priorityIndex)
            {
                case 1:
                    {
                        priority.Background = Brushes.Yellow;
                        BorderB.BorderBrush = Brushes.Yellow;
                        priorityIndex = 2;
                        break;
                    }
                case 2:
                    {
                        priority.Background = Brushes.Red;
                        BorderB.BorderBrush = Brushes.Red;
                        priorityIndex = 3;
                        break;
                    }
                case 3:
                    {
                        priorityIndex = 1;
                        priority.Background = Brushes.Green;
                        BorderB.BorderBrush = Brushes.Green;
                        break;
                    }
            }
        }

        //Actualitza la posició de la tasca.
        public void changePosition(int pos)
        {
            if (pos >= 1 && pos <= 3)
            {
                card.position = pos;
                DbContext.UpdateCard(card);
            }
        }

        
        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            //Funcionalitat DragAndDrop, en cas de estar editant, esta deshabilitat.
            if (!editing)
            {
                base.OnMouseMove(e);
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    DragDrop.DoDragDrop(this, this, DragDropEffects.Move);
                }
            }
        }





    }
}

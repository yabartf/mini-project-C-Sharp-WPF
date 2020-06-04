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
using System.Windows.Shapes;
using System.Runtime.CompilerServices;
using BE;
using BL;
using System.Net.Mail;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for AddGuestRequest.xaml
    /// </summary>
    public partial class AddGuestRequest : Window
    {
        IBL tempMyBl = FuctoryBl.getBl();
        private int min, max, temp;
        private bool NoProblem = false;
        GuestRequest inputGuestRequest = new GuestRequest();


        public AddGuestRequest()
        {
            InitializeComponent();
            message.Visibility = Visibility.Hidden;
            Dates dates = new Dates();
            EntryDate.DisplayDateStart = dates.Today;
            RelaeseDate.DisplayDateStart = dates.Today.AddDays(1);
            EntryDate.DisplayDateEnd = dates.EndDate;
            RelaeseDate.DisplayDateEnd = dates.EndDate;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }





        private void Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            inputGuestRequest.Type = (Type.SelectedIndex + 1);
        }

        private void Area_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            inputGuestRequest.Area = (Area.SelectedIndex + 1);
        }

        private void Spa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            inputGuestRequest.spa = (Spa.SelectedIndex + 1);
        }

        private void FlatTv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            inputGuestRequest.flatTv = (FlatTv.SelectedIndex + 1);
        }

        private void AirCondition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            inputGuestRequest.airCondition = (AirCondition.SelectedIndex + 1);
        }

        private void Pool_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            inputGuestRequest.Pool = (Pool.SelectedIndex + 1);
        }

        private void Jacuzzy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            inputGuestRequest.Jacuzzy = (Jacuzzy.SelectedIndex + 1);
        }

        private void Garden_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            inputGuestRequest.Garden = (Garden.SelectedIndex + 1);
        }

        private void NoMeals(object sender, RoutedEventArgs e)
        {
            inputGuestRequest.Meals = 0;
        }

        private void HalfMeals(object sender, RoutedEventArgs e)
        {
            inputGuestRequest.Meals = 2;
        }

        private void ThreeMeals(object sender, RoutedEventArgs e)
        {
            inputGuestRequest.Meals = 3;
        }

        private void PrivateName_TextChanged(object sender, TextChangedEventArgs e)
        {
            inputGuestRequest.PrivateName = PrivateName.Text;
        }

        private void LastName_TextChanged(object sender, TextChangedEventArgs e)
        {
            inputGuestRequest.FamilyName = LastName.Text;
        }

        private void Mail_TextChanged(object sender, TextChangedEventArgs e)
        {
            inputGuestRequest.MailAddress = Mail.Text;
        }
        private void CheckInSelected(object sender, SelectionChangedEventArgs e)
        {
            inputGuestRequest.EntryDate = (DateTime)EntryDate.SelectedDate;
        }

        private void EndDate(object sender, SelectionChangedEventArgs e)
        {
            inputGuestRequest.ReleaseDate = (DateTime)RelaeseDate.SelectedDate;
        }



        private void Atraction_Changed(object sender, SelectionChangedEventArgs e)
        {
            inputGuestRequest.ChildrensAttractions = (Atraction.SelectedIndex + 1);
        }

        private void Private_GotFocus(object sender, RoutedEventArgs e)
        {
            if (PrivateName.Text == "שם פרטי")
                PrivateName.Clear();
        }

        private void LastName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (LastName.Text == "שם משפחה")
                LastName.Clear();
        }

        private void Mail_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Mail.Text == "כתובת מייל")
                Mail.Clear();
        }

        private void MinCost_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {

                if (int.TryParse(MinCost.Text, out temp))
                    min = int.Parse(MinCost.Text);
                else if (!string.IsNullOrWhiteSpace(MinCost.Text))
                    throw new OurException("צריך להכניס מספר");
            }
            catch (OurException ex)
            {
                MinCost.Clear();
                MessageBox.Show(ex.Message);
            }
        }

        private void MaxCost_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {

                if (int.TryParse(MaxCost.Text, out temp))
                    max = int.Parse(MaxCost.Text);
                else if (!string.IsNullOrWhiteSpace(MaxCost.Text))
                    throw new OurException("צריך להכניס מספר");
            }
            catch (OurException ex)
            {
                MaxCost.Clear();
                MessageBox.Show(ex.Message);
            }
        }

        private void Adults_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {

                if (int.TryParse(Adults.Text, out temp))
                    inputGuestRequest.Adults = int.Parse(Adults.Text);
                else if (!string.IsNullOrWhiteSpace(Adults.Text))
                    throw new OurException("מספר המבוגרים צריך להיות ספרה");
            }
            catch (OurException ex)
            {
                Adults.Clear();
                MessageBox.Show(ex.Message);
            }
        }



        private void Chilldren_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.TryParse(Chilldren.Text, out temp))
                    inputGuestRequest.Children = int.Parse(Chilldren.Text);
                else if (!string.IsNullOrWhiteSpace(Chilldren.Text))
                    throw new OurException("מספר הילדים צריך להיות ספרה צריך להיות ספרה");
            }

            catch (OurException ex)
            {
                Chilldren.Clear();
                MessageBox.Show(ex.Message);
            }
        }

        private void PrivateName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PrivateName.Text))
                PrivateName.Text = "שם פרטי";
        }

        private void LastName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LastName.Text))
                LastName.Text = "שם משפחה";
        }

        private void Mail_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Mail.Text))
                Mail.Text = "כתובת מייל";
        }
        private void Check_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NoProblem = true;
                if (!checkIfAllTextBoxAreFilled() || !checkIfAllComboboxAreFilled() || !checkIfAllRadioButtonAreFilled() || !checkIfCalanderWasFilled())
                    throw new OurException("אחד או יותר מהשדות אינו מלא");
                if (NoProblem)
                {
                    MessageBox.Show("תודה רבה! הבקשה נקלטה");
                    tempMyBl.addGuestRequest(inputGuestRequest);
                    Hide();
                }
            }
            catch (OurException ex)
            {
                MessageBox.Show(ex.Message);
                NoProblem = false;
            }
        }
        private bool checkIfAllTextBoxAreFilled()
        {
            bool invalid = grid.Children.OfType<TextBox>()
     .Where(t => t.IsEnabled)
     .Any(t => string.IsNullOrWhiteSpace(t.Text));
            try
            {
                MailAddress mail = new MailAddress(Mail.Text);
            }
            catch
            {                
                Mail.Background = Brushes.Red;
                Mail.Clear();
                return false;
            }
            if (invalid)
                return false;
            return true;
        }
        private bool checkIfAllComboboxAreFilled()
        {
            bool invalid = grid.Children.OfType<ComboBox>()
     .Where(t => t.IsEnabled)
     .Any(t => string.IsNullOrWhiteSpace(t.Text));
            if (invalid)
                return false;
            return true;
        }
        private bool checkIfAllRadioButtonAreFilled()
        {
            if (no.IsChecked == false && twoMeals.IsChecked == false && fullMeals.IsChecked == false)
                return false;
            return true;
        }
        private bool checkIfCalanderWasFilled()
        {
            if (EntryDate.SelectedDate == null || RelaeseDate.SelectedDate == null)
                return false;
            return true;
        }
    }
    public class Dates
    {
        public DateTime Today;
        public DateTime EndDate;
        public Dates()
        {
            Today = DateTime.Now.AddDays(1);
            EndDate = DateTime.Now.AddMonths(11);
        }
    }


}

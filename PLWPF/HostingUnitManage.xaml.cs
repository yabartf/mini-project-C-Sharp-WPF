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

using BE;
using BL;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for HostingUnitManage.xaml
    /// </summary>
    public partial class HostingUnitManage : Window
    {
        BL_imp bl = BL_imp.getBl();
        HostingUnit hostingunit = new HostingUnit();
        Host owner = new Host();
        BankBranch BankBranchDetails = new BankBranch();
        public HostingUnitManage()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void AddOrUpdate_Click(object sender, RoutedEventArgs e)
        {
            int isInt;
            object key = null;

            if (!string.IsNullOrWhiteSpace(hostingUnitName.Text))
                key = hostingUnitName.Text;
            else if (!string.IsNullOrWhiteSpace(hostingUnitKey.Text) && int.TryParse(hostingUnitKey.Text, out isInt))
                key = hostingUnitKey.Text;
            enableFields();
            if (bl.hostingUnitExist(key) != null)
            {
                hostingunit = bl.hostingUnitExist(key);
                fillAllFields();
                delete.IsEnabled = true;
                update.IsEnabled = true;
            }
            else
            {
                add.IsEnabled = true;
                hostingUnitKey.Text = Configuration.hostingUnitKey.ToString();

            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            int isInt;
            object key;

            if (!string.IsNullOrWhiteSpace(hostingUnitName.Text))
                key = hostingUnitName.Text;
            else if (!string.IsNullOrWhiteSpace(hostingUnitKey.Text) && int.TryParse(hostingUnitKey.Text, out isInt))
                key = hostingUnitKey.Text;
            else
                throw new OurException("ERROR");
            bl.deleteHostingUnit(hostingunit.hostingUnitKey);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!checkIfAllComboboxAreFilled() || !checkIfAllFildesAreFilled())
                    MessageBox.Show("שדות לא מלאים");
                else
                {
                    hostingunit.Owner = owner;
                    hostingunit.Owner.BankBranchDetails = BankBranchDetails;
                    getCheckBoxes();
                    bl.addHostingUnit(hostingunit);
                    MessageBox.Show("יחידת דיור עודכנה. הסיסמא לכניסה 1234");
                    hostingunit = null;
                    Close();
                }
            }
            catch (OurException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (checkIfAllFildesAreFilled() && checkIfAllComboboxAreFilled())
                bl.updateHostingUnit(hostingunit);
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            var textBoxes = grid.Children.OfType<TextBox>();
            foreach (var item in textBoxes)
            {
                item.Clear();
            }
            var checkBoxes = grid.Children.OfType<CheckBox>();
            foreach (var item in checkBoxes)
            {
                item.IsChecked = false;
            }
            var ComboBoxes = grid.Children.OfType<ComboBox>();
            foreach (var item in ComboBoxes)
            {
                item.SelectedItem = null;
            }
            hostingUnitKey.IsEnabled = true;

        }
        private void fillAllFields()
        {
            hostingUnitName.Text = hostingunit.HostingUnitName;
            privateName.Text = hostingunit.Owner.PrivateName;
            familyName.Text = hostingunit.Owner.FamilyName;
            id.Text = hostingunit.Owner.HostId.ToString();
            fhoneNumber.Text = hostingunit.Owner.FhoneNumber.ToString();
            email.Text = hostingunit.Owner.MailAddress;
            bankName.Text = hostingunit.Owner.BankBranchDetails.BankName;
            bankNumber.Text = hostingunit.Owner.BankBranchDetails.BankNumber.ToString();
            brunchNumber.Text = hostingunit.Owner.BankBranchDetails.BranchNumber.ToString();
            BranchAddress.Text = hostingunit.Owner.BankBranchDetails.BranchAddress;
            BranchCity.Text = hostingunit.Owner.BankBranchDetails.BranchCity;
            BankAccountNumber.Text = hostingunit.Owner.BankAccountNumber.ToString();
            Type.SelectedIndex = hostingunit.Type - 1;
            Area.SelectedIndex = hostingunit.area - 1;
            if (hostingunit.Meals == 0) pension.SelectedIndex = hostingunit.Meals;
            else pension.SelectedIndex = hostingunit.Meals - 1;
            prise.Text = hostingunit.pricePerNight.ToString();
            adress.Text = hostingunit.adress;
            city.Text = hostingunit.city;
            adults.Text = hostingunit.Adults.ToString();
            chilldren.Text = hostingunit.Children.ToString();
            spa.IsChecked = hostingunit.spa;
            airCondition.IsChecked = hostingunit.airCondition;
            flatTv.IsChecked = hostingunit.flatTv;
            Pool.IsChecked = hostingunit.Pool;
            Garden.IsChecked = hostingunit.Garden;
            ChilldrensAttractions.IsChecked = hostingunit.ChilldrensAttractions;
            Jacuzzy.IsChecked = hostingunit.Jacuzzy;
        }
        private void getCheckBoxes()
        {
            hostingunit.spa = (bool)spa.IsChecked;
            hostingunit.Pool = (bool)Pool.IsChecked;
            hostingunit.Jacuzzy = (bool)Jacuzzy.IsChecked;
            hostingunit.Garden = (bool)Garden.IsChecked;
            hostingunit.flatTv = (bool)flatTv.IsChecked;
            hostingunit.airCondition = (bool)airCondition.IsChecked;
            hostingunit.ChilldrensAttractions = (bool)ChilldrensAttractions.IsChecked;
        }


        private bool checkIfAllFildesAreFilled()
        {
            bool invalid = grid.Children.OfType<TextBox>()
     .Where(t => t.IsEnabled)
     .Any(t => string.IsNullOrWhiteSpace(t.Text));
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
        private void enableFields()
        {

            var textBoxes = grid.Children.OfType<TextBox>();
            var comboBoxes = grid.Children.OfType<ComboBox>();
            var checkBoxes = grid.Children.OfType<CheckBox>();
            foreach (var textBox in textBoxes)
            {
                textBox.IsEnabled = true;
            }
            hostingUnitKey.IsEnabled = false;
            foreach (var comboBox in comboBoxes)
            {
                comboBox.IsEnabled = true;
            }//
            foreach (var checkBox in checkBoxes)
            {
                checkBox.IsEnabled = true;
            }//
            BranchCity.IsEnabled = false;
            bankNumber.IsEnabled = false;
            BankAccountNumber.IsEnabled = true;
        }
        private void back()
        {
            Close();
        }

        private void HostingUnitKey_LostFocus(object sender, RoutedEventArgs e)
        {
            int key;
            if (!int.TryParse(hostingUnitKey.Text, out key) && (!string.IsNullOrWhiteSpace(hostingUnitKey.Text)))
            {
                MessageBox.Show("מספר יחידה לא תקין");
                hostingUnitKey.Clear();
            }
        }

        private void HostingUnitName_LostFocus(object sender, RoutedEventArgs e)
        {
            hostingunit.HostingUnitName = hostingUnitName.Text;
        }

        private void PrivateName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(privateName.Text))
                owner.PrivateName = privateName.Text;
        }

        private void FamilyName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(privateName.Text))
                owner.FamilyName = familyName.Text;
        }

        private void FhoneNumber_LostFocus(object sender, RoutedEventArgs e)
        {
            int fnumber;
            if (int.TryParse(fhoneNumber.Text, out fnumber))
                owner.FhoneNumber = fnumber;
            else if (!string.IsNullOrWhiteSpace(fhoneNumber.Text))
            {
                MessageBox.Show("ניתן למלא רק מספרים");
                fhoneNumber.Clear();
            }
        }

        private void Id_LostFocus(object sender, RoutedEventArgs e)
        {
            int ID;
            if (int.TryParse(id.Text, out ID))
                owner.HostId = ID;
            else if (!string.IsNullOrWhiteSpace(id.Text))
            {
                MessageBox.Show("ניתן למלא רק מספרים");
                id.Clear();
            }
        }

        private void Email_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(email.Text))
                owner.MailAddress = email.Text;
        }


        private void BankNumber_LostFocus(object sender, RoutedEventArgs e)
        {
            int bankNum;
            if (int.TryParse(bankNumber.Text, out bankNum))
                BankBranchDetails.BankNumber = bankNum;
            else if (!string.IsNullOrWhiteSpace(bankNumber.Text))
            {
                MessageBox.Show("ניתן למלא רק מספרים");
                bankNumber.Clear();
            }
        }

        private void BrunchNumber_LostFocus(object sender, RoutedEventArgs e)
        {
            int BrunchNum;
            if (int.TryParse(brunchNumber.Text, out BrunchNum))
            {
                if (bl.GetAllBankBranch().Exists(x => x.BranchNumber.ToString() == brunchNumber.Text))
                {
                    BankBranchDetails.BranchNumber = BrunchNum;
                    setBankAdress();
                    BankBranchDetails.BranchAddress = BranchAddress.Text;
                }
                else
                    MessageBox.Show("מספר סניף לא קיים");
            }
            else if (!string.IsNullOrWhiteSpace(brunchNumber.Text))
            {
                MessageBox.Show("ניתן למלא רק מספרים");
                brunchNumber.Clear();
            }
        }

        private void BankAccountNumber_LostFocus(object sender, RoutedEventArgs e)
        {
            int BankAccountNum;
            if (int.TryParse(BankAccountNumber.Text, out BankAccountNum))
                owner.BankAccountNumber = BankAccountNum;
            else if (!string.IsNullOrWhiteSpace(BankAccountNumber.Text))
            {
                MessageBox.Show("ניתן למלא רק מספרים");
                BankAccountNumber.Clear();
            }
        }

        private void Prise_LostFocus(object sender, RoutedEventArgs e)
        {
            int prc;
            if (int.TryParse(prise.Text, out prc))
                hostingunit.pricePerNight = prc;
            else if (!string.IsNullOrWhiteSpace(prise.Text))
            {
                MessageBox.Show("ניתן למלא רק מספרים");
                prise.Clear();
            }
        }

        private void Type_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!(Type.SelectedItem == null))
                hostingunit.Type = Type.SelectedIndex + 1;
        }

        private void Adress_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(adress.Text))
                hostingunit.adress = adress.Text;
        }

        private void City_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(city.Text))
                hostingunit.city = city.Text;
        }

        private void Pension_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!(pension.SelectedItem == null))
                hostingunit.Meals = pension.SelectedIndex == 0 ? pension.SelectedIndex : pension.SelectedIndex + 1;
        }

        private void Area_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!(Area.SelectedItem == null))
                hostingunit.area = Area.SelectedIndex + 1;
        }

        private void BranchCity_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(BranchCity.Text))
                BankBranchDetails.BranchCity = BranchCity.Text;
        }

        private void Chilldren_LostFocus(object sender, RoutedEventArgs e)
        {
            int childrn;
            if (int.TryParse(chilldren.Text, out childrn))
                hostingunit.Children = childrn;
            else if (!string.IsNullOrWhiteSpace(chilldren.Text))
            {
                MessageBox.Show("ניתן למלא רק מספרים");
                chilldren.Clear();
            }
        }

        private void Adults_LostFocus(object sender, RoutedEventArgs e)
        {
            int adult;
            if (int.TryParse(adults.Text, out adult))
                hostingunit.Adults = adult;
            else if (!string.IsNullOrWhiteSpace(adults.Text))
            {
                MessageBox.Show("ניתן למלא רק מספרים");
                adults.Clear();
            }
        }

        private void HostingUnitName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(hostingUnitKey.Text) || !string.IsNullOrEmpty(hostingUnitName.Text))
                addOrUpdate.IsEnabled = true;
            else
                addOrUpdate.IsEnabled = false;
        }

        private void HostingUnitKey_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(hostingUnitName.Text) || !string.IsNullOrEmpty(hostingUnitKey.Text))
                addOrUpdate.IsEnabled = true;
            else
                addOrUpdate.IsEnabled = false;
        }
        private void setBankNumber()
        {
            bankNumber.Text = bl.GetAllBankBranch().Find(x => x.BankName == bankName.Text).BankNumber.ToString();
        }
        private void setBankAdress()
        {
            BranchAddress.Text = bl.GetAllBankBranch().Find(x => x.BranchNumber.ToString() == brunchNumber.Text).BranchAddress;
        }
        private void checkBnakDetials()
        {
            var bankList = bl.GetAllBankBranch();


            if (!bankList.Exists(x => x.BranchCity == BranchCity.Text))
            {
                BranchCity.Clear();
                MessageBox.Show("לא קיים סניף בעיר הזאת");
            }
            if (!bankList.Exists(x => x.BranchNumber.ToString() == brunchNumber.Text))
            {
                brunchNumber.Clear();
                MessageBox.Show("לא קיים סניף עם הכתובת הזו");
            }

        }

        private void BankName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(bankName.Text))
            {
                BankBranchDetails.BankName = bankName.Text;
                setBankNumber();
                BranchCity.IsEnabled = true;
                bankNumber.IsEnabled = true;
                BankAccountNumber.IsEnabled = true;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Threading;
using System.Windows.Threading;
using System.Net.Mail;
using BE;
using BL;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for HostingUnitManage.xaml
    /// </summary>
    public partial class HostingUnitManage : Window
    {
        static public bool PasTrue = false;
        BL_imp bl = BL_imp.getBl();
        HostingUnit hostingunit = new HostingUnit();
        Host owner = new Host();
        BankBranch BankBranchDetails = new BankBranch();
        ObservableCollection<bruches> AllBrunchs = new ObservableCollection<bruches>();
        public HostingUnitManage()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            DataContext = AllBrunchs;
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
                owner = hostingunit.Owner;
                BankBranchDetails = hostingunit.Owner.BankBranchDetails;
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
            PasTrue = false;
            if (!string.IsNullOrWhiteSpace(hostingUnitName.Text))
                key = hostingUnitName.Text;
            else if (!string.IsNullOrWhiteSpace(hostingUnitKey.Text) && int.TryParse(hostingUnitKey.Text, out isInt))
                key = hostingUnitKey.Text;
            else

                throw new OurException("ERROR");
            try
            {
                Password pas = new Password();
                pas.ShowDialog();
                if (PasTrue)
                {
                    PasTrue = false;
                    bl.deleteHostingUnit(hostingunit.hostingUnitKey);
                    MessageBox.Show("יחידת נופש נמחקה");
                }
                
            }
            catch(OurException ex)
            {
                MessageBox.Show(ex.Message);
            }
            Close();
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
            PasTrue = false;
            if (checkIfAllFildesAreFilled() && checkIfAllComboboxAreFilled())
            {
                hostingunit.Owner = owner;
                hostingunit.Owner.BankBranchDetails = BankBranchDetails;
                try
                {
                    Password pas = new Password();
                    pas.ShowDialog();
                    if (PasTrue)
                    {
                        PasTrue = false;
                        bl.updateHostingUnit(hostingunit);
                        MessageBox.Show("יחידת נופש עודכנה");
                    }

                    Close();
                }
                catch (OurException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
                MessageBox.Show("שדות לא מלאים");
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            var ComboBoxes = grid.Children.OfType<ComboBox>();
            foreach (var item in ComboBoxes)
            {
                item.SelectedItem = null;
            }
            var checkBoxes = grid.Children.OfType<CheckBox>();
            foreach (var item in checkBoxes)
            {
                item.IsChecked = false;
            }
            var textBoxes = grid.Children.OfType<TextBox>();
            foreach (var item in textBoxes)
            {
                item.Clear();
            }
            hostingUnitKey.IsEnabled = true;

        }
        private void fillAllFields()
        {
            hostingUnitKey.Text = hostingunit.hostingUnitKey.ToString();
            hostingUnitName.Text = hostingunit.HostingUnitName;
            privateName.Text = hostingunit.Owner.PrivateName;
            familyName.Text = hostingunit.Owner.FamilyName;
            id.Text = hostingunit.Owner.HostId.ToString();
            fhoneNumber.Text = hostingunit.Owner.FhoneNumber.ToString();
            email.Text = hostingunit.Owner.MailAddress;
            fillBankName();
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
            try
            {
                MailAddress temp = new MailAddress(email.Text);
            }
            catch 
            {
                email.Background = Brushes.Red;
                email.Clear();
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
                    //setBankAdress();
                    BankBranchDetails.BranchAddress = BranchAddress.Text;
                    BankBranchDetails.BranchCity = BranchCity.Text;
                }
                else
                    MessageBox.Show("מספר סניף לא קיים");
            }
            else if (!string.IsNullOrWhiteSpace(brunchNumber.Text))
            {
                MessageBox.Show("ניתן למלא רק מספרים");
                //  brunchNumber.Clear();
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
            List<BankBranch> AllBanks = bl.GetAllBankBranch();
            //if (!string.IsNullOrWhiteSpace(bankName.Text) && AllBanks.Count != 0)
            //    bankNumber.Text = AllBanks.Find(x => x.BankName == bankName.Text).BankNumber.ToString();
            bankNumber.Dispatcher.Invoke(() =>
            {
                while (AllBanks.Count == 0)
                {
                    Thread.Sleep(1000);
                    AllBanks = bl.GetAllBankBranch();
                    setBankNumber();
                }
                if (!string.IsNullOrWhiteSpace(bankName.Text) && AllBanks.Count != 0)
                {
                    bankNumber.Text = AllBanks.Find(x => x.BankName == bankName.Text).BankNumber.ToString();
                    BankBranchDetails.BankNumber = int.Parse(bankNumber.Text);
                }
            });

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
                //brunchNumber.Clear();
                MessageBox.Show("לא קיים סניף עם הכתובת הזו");
            }

        }
        public class bruches
        {
            public int number;
            public string city, address;
            public override string ToString()
            {
                return string.Format("" + number);
            }
        }

        private void BankName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BankBranchDetails.BankName = bankName.Text;
            new Thread(setBankNumber).Start();
            // BranchCity.IsEnabled = true;
            BankAccountNumber.IsEnabled = true;

            var numbers = from item in bl.GetAllBankBranch()
                          where item.BankName == bankName.Text
                          select item;
            foreach (var item in numbers)
            {
                bruches adding = new bruches();
                adding.number = item.BranchNumber;
                BankBranchDetails.BankNumber = item.BranchNumber;
                adding.city = item.BranchCity;
                adding.address = item.BranchAddress;
                AllBrunchs.Add(adding);
            }
            

        }

        private void BrunchNumber_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(brunchNumber.Text))
            {
                foreach (var item in AllBrunchs)
                {
                    if (brunchNumber.Text == ("" + item.number))
                    {
                        BranchCity.Text = item.city;
                        BranchAddress.Text = item.address;
                    }
                }
            }
        }

        private void BankName_LostFocus(object sender, RoutedEventArgs e)
        {
            BankBranchDetails.BankName = bankName.Text;
            new Thread(setBankNumber).Start();
            //BranchCity.IsEnabled = true;
            BankAccountNumber.IsEnabled = true;

            var numbers = bl.GetAllBankBranch().Where(x => x.BankName == bankName.Text).
                OrderBy(x => x.BranchNumber);
            //var numbers = from item in bl.GetAllBankBranch()
            //              where item.BankName == bankName.Text
            //              select item;
            AllBrunchs.Clear();
            foreach (var item in numbers)
            {
                bruches adding = new bruches();
                adding.number = item.BranchNumber;
                adding.city = item.BranchCity;
                adding.address = item.BranchAddress;
                AllBrunchs.Add(adding);
            }
            
        }
        private void fillBankName()
        {
            bankName.Text = hostingunit.Owner.BankBranchDetails.BankName;
            var numbers = bl.GetAllBankBranch().Where(x => x.BankName == bankName.Text).
                OrderBy(x => x.BranchNumber);
            foreach (var item in numbers)
            {
                bruches adding = new bruches();
                adding.number = item.BranchNumber;
                adding.city = item.BranchCity;
                adding.address = item.BranchAddress;
                AllBrunchs.Add(adding);
            }
        }

        private void BranchAddress_LostFocus(object sender, RoutedEventArgs e)
        {
            BankBranchDetails.BranchAddress = BranchAddress.Text;
        }

    }
}


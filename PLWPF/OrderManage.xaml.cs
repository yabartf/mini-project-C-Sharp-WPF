using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Net.Mail;
using System.ComponentModel;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for OrderManage.xaml
    /// </summary>
    public partial class OrderManage : Window
    {

        IBL bl = FuctoryBl.getBl();
        private HostingUnit CurntHostingUnit = new HostingUnit();
        private int coutOfOreder;
        private bool IsPressed = false, IsString = false,haveOrders = false;
        Button[] button;
        ListBoxItem[] boxItem;
        BackgroundWorker MailSend;
        GuestRequest ForMail;

        public OrderManage()
        {
            InitializeComponent();
            WindowOfOrder.Opacity = 0;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            MailSend = new BackgroundWorker();
            MailSend.DoWork += DoMail;
            MailSend.ProgressChanged += progressMail;
            MailSend.RunWorkerCompleted += complateMail;
            //MailSend.ReportProgress = true;
        }

        private void complateMail(object sender, RunWorkerCompletedEventArgs e)
        {
            object result = e.Result;
        }

        private void progressMail(object sender, ProgressChangedEventArgs e)
        {
            int temp = e.ProgressPercentage;
        }

        private void DoMail(object sender, DoWorkEventArgs e)
        {
            SendMail();
        }

        /// <summary>
        /// print all order of the hosting unit. all ListBoxItem is TextBox and Button.
        /// </summary>
        private void PrintOrder()
        {
            //int i = 0;
            boxItem = new ListBoxItem[coutOfOreder];             
            for (int i=0; i< coutOfOreder; i++)
            {
                GuestRequest gu = bl.guestRequestByOrder(CurntHostingUnit.orders[i]);
                Canvas canvas = new Canvas();
                canvas.Height = 23;
                boxItem[i] = new ListBoxItem();
                boxItem[i].Height = 28;
                boxItem[i].Name = "a" + i;
                TextBlock textBlock = new TextBlock();
                Thickness thickness = new Thickness(150, 0, 0, 0);
                textBlock.Margin = thickness;
                textBlock.Background = Brushes.Beige;
                textBlock.Width = 700;
                textBlock.Height = 23;
                textBlock.FontSize = 18;
                textBlock.Foreground = Brushes.Brown;
                textBlock.Text = "entry date: " + gu.EntryDate.ToString("dd/MM/yyyy") + "  release date: " + gu.ReleaseDate.ToString("dd/MM/yyyy") ;
                canvas.Children.Add(textBlock);
                thickness = new Thickness(0, 0, 700, 0);
                button[i] = new Button();
                button[i].HorizontalAlignment = HorizontalAlignment.Left;
                button[i].Content = "אשר";
                button[i].Click += OrderChoosed;
                button[i].Height = 23;
                button[i].Width = 110;
                button[i].Margin = thickness;
                button[i].Name = "button" + i;
                if (CurntHostingUnit.orders[i].status == (int)Status.Complate || CurntHostingUnit.orders[i].status == (int)Status.Faild || CurntHostingUnit.orders[i].status == (int)Status.SentMail)
                {
                    button[i].IsEnabled = false;
                    button[i].Background = Brushes.Red;
                    button[i].Foreground = Brushes.Black;
                    button[i].Content = "סגור";
                }
                canvas.Children.Add(button[i]);
                boxItem[i].Content = canvas;
                WindowOfOrder.Items.Add(boxItem[i]);                
                WindowOfOrder.Opacity = 1;
            }
            if (coutOfOreder==0)
                MessageBox.Show("אין נתונים להצגה");
        }
        private void OrderChoosed(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = 0;
                for (int i = 0; i < coutOfOreder; i++)
                {
                    if (button[i] == (Button)sender)
                    {
                        index = i;
                    }
                }
                CurntHostingUnit.orders[index].status = (int)Status.Complate;
                bl.updateOrder(CurntHostingUnit.orders[index]);
                ForMail = bl.guestRequestByOrder(CurntHostingUnit.orders[index]);
                MailSend.RunWorkerAsync();
                /// need to send mail to the client by thread///
                MessageBox.Show("נשלח מייל ללקוח");
                button[index].IsEnabled = false;
                button[index].Background = Brushes.Red;
                button[index].Foreground = Brushes.Black;
                button[index].Content = "סגור";                
                
            }
            catch (OurException ex)
            {
                MessageBox.Show("יש בעיה בהזמנה");                
            }
        }

        private void IsKey(object sender, KeyEventArgs e)
        {
            if (e.Key <= Key.Z && e.Key >= Key.A)
            {
                IsString = true;
                e.Handled = false;
            }
        }
        private void EnterHostingClick(object sender, RoutedEventArgs e)
        {
            try
            {
                CurntHostingUnit = IsString ? bl.TheHostingUnitByName(HostingKey.Text) : bl.TheHostingUnitByKey(int.Parse(HostingKey.Text));                
                if (CurntHostingUnit == null)
                    throw new OurException();
                coutOfOreder = CurntHostingUnit.orders.Count;
                button = new Button[coutOfOreder];
                IsPressed = true;
                IsString = false;
            }
            catch (OurException)
            {
                MessageBox.Show("!הפרטים אינם נכונים");
                IsPressed = false;
            }
            if (IsPressed)
                PrintOrder();
        }
        public void SendMail()
        {            
            MailMessage message = new MailMessage();
            message.To.Add(ForMail.MailAddress);
            message.From = new MailAddress(CurntHostingUnit.Owner.MailAddress);
            message.Subject = "!נמצאה לך יחידת אירוח מתאימה";
            message.Body = "hi! i want to sugggest you to relax in " + CurntHostingUnit.HostingUnitName
                + "\n" + "my phone is: " + CurntHostingUnit.Owner.FhoneNumber + "\n i would like to speak with you! \n"
                + CurntHostingUnit.Owner.PrivateName + CurntHostingUnit.Owner.FamilyName;
            message.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");           
            smtp.Credentials = new System.Net.NetworkCredential("nicerest5@gmail.com","Berto052783");
            smtp.UseDefaultCredentials = false;
            //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            //smtp.Port = 587;
            smtp.EnableSsl = true;
            try
            {
                smtp.Send(message);
            }
            catch 
            {
               // MessageBox.Show("המייל לא נשלח");               
            }
        }

    }


}

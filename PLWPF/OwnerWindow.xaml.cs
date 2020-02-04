﻿using System;
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

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for OwnerWindow.xaml
    /// </summary>
    public partial class OwnerWindow : Window
    {
        public OwnerWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void ToOwner(object sender, RoutedEventArgs e)
        {
            Owner NewWindow = new Owner();
            NewWindow.ShowDialog();
        }

        private void ToHostings(object sender, RoutedEventArgs e)
        {
            AllHostingUnit allHostingUnitWindow = new AllHostingUnit();
            allHostingUnitWindow.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new AllOrders().ShowDialog();
        }
    }
}

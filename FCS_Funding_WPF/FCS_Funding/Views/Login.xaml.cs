﻿using System;
//using System.Collections.Generic;
using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;

namespace FCS_Funding
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        /// <summary>
        /// This method logs you into the system
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Models.FCS_DBModel db = new Models.FCS_DBModel();
            string pw = Password.Password.ToString();
            string us = Username.Text;
            string hashedPassword = FCS_DataTesting.PasswordHashing.GetHashString(pw);
            try
            {
                var query = (from p in db.Staffs
                             where p.StaffUserName == us && p.StaffPassword == hashedPassword
                             select p).Distinct().First();
                MainWindow mw = new MainWindow(query.StaffDBRole);
                if(query.StaffDBRole == "No Access")
                {
                    MessageBox.Show("Invalid Credentials");
                    mw.Close();
                    return;
                }
                else if(query.StaffDBRole == "Basic")
                {
                    mw.CreateNewPat.IsEnabled = false;
                    mw.CreateGrantProp.IsEnabled = false;
                    mw.CreateNewDon.IsEnabled = false;
                    mw.AddItem.IsEnabled = false;
                    mw.AddService.IsEnabled = false;
                    mw.CreateEven.IsEnabled = false;
                    mw.AdminTab.IsEnabled = false;
                    mw.CreateNewsession.IsEnabled = false;
                    mw.AdminTab.Visibility = Visibility.Collapsed;
                }
                else if (query.StaffDBRole == "User")
                {
                    mw.AdminTab.Visibility = Visibility.Collapsed;

                }
                else if (query.StaffDBRole == "Admin")
                {
                    mw.CreateNewPat.IsEnabled = true;
                    mw.CreateGrantProp.IsEnabled = true;
                    mw.CreateNewDon.IsEnabled = true;
                    mw.AddItem.IsEnabled = true;
                    mw.AddService.IsEnabled = true;
                    mw.CreateEven.IsEnabled = true;
                    mw.AdminTab.IsEnabled = true;
                    mw.AdminTab.Visibility = Visibility.Visible;
                }
                mw.Show();
                this.Close();
            }
            catch 
            {
                MessageBox.Show("The credentials used are either invalid,or\nsomeone else is currently logged in.");
            }
            
        }
        
    }
}

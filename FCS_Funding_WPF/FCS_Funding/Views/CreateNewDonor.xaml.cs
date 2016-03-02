﻿using FCS_Funding.Models;
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

namespace FCS_Funding.Views
{
    /// <summary>
    /// Interaction logic for CreateNewDonor.xaml
    /// </summary>
    public partial class CreateNewDonor : Window
    {
        public string DonorFirstName { get; set; } //not
        public string DonorLastName { get; set; } //not
        public string DonorAddress1 { get; set; } //
        public string DonorAddress2 { get; set; } //
        public string DonorCity { get; set; } //
        public string DonorState { get; set; } //
        public string DonorType { get; set; } //Not
        public string DonorZip { get; set; } //
        public string OrganizationName { get; set; } //
        public CreateNewDonor()
        {
            InitializeComponent();
        }

        private void CreateDonor(object sender, RoutedEventArgs e)
        {
            Determine_DonorType(this.dType.SelectedIndex);
            //DonorFirstName != null && DonorFirstName != "" && DonorLastName != null && DonorLastName != ""
            if (DonorType != null && DonorType != "")
            {
                FCS_FundingContext db = new FCS_FundingContext();
                if (DonorType == "Organization" || DonorType == "Government")
                {
                    MessageBox.Show(DonorAddress1 + "\n" + DonorAddress2 + "\n" + DonorCity + "\n" + DonorState + "\n" + DonorZip
                        + "\n" + DonorType + "\n" + OrganizationName);
                    Donor d = new Donor(DonorType, OrganizationName, DonorAddress1, DonorAddress2, DonorState, DonorCity, DonorZip);
                    db.Donors.Add(d);
                    db.SaveChanges();
                    MessageBox.Show("Successfully added Donor!");
                    this.Close();
                }
                else if(DonorType == "Individual")
                {
                    MessageBox.Show(DonorAddress1 + "\n" + DonorAddress2 + "\n" + DonorCity + "\n" + DonorState + "\n" + DonorZip
                        + "\n" + DonorType + "\n" + OrganizationName);
                    Donor d = new Donor(DonorType, OrganizationName, DonorAddress1, DonorAddress2, DonorState, DonorCity, DonorZip);
                    CreateIndividualContact cic = new CreateIndividualContact(d);
                    this.Close();
                    cic.Show();
                }
                //its anonymous
                else
                {

                }
            }
            //add both patient and household
            else
            {
                MessageBox.Show("Make sure you select Donor Type.");
            }

        }

        private void Determine_DonorType(int selection)
        {
            switch (selection)
            {
                case 0:
                    DonorType = "Organization"; break;
                case 1:
                    DonorType = "Individual"; break;
                case 2:
                    DonorType = "Anonymous"; break;
                case 3:
                    DonorType = "Government"; break;
            }
        }
    }
}

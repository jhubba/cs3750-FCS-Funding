﻿using FCS_DataTesting;
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
    /// Interaction logic for UpdatePatient.xaml
    /// </summary>
    public partial class UpdatePatient : Window
    {
        public int patientOQ { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string relationToHead { get; set; }
        public string gender { get; set; }

        //setter values
        private string ageGroup { get; set; }
        private string PatientGender { get; set; }
        private string ethnicGroup { get; set; }

        //Helper Variables
        private int headOfHousehold { get; set; }
        private DateTime date { get; set; }

        public UpdatePatient(PatientGrid p)
        {
            //var check = sender as CheckBox;
            //check.IsChecked = p.IsHead;
            //check.SetCurrentValue(CheckBox.IsCheckedProperty, p.IsHead);
            //TheHead.SetCurrentValue
            firstName = p.FirstName;
            lastName = p.LastName;
            patientOQ = p.PatientOQ;
            relationToHead = p.RelationToHead;
            date = p.Time;
            InitializeComponent();
        }

        private void Update_Patient(object sender, RoutedEventArgs e)
        {
            FCS_Funding.Models.FCS_FundingContext db = new FCS_Funding.Models.FCS_FundingContext();
            int patID = db.Patients.Where(x => x.PatientOQ == patientOQ).Select(x => x.PatientID).Distinct().First();
            //MessageBox.Show(patID.ToString());
            //var orig = db.Patients.Find(patID);
            //if(orig != null)
            //{
            //    Determine_Gender(this.Gender.SelectedIndex);
            //    Determine_AgeGroup(this.AgeGroup.SelectedIndex);
            //    Determine_EthnicGroup(this.Ethnicity.SelectedIndex);
            //    orig.PatientOQ = patientOQ;
            //    orig.PatientFirstName = firstName;
            //    orig.PatientLastName = lastName;
            //    orig.RelationToHead = relationToHead;
            //    orig.PatientGender = PatientGender;
            //    orig.PatientAgeGroup = ageGroup;
            //    orig.PatientEthnicity = ethnicGroup;
            //    orig.IsHead = TheHead.IsChecked.Value;
            //    db.SaveChanges();
            //    MessageBox.Show("You have updated " + firstName + " " + lastName);
            //}
            //int householdID = db.Patients.Where(x => x.PatientOQ == patientOQ).Select(x => x.HouseholdID).Distinct().First();
            //FCS_Funding.Models.Patient update = new FCS_Funding.Models.Patient(patientOQ, householdID, firstName, lastName, PatientGender,
            //    ageGroup, ethnicGroup, date, TheHead.IsChecked.Value, relationToHead);
            //db.Patients.Attach(update);
            //var entry = db.Entry(update);
        }
        private void Determine_Gender(int selection)
        {
            switch (selection)
            {
                case 0:
                    PatientGender = "Male"; break;
                case 1:
                    PatientGender = "Female"; break;
                case 2:
                    PatientGender = "Other"; break;
            }
        }
        private void Determine_AgeGroup(int selection)
        {
            switch (selection)
            {
                case 0:
                    ageGroup = "0-5"; break;
                case 1:
                    ageGroup = "6-11"; break;
                case 2:
                    ageGroup = "12-17"; break;
                case 3:
                    ageGroup = "18-23"; break;
                case 4:
                    ageGroup = "24-44"; break;
                case 5:
                    ageGroup = "45-54"; break;
                case 6:
                    ageGroup = "55-69"; break;
                case 7:
                    ageGroup = "70+"; break;
            }
        }

        private void Determine_EthnicGroup(int selection)
        {
            switch (selection)
            {
                case 0:
                    ethnicGroup = "African American"; break;
                case 1:
                    ethnicGroup = "Native/Alaskan"; break;
                case 2:
                    ethnicGroup = "Pacific Islander"; break;
                case 3:
                    ethnicGroup = "Asian"; break;
                case 4:
                    ethnicGroup = "Caucasian"; break;
                case 5:
                    ethnicGroup = "Hispanic"; break;
                case 6:
                    ethnicGroup = "Other"; break;
            }
        }
    }
}

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Diagnostics;

namespace FCS_Funding.Views.Windows
{
    using FCS_Funding;
    using System.Windows.Controls.Primitives;
    using Models;
    using System.Linq;

    /// <summary>
    /// Interaction logic for AddNewClient.xaml
    /// </summary>
    /// 

    public class ProbCheckBoxModel
    {
        int PatientID { get; set; }
        int ProblemID { get; set; }
    }
    
    public partial class AddNewClient : Window
    {
        public int patientID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public Boolean headOfHouse { get; set; }
        public string PatientGender { get; set; }
        public DateTime date { get; set; }
        public string familyOQNumber { get; set; }
        public string relationToHead { get; set; }
        public string ageGroup { get; set; }
        public string ethnicGroup { get; set; }
        //	household properties
        public string Income { get; set; }
        public int HouseholdPopulation { get; set; }
        public string County { get; set; }

        public string searchName { get; set; }

        public AddNewClient()
        {

            HouseholdPopulation = 1;
            InitializeComponent();

        }

        protected bool ValidateData()
        {
            var dataValid = true;
            DateTime ClientIntakeDateTime = DateTime.Now;

            try
            {
                DateTime.TryParse(date_ClientCreationDate.Text, out ClientIntakeDateTime);
            }
            catch (Exception e)
            {
                MessageBox.Show("Please enter a valid Client Creation date");
            }
            
            if (firstName == null || lastName == null || PatientGender == null || ClientIntakeDateTime == null || ageGroup == null || ethnicGroup == null)
            {
                dataValid = false;
            }

            if (headOfHouse == true && (Income == null || HouseholdPopulation == 0 || County == null))
            {
                dataValid = false;
            }

            return dataValid;

        }
                
        private void radio_isHead_Checked(object sender, RoutedEventArgs e)
        {
            var rb = sender as RadioButton;
            if (rb.Name.Equals("radio_Dependent"))
            {
                textbox_RelationToHead.IsEnabled = true;
                textbox_SearchHead.IsEnabled = true;
                headOfHouse = false;
            }

            if (rb.Name.Equals("radio_Head"))
            {
                textbox_RelationToHead.IsEnabled = false;
                textbox_SearchHead.IsEnabled = false;
                textbox_HouseholdPopulation.IsEnabled = true;
                combobox_County.IsEnabled = true;
                combobox_IncomeBracket.IsEnabled = true;
                headOfHouse = true;

            }
        }

        private void useEnterAsTab(object sender, System.Windows.Input.KeyEventArgs e)
        {
            CommonControl.IntepretEnterAsTab(sender, e);
        }
                
        private void button_SearchHeadOfHousehold_Click(object sender, RoutedEventArgs e)
        {
       
            if (searchName != null)
            {
              // code to be added later 
               
            }
        }

        private void txt_NumberOnlyCheck(object sender, TextCompositionEventArgs e)
        {
            CommonControl.NumberOnlyEventCheckNoPeriod(sender, e);
        }

        private void Close_Window(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void button_AddUpdateClient_Click(object sender, RoutedEventArgs e)
        {
            if (headOfHouse)
            {

                bool validData = ValidateData();
                if (validData)
                {
                    InsertToDatabase();
                }
                else
                {
                    MessageBox.Show("Please fill out all of the form");
                }
            }
        }

        private void InsertToDatabase()
        {
            Patient tempPatient = new Patient();
            DateTime ClientIntakeDateTime = DateTime.Now;
            FCS_DBModel db = new FCS_DBModel();

            tempPatient.PatientOQ = GenerateOQ();
            tempPatient.PatientFirstName = firstName;
            tempPatient.PatientLastName = lastName;
            tempPatient.PatientGender = PatientGender;
            tempPatient.PatientAgeGroup = ageGroup;
            tempPatient.PatientEthnicity = ethnicGroup;
            tempPatient.IsHead = headOfHouse;
            tempPatient.NewClientIntakeHour = ClientIntakeDateTime;
            tempPatient.RelationToHead = (headOfHouse) ? "Head" : relationToHead;

            if (headOfHouse == true)
            {
                tempPatient.HouseholdID = (headOfHouse) ? NewHousehold() : 1;  // change after search has been added
            }

            try
            {
                db.Patients.Add(tempPatient);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error: " + ex.Message);
                throw;
            }

            Determine_Problems(tempPatient.PatientOQ);

            this.Close();

        }


        public string GenerateOQ()
        {
            FCS_DBModel db = new FCS_DBModel();

            string patientOQ = "1";

            try
            {
                string lastPatientOQ = db.Patients.OrderByDescending(x => x.PatientOQ).Select(x => x.PatientOQ).FirstOrDefault();
                if (lastPatientOQ == null)
                {
                    patientOQ = "1";
                    return patientOQ;
                }
                else
                {
                    int newPatientOQ = int.Parse(lastPatientOQ) + 1;
                    patientOQ = Convert.ToString(newPatientOQ);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error: " + ex.Message);
                throw;
            }

            return patientOQ;
        }

        /*
         * adds new household to database if member is head
         */
        public int NewHousehold()
        {
            PatientHousehold household = new PatientHousehold();
            FCS_DBModel db = new FCS_DBModel();


            household.HouseholdCounty = County;
            household.HouseholdIncomeBracket = Income;
            household.HouseholdPopulation = HouseholdPopulation;
            db.PatientHouseholds.Add(household);
            db.SaveChanges();

            return household.HouseholdID;
        }

        /*
         * adds Patient problems to database
         */
        public void Determine_Problems(string OQ)
        {
            FCS_DBModel db = new FCS_DBModel();
            var toggle = PatientProblemsCheckBoxes.Children;
            var problemTable = db.Problems;

            foreach (var item in toggle)
            {
                string checkboxProblemName = (((ContentControl)item).Content).ToString();
                if (((ToggleButton)item).IsChecked == true)
                {
                    try
                    {
                        PatientProblem patProb = new PatientProblem();
                        int patID = db.Patients.Where(x => x.PatientOQ == OQ).Select(x => x.PatientID).Distinct().First();
                        patProb.PatientID = patID;
                        patProb.ProblemID = GetProblemID(checkboxProblemName);
                        db.PatientProblems.Add(patProb);
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Database: " + ex);
                    }
                }
            }
        }


        private int GetProblemID(string strProblemName)
        {
            switch (strProblemName)
            {
                case "Depression":
                    return 1;
                case "Bereavement/Loss":
                    return 2;
                case "Communication":
                    return 3;
                case "Domestic Violence":
                    return 4;
                case "Hopelessness":
                    return 5;
                case "Work Problems":
                    return 6;
                case "Parent Problems":
                    return 7;
                case "Substance Abuse":
                    return 8;
                case "Problems w/ School":
                    return 9;
                case "Marriage/Relationship/Family":
                    return 10;
                case "Thoughts of Hurting Self":
                    return 11;
                case "Angry Feelings":
                    return 12;
                case "Sexual Abuse":
                    return 13;
                case "Emotional Abuse":
                    return 14;
                case "Physical Abuse":
                    return 15;
                case "Problems with the Law":
                    return 16;
                case "Unhappy with Life":
                    return 17;
                case "Anxiety":
                    return 18;
                case "Other":
                    return 19;
            }

            return -1;
        }

        private void AddNewSession(object sender, RoutedEventArgs e)
        {

        }

    }
}

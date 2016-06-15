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
using System.Windows.Navigation;
using System.Windows.Shapes;
using FCS_Funding.Views;
using FCS_DataTesting;
using System.Collections.ObjectModel;
using FCS_Funding.Models;
using System.Data;
using System.Data.Objects.SqlClient;
using System.Data.Entity;
using System.Windows.Forms;
namespace FCS_Funding.Views.UserControls
{
    /// <summary>
    /// Interaction logic for Report_Demographics.xaml
    /// </summary>
    public partial class Report_Demographics : System.Windows.Controls.UserControl
    {
        public class demoReport
        {
            public int newClients { get; set; }
            public int ongoingClients { get; set; }
            public int totalClients { get; set; }
            public int individualSessions { get; set; }
            public int familySessions { get; set; }
            public int groupSessions { get; set; }
            public int headOfHouseMale { get; set; }
            public int headOfHouseFemale { get; set; }
            public int totalFamiles { get; set; }
            public int totalFamilyMembers { get; set; }
            public int totalHoursOfService { get; set; }
            public int ageGroup0 { get; set; }
            public int ageGroup1 { get; set; }
            public int ageGroup2 { get; set; }
            public int ageGroup3 { get; set; }
            public int ageGroup4 { get; set; }
            public int ageGroup5 { get; set; }
            public int ageGroup6 { get; set; }
            public int ageGroup7 { get; set; }
            public int genderOfClientMale { get; set; }
            public int genderOfClientFemale { get; set; }
            public int genderOfClientTotal { get; set; }
            public int ethnicityGroup0 { get; set; }
            public int ethnicityGroup1 { get; set; }
            public int ethnicityGroup2 { get; set; }
            public int ethnicityGroup3 { get; set; }
            public int ethnicityGroup4 { get; set; }
            public int ethnicityGroup5 { get; set; }
            public int ethnicityGroup6 { get; set; }
            public int incomeGroup0 { get; set; }
            public int incomeGroup1 { get; set; }
            public int incomeGroup2 { get; set; }
            public int incomeGroup3 { get; set; }
            public int incomeGroup4 { get; set; }
            public int incomeGroup5 { get; set; }
            public int countyGroup0 { get; set; }
            public int countyGroup1 { get; set; }
            public int countyGroup2 { get; set; }
            public int countyGroup3 { get; set; }
            public int countyGroup4 { get; set; }
            public int countyGroup5 { get; set; }
        }

        public Report_Demographics()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            demoReport reportThis = new demoReport();

            /*int DemographicsReportDayStart, DemographicsReportDayEnd, DemographicsReportMonthStart, DemographicsReportMonthEnd, DemographicsReportYear;
            if (DemographicsReportMonth_comboBox.Text == "All")
            {
                DemographicsReportDayStart = 1;
                DemographicsReportDayEnd = DateTime.DaysInMonth(int.Parse(DemographicsReportYear_comboBox.Text), 12);
                DemographicsReportMonthStart = 1;
                DemographicsReportMonthEnd = 12;
                DemographicsReportYear = Int32.Parse(DemographicsReportYear_comboBox.Text);
            }
            else
            {
                DemographicsReportDayStart = 1;
                DemographicsReportDayEnd = DateTime.DaysInMonth(Int32.Parse(DemographicsReportYear_comboBox.Text), DemographicsReportMonth_comboBox.SelectedIndex);
                DemographicsReportMonthStart = DemographicsReportMonth_comboBox.SelectedIndex;
                DemographicsReportMonthEnd = DemographicsReportMonth_comboBox.SelectedIndex;
                DemographicsReportYear = Int32.Parse(DemographicsReportYear_comboBox.Text);
            }
            DateTime requestedDateStart = Convert.ToDateTime(DemographicsReportMonthStart + "/" + DemographicsReportDayStart + "/" + DemographicsReportYear);
            DateTime requestedDateEnd = Convert.ToDateTime(DemographicsReportMonthEnd + "/" + DemographicsReportDayEnd + "/" + DemographicsReportYear);
            */
            //MessageBox.Show("Start: " + requestedDateStart + " End: " + requestedDateEnd);
            if (demographicsReportFrom_datepicker.SelectedDate != null && demographicsReportFrom_datepicker.SelectedDate.GetValueOrDefault() != DateTime.MinValue && demographicsReportTo_datepicker.SelectedDate != null && demographicsReportTo_datepicker.SelectedDate.GetValueOrDefault() != DateTime.MinValue)
            {
                //Console.WriteLine("Trying to query database");
                var db = new FCS_DBModel();
                DateTime requestedDateStart = DateTime.Parse(demographicsReportFrom_datepicker.Text);
                DateTime requestedDateEnd = DateTime.Parse(demographicsReportTo_datepicker.Text);
                /*
                                var listOfAllSessions = from ap in db.Appointments
                           join st in db.Staff on ap.StaffID equals st.StaffID
                           join ex in db.Expenses on ap.AppointmentID equals ex.AppointmentID
                           join et in db.ExpenseTypes on ex.ExpenseTypeID equals et.ExpenseTypeID
                           join pa in db.Patients on ex.PatientID equals pa.PatientID
                           join ph in db.PatientHouseholds on pa.HouseholdID equals ph.HouseholdID
                           where ap.AppointmentStartDate > requestedDateStart && ap.AppointmentEndDate < requestedDateEnd
                           select new
                           {
                               patientID = pa.PatientID,
                               patientFirstName = pa.PatientFirstName,
                               patientLastName = pa.PatientLastName,
                               staff = st.StaffLastName + ", " + st.StaffFirstName,
                               patientIntakeTimeDate = pa.NewClientIntakeHour,
                               appointmentDateTimeStart = ap.AppointmentStartDate,
                               appointmentDateTimeEnd = ap.AppointmentEndDate,
                               gender = pa.PatientGender,
                               ethnicity = pa.PatientEthnicity,
                               patientIncome = ph.HouseholdIncomeBracket,
                               patientCounty = ph.HouseholdCounty,
                               isHeadOfHousehold = pa.IsHead,
                               householdID = pa.HouseholdID,
                               householdCount = ph.HouseholdPopulation,
                               ageGroup = pa.PatientAgeGroup,
                               fundingSource = ex.DonationID,
                               expenseTypeID = ex.ExpenseTypeID,
                               typeofCx = ap.AppointmentCancelationType,
                               patientProblems = (from pp in db.PatientProblems
                                                  where pp.PatientID == pa.PatientID
                                                  select new
                                                  {
                                                      problemID = pp.PatientProblemID
                                                  })
                           };
                           join st in db.Staff on ap.StaffID equals st.StaffID
                           join ex in db.Expenses on ap.AppointmentID equals ex.AppointmentID
                           join et in db.ExpenseTypes on ex.ExpenseTypeID equals et.ExpenseTypeID
                           join pa in db.Patients on ex.PatientID equals pa.PatientID
                           join ph in db.PatientHouseholds on pa.HouseholdID equals ph.HouseholdID
                */

                var listOfAllMatchingPatients = (from ap in db.Appointments
                                                 join ex in db.Expenses on ap.AppointmentID equals ex.AppointmentID
                                                 join pa in db.Patients on ex.PatientID equals pa.PatientID
                                                 where ap.AppointmentStartDate > requestedDateStart && ap.AppointmentEndDate < requestedDateEnd
                                                 group pa by pa.PatientID into list
                                                 select new
                                                 {
                                                     patientID = list.Key,
                                                     patientInformation = (from pat in db.Patients
                                                                           where pat.PatientID == list.Key
                                                                           select new
                                                                           {
                                                                               patientFirstName = pat.PatientFirstName,
                                                                               patientLastName = pat.PatientLastName,
                                                                               ageGroup = pat.PatientAgeGroup,
                                                                               gender = pat.PatientGender,
                                                                               ethnicity = pat.PatientEthnicity,
                                                                               patientIntakeTimeDate = pat.NewClientIntakeHour,
                                                                               newClient = false
                                                                           }),
                                                     sessionInformation = (from app in db.Appointments
                                                                           join exp in db.Expenses on app.AppointmentID equals exp.AppointmentID
                                                                           join st in db.Staff on app.StaffID equals st.StaffID
                                                                           where app.AppointmentStartDate > requestedDateStart && app.AppointmentEndDate < requestedDateEnd && exp.PatientID == list.Key
                                                                           orderby requestedDateStart
                                                                           select new
                                                                           {
                                                                               staff = st.StaffLastName + ", " + st.StaffFirstName,
                                                                               appointmentDateTimeStart = app.AppointmentStartDate,
                                                                               appointmentDateTimeEnd = app.AppointmentEndDate,
                                                                               fundingSource = exp.DonationID,
                                                                               expenseTypeID = exp.ExpenseTypeID,
                                                                               typeofCx = app.AppointmentCancelationType
                                                                           }),
                                                     houseHoldInformation = (from pat in db.Patients
                                                                             join ph in db.PatientHouseholds on pat.HouseholdID equals ph.HouseholdID
                                                                             where pat.PatientID == list.Key
                                                                             select new
                                                                             {
                                                                                 patientIncome = ph.HouseholdIncomeBracket,
                                                                                 patientCounty = ph.HouseholdCounty,
                                                                                 isHeadOfHousehold = pat.IsHead,
                                                                                 householdID = pat.HouseholdID,
                                                                                 householdCount = ph.HouseholdPopulation,
                                                                                 ageGroup = pat.PatientAgeGroup
                                                                             }),
                                                     patientProblems = (from pp in db.PatientProblems
                                                                        where pp.PatientID == list.Key
                                                                        select new
                                                                        {
                                                                            problemID = pp.ProblemID
                                                                        })
                                                 }).ToList();

                //FILTER BASED ON CLIENT TYPE
                var NEWlistOfAllMatchingPatients = listOfAllMatchingPatients;
                var asd = 1;
                foreach (var query in listOfAllMatchingPatients.AsEnumerable().Reverse().ToList())
                //for (int i = 0; i < listOfAllMatchingPatients.Count(); i++)
                {
                    //var query = listOfAllMatchingPatients[i];
                    var patientInformation = query.patientInformation.AsEnumerable().ToList();
                    var sessionInformation = query.sessionInformation.AsEnumerable().ToList();
                    var houseHoldInformation = query.houseHoldInformation.AsEnumerable().ToList();
                    var patientProblems = query.patientProblems.AsEnumerable().ToList();

                    //FILTER BASED ON CLIENTTYPE
                    DateTime patientIntakeTimeDate = patientInformation.Single().patientIntakeTimeDate;
                    DateTime patientFirstSession = sessionInformation.First().appointmentDateTimeStart;
                    if (patientIntakeTimeDate < patientFirstSession)
                    {
                        if (demoNew_checkBox.IsChecked.Value == false || !demoNew_checkBox.IsChecked.HasValue)
                        {
                            //listOfAllMatchingPatients.RemoveAt(i--);
                            listOfAllMatchingPatients.Remove(query);
                            break;
                        }
                    }
                    else
                    {
                        if (demoOngoing_checkBox.IsChecked.Value == false || !demoOngoing_checkBox.IsChecked.HasValue)
                        {
                            //listOfAllMatchingPatients.RemoveAt(i--);
                            listOfAllMatchingPatients.Remove(query);
                            break;
                        }
                    }

                    //FILTER BASED ON SESSIONTYPE
                    foreach (var session in sessionInformation.AsEnumerable().Reverse().ToList())
                    {
                        if (demoindividual_checkBox.IsChecked.Value == false || !demoindividual_checkBox.IsChecked.HasValue)
                        {
                            if (session.expenseTypeID == 1)
                            {
                                sessionInformation.Remove(session);
                            }
                        }
                        if (demoFamily_checkBox.IsChecked.Value == false || !demoFamily_checkBox.IsChecked.HasValue)
                        {

                            if (session.expenseTypeID == 3)
                            {
                                sessionInformation.Remove(session);
                            }
                        }
                        if (demoGroup_checkBox.IsChecked.Value == false || !demoGroup_checkBox.IsChecked.HasValue)
                        {
                            if (session.expenseTypeID == 2)
                            {
                                sessionInformation.Remove(session);
                            }
                        }
                    }

                    //FILTER BASED ON HoH
                    if (demoHOHMale_checkBox.IsChecked.Value == false || !demoHOHMale_checkBox.IsChecked.HasValue)
                    {
                        if (patientInformation.Single().gender == "Male" && houseHoldInformation.Single().isHeadOfHousehold == true)
                        {
                            listOfAllMatchingPatients.Remove(query);
                            break;
                        }
                    }
                    if (demoHOHFemale_checkBox.IsChecked.Value == false || !demoHOHFemale_checkBox.IsChecked.HasValue)
                    {
                        if (patientInformation.Single().gender == "Female" && houseHoldInformation.Single().isHeadOfHousehold == true)
                        {
                            listOfAllMatchingPatients.Remove(query);
                            break;
                        }
                    }

                    //FILTER BASED ON AGE
                    if (demoAge05_checkBox.IsChecked.Value == false || !demoAge05_checkBox.IsChecked.HasValue)
                    {
                        if (patientInformation.Single().ageGroup == "0-5")
                        {
                            listOfAllMatchingPatients.Remove(query);
                            break;
                        }
                    }
                    if (demoAge611_checkBox.IsChecked.Value == false || !demoAge611_checkBox.IsChecked.HasValue)
                    {
                        if (patientInformation.Single().ageGroup == "6-11")
                        {
                            listOfAllMatchingPatients.Remove(query);
                            break;
                        }
                    }
                    if (demoAge1217_checkBox.IsChecked.Value == false || !demoAge1217_checkBox.IsChecked.HasValue)
                    {
                        if (patientInformation.Single().ageGroup == "12-17")
                        {
                            listOfAllMatchingPatients.Remove(query);
                            break;
                        }
                    }
                    if (demoAge1823_checkBox.IsChecked.Value == false || !demoAge1823_checkBox.IsChecked.HasValue)
                    {
                        if (patientInformation.Single().ageGroup == "18-23")
                        {
                            listOfAllMatchingPatients.Remove(query);
                            break;
                        }
                    }
                    if (demoAge2444_checkBox.IsChecked.Value == false || !demoAge2444_checkBox.IsChecked.HasValue)
                    {
                        if (patientInformation.Single().ageGroup == "24-44")
                        {
                            listOfAllMatchingPatients.Remove(query);
                            break;
                        }
                    }
                    if (demoAge4554_checkBox.IsChecked.Value == false || !demoAge4554_checkBox.IsChecked.HasValue)
                    {
                        if (patientInformation.Single().ageGroup == "45-54")
                        {
                            listOfAllMatchingPatients.Remove(query);
                            break;
                        }
                    }
                    if (demoAge5569_checkBox.IsChecked.Value == false || !demoAge5569_checkBox.IsChecked.HasValue)
                    {
                        if (patientInformation.Single().ageGroup == "55+")
                        {
                            listOfAllMatchingPatients.Remove(query);
                            break;
                        }
                    }
                    if (demoAge70_checkBox.IsChecked.Value == false || !demoAge70_checkBox.IsChecked.HasValue)
                    {
                        if (patientInformation.Single().ageGroup == "70+")
                        {
                            listOfAllMatchingPatients.Remove(query);
                            break;
                        }
                    }

                    //FILTER BASED ON GENDER
                    if (demoGenderMale_checkBox.IsChecked.Value == false || !demoGenderMale_checkBox.IsChecked.HasValue)
                    {
                        if (patientInformation.Single().gender == "Male")
                        {
                            listOfAllMatchingPatients.Remove(query);
                            break;
                        }
                    }
                    if (demoGenderFemale_checkBox.IsChecked.Value == false || !demoGenderFemale_checkBox.IsChecked.HasValue)
                    {
                        if (patientInformation.Single().gender == "Female")
                        {
                            listOfAllMatchingPatients.Remove(query);
                            break;
                        }
                    }

                    //FILTER BASED ON ETHNICITY
                    if (demoEthnicityAA_checkBox.IsChecked.Value == false || !demoEthnicityAA_checkBox.IsChecked.HasValue)
                    {
                        if (patientInformation.Single().ethnicity == "African American")
                        {
                            listOfAllMatchingPatients.Remove(query);
                            break;
                        }
                    }
                    if (demoEthnicityNATALASK_checkBox.IsChecked.Value == false || !demoEthnicityNATALASK_checkBox.IsChecked.HasValue)
                    {
                        if (patientInformation.Single().ethnicity == "Native/Alaskan")
                        {
                            listOfAllMatchingPatients.Remove(query);
                            break;
                        }
                    }
                    if (demoEthnicityPACISL_checkBox.IsChecked.Value == false || !demoEthnicityPACISL_checkBox.IsChecked.HasValue)
                    {
                        if (patientInformation.Single().ethnicity == "Pacific Islander")
                        {
                            listOfAllMatchingPatients.Remove(query);
                            break;
                        }
                    }
                    if (demoEthnicityASIAN_checkBox.IsChecked.Value == false || !demoEthnicityASIAN_checkBox.IsChecked.HasValue)
                    {
                        if (patientInformation.Single().ethnicity == "Asian")
                        {
                            listOfAllMatchingPatients.Remove(query);
                            break;
                        }
                    }
                    if (demoEthnicityCAUC_checkBox.IsChecked.Value == false || !demoEthnicityCAUC_checkBox.IsChecked.HasValue)
                    {
                        if (patientInformation.Single().ethnicity == "Caucasian")
                        {
                            listOfAllMatchingPatients.Remove(query);
                            break;
                        }
                    }
                    if (demoEthnicityHISP_checkBox.IsChecked.Value == false || !demoEthnicityHISP_checkBox.IsChecked.HasValue)
                    {
                        if (patientInformation.Single().ethnicity == "Hispanic")
                        {
                            listOfAllMatchingPatients.Remove(query);
                            break;
                        }
                    }
                    if (demoEthnicityOther_checkBox.IsChecked.Value == false || !demoEthnicityOther_checkBox.IsChecked.HasValue)
                    {
                        if (patientInformation.Single().ethnicity == "Other")
                        {
                            listOfAllMatchingPatients.Remove(query);
                            break;
                        }
                    }

                    //FILTER BASED ON INCOME
                    if (demoIncome09999_checkBox.IsChecked.Value == false || !demoIncome09999_checkBox.IsChecked.HasValue)
                    {
                        if (houseHoldInformation.Single().patientIncome == "$0-9,999")
                        {
                            listOfAllMatchingPatients.Remove(query);
                            break;
                        }
                    }
                    if (demoIncome1000014999_checkBox.IsChecked.Value == false || !demoIncome1000014999_checkBox.IsChecked.HasValue)
                    {
                        if (houseHoldInformation.Single().patientIncome == "$10,000-14,999")
                        {
                            listOfAllMatchingPatients.Remove(query);
                            break;
                        }
                    }
                    if (demoIncome1500024000_checkBox.IsChecked.Value == false || !demoIncome1500024000_checkBox.IsChecked.HasValue)
                    {
                        if (houseHoldInformation.Single().patientIncome == "$15,000-24,999")
                        {
                            listOfAllMatchingPatients.Remove(query);
                            break;
                        }
                    }
                    if (demoIncome2500034999_checkBox.IsChecked.Value == false || !demoIncome2500034999_checkBox.IsChecked.HasValue)
                    {
                        if (houseHoldInformation.Single().patientIncome == "$25,000-34,999")
                        {
                            listOfAllMatchingPatients.Remove(query);
                            break;
                        }
                    }
                    if (demoIncome35000_checkBox.IsChecked.Value == false || !demoIncome35000_checkBox.IsChecked.HasValue)
                    {
                        if (houseHoldInformation.Single().patientIncome == "$35,000+")
                        {
                            listOfAllMatchingPatients.Remove(query);
                            break;
                        }
                    }

                    //FILTER BASED ON COUNTY
                    if (demoCountyWeber_checkBox.IsChecked.Value == false || !demoCountyWeber_checkBox.IsChecked.HasValue)
                    {
                        if (houseHoldInformation.Single().patientCounty == "Weber")
                        {
                            listOfAllMatchingPatients.Remove(query);
                            break;
                        }
                    }
                    if (demoCountyDavis_checkBox.IsChecked.Value == false || !demoCountyDavis_checkBox.IsChecked.HasValue)
                    {
                        if (houseHoldInformation.Single().patientCounty == "Davis")
                        {
                            listOfAllMatchingPatients.Remove(query);
                            break;
                        }
                    }
                    if (demoCountyDCLC_checkBox.IsChecked.Value == false || !demoCountyDCLC_checkBox.IsChecked.HasValue)
                    {
                        if (houseHoldInformation.Single().patientCounty == "DCLC")
                        {
                            listOfAllMatchingPatients.Remove(query);
                            break;
                        }
                    }
                    if (demoCountyMorgan_checkBox.IsChecked.Value == false || !demoCountyMorgan_checkBox.IsChecked.HasValue)
                    {
                        if (houseHoldInformation.Single().patientCounty == "Morgan")
                        {
                            listOfAllMatchingPatients.Remove(query);
                            break;
                        }
                    }
                    if (demoCountyBoxElder_checkBox.IsChecked.Value == false || !demoCountyBoxElder_checkBox.IsChecked.HasValue)
                    {
                        if (houseHoldInformation.Single().patientCounty == "Box Elder")
                        {
                            listOfAllMatchingPatients.Remove(query);
                            break;
                        }
                    }
                    if (demoCountyOther_checkBox.IsChecked.Value == false || !demoCountyOther_checkBox.IsChecked.HasValue)
                    {
                        if (houseHoldInformation.Single().patientCounty == "Other")
                        {
                            listOfAllMatchingPatients.Remove(query);
                            break;
                        }
                    }

                    //FILTER BASED ON Problem Type

                    //FILTER BASED ON Funding Type

                    //PURGE ALL PEOPLE WITH NO SESSIONS.  SOME SESSIONS MIGHT HAVE BEEN DELETED DUE TO FILTERS.
                    //ITERATE OVER LIST AND DO A COUNT TO FIND PEOPLE WITH 0 SESSIONS
                }
                listOfAllMatchingPatients = listOfAllMatchingPatients;

                int newPatients = 0;
                int ongoingPatients = 0;
                double totalMinutesofService = 0;
                double totalMinutesofClientService = 0;
                var individualSessions = 0;
                var groupSessions = 0;
                var familySessions = 0;
                //HoH
                var hoHMaleCount = 0;
                var hoHFemaleCount = 0;
                var hoHTotalFamiles = 0;
                var hoHIndividuals = 0;
                //Age count
                int[] ageTotals = {0,0,0,0,0,0,0,0};
                //Count based on gender
                int totalMales = 0;
                int totalFemales = 0;
                //Ethnicity count
                int[] totalEthnicity = {0, 0, 0, 0, 0, 0, 0};
                int[] totalIncome = { 0, 0, 0, 0, 0 };
                int[] totalCounty = { 0, 0, 0, 0, 0, 0 };

                //This has to be determined to be used in the for loop below.
                var listOfAllKnownProblems = (from pr in db.Problems
                                              orderby pr.ProblemID
                                              select new
                                              {
                                                  problemID = pr.ProblemID,
                                                  problemType = pr.ProblemType,

                                              });
                int[] arrayOfProblemCounts = new int[20];

                var listOfAllKnownFunding = (from don in db.Donations.AsEnumerable()
                                         join donor in db.Donors on don.DonationID equals donor.DonorID
                                         where don.GrantProposalID == null
                                         select new
                                         {
                                             fundingSourceID = don.DonationID,
                                             fundingSource = donor.OrganizationName
                                         }).ToList();
                var listOfAllKnownFundingGP = (from don in db.Donations.AsEnumerable()
                                             join gp in db.GrantProposals on don.GrantProposalID equals gp.GrantProposalID
                                             where don.GrantProposalID != null
                                             select new
                                             {
                                                 fundingSourceID = don.DonationID,
                                                 fundingSource = gp.GrantName
                                             }).ToList();
                listOfAllKnownFunding.AddRange(listOfAllKnownFundingGP);
                int[,] arrayOfFundingCounts = new int[listOfAllKnownFunding.Count(),2];
                int x = 0;
                foreach (var ses in listOfAllKnownFunding)
                {
                    arrayOfFundingCounts[x,0] = ses.fundingSourceID;
                    //arrayOfFundingCounts[x,1] = 0;
                    x++;
                }

                int[] arrayOfCancellations = new int[3];
                //Loop through all patients pulled from database with sessions matching the dates specified by the end user
                foreach (var query in listOfAllMatchingPatients)
                {
                    var patientInformation = query.patientInformation.ToList();
                    var sessionInformation = query.sessionInformation.ToList();
                    var houseHoldInformation = query.houseHoldInformation.ToList();
                    var patientProblems = query.patientProblems.ToList();

                    // Calculate new and ongoing client totals
                    DateTime patientIntakeTimeDate = patientInformation.Single().patientIntakeTimeDate;
                    DateTime patientFirstSession = sessionInformation.First().appointmentDateTimeStart;
                    if (patientIntakeTimeDate < patientFirstSession)
                    {
                        newPatients++;
                    }
                    else
                    {
                        ongoingPatients++;
                    }


                    // - Session times - Add all sessions together in minutes and convert to hours.
                    foreach (var session in sessionInformation)
                    {
                        if (session.expenseTypeID == 1)
                        {
                            individualSessions++;
                        }
                        else if (session.expenseTypeID == 2)
                        {
                            groupSessions++;
                        }
                        else if (session.expenseTypeID == 3)
                        {
                            familySessions++;
                        }
                        totalMinutesofClientService += session.appointmentDateTimeEnd.Subtract(session.appointmentDateTimeStart).TotalMinutes;  //Cannot add hours because sessions less than 1 hour will not be tallied.
                    }

                    //Total count per funding source.
                    foreach (var se in sessionInformation)
                    {
                        if (se.fundingSource != null)
                        {
                            for (var y = 0; y < listOfAllKnownFunding.Count(); y++)
                            {
                                if (arrayOfFundingCounts[y, 0].Equals(se.fundingSource))
                                {
                                    arrayOfFundingCounts[y, 1]++;
                                    break;
                                }
                            }
                        }
                        if (!se.typeofCx.Equals("Not Cxl"))
                        {
                            switch (se.typeofCx)
                            {
                                case "Lt Cxl":
                                    arrayOfCancellations[0]++;
                                    break;
                                case "Cxl":
                                    arrayOfCancellations[1]++;
                                    break;
                                case "No Show":
                                    arrayOfCancellations[2]++;
                                    break;
                            }
                        }
                    }
                    totalMinutesofService = totalMinutesofService + totalMinutesofClientService;
                    totalMinutesofClientService = 0;    //Reset for next client.

                    // Calculate Head of Household counts
                    if (houseHoldInformation.Single().isHeadOfHousehold && patientInformation.Single().gender == "Male")
                    {
                        hoHMaleCount++;
                    }
                    else if (houseHoldInformation.Single().isHeadOfHousehold && patientInformation.Single().gender == "Female")
                    {
                        hoHFemaleCount++;
                    }

                    hoHTotalFamiles += 1;
                    hoHIndividuals += houseHoldInformation.Single().householdCount;

                    //Total count per age group.
                    switch (patientInformation.Single().ageGroup)
                    {
                        case "0-5":
                            ageTotals[0]++;
                            break;
                        case "6-11":
                            ageTotals[1]++;
                            break;
                        case "12-17":
                            ageTotals[2]++;
                            break;
                        case "18-23":
                            ageTotals[3]++;
                            break;
                        case "24-44":
                            ageTotals[4]++;
                            break;
                        case "45-54":
                            ageTotals[5]++;
                            break;
                        case "55-69":
                            ageTotals[6]++;
                            break;
                        case "70+":
                            ageTotals[7]++;
                            break;
                    }
                    if (patientInformation.Single().gender == "Male")
                    {
                        totalMales++;
                    }
                    else if (patientInformation.Single().gender == "Female")
                    {
                        totalFemales++;
                    }
                    //Total count per age group.
                    switch (patientInformation.Single().ethnicity)
                    {
                        case "African American":
                            totalEthnicity[0]++;
                            break;
                        case "Native/Alaskan":
                            totalEthnicity[1]++;
                            break;
                        case "Pacific Islander":
                            totalEthnicity[2]++;
                            break;
                        case "Asian":
                            totalEthnicity[3]++;
                            break;
                        case "Caucasian":
                            totalEthnicity[4]++;
                            break;
                        case "Hispanic":
                            totalEthnicity[5]++;
                            break;
                        case "Other":
                            totalEthnicity[6]++;
                            break;
                    }
                    //Total count per income.
                    switch (houseHoldInformation.Single().patientIncome)
                    {
                        case "$0-9,999":
                            totalIncome[0]++;
                            break;
                        case "$10,000-14,999":
                            totalIncome[1]++;
                            break;
                        case "$15,000-24,999":
                            totalIncome[2]++;
                            break;
                        case "$25,000-34,999":
                            totalIncome[3]++;
                            break;
                        case "$35,000+":
                            totalIncome[4]++;
                            break;
                    }
                    //Total count per county.
                    switch (houseHoldInformation.Single().patientCounty)
                    {
                        case "Weber":
                            totalCounty[0]++;
                            break;
                        case "Davis":
                            totalCounty[1]++;
                            break;
                        case "DCLC":
                            totalCounty[2]++;
                            break;
                        case "Morgan":
                            totalCounty[3]++;
                            break;
                        case "Box Elder":
                            totalCounty[4]++;
                            break;
                        case "Other":
                            totalCounty[5]++;
                            break;
                    }

                    //Total problem count
                    foreach (var pr in patientProblems)
                    {
                        foreach (var ap in listOfAllKnownProblems)
                        {
                            if (pr.problemID.Equals(ap.problemID))
                            {
                                arrayOfProblemCounts[pr.problemID]++;
                            }
                        }
                    }

                    //arrayOfFundingCounts
                }

                //int ageBracket1 = houseHoldInformation.Single().Where(x => x.ageGroup.Contains("0-5")).Count();
                int totalPatients = newPatients + ongoingPatients;
                double totalHoursofService = totalMinutesofService / 60;


                // GENERATE HTML STRING OR FORMAT FOR REPORTING MECHANISM
                String toPrint = "<!DOCTYPE html>"
                + "<html>"
                + "<head>"
                + " <style>"
                + "header nav, footer {"
                + "   display: none;"
                + "}"
                + "body {"
                + "    font-size:11pt;"
                + "    margin: -40px;"
                + "}"
                + "</style>"
                + "</head>"
                + "<body>"
                + "<div style='position:relative;' id='wrap'>"
                + "    <div style='color:#000000;font-size:18pt;position:relative;left:25px;top:20px;'>Demographic Report</div>"
                + "    <div style='color:#000000;position:absolute;left:25px;top:80px;'>Staff Name: </div>"
                + "    <div style='color:#000000;position:absolute;left:425px;top:80px;'>Location: </div>"
                + "    <div style='color:#000000;position:absolute;left:25px;top:105px;'>New: " + newPatients + "</div>"
                + "    <div style='color:#000000;position:absolute;left:225px;top:105px;'>Ongoing: " + ongoingPatients + "</div>"
                + "    <div style='color:#000000;position:absolute;left:425px;top:105px;'>Total Clients: " + totalPatients + "</div>"
                + "    <div style='color:#000000;position:absolute;left:25px;top:130px;'>Individual: " + individualSessions + "</div>"
                + "    <div style='color:#000000;position:absolute;left:225px;top:130px;'>Family Sessions: " + familySessions + "</div>"
                + "    <div style='color:#000000;position:absolute;left:425px;top:130px;'>Groups: " + groupSessions + "</div>"
                + "    <div style='color:#000000;position:absolute;left:25px;top:155px;'>Head of household</div>"
                + "    <div style='color:#000000;position:absolute;left:225px;top:155px;'>M: " + hoHMaleCount + "</div>"
                + "    <div style='color:#000000;position:absolute;left:325px;top:155px;'>F: " + hoHFemaleCount + "</div>"
                + "    <div style='color:#000000;position:absolute;left:425px;top:155px;'>Total Families: " + familySessions + "</div>"
                + "    <div style='color:#000000;position:absolute;left:25px;top:180px;'># Individuals in the household: " + hoHIndividuals + "</div>"
                + "    <div style='color:#000000;position:absolute;left:600px;top:175px;'>LtCxl: " + arrayOfCancellations[0] + "</div>"
                + "    <div style='color:#000000;position:absolute;left:600px;top:195px;'>Cxl: " + arrayOfCancellations[1] + "</div>"
                + "    <div style='color:#000000;position:absolute;left:600px;top:214px;'>No Show: " + arrayOfCancellations[2] + "</div>"
                + "    <div style='color:#000000;position:absolute;left:25px;top:214px;'>Total Hours of service: " + totalHoursofService + "</div>"
                + "    <div style='color:#000000;position:absolute;left:25px;top:255px;'>Age:</div>"
                + "    <div style='color:#000000;position:absolute;left:150px;top:255px;'>1. 0-5: " + ageTotals[0] + "</div>"
                + "    <div style='color:#000000;position:absolute;left:300px;top:255px;'>2. 6-11: " + ageTotals[1] + "</div>"
                + "    <div style='color:#000000;position:absolute;left:450px;top:255px;'>3. 12-17: " + ageTotals[2] + "</div>"
                + "    <div style='color:#000000;position:absolute;left:600px;top:255px;'>4. 18-23: " + ageTotals[3] + "</div>"
                + "    <div style='color:#000000;position:absolute;left:150px;top:275px;'>5. 24-44: " + ageTotals[4] + "</div>"
                + "    <div style='color:#000000;position:absolute;left:300px;top:275px;'>6. 45-54: " + ageTotals[5] + "</div>"
                + "    <div style='color:#000000;position:absolute;left:450px;top:275px;'>7. 55-69: " + ageTotals[6] + "</div>"
                + "    <div style='color:#000000;position:absolute;left:600px;top:275px;'>8. 70+: " + ageTotals[7] + "</div>"
                + "    <div style='color:#000000;position:absolute;left:25px;top:295px;'>Gender of Client</div>"
                + "    <div style='color:#000000;position:absolute;left:225px;top:295px;'>M: " + totalMales + "</div>"
                + "    <div style='color:#000000;position:absolute;left:325px;top:295px;'>F: " + totalFemales + "</div>"
                + "    <div style='color:#000000;position:absolute;left:25px;top:320px;'>Ethnicity</div>"
                + "    <div style='color:#000000;position:absolute;left:150px;top:320px;'>1. African American: " + totalEthnicity[0] + "</div>"
                + "    <div style='color:#000000;position:absolute;left:340px;top:320px;'>2. Natice/Alaskan: " + totalEthnicity[1] + "</div>"
                + "    <div style='color:#000000;position:absolute;left:510px;top:320px;'>3. Pacific Islander: " + totalEthnicity[2] + "</div>"
                + "    <div style='color:#000000;position:absolute;left:700px;top:320px;'>4. Asian: " + totalEthnicity[3] + "</div>"
                + "    <div style='color:#000000;position:absolute;left:150px;top:340px;'>5. Caucasian: " + totalEthnicity[4] + "</div>"
                + "    <div style='color:#000000;position:absolute;left:340px;top:340px;'>6. Hispanic: " + totalEthnicity[5] + "</div>"
                + "    <div style='color:#000000;position:absolute;left:510px;top:340px;'>7. Other: " + totalEthnicity[6] + "</div>"
                + "    <div style='color:#000000;position:absolute;left:25px;top:380px;'>Income</div>"
                + "    <div style='color:#000000;position:absolute;left:150px;top:380px;'>1. $0-9,999: " + totalIncome[0] + "</div>"
                + "    <div style='color:#000000;position:absolute;left:300px;top:380px;'>2. $10,000-14,999: " + totalIncome[1] + "</div>"
                + "    <div style='color:#000000;position:absolute;left:450px;top:380px;'>3. $15,000-24,999: " + totalIncome[2] + "</div>"
                + "    <div style='color:#000000;position:absolute;left:600px;top:380px;'>4. $25,000-34,999: " + totalIncome[3] + "</div>"
                + "    <div style='color:#000000;position:absolute;left:150px;top:400px;'>5. $35,000+: " + totalIncome[4] + "</div>"
                + "    <div style='color:#000000;position:absolute;left:25px;top:425px;'>County:</div>"
                + "    <div style='color:#000000;position:absolute;left:125px;top:425px;'>1. Weber: " + totalCounty[0] + "</div>"
                + "    <div style='color:#000000;position:absolute;left:250px;top:425px;'>2. Davis: " + totalCounty[1] + "</div>"
                + "    <div style='color:#000000;position:absolute;left:375px;top:425px;'>3. DCLC: " + totalCounty[2] + "</div>"
                + "    <div style='color:#000000;position:absolute;left:500px;top:425px;'>4. Morgan: " + totalCounty[3] + "</div>"
                + "    <div style='color:#000000;position:absolute;left:625px;top:425px;'>5. Box Elder: " + totalCounty[4] + "</div>"
                + "    <div style='color:#000000;position:absolute;left:770px;top:425px;'>6. Other: " + totalCounty[5] + "</div>";

                String toPrintFunding = "    <div style='color:#000000;position:absolute;left:25px;top:455px;'>Funding Source:</div>";
                int fCount = 0;
                int xLoc = 150;
                int yLoc = 455;
                int totalCount = 1;
                foreach (var fu in listOfAllKnownFunding)
                {
                    toPrintFunding += "    <div style='color:#000000;position:absolute;left:" + xLoc + "px;top:" + yLoc + "px;'>" + totalCount + ". " + fu.fundingSource + ": " + arrayOfFundingCounts[totalCount-1,1] + "</div>";
                    if (fCount <= 5)    // 5 per row
                    {
                        xLoc += 125;
                        fCount++;
                    }
                    else
                    {
                        yLoc += 20;
                        xLoc = 150;
                        fCount = 0;
                    }
                    totalCount++;
                }
                yLoc += 40;
                String toPrinProblems = "     <div style='color:#000000;position:absolute;left:25px;top:" + yLoc + "px; '>Problem:</div>";
                int pCount = 1;
                totalCount = 1;
                xLoc = 45;
                yLoc += 20;
                foreach (var pr in listOfAllKnownProblems)
                {
                    toPrinProblems += "    <div style='color:#000000;position:absolute;left:" + xLoc + "px;top:" + yLoc + "px;'>" + totalCount + ". " + pr.problemType+ ": " + arrayOfProblemCounts[pr.problemID] + "</div>";
                    if (pCount <= 3)    // 3 per row
                    {
                        if (pCount == 1)
                        { xLoc += 195; }
                        else if (pCount == 2) { xLoc += 235; }
                        else { xLoc += 210; }
                        pCount++;
                    }
                    else
                    {
                        yLoc += 20;
                        xLoc = 45;
                        pCount = 1;
                    }
                    totalCount++;

                }
                toPrint += toPrintFunding + toPrinProblems;
                toPrint += "</body>"
                + "</html>";

                Console.WriteLine("newPatients: " + newPatients + " - ongoingPatients: " + ongoingPatients + " - Total: " + totalPatients);
                Console.WriteLine("individualSessions: " + individualSessions + " - groupSessions: " + groupSessions + " - familySessions: " + familySessions);
                Console.WriteLine("hoHMaleCount: " + hoHMaleCount + " - hoHFemaleCount: " + hoHFemaleCount + " - - hoHTotalFamiles: " + hoHTotalFamiles + " - hoHIndividuals: " + hoHIndividuals);
                Console.WriteLine("Total Hours of service: " + totalHoursofService);
                Console.WriteLine("Age brackets   0-5: " + ageTotals[0] + "  6 - 11: " + ageTotals[1] + "  12-17: " + ageTotals[2] + "  18-23: " + ageTotals[3] + "  24-44: " + ageTotals[4] + "  45-54: " + ageTotals[5] + " 55-69: " + ageTotals[6] + "  70+: " + ageTotals[7]);
                Console.WriteLine("totalMales: " + totalMales + " - totalFemales: " + totalFemales);
                Console.WriteLine("Ethnicities  0: " + totalEthnicity[0] + "  1: " + totalEthnicity[1] + "  2: " + totalEthnicity[2] + "  3: " + totalEthnicity[3] + "  4: " + totalEthnicity[4] + "  5: " + totalEthnicity[5] + " 6: " + totalEthnicity[6]);
                Console.WriteLine("Income  0: " + totalIncome[0] + "  1: " + totalIncome[1] + "  2: " + totalIncome[2] + "  3: " + totalIncome[3] + "  4: " + totalIncome[4]);
                Console.WriteLine("County  Weber: " + totalCounty[0] + "  Davis: " + totalCounty[1] + "  DCLC: " + totalCounty[2] + "  Morgan: " + totalCounty[3] + "  Box Elder: " + totalCounty[4] + "  Other: " + totalCounty[5]);

                Console.WriteLine("\n\nFunding Source:");
                fCount = 0;
                foreach (var fu in listOfAllKnownFunding)
                {
                    if (fCount <=5)
                    {
                        Console.Write(" " + fu.fundingSource + ": " + arrayOfFundingCounts[fCount, 1]);
                    }
                    else
                    {
                        Console.WriteLine(" " + fu.fundingSource + ": " + arrayOfFundingCounts[fCount,1]);
                        fCount = 0;
                    }
                    fCount++;
                }
                //Print 4 problems per line.
                Console.WriteLine("\n\nProblems:");
                pCount = 1;
                foreach (var pr in listOfAllKnownProblems)
                {
                    if (pCount <= 3)
                    {
                        Console.Write(" " + pr.problemType + ": " + arrayOfProblemCounts[pr.problemID]);
                    }
                    else
                    {
                        Console.WriteLine(" " + pr.problemType + ": " + arrayOfProblemCounts[pr.problemID]);
                        pCount = 0;
                    }
                    pCount++;
                }
                Console.WriteLine(toPrint);
            } else
            {
                System.Windows.MessageBox.Show("Please pick a valid starting and ending date prior to clicking Generate Report");
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            demographicsReportFrom_datepicker.SelectedDate = new DateTime(DateTime.Now.Year, 1, 1);
            demographicsReportTo_datepicker.SelectedDate = new DateTime(DateTime.Now.Year, 12, 31);
            var db = new FCS_DBModel();
            var fundingSourceList = (from don in db.Donations.AsEnumerable()
                                     join donor in db.Donors on don.DonationID equals donor.DonorID
                                     where don.GrantProposalID == null
                                     select new
                                     {
                                         fundingSourceID = don.DonationID,
                                         fundingSource = donor.OrganizationName
                                     }).ToList();

            var grantSourceList = (from don in db.Donations.AsEnumerable()
                                   join gp in db.GrantProposals on don.GrantProposalID equals gp.GrantProposalID
                                   where don.GrantProposalID != null
                                   select new
                                   {
                                       fundingSourceID = don.DonationID,
                                       fundingSource = gp.GrantName
                                   }).ToList();

            fundingSourceList.AddRange(grantSourceList);
            reportDemoFundingSource_listbox.ItemsSource = fundingSourceList;

            db = new FCS_DBModel();
            var problemList = (from pp in db.Problems
                               orderby pp.ProblemType
                               select new
                               {
                                   problemType = pp.ProblemType
                               }).Distinct();
            reportDemoProblems_listbox.ItemsSource = problemList.ToList();

            reportDemoFundingSource_listbox.SelectAll();
            reportDemoProblems_listbox.SelectAll();
        }

        private void MonthlyDemoReport_checkbox_Click(object sender, RoutedEventArgs e)
        {
            YearlyDemoReport_checkbox.IsChecked = !MonthlyDemoReport_checkbox.IsChecked;
        }

        private void YearlyDemoReport_checkbox_Click(object sender, RoutedEventArgs e)
        {
            MonthlyDemoReport_checkbox.IsChecked = !YearlyDemoReport_checkbox.IsChecked;
        }

        private void demographicsReport_button_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Forms.WebBrowser newBrowser = new System.Windows.Forms.WebBrowser();

            newBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(PrintDocument);
            //newBrowser.DocumentText = "<!DOCTYPE html><html><style>@page { size: landscape; margin: 0px;}</style></head><body>Test Yay!</body></html>";
            newBrowser.DocumentText = "<!DOCTYPE html>"
                + "<html>"
                + "<head>"
                + " <style>"
                + "header nav, footer {"
                + "   display: none;"
                + "}"
                + "body {"
                + "    font-size:11pt;"
                + "    margin: -40px;"
                + "}"
                + "</style>"
                + "</head>"
                + "<body>"
                + "<div style='position:relative;' id='wrap'>"
                + "    <div style='color:#000000;font-size:18pt;position:relative;left:25px;top:20px;'>Demographic Report</div>"
                + "    <div style='color:#000000;position:absolute;left:25px;top:80px;'>Staff Name:</div>"
                + "    <div style='color:#000000;position:absolute;left:425px;top:80px;'>Location:</div>"
                + "    <div style='color:#000000;position:absolute;left:25px;top:105px;'>New:</div>"
                + "    <div style='color:#000000;position:absolute;left:225px;top:105px;'>Ongoing:</div>"
                + "    <div style='color:#000000;position:absolute;left:425px;top:105px;'>Total Clients:</div>"
                + "    <div style='color:#000000;position:absolute;left:25px;top:130px;'>Individual:</div>"
                + "    <div style='color:#000000;position:absolute;left:225px;top:130px;'>Family Sessions:</div>"
                + "    <div style='color:#000000;position:absolute;left:425px;top:130px;'>Groups:</div>"
                + "    <div style='color:#000000;position:absolute;left:25px;top:155px;'>Head of household</div>"
                + "    <div style='color:#000000;position:absolute;left:225px;top:155px;'>M:</div>"
                + "    <div style='color:#000000;position:absolute;left:325px;top:155px;'>F:</div>"
                + "    <div style='color:#000000;position:absolute;left:425px;top:155px;'>Total Families:</div>"
                + "    <div style='color:#000000;position:absolute;left:25px;top:180px;'># Individuals in the household:</div>"
                + "    <div style='color:#000000;position:absolute;left:600px;top:175px;'>LtCxl:</div>"
                + "    <div style='color:#000000;position:absolute;left:600px;top:195px;'>Cxl:</div>"
                + "    <div style='color:#000000;position:absolute;left:600px;top:214px;'>No Show:</div>"
                + "    <div style='color:#000000;position:absolute;left:25px;top:214px;'>Total Hours of service:</div>"
                + "    <div style='color:#000000;position:absolute;left:25px;top:255px;'>Age</div>"
                + "    <div style='color:#000000;position:absolute;left:150px;top:255px;'>1. 0-5:</div>"
                + "    <div style='color:#000000;position:absolute;left:300px;top:255px;'>2. 6-11:</div>"
                + "    <div style='color:#000000;position:absolute;left:450px;top:255px;'>3. 12-17:</div>"
                + "    <div style='color:#000000;position:absolute;left:600px;top:255px;'>4. 18-23:</div>"
                + "    <div style='color:#000000;position:absolute;left:150px;top:275px;'>5. 24-44:</div>"
                + "    <div style='color:#000000;position:absolute;left:300px;top:275px;'>6. 45-54:</div>"
                + "    <div style='color:#000000;position:absolute;left:450px;top:275px;'>7. 55-69:</div>"
                + "    <div style='color:#000000;position:absolute;left:600px;top:275px;'>8. 70+:</div>"
                + "    <div style='color:#000000;position:absolute;left:25px;top:295px;'>Gender of Client</div>"
                + "    <div style='color:#000000;position:absolute;left:225px;top:295px;'>M:</div>"
                + "    <div style='color:#000000;position:absolute;left:325px;top:295px;'>F:</div>"
                + "    <div style='color:#000000;position:absolute;left:25px;top:320px;'>Ethnicity</div>"
                + "    <div style='color:#000000;position:absolute;left:150px;top:320px;'>1. African American:</div>"
                + "    <div style='color:#000000;position:absolute;left:340px;top:320px;'>2. Natice/Alaskan:</div>"
                + "    <div style='color:#000000;position:absolute;left:510px;top:320px;'>3. Pacific Islander:</div>"
                + "    <div style='color:#000000;position:absolute;left:700px;top:320px;'>4. Asian:</div>"
                + "    <div style='color:#000000;position:absolute;left:150px;top:340px;'>5. Caucasian:</div>"
                + "    <div style='color:#000000;position:absolute;left:340px;top:340px;'>6. Hispanic:</div>"
                + "    <div style='color:#000000;position:absolute;left:510px;top:340px;'>7. Other:</div>"
                + "    <div style='color:#000000;position:absolute;left:25px;top:380px;'>Income</div>"
                + "    <div style='color:#000000;position:absolute;left:150px;top:380px;'>1. $0-9,999:</div>"
                + "    <div style='color:#000000;position:absolute;left:300px;top:380px;'>2. $10,000-14,999:</div>"
                + "    <div style='color:#000000;position:absolute;left:450px;top:380px;'>3. $15,000-24,999:</div>"
                + "    <div style='color:#000000;position:absolute;left:600px;top:380px;'>4. $25,000-34,999:</div>"
                + "    <div style='color:#000000;position:absolute;left:150px;top:400px;'>5. $35,000+:</div>"
                + "    <div style='color:#000000;position:absolute;left:25px;top:425px;'>County</div>"
                + "    <div style='color:#000000;position:absolute;left:125px;top:425px;'>1. Weber:</div>"
                + "    <div style='color:#000000;position:absolute;left:250px;top:425px;'>2. Davis:</div>"
                + "    <div style='color:#000000;position:absolute;left:375px;top:425px;'>3. DCLC:</div>"
                + "    <div style='color:#000000;position:absolute;left:500px;top:425px;'>4. Morgan:</div>"
                + "    <div style='color:#000000;position:absolute;left:625px;top:425px;'>5. Box Elder:</div>"
                + "    <div style='color:#000000;position:absolute;left:770px;top:425px;'>6. Other:</div>"
                + "    <div style='color:#000000;position:absolute;left:25px;top:455px;'>Funding Source:</div>"
                + "    <div style='color:#000000;position:absolute;left:150px;top:455px;'>1. ABC:</div>"
                + "    <div style='color:#000000;position:absolute;left:275px;top:455px;'>2. EFG:</div>"
                + "    <div style='color:#000000;position:absolute;left:400px;top:455px;'>3. HIJ:</div>"
                + "    <div style='color:#000000;position:absolute;left:525px;top:455px;'>4. LMN?:</div>"
                + "    <div style='color:#000000;position:absolute;left:650px;top:455px;'>5. OPQ:</div>"
                + "    <div style='color:#000000;position:absolute;left:25px;top:500px;'>Problem:</div>"
                + "    <div style='color:#000000;font-size:10pt;position:absolute;left:95px;top:500px;'>1. Depression: XXXX</div>"
                + "    <div style='color:#000000;font-size:10pt;position:absolute;left:275px;top:500px;'>2. Bereavement/Loss: XXXX</div>"
                + "    <div style='color:#000000;font-size:10pt;position:absolute;left:525px;top:500px;'>3. Communication: XXXX</div>"
                + "    <div style='color:#000000;font-size:10pt;position:absolute;left:720px;top:500px;'>4. Domestic Violence: XXXX</div>"
                + "    <div style='color:#000000;font-size:10pt;position:absolute;left:95px;top:520px;'>5. Hopelessness: XXXX</div>"
                + "    <div style='color:#000000;font-size:10pt;position:absolute;left:275px;top:520px;'>6. Work Problems: XXXX</div>"
                + "    <div style='color:#000000;font-size:10pt;position:absolute;left:525px;top:520px;'>7. Parent Problems: XXXX</div>"
                + "    <div style='color:#000000;font-size:10pt;position:absolute;left:720px;top:520px;'>8. Substance Abuse: XXXX</div>"
                + "    <div style='color:#000000;font-size:10pt;position:absolute;left:95px;top:540px;'>9. Problems w/School: XXXX</div>"
                + "    <div style='color:#000000;font-size:10pt;position:absolute;left:275px;top:540px;'>10. Marriage/Relationship/Family: XXXX</div>"
                + "    <div style='color:#000000;font-size:10pt;position:absolute;left:525px;top:540px;'>11. Thought of hurting self: XXXX</div>"
                + "    <div style='color:#000000;font-size:10pt;position:absolute;left:720px;top:540px;'>12. Angry Feelings: XXXX</div>"
                + "    <div style='color:#000000;font-size:10pt;position:absolute;left:95px;top:560px;'>13. Sexual Abuse: XXXX</div>"
                + "    <div style='color:#000000;font-size:10pt;position:absolute;left:275px;top:560px;'>14. Emotional Abuse: XXXX</div>"
                + "    <div style='color:#000000;font-size:10pt;position:absolute;left:525px;top:560px;'>15. Physical Abuse: XXXX</div>"
                + "    <div style='color:#000000;font-size:10pt;position:absolute;left:720px;top:560px;'>16. Problems w/the law: XXXX</div>"
                + "    <div style='color:#000000;font-size:10pt;position:absolute;left:95px;top:580px;'>17. Unhappy with Life: XXXX</div>"
                + "    <div style='color:#000000;font-size:10pt;position:absolute;left:275px;top:580px;'>18. Anxiety: XXXX</div>"
                + "    <div style='color:#000000;font-size:10pt;position:absolute;left:525px;top:580px;'>19. Other: XXXX</div>"
                + "</body>"
                + "</html>";
        }

        private void PrintDocument(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            ((System.Windows.Forms.WebBrowser)sender).ShowPageSetupDialog();
            ((System.Windows.Forms.WebBrowser)sender).Print();
            //((System.Windows.Forms.WebBrowser)sender).ShowPrintPreviewDialog();

        }
    }
}

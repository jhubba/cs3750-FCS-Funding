using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FCS_Funding.Models;
using System.Diagnostics;
using System.Windows.Forms;
using SelectPdf;
using FCS_Funding.DBService;
using FCS_Funding.Reports;

namespace FCS_Funding.Views.UserControls
{
    /// <summary>
    /// Interaction logic for Report_Demographics.xaml
    /// </summary>
    public partial class Report_Demographics : System.Windows.Controls.UserControl
    {
        private string toPrint;
        private System.Windows.Controls.Button btn;
        private bool boardBtnSelected = true;

        public List<DBService.PatientProblem> problemTotalList = new List<DBService.PatientProblem>();

        public List<FundingItem> fundingSourceTotalList = new List<FundingItem>();

        public Report_Demographics()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            btn = (System.Windows.Controls.Button)sender;
            EnableButton(false, "Please Wait");

            if (UserSelectedDatesAreValid())
            {
                var demoService = new demographicsService();
                //Get dates from date picker and convert to DATETIME
                DateTime requestedDateStart = DateTime.Parse(demographicsReportFrom_datepicker.Text);
                DateTime requestedDateEnd = DateTime.Parse(demographicsReportTo_datepicker.Text);

                //QUERY FOR ALL PATIENTS WITH SESSIONS BETWEEN DATES FROM ABOVE
                var listOfAllMatchingPatients = demoService.patientsBetweenDates(requestedDateStart, requestedDateEnd);

                //QUERY FOR ALL SESSIONS BETWEEN DATES FROM ABOVE
                var sessionInformation = demoService.sessionsBetweenDates(requestedDateStart, requestedDateEnd);

                // LOOP TO DROP SESSIONS AND CLIENTS WHICH DO NOT MEET THE UNCHECKED FILTERS SPECIFIED IN THE GUI
                LoopAndDropCheckBoxes(listOfAllMatchingPatients, sessionInformation);

                //START COUNTING BASED ON THE FILTER RESULTS --Now Using DemoHelper
                var demoHelper = new DemographicsReportHelper();

                //GET A LIST OF ALL PROBLEM TYPES FOR REPORT GENERATION
                var listOfAllKnownProblems = demoService.getAllKnownProblems();

                int[] arrayOfProblemCounts = new int[20];

                //GET A LIST OF ALL FUNDING TYPES FOR REPORT GENERATION
                //FINDS ALL DONORS/DONATIONS NOT PART OF A GRANT
                var listOfAllKnownFunding = demoService.getDonorsNotPartOfGrant();

                //FINDS ALL GRANT PROPOSALS
                var listOfAllKnownFundingGP = demoService.getAllKnownGrantProposals();

                //MERGE THE LISTS FOR DONATIONS AND GRANTS
                listOfAllKnownFunding.AddRange(listOfAllKnownFundingGP);
                int[,] arrayOfFundingCounts = new int[listOfAllKnownFunding.Count(), 2];
                int x = 0;
                foreach (var ses in listOfAllKnownFunding)
                {
                    arrayOfFundingCounts[x, 0] = ses.fundingSourceID;
                    //arrayOfFundingCounts[x,1] = 0;
                    x++;
                }

                //Get counts for each type of grant proposal
                var listOfFundingTypeCounts = demoService.GetFundingTypeCounts();

                int[] arrayOfCancellations = new int[3];

                //Loop through all patients pulled from database with sessions matching the dates specified by the end user
                LoopThroughClientsGetCounts(listOfAllMatchingPatients, sessionInformation, listOfAllKnownProblems,
                    demoHelper, arrayOfProblemCounts, listOfAllKnownFunding, arrayOfFundingCounts);

                // - Session times - Add all sessions together in minutes and convert to hours.
                CalculateSessionTimes(sessionInformation, demoHelper);

                //Total count per funding source.
                TotalCountPerFundingSource(sessionInformation, arrayOfCancellations);

                //int ageBracket1 = houseHoldInformation.Single().Where(x => x.ageGroup.Contains("0-5")).Count();

                //Load Demo Helper
                demoHelper.location = locationComboBox.Text;
                demoHelper.listOfAllKnownFunding = listOfAllKnownFunding;
                demoHelper.listOfAllKnownProblems = listOfAllKnownProblems;
                demoHelper.arrayOfCancellations = arrayOfCancellations;
                demoHelper.arrayOfFundingCounts = arrayOfFundingCounts;
                demoHelper.arrayOfProblemCounts = arrayOfProblemCounts;
                demoHelper.listOfFundingTypeCounts = listOfFundingTypeCounts;

                //generate string based on chosen report
                toPrint = boardBtnSelected ? demoHelper.generateBoardReportString() : demoHelper.generateReportString();

                //get pdf doc
                showPdf();
            }
            else
            {
                System.Windows.MessageBox.Show(
                    "Please pick a valid starting and ending date prior to clicking Generate Report");
            }
        }

        private void LoopAndDropCheckBoxes(List<MatchingPatient> listOfAllMatchingPatients, List<SessionInfo> sessionInformation)
        {
            for (int i = listOfAllMatchingPatients.Count() - 1; i >= 0; i--)
            {
                var query = listOfAllMatchingPatients[i];
                var patientInformation = query.patientInformation.ToList();
                var houseHoldInformation = query.houseHoldInformation.ToList();
                var patientProblems = query.patientProblems.ToList();
                //FILTER BASED ON CLIENTTYPE
                DateTime patientIntakeTimeDate = patientInformation.Single().patientIntakeTimeDate;
                DateTime patientFirstSession = sessionInformation.First().appointmentDateTimeStart;
                //DETERMINE IF PATIENT IS CONSIDERED NEW OR ONGOING
                if (patientIntakeTimeDate < patientFirstSession)
                {
                    //IF FILTERING NEW PATIENTS THIS WILL REMOVE THE PATIENT AND HIS SESSIONS.
                    if (demoNew_checkBox.IsChecked.Value == false || !demoNew_checkBox.IsChecked.HasValue)
                    {
                        listOfAllMatchingPatients.RemoveAt(i);
                        sessionInformation.RemoveAll(item => item.patientID == query.patientID);
                        continue;
                    }
                }
                else
                {
                    //IF FILTERING ONGOING PATIENTS THIS WILL REMOVE THE PATIENT AND HIS SESSIONS.
                    if (demoOngoing_checkBox.IsChecked.Value == false || !demoOngoing_checkBox.IsChecked.HasValue)
                    {
                        listOfAllMatchingPatients.RemoveAt(i);
                        sessionInformation.RemoveAll(item => item.patientID == query.patientID);
                        continue;
                    }
                }

                //FILTER BASED ON SESSIONTYPE.
                //THIS WILL FILTER ALL SESSIONS BASED SESSION TYPES.  0=INDIVIDUAL, 1=FAMILY, 3=GROUP
                for (int j = sessionInformation.Count() - 1; j >= 0; j--)
                {
                    var session = sessionInformation[j];
                    if (session.patientID.Equals(query.patientID))
                    {
                        //FILTER BASED ON INDIVIDUAL
                        if (demoindividual_checkBox.IsChecked.Value == false ||
                            !demoindividual_checkBox.IsChecked.HasValue)
                        {
                            if (session.expenseTypeID == 1)
                            {
                                sessionInformation.RemoveAt(j);
                                continue;
                            }
                        }
                        //FILTER BASED ON FAMILY SESSION TYPE
                        if (demoFamily_checkBox.IsChecked.Value == false || !demoFamily_checkBox.IsChecked.HasValue)
                        {
                            if (session.expenseTypeID == 3)
                            {
                                sessionInformation.RemoveAt(j);
                                continue;
                            }
                        }
                        //FILTER BASED ON GROUP SESSION TYPE
                        if (demoGroup_checkBox.IsChecked.Value == false || !demoGroup_checkBox.IsChecked.HasValue)
                        {
                            if (session.expenseTypeID == 2)
                            {
                                sessionInformation.RemoveAt(j);
                                continue;
                            }
                        }
                    }
                }

                //HEAD OF HOUSEHOLD FILTERS
                //FILTER BASED ON MALE HOH
                if (demoHOHMale_checkBox.IsChecked.Value == false || !demoHOHMale_checkBox.IsChecked.HasValue)
                {
                    if (patientInformation.Single().gender == "Male" &&
                        houseHoldInformation.Single().isHeadOfHousehold == true)
                    {
                        listOfAllMatchingPatients.RemoveAt(i);
                        sessionInformation.RemoveAll(item => item.patientID == query.patientID);
                        continue;
                    }
                }
                //FILTER BASED ON FEMALE HOH
                if (demoHOHFemale_checkBox.IsChecked.Value == false || !demoHOHFemale_checkBox.IsChecked.HasValue)
                {
                    if (patientInformation.Single().gender == "Female" &&
                        houseHoldInformation.Single().isHeadOfHousehold == true)
                    {
                        listOfAllMatchingPatients.RemoveAt(i);
                        sessionInformation.RemoveAll(item => item.patientID == query.patientID);
                        continue;
                    }
                }

                //AGE FILTERING
                //FILTER BASED ON 0-5 AGE GROUP
                if (demoAge05_checkBox.IsChecked.Value == false || !demoAge05_checkBox.IsChecked.HasValue)
                {
                    if (patientInformation.Single().ageGroup == "0-5")
                    {
                        listOfAllMatchingPatients.RemoveAt(i);
                        sessionInformation.RemoveAll(item => item.patientID == query.patientID);
                        continue;
                    }
                }
                //FILTER BASED ON 6-11 AGE GROUP
                if (demoAge611_checkBox.IsChecked.Value == false || !demoAge611_checkBox.IsChecked.HasValue)
                {
                    if (patientInformation.Single().ageGroup == "6-11")
                    {
                        listOfAllMatchingPatients.RemoveAt(i);
                        sessionInformation.RemoveAll(item => item.patientID == query.patientID);
                        continue;
                    }
                }
                //FILTER BASED ON 12-17 AGE GROUP
                if (demoAge1217_checkBox.IsChecked.Value == false || !demoAge1217_checkBox.IsChecked.HasValue)
                {
                    if (patientInformation.Single().ageGroup == "12-17")
                    {
                        listOfAllMatchingPatients.RemoveAt(i);
                        sessionInformation.RemoveAll(item => item.patientID == query.patientID);
                        continue;
                    }
                }
                //FILTER BASED ON 18-23 AGE GROUP
                if (demoAge1823_checkBox.IsChecked.Value == false || !demoAge1823_checkBox.IsChecked.HasValue)
                {
                    if (patientInformation.Single().ageGroup == "18-23")
                    {
                        listOfAllMatchingPatients.RemoveAt(i);
                        sessionInformation.RemoveAll(item => item.patientID == query.patientID);
                        continue;
                    }
                }
                //FILTER BASED ON 24-44 AGE GROUP
                if (demoAge2444_checkBox.IsChecked.Value == false || !demoAge2444_checkBox.IsChecked.HasValue)
                {
                    if (patientInformation.Single().ageGroup == "24-44")
                    {
                        listOfAllMatchingPatients.RemoveAt(i);
                        sessionInformation.RemoveAll(item => item.patientID == query.patientID);
                        continue;
                    }
                }
                //FILTER BASED ON 45-54 AGE GROUP
                if (demoAge4554_checkBox.IsChecked.Value == false || !demoAge4554_checkBox.IsChecked.HasValue)
                {
                    if (patientInformation.Single().ageGroup == "45-54")
                    {
                        listOfAllMatchingPatients.RemoveAt(i);
                        sessionInformation.RemoveAll(item => item.patientID == query.patientID);
                        continue;
                    }
                }
                //FILTER BASED ON 55-69 AGE GROUP
                if (demoAge5569_checkBox.IsChecked.Value == false || !demoAge5569_checkBox.IsChecked.HasValue)
                {
                    if (patientInformation.Single().ageGroup == "55+")
                    {
                        listOfAllMatchingPatients.RemoveAt(i);
                        sessionInformation.RemoveAll(item => item.patientID == query.patientID);
                        continue;
                    }
                }
                //FILTER BASED ON 70+ AGE GROUP
                if (demoAge70_checkBox.IsChecked.Value == false || !demoAge70_checkBox.IsChecked.HasValue)
                {
                    if (patientInformation.Single().ageGroup == "70+")
                    {
                        listOfAllMatchingPatients.RemoveAt(i);
                        sessionInformation.RemoveAll(item => item.patientID == query.patientID);
                        continue;
                    }
                }

                //FILTER BASED ON GENDER
                //FILTER BASED ON MALE GENDER
                if (demoGenderMale_checkBox.IsChecked.Value == false || !demoGenderMale_checkBox.IsChecked.HasValue)
                {
                    if (patientInformation.Single().gender == "Male")
                    {
                        listOfAllMatchingPatients.RemoveAt(i);
                        sessionInformation.RemoveAll(item => item.patientID == query.patientID);
                        continue;
                    }
                }
                //FILTER BASED ON FEMALE GENDER
                if (demoGenderFemale_checkBox.IsChecked.Value == false ||
                    !demoGenderFemale_checkBox.IsChecked.HasValue)
                {
                    if (patientInformation.Single().gender == "Female")
                    {
                        listOfAllMatchingPatients.RemoveAt(i);
                        sessionInformation.RemoveAll(item => item.patientID == query.patientID);
                        continue;
                    }
                }

                //FILTER BASED ON ETHNICITY
                //FILTER BASED ON AFRICAN AMERICAN ETHNICITY
                if (demoEthnicityAA_checkBox.IsChecked.Value == false ||
                    !demoEthnicityAA_checkBox.IsChecked.HasValue)
                {
                    if (patientInformation.Single().ethnicity == "African American")
                    {
                        listOfAllMatchingPatients.RemoveAt(i);
                        sessionInformation.RemoveAll(item => item.patientID == query.patientID);
                        continue;
                    }
                }
                //FILTER BASED ON NATIVE/ALASKAN ETHNICITY
                if (demoEthnicityNATALASK_checkBox.IsChecked.Value == false ||
                    !demoEthnicityNATALASK_checkBox.IsChecked.HasValue)
                {
                    if (patientInformation.Single().ethnicity == "Native/Alaskan")
                    {
                        listOfAllMatchingPatients.RemoveAt(i);
                        sessionInformation.RemoveAll(item => item.patientID == query.patientID);
                        continue;
                    }
                }
                //FILTER BASED ON PACIFIC ISLANDER ETHNICITY
                if (demoEthnicityPACISL_checkBox.IsChecked.Value == false ||
                    !demoEthnicityPACISL_checkBox.IsChecked.HasValue)
                {
                    if (patientInformation.Single().ethnicity == "Pacific Islander")
                    {
                        listOfAllMatchingPatients.RemoveAt(i);
                        sessionInformation.RemoveAll(item => item.patientID == query.patientID);
                        continue;
                    }
                }
                //FILTER BASED ON ASIAN ETHNICITY
                if (demoEthnicityASIAN_checkBox.IsChecked.Value == false ||
                    !demoEthnicityASIAN_checkBox.IsChecked.HasValue)
                {
                    if (patientInformation.Single().ethnicity == "Asian")
                    {
                        listOfAllMatchingPatients.RemoveAt(i);
                        sessionInformation.RemoveAll(item => item.patientID == query.patientID);
                        continue;
                    }
                }
                //FILTER BASED ON CAUCASIAN ETHNICITY
                if (demoEthnicityCAUC_checkBox.IsChecked.Value == false ||
                    !demoEthnicityCAUC_checkBox.IsChecked.HasValue)
                {
                    if (patientInformation.Single().ethnicity == "Caucasian")
                    {
                        listOfAllMatchingPatients.RemoveAt(i);
                        sessionInformation.RemoveAll(item => item.patientID == query.patientID);
                        continue;
                    }
                }
                //FILTER BASED ON HISPANIC ETHNICITY
                if (demoEthnicityHISP_checkBox.IsChecked.Value == false ||
                    !demoEthnicityHISP_checkBox.IsChecked.HasValue)
                {
                    if (patientInformation.Single().ethnicity == "Hispanic")
                    {
                        listOfAllMatchingPatients.RemoveAt(i);
                        sessionInformation.RemoveAll(item => item.patientID == query.patientID);
                        continue;
                    }
                }
                //FILTER BASED ON OTHER ETHNICITY
                if (demoEthnicityOther_checkBox.IsChecked.Value == false ||
                    !demoEthnicityOther_checkBox.IsChecked.HasValue)
                {
                    if (patientInformation.Single().ethnicity == "Other")
                    {
                        listOfAllMatchingPatients.RemoveAt(i);
                        sessionInformation.RemoveAll(item => item.patientID == query.patientID);
                        continue;
                    }
                }

                //FILTER BASED ON INCOME
                //FILTER BASED ON 0-9,999 INCOME LEVEL
                if (demoIncome09999_checkBox.IsChecked.Value == false ||
                    !demoIncome09999_checkBox.IsChecked.HasValue)
                {
                    if (houseHoldInformation.Single().patientIncome == "$0-9,999")
                    {
                        listOfAllMatchingPatients.RemoveAt(i);
                        sessionInformation.RemoveAll(item => item.patientID == query.patientID);
                        continue;
                    }
                }
                //FILTER BASED ON  INCOME LEVEL
                if (demoIncome1000014999_checkBox.IsChecked.Value == false ||
                    !demoIncome1000014999_checkBox.IsChecked.HasValue)
                {
                    if (houseHoldInformation.Single().patientIncome == "$10,000-14,999")
                    {
                        listOfAllMatchingPatients.RemoveAt(i);
                        sessionInformation.RemoveAll(item => item.patientID == query.patientID);
                        continue;
                    }
                }
                //FILTER BASED ON  INCOME LEVEL
                if (demoIncome1500024000_checkBox.IsChecked.Value == false ||
                    !demoIncome1500024000_checkBox.IsChecked.HasValue)
                {
                    if (houseHoldInformation.Single().patientIncome == "$15,000-24,999")
                    {
                        listOfAllMatchingPatients.RemoveAt(i);
                        sessionInformation.RemoveAll(item => item.patientID == query.patientID);
                        continue;
                    }
                }
                //FILTER BASED ON  INCOME LEVEL
                if (demoIncome2500034999_checkBox.IsChecked.Value == false ||
                    !demoIncome2500034999_checkBox.IsChecked.HasValue)
                {
                    if (houseHoldInformation.Single().patientIncome == "$25,000-34,999")
                    {
                        listOfAllMatchingPatients.RemoveAt(i);
                        sessionInformation.RemoveAll(item => item.patientID == query.patientID);
                        continue;
                    }
                }
                //FILTER BASED ON  INCOME LEVEL
                if (demoIncome35000_checkBox.IsChecked.Value == false ||
                    !demoIncome35000_checkBox.IsChecked.HasValue)
                {
                    if (houseHoldInformation.Single().patientIncome == "$35,000+")
                    {
                        listOfAllMatchingPatients.RemoveAt(i);
                        sessionInformation.RemoveAll(item => item.patientID == query.patientID);
                        continue;
                    }
                }

                //FILTER BASED ON WEBER COUNTY
                if (demoCountyWeber_checkBox.IsChecked.Value == false ||
                    !demoCountyWeber_checkBox.IsChecked.HasValue)
                {
                    if (houseHoldInformation.Single().patientCounty == "Weber")
                    {
                        listOfAllMatchingPatients.RemoveAt(i);
                        sessionInformation.RemoveAll(item => item.patientID == query.patientID);
                        continue;
                    }
                }
                //FILTER BASED ON DAVIS COUNTY
                if (demoCountyDavis_checkBox.IsChecked.Value == false ||
                    !demoCountyDavis_checkBox.IsChecked.HasValue)
                {
                    if (houseHoldInformation.Single().patientCounty == "Davis")
                    {
                        listOfAllMatchingPatients.RemoveAt(i);
                        sessionInformation.RemoveAll(item => item.patientID == query.patientID);
                        continue;
                    }
                }
                //FILTER BASED ON DCLC COUNTY
                if (demoCountyDCLC_checkBox.IsChecked.Value == false || !demoCountyDCLC_checkBox.IsChecked.HasValue)
                {
                    if (houseHoldInformation.Single().patientCounty == "DCLC")
                    {
                        listOfAllMatchingPatients.RemoveAt(i);
                        sessionInformation.RemoveAll(item => item.patientID == query.patientID);
                        continue;
                    }
                }
                //FILTER BASED ON MORGAN COUNTY
                if (demoCountyMorgan_checkBox.IsChecked.Value == false ||
                    !demoCountyMorgan_checkBox.IsChecked.HasValue)
                {
                    if (houseHoldInformation.Single().patientCounty == "Morgan")
                    {
                        listOfAllMatchingPatients.RemoveAt(i);
                        sessionInformation.RemoveAll(item => item.patientID == query.patientID);
                        continue;
                    }
                }
                //FILTER BASED ON BOX ELDER COUNTY
                if (demoCountyBoxElder_checkBox.IsChecked.Value == false ||
                    !demoCountyBoxElder_checkBox.IsChecked.HasValue)
                {
                    if (houseHoldInformation.Single().patientCounty == "Box Elder")
                    {
                        listOfAllMatchingPatients.RemoveAt(i);
                        sessionInformation.RemoveAll(item => item.patientID == query.patientID);
                        continue;
                    }
                }
                //FILTER BASED ON OTHER COUNTY
                if (demoCountyOther_checkBox.IsChecked.Value == false ||
                    !demoCountyOther_checkBox.IsChecked.HasValue)
                {
                    if (houseHoldInformation.Single().patientCounty == "Other")
                    {
                        listOfAllMatchingPatients.RemoveAt(i);
                        sessionInformation.RemoveAll(item => item.patientID == query.patientID);
                        continue;
                    }
                }

                //FILTER BASED ON Problem Type
                //THIS WILL FILTER ALL Problems of a certaion type
                if (reportDemoProblems_listbox.SelectedIndex != 0) //All is selected so no filtering is needed
                {
                    bool problemMatch = false;
                    foreach (Object pr in reportDemoProblems_listbox.SelectedItems)
                    {
                        int selectedproblemID = (int)pr.GetType().GetProperty("problemID").GetValue(pr, null);
                        String selectedproblemName =
                            (String)pr.GetType().GetProperty("problemType").GetValue(pr, null);
                        Console.WriteLine("Look for selectedproblem ID: " + selectedproblemID +
                                          " in patient problem list for patient: " + query.patientID);

                        if (patientProblems.Where(z => z.problemID.Equals(selectedproblemID)).Count() >= 1 &&
                            problemMatch == false)
                        {
                            Console.WriteLine("     Found match: " + selectedproblemName);
                            problemMatch = true; //Keep it.
                            break;
                        }
                    }
                    // No matches were found.  Remove patient from list.
                    if (problemMatch == false)
                    {
                        listOfAllMatchingPatients.RemoveAt(i);
                        sessionInformation.RemoveAll(item => item.patientID == query.patientID);
                        continue;
                    }
                }

                //FILTER BASED ON Funding Type
                if (reportDemoFundingSource_listbox.SelectedIndex != 0) //All is selected so no filtering is needed
                {
                    bool fundingMatch = false;
                    //foreach (var sessionFundingSource in sessionInformation)
                    //{
                    //Match patient ID in query to session element.
                    //if (query.patientID.Equals(sessionFundingSource.patientID))
                    //{
                    foreach (Object pr in reportDemoFundingSource_listbox.SelectedItems)
                    {
                        int selectedfundingID = (int)pr.GetType().GetProperty("fundingSourceID").GetValue(pr, null);
                        String selectedfundingName =
                            (String)pr.GetType().GetProperty("fundingSource").GetValue(pr, null);
                        Console.WriteLine("Look for fundingSource ID: " + selectedfundingID +
                                          " in funding source list for patient ID: " + query.patientID);

                        // if (sessionInformation.(z => (z.fundingSource.Equals(selectedfundingID) && (z.patientID.Equals(query.patientID))).Count() >= 1 && fundingMatch == false)
                        int counting =
                            sessionInformation.Count(
                                z =>
                                    z.fundingSource.Equals(selectedfundingID) &&
                                    z.patientID.Equals(query.patientID));
                        if (
                        (sessionInformation.Count(
                             z =>
                                 z.fundingSource.Equals(selectedfundingID) &&
                                 z.patientID.Equals(query.patientID)) >= 1) && fundingMatch == false)
                        {
                            Console.WriteLine("     Found match: " + selectedfundingName);
                            continue;
                        }
                        else
                        {
                            sessionInformation.RemoveAll(item => item.patientID == query.patientID);
                            Console.WriteLine("     No Found match: " + selectedfundingName + ". Deleting session.");
                        }
                    }
                    //}
                    //}
                }


                //PURGE ALL PEOPLE WITH NO SESSIONS.  SOME SESSIONS MIGHT HAVE BEEN DELETED DUE TO FILTERS AND NEED TO BE REMOVED
                if (sessionInformation.Where(z => z.patientID.Equals(query.patientID)).Count() == 0)
                {
                    //Purge all patient elements with no matching sessions
                    listOfAllMatchingPatients.RemoveAt(i);
                }
            }

        }

        private void LoopThroughClientsGetCounts(List<MatchingPatient> listOfAllMatchingPatients,
            List<SessionInfo> sessionInformation,IQueryable<DBService.PatientProblem> listOfAllKnownProblems,
            DemographicsReportHelper demoHelper,int[] arrayOfProblemCounts,List<FundingItem> listOfAllKnownFunding,
            int[,] arrayOfFundingCounts)
        {
            foreach (var query in listOfAllMatchingPatients)
            {
                var patientInformation = query.patientInformation.ToList();
                //var sessionInformation = query.sessionInformation.ToList();
                var houseHoldInformation = query.houseHoldInformation.ToList();
                var patientProblems = query.patientProblems.ToList();

                // Calculate new and ongoing client totals
                DateTime patientIntakeTimeDate = patientInformation.Single().patientIntakeTimeDate;
                DateTime patientFirstSession = sessionInformation.First().appointmentDateTimeStart;
                if (patientIntakeTimeDate < patientFirstSession)
                {
                    demoHelper.newPatients++;
                }
                else
                {
                    demoHelper.ongoingPatients++;
                }
                //Count individual/group/family sessions
                bool found = false;
                foreach (var session in sessionInformation)
                {
                    if (session.patientID.Equals(query.patientID) && found == false)
                    {
                        if (session.expenseTypeID == 1)
                        {
                            demoHelper.individualSessions++;
                            found = true;
                            continue;
                        }
                        else if (session.expenseTypeID == 2)
                        {
                            demoHelper.groupSessions++;
                            found = true;
                            continue;
                        }
                        else if (session.expenseTypeID == 3)
                        {
                            demoHelper.familySessions++;
                            found = true;
                            continue;
                        }
                    }
                }


                // Calculate Head of Household counts
                if (houseHoldInformation.Single().isHeadOfHousehold.Equals(true) &&
                    patientInformation.Single().gender == "Male")
                {
                    demoHelper.hoHMaleCount++;
                }
                else if (houseHoldInformation.Single().isHeadOfHousehold.Equals(true) &&
                         patientInformation.Single().gender == "Female")
                {
                    demoHelper.hoHFemaleCount++;
                }

                demoHelper.hoHTotalFamiles += 1;
                demoHelper.hoHIndividuals += houseHoldInformation.Single().householdCount;

                //Total count per age group.
                switch (patientInformation.Single().ageGroup)
                {
                    case "0-5":
                        demoHelper.ageTotals[0]++;
                        break;
                    case "6-11":
                        demoHelper.ageTotals[1]++;
                        break;
                    case "12-17":
                        demoHelper.ageTotals[2]++;
                        break;
                    case "18-23":
                        demoHelper.ageTotals[3]++;
                        break;
                    case "24-44":
                        demoHelper.ageTotals[4]++;
                        break;
                    case "45-54":
                        demoHelper.ageTotals[5]++;
                        break;
                    case "55-69":
                        demoHelper.ageTotals[6]++;
                        break;
                    case "70+":
                        demoHelper.ageTotals[7]++;
                        break;
                }
                if (patientInformation.Single().gender == "Male")
                {
                    demoHelper.totalMales++;
                }
                else if (patientInformation.Single().gender == "Female")
                {
                    demoHelper.totalFemales++;
                }
                //Total count per age group.
                switch (patientInformation.Single().ethnicity)
                {
                    case "African American":
                        demoHelper.totalEthnicity[0]++;
                        break;
                    case "Native/Alaskan":
                        demoHelper.totalEthnicity[1]++;
                        break;
                    case "Pacific Islander":
                        demoHelper.totalEthnicity[2]++;
                        break;
                    case "Asian":
                        demoHelper.totalEthnicity[3]++;
                        break;
                    case "Caucasian":
                        demoHelper.totalEthnicity[4]++;
                        break;
                    case "Hispanic":
                        demoHelper.totalEthnicity[5]++;
                        break;
                    case "Other":
                        demoHelper.totalEthnicity[6]++;
                        break;
                }
                //Total count per income.
                switch (houseHoldInformation.Single().patientIncome)
                {
                    case "$0-9,999":
                        demoHelper.totalIncome[0]++;
                        break;
                    case "$10,000-14,999":
                        demoHelper.totalIncome[1]++;
                        break;
                    case "$15,000-24,999":
                        demoHelper.totalIncome[2]++;
                        break;
                    case "$25,000-34,999":
                        demoHelper.totalIncome[3]++;
                        break;
                    case "$35,000+":
                        demoHelper.totalIncome[4]++;
                        break;
                }
                //Total count per county.
                switch (houseHoldInformation.Single().patientCounty)
                {
                    case "Weber":
                        demoHelper.totalCounty[0]++;
                        break;
                    case "Davis":
                        demoHelper.totalCounty[1]++;
                        break;
                    case "DCLC":
                        demoHelper.totalCounty[2]++;
                        break;
                    case "Morgan":
                        demoHelper.totalCounty[3]++;
                        break;
                    case "Box Elder":
                        demoHelper.totalCounty[4]++;
                        break;
                    case "Other":
                        demoHelper.totalCounty[5]++;
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
                //Find first instance of a session with a funding source, add to total count.
                //
                var fundingSourceFound =
                    sessionInformation.Where(z => z.patientID.Equals(query.patientID) && z.fundingSource != 0)
                        .First();

                for (int y = 0; y < listOfAllKnownFunding.Count(); y++)
                {
                    if (arrayOfFundingCounts[y, 0].Equals(fundingSourceFound.fundingSource))
                    {
                        arrayOfFundingCounts[y, 1]++;
                        break;
                    }
                }

                //arrayOfFundingCounts[y, 0].Equals(se.fundingSource))
            }
        }

        private void TotalCountPerFundingSource(List<SessionInfo> sessionInformation, int[] arrayOfCancellations)
        {
            foreach (var se in sessionInformation)
            {
                /*if (se.fundingSource != null)
                {
                    int tempCount = listOfAllKnownFunding.Count();

                    for (int y = 0; y < tempCount; y++)
                    {
                        if (arrayOfFundingCounts[y, 0].Equals(se.fundingSource))
                        {
                            arrayOfFundingCounts[y, 1]++;
                            break;
                        }
                    }
                }*/
                if (se.typeofCx != "Not Cxl" && se.typeofCx != null)
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
        }

        private void CalculateSessionTimes(List<SessionInfo> sessionInformation, DemographicsReportHelper demoHelper)
        {
            foreach (var session in sessionInformation)
            {
                /*if (session.expenseTypeID == 1)
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
                }*/
                demoHelper.totalMinutesofClientService +=
                    session.appointmentDateTimeEnd.Subtract(session.appointmentDateTimeStart).TotalMinutes;
                //Cannot add hours because sessions less than 1 hour will not be tallied.
            }
        }

        private string GeneratePdfDoc()
        {
            HtmlToPdf converter = new HtmlToPdf();

            converter.Options.PdfPageSize = PdfPageSize.Letter;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
            converter.Options.WebPageWidth = 1024;
            converter.Options.WebPageHeight = 0;

            PdfDocument doc = converter.ConvertHtmlString(toPrint, null);

            //build temporary path for pdf

            //add milliseconds to path to make it "unique"
            long milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            string tempFileName = "tempReport" + milliseconds.ToString() + ".pdf";

            var path = System.IO.Path.GetTempPath();
            var totalPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), tempFileName);

            doc.Save(totalPath);

            return totalPath;
        }

        private async void showPdf()
        {
            string pdfPath = await Task.Run(() => GeneratePdfDoc());
            try
            {
                Process.Start(pdfPath);
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("Install a PDF reader and make it your default");
            }
            EnableButton(true, "Generate Report");
        }

        private void EnableButton(bool enabled, string content)
        {
            btn.IsEnabled = enabled;
            btn.Content = content;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			problemTotalList.Clear();
			demographicsReportFrom_datepicker.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
			demographicsReportTo_datepicker.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            var demoService = new demographicsService();
            var fundingSourceList = demoService.getDonorsNotPartOfGrant();

            var grantSourceList = demoService.getAllKnownGrantProposals();

			fundingSourceList.AddRange(grantSourceList);

			fundingSourceTotalList.Clear();
			fundingSourceTotalList.Add(new FundingItem { fundingSourceID = 0, fundingSource = "All" });
			foreach (var fs in fundingSourceList)
			{
				fundingSourceTotalList.Add(new FundingItem { fundingSourceID = fs.fundingSourceID, fundingSource = fs.fundingSource });
			}
			//reportDemoFundingSource_listbox.ItemsSource = fundingSourceList;
			reportDemoFundingSource_listbox.ItemsSource = fundingSourceTotalList.ToList();

            var db = new FCS_DBModel();
			db = new FCS_DBModel();
            var problemList = demoService.getAllKnownProblemsDistinct();
			//reportDemoProblems_listbox.ItemsSource = problemList;

			problemTotalList.Clear();
			problemTotalList.Add(new DBService.PatientProblem { problemID = 0, problemType = "All" });
			foreach (var df in problemList)
			{
				problemTotalList.Add(new DBService.PatientProblem { problemID = df.problemID, problemType = df.problemType });
			}

			reportDemoProblems_listbox.ItemsSource = problemTotalList.ToList();

			//reportDemoFundingSource_listbox.SelectAll();
			reportDemoFundingSource_listbox.SelectedIndex = 0;
			//reportDemoProblems_listbox.SelectAll();
			reportDemoProblems_listbox.SelectedIndex = 0;
		}

		private void demographicsReport_button_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			System.Windows.Forms.WebBrowser newBrowser = new System.Windows.Forms.WebBrowser();
            var demoHelper = new DemographicsReportHelper();
			newBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(PrintDocument);
            //newBrowser.DocumentText = "<!DOCTYPE html><html><style>@page { size: landscape; margin: 0px;}</style></head><body>Test Yay!</body></html>";
            newBrowser.DocumentText = demoHelper.demographicsString;
		}

		private void PrintDocument(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
		    System.Windows.Forms.WebBrowser wb = (System.Windows.Forms.WebBrowser)sender;
		    wb.ShowPrintPreviewDialog();
		    
		} 

        private bool UserSelectedDatesAreValid()
        {
            return demographicsReportFrom_datepicker.SelectedDate != null &&
                demographicsReportFrom_datepicker.SelectedDate.GetValueOrDefault() != DateTime.MinValue &&
                demographicsReportTo_datepicker.SelectedDate != null &&
                demographicsReportTo_datepicker.SelectedDate.GetValueOrDefault() != DateTime.MinValue;
        }

        private void RadioBtnChecked(object sender, RoutedEventArgs e)
        {
            boardBtnSelected = sender.Equals(boardReportbtn);
        }
    }
}

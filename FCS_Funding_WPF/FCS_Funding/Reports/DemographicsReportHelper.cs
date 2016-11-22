using FCS_Funding.DBService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCS_Funding.Reports
{
    class DemographicsReportHelper
    {

        public DemographicsReportHelper()
        {
            ageTotals = new int[8];
            totalEthnicity = new int[7];
            totalIncome = new int[5];
            totalCounty = new int[6];
            staffName = System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().StaffDBName;       
        }

        public string generateReportString()
        {
            totalMinutesofService = totalMinutesofService + totalMinutesofClientService;
            totalMinutesofClientService = 0; //Reset for next client.

            int totalPatients = newPatients + ongoingPatients;
            double totalHoursofService = totalMinutesofService / 60;


            string toPrint = "<!DOCTYPE html>"
                          + "<html>"
                          + "<head>"
                          + " <style>"
                          + "     header nav, footer {"
                          + "         display: none;"
                          + "     }"
                          + "     body {"
                          + "         font-size:10pt;"
                          + "         margin: 0;"
                          + "     }"
                          + "</style>"
                          + "</head>"
                          + "<body>"
                          + "<div style='position:relative;' id='wrap'>"
                          +
                          "    <div style='color:#000000;font-size:18pt;position:relative;left:25px;top:20px;'>Demographic Report</div>"
                          + "    <div style='color:#000000;position:absolute;left:25px;top:80px;'>Staff Name: " +
                          staffName + "</div>"
                          + "    <div style='color:#000000;position:absolute;left:560px;top:80px;'>Location: " +
                          location + "</div>"
                          + "    <div style='color:#000000;position:absolute;left:25px;top:105px;'>New: " + newPatients +
                          "</div>"
                          + "    <div style='color:#000000;position:absolute;left:225px;top:105px;'>Ongoing: " +
                          ongoingPatients + "</div>"
                          + "    <div style='color:#000000;position:absolute;left:425px;top:105px;'>Total Clients: " +
                          totalPatients + "</div>"
                          +
                          "    <div style='color:#000000;position:absolute;left:25px;top:130px;'>Individual Sessions: " +
                          individualSessions + "</div>"
                          + "    <div style='color:#000000;position:absolute;left:225px;top:130px;'>Family Sessions: " +
                          familySessions + "</div>"
                          + "    <div style='color:#000000;position:absolute;left:425px;top:130px;'>Group Sessions: " +
                          groupSessions + "</div>"
                          +
                          "    <div style='color:#000000;position:absolute;left:25px;top:155px;'>Head of household</div>"
                          + "    <div style='color:#000000;position:absolute;left:225px;top:155px;'>M: " + hoHMaleCount +
                          "</div>"
                          + "    <div style='color:#000000;position:absolute;left:325px;top:155px;'>F: " +
                          hoHFemaleCount + "</div>"
                          + "    <div style='color:#000000;position:absolute;left:425px;top:155px;'>Total Families: " +
                          hoHTotalFamiles + "</div>"
                          +
                          "    <div style='color:#000000;position:absolute;left:25px;top:180px;'># Individuals in the household: " +
                          hoHIndividuals + "</div>"
                          + "    <div style='color:#000000;position:absolute;left:600px;top:105px;'>LtCxl: " +
                          arrayOfCancellations[0] + "</div>"
                          + "    <div style='color:#000000;position:absolute;left:600px;top:130px;'>Cxl: " +
                          arrayOfCancellations[1] + "</div>"
                          + "    <div style='color:#000000;position:absolute;left:600px;top:155px;'>No Show: " +
                          arrayOfCancellations[2] + "</div>"
                          +
                          "    <div style='color:#000000;position:absolute;left:25px;top:205px;'>Total Hours of service: " +
                          totalHoursofService + "</div>"
                          + "    <div style='color:#000000;position:absolute;left:25px;top:235px;'>Age:</div>"
                          + "    <div style='color:#000000;position:absolute;left:150px;top:235px;'>1. 0-5: " +
                          ageTotals[0] + "</div>"
                          + "    <div style='color:#000000;position:absolute;left:300px;top:235px;'>2. 6-11: " +
                          ageTotals[1] + "</div>"
                          + "    <div style='color:#000000;position:absolute;left:450px;top:235px;'>3. 12-17: " +
                          ageTotals[2] + "</div>"
                          + "    <div style='color:#000000;position:absolute;left:600px;top:235px;'>4. 18-23: " +
                          ageTotals[3] + "</div>"
                          + "    <div style='color:#000000;position:absolute;left:150px;top:255px;'>5. 24-44: " +
                          ageTotals[4] + "</div>"
                          + "    <div style='color:#000000;position:absolute;left:300px;top:255px;'>6. 45-54: " +
                          ageTotals[5] + "</div>"
                          + "    <div style='color:#000000;position:absolute;left:450px;top:255px;'>7. 55-69: " +
                          ageTotals[6] + "</div>"
                          + "    <div style='color:#000000;position:absolute;left:600px;top:255px;'>8. 70+: " +
                          ageTotals[7] + "</div>"
                          +
                          "    <div style='color:#000000;position:absolute;left:25px;top:295px;'>Gender of Client</div>"
                          + "    <div style='color:#000000;position:absolute;left:225px;top:295px;'>M: " + totalMales +
                          "</div>"
                          + "    <div style='color:#000000;position:absolute;left:325px;top:295px;'>F: " + totalFemales +
                          "</div>"
                          + "    <div style='color:#000000;position:absolute;left:25px;top:320px;'>Ethnicity</div>"
                          +
                          "    <div style='color:#000000;position:absolute;left:150px;top:320px;'>1. African American: " +
                          totalEthnicity[0] + "</div>"
                          + "    <div style='color:#000000;position:absolute;left:340px;top:320px;'>2. Natice/Alaskan: " +
                          totalEthnicity[1] + "</div>"
                          +
                          "    <div style='color:#000000;position:absolute;left:510px;top:320px;'>3. Pacific Islander: " +
                          totalEthnicity[2] + "</div>"
                          + "    <div style='color:#000000;position:absolute;left:700px;top:320px;'>4. Asian: " +
                          totalEthnicity[3] + "</div>"
                          + "    <div style='color:#000000;position:absolute;left:150px;top:340px;'>5. Caucasian: " +
                          totalEthnicity[4] + "</div>"
                          + "    <div style='color:#000000;position:absolute;left:340px;top:340px;'>6. Hispanic: " +
                          totalEthnicity[5] + "</div>"
                          + "    <div style='color:#000000;position:absolute;left:510px;top:340px;'>7. Other: " +
                          totalEthnicity[6] + "</div>"
                          + "    <div style='color:#000000;position:absolute;left:25px;top:380px;'>Income</div>"
                          + "    <div style='color:#000000;position:absolute;left:150px;top:380px;'>1. $0-9,999: " +
                          totalIncome[0] + "</div>"
                          + "    <div style='color:#000000;position:absolute;left:300px;top:380px;'>2. $10,000-14,999: " +
                          totalIncome[1] + "</div>"
                          + "    <div style='color:#000000;position:absolute;left:450px;top:380px;'>3. $15,000-24,999: " +
                          totalIncome[2] + "</div>"
                          + "    <div style='color:#000000;position:absolute;left:600px;top:380px;'>4. $25,000-34,999: " +
                          totalIncome[3] + "</div>"
                          + "    <div style='color:#000000;position:absolute;left:150px;top:400px;'>5. $35,000+: " +
                          totalIncome[4] + "</div>"
                          + "    <div style='color:#000000;position:absolute;left:25px;top:425px;'>County:</div>"
                          + "    <div style='color:#000000;position:absolute;left:125px;top:425px;'>1. Weber: " +
                          totalCounty[0] + "</div>"
                          + "    <div style='color:#000000;position:absolute;left:250px;top:425px;'>2. Davis: " +
                          totalCounty[1] + "</div>"
                          + "    <div style='color:#000000;position:absolute;left:375px;top:425px;'>3. DCLC: " +
                          totalCounty[2] + "</div>"
                          + "    <div style='color:#000000;position:absolute;left:500px;top:425px;'>4. Morgan: " +
                          totalCounty[3] + "</div>"
                          + "    <div style='color:#000000;position:absolute;left:625px;top:425px;'>5. Box Elder: " +
                          totalCounty[4] + "</div>"
                          + "    <div style='color:#000000;position:absolute;left:770px;top:425px;'>6. Other: " +
                          totalCounty[5] + "</div>";

            String toPrintFunding =
                "    <div style='color:#000000;position:absolute;left:25px;top:455px;'>Funding Source:</div>";
            int fCount = 0;
            int xLoc = 25;
            int yLoc = 475;
            int totalCount = 1;
            foreach (var fu in listOfAllKnownFunding)
            {
                toPrintFunding += "    <div style='color:#000000;position:absolute;left:" + xLoc + "px;top:" + yLoc +
                                  "px;'>" + totalCount + ". " + fu.fundingSource + ": " +
                                  arrayOfFundingCounts[totalCount - 1, 1] + "</div>";
                if (fCount <= 2)
                {
                    xLoc += 300;
                    fCount++;
                }
                else
                {
                    yLoc += 20;
                    xLoc = 25;
                    fCount = 0;
                }
                totalCount++;
            }
            yLoc += 40;
            String toPrinProblems = "     <div style='color:#000000;position:absolute;left:25px;top:" + yLoc +
                                    "px; '>Problem:</div>";
            int pCount = 1;
            totalCount = 1;
            xLoc = 45;
            yLoc += 20;
            foreach (var pr in listOfAllKnownProblems)
            {
                toPrinProblems += "    <div style='color:#000000;position:absolute;left:" + xLoc + "px;top:" + yLoc +
                                  "px;'>" + totalCount + ". " + pr.problemType + ": " +
                                  arrayOfProblemCounts[pr.problemID] + "</div>";
                if (pCount <= 3) // 3 per row
                {
                    if (pCount == 1)
                    {
                        xLoc += 195;
                    }
                    else if (pCount == 2)
                    {
                        xLoc += 235;
                    }
                    else
                    {
                        xLoc += 210;
                    }
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

            return toPrint;
        }

        public int newPatients { get; set; }
        public int ongoingPatients { get; set; }
        public double totalMinutesofService { get; set; }
        public double totalMinutesofClientService { get; set; }
        public int individualSessions { get; set; }
        public int groupSessions { get; set; }
        public int familySessions { get; set; }
        //HoH
        public int hoHMaleCount { get; set; }
        public int hoHFemaleCount { get; set; }
        public int hoHTotalFamiles { get; set; }
        public int hoHIndividuals { get; set; }
        //Age count
        public int[] ageTotals { get; set; }
        //Count based on gender
        public int totalMales { get; set; }
        public int totalFemales { get; set; }
        //Ethnicity count
        public int[] totalEthnicity { get; set; }
        public int[] totalIncome { get; set; }
        public int[] totalCounty { get; set; }
        public List<FundingItem> listOfAllKnownFunding { get; set; }
        public IQueryable<PatientProblem> listOfAllKnownProblems { get; set; }
        public string staffName{get;set;}
        public string location { get; set; }
        public int[] arrayOfCancellations { get; set; }
        public int[,] arrayOfFundingCounts { get; set; }
        public int[] arrayOfProblemCounts { get; set; }

        public string demographicsString => "<!DOCTYPE html>"
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
}

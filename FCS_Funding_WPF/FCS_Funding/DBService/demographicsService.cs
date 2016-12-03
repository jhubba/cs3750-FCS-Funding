using FCS_Funding.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCS_Funding.DBService
{
    class demographicsService
    {
        public demographicsService()
        {
            db = new FCS_DBModel();
        }

        public List<MatchingPatient> patientsBetweenDates(DateTime requestedDateStart, DateTime requestedDateEnd)
        {
            var list = (from ap in db.Appointments
                        join ex in db.Expenses on ap.AppointmentID equals ex.AppointmentID
                        join pa in db.Patients on ex.PatientID equals pa.PatientID
                        where ap.AppointmentStartDate > requestedDateStart && ap.AppointmentEndDate < requestedDateEnd
                        group pa by pa.PatientID
                                             into listOne
                        select new MatchingPatient
                        {
                            patientID = listOne.Key,
                            patientInformation = (from pat in db.Patients
                                                  where pat.PatientID == listOne.Key
                                                  select new PatientInformation
                                                  {
                                                      patientFirstName = pat.PatientFirstName,
                                                      patientLastName = pat.PatientLastName,
                                                      ageGroup = pat.PatientAgeGroup,
                                                      gender = pat.PatientGender,
                                                      ethnicity = pat.PatientEthnicity,
                                                      patientIntakeTimeDate = pat.NewClientIntakeHour,
                                                      newClient = false
                                                  }).ToList(),
                            houseHoldInformation = (from pat in db.Patients
                                                    join ph in db.PatientHouseholds on pat.HouseholdID equals ph.HouseholdID
                                                    where pat.PatientID == listOne.Key
                                                    select new HouseHoldInformation
                                                    {
                                                        patientIncome = ph.HouseholdIncomeBracket,
                                                        patientCounty = ph.HouseholdCounty,
                                                        isHeadOfHousehold = pat.IsHead,
                                                        householdID = pat.HouseholdID,
                                                        householdCount = ph.HouseholdPopulation,
                                                        ageGroup = pat.PatientAgeGroup
                                                    }).ToList(),
                            patientProblems = (from pp in db.PatientProblems
                                               where pp.PatientID == listOne.Key
                                               select new PatientProblem
                                               {
                                                   problemID = pp.ProblemID
                                               }).Distinct().ToList()
                        }).ToList();

            return list;
        }

        public List<SessionInfo> sessionsBetweenDates(DateTime requestedDateStart, DateTime requestedDateEnd)
        {
            var list = (from app in db.Appointments
                        join exp in db.Expenses on app.AppointmentID equals exp.AppointmentID
                        join st in db.Staff on app.StaffID equals st.StaffID
                        where app.AppointmentStartDate > requestedDateStart && app.AppointmentEndDate < requestedDateEnd
                        orderby requestedDateStart
                        select new SessionInfo
                        {
                            patientID = exp.PatientID,
                            staff = st.StaffLastName + ", " + st.StaffFirstName,
                            appointmentDateTimeStart = app.AppointmentStartDate,
                            appointmentDateTimeEnd = app.AppointmentEndDate,
                            fundingSource = exp.DonationID,
                            expenseTypeID = exp.ExpenseTypeID,
                            typeofCx = app.AppointmentCancelationType
                        }).ToList();
            return list;
        }

        public List<FundingItem> getDonorsNotPartOfGrant()
        {
            var list = (from don in db.Donations.AsEnumerable()
                        join donor in db.Donors on don.DonationID equals donor.DonorID
                        where don.GrantProposalID == null
                        select new FundingItem
                        {
                            fundingSourceID = don.DonationID,
                            fundingSource = donor.OrganizationName
                        }).ToList();
            return list;
        }

        public List<FundingItem> getAllKnownGrantProposals()
        {
            var list = (from don in db.Donations.AsEnumerable()
                        join gp in db.GrantProposals on don.GrantProposalID equals gp.GrantProposalID
                        where don.GrantProposalID != null
                        select new FundingItem
                        {
                            fundingSourceID = don.DonationID,
                            fundingSource = gp.GrantName
                        }).ToList();
            return list;
        }

        //get count of funding type Insurance
        public int getInsuranceTypeCount()
        {
            return (from it in db.GrantProposals.AsEnumerable()
                    where it.GrantClass == "Insurance"
                    select it.GrantProposalID).Count();
        }

        //get count of funding type EAP
        public int getEAPTypeCount()
        {
            return (from it in db.GrantProposals.AsEnumerable()
                    where it.GrantClass == "EAP"
                    select it.GrantProposalID).Count();
        }

        //get count of funging type Grant
        public int getGrantTypeCount()
        {
            return (from it in db.GrantProposals.AsEnumerable()
                    where it.GrantClass == "Grant"
                    select it.GrantProposalID).Count();
        }

        //get count of funding type Other
        public int getOtherTypeCount()
        {
            return (from it in db.GrantProposals.AsEnumerable()
                    where it.GrantClass == "Other"
                    select it.GrantProposalID).Count();
        }

        public IQueryable<PatientProblem> getAllKnownProblems()
        {
            var list = (from pr in db.Problems
                        orderby pr.ProblemID
                        select new PatientProblem
                        {
                            problemID = pr.ProblemID,
                            problemType = pr.ProblemType,
                        });
            return list;
        }

        public List<PatientProblem> getAllKnownProblemsDistinct()
        {
            var list = (from pr in db.Problems
                        orderby pr.ProblemID
                        select new PatientProblem
                        {
                            problemID = pr.ProblemID,
                            problemType = pr.ProblemType,
                        }).Distinct().ToList();
            return list;
        }


        private FCS_DBModel db { get; set; }
    }

    #region HELPER CLASSES

    public class MatchingPatient
    {
        public int patientID { get; set; }
        public List<PatientInformation> patientInformation { get; set; }
        public List<HouseHoldInformation> houseHoldInformation { get; set; }
        public List<PatientProblem> patientProblems { get; set; }
    }

    public class PatientInformation
    {
        public string patientFirstName { get; set; }
        public string patientLastName { get; set; }
        public string ageGroup { get; set; }
        public string gender { get; set; }
        public string ethnicity { get; set; }
        public DateTime patientIntakeTimeDate { get; set; }
        public bool newClient { get; set; }
    }

    public class HouseHoldInformation
    {
        public string patientIncome { get; set; }
        public string patientCounty { get; set; }
        public bool isHeadOfHousehold { get; set; }
        public int householdID { get; set; }
        public int householdCount { get; set; }
        public string ageGroup { get; set; }
    }

    public class PatientProblem
    {
        public int problemID { get; set; }
        public string problemType { get; set; }
    }

    public class SessionInfo
    {
        public int? patientID { get; set; }
        public string staff { get; set; }
        public DateTime appointmentDateTimeStart { get; set; }
        public DateTime appointmentDateTimeEnd { get; set; }
        public int? fundingSource { get; set; }
        public int expenseTypeID { get; set; }
        public string typeofCx { get; set; }
    }

    public class FundingItem
    {
        public int fundingSourceID { get; set; }
        public string fundingSource { get; set; }
    }
    #endregion

}

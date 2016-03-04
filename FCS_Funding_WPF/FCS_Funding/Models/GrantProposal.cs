using System;
using System.Collections.Generic;

namespace FCS_Funding.Models
{
    public partial class GrantProposal
    {
        public GrantProposal()
        { }
        public GrantProposal(int donorID, string grantName, DateTime subDueDate, string gStatus)
        {
            DonorID = donorID;
            GrantName = grantName;
            SubmissionDueDate = subDueDate;
            GrantStatus = gStatus;
        }
        public int GrantProposalID { get; set; }
        public int DonorID { get; set; }
        public string GrantName { get; set; }
        public System.DateTime SubmissionDueDate { get; set; }
        public string GrantStatus { get; set; }
        public virtual Donor Donor { get; set; }
    }
}

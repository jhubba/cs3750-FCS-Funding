using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FCS_Funding.Views
{
    /// <summary>
    /// Interaction logic for CreateGrantProposal.xaml
    /// </summary>
    public partial class CreateGrantProposal : Window
    {
        public string GrantName { get; set; }

        public string Classification { get; set; }
       
        //private IEnumerable<string> classifications = new List<string>
        //{
        //    "Insurance", "EAP", "Grant", "Other"
        //};
        //public IEnumerable<string> Classifications
        //{
        //    get
        //    {
        //        return classifications;
        //    }
        //}

        public CreateGrantProposal()
        {
            InitializeComponent();

			textbox_grantName.Focus();
        }

        /// <summary>
        /// This method logs you into the system
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Organization_DropDown(object sender, RoutedEventArgs e)
        {
            List<string> p = new List<string>()
            {
                "HAFB", "Weber", "Clearfield"
            };
            Models.FCS_DBModel db = new Models.FCS_DBModel();
            var query = (from o in db.Donors
                         where o.DonorType == "Organization" || o.DonorType == "Government" 
                         orderby o.OrganizationName
                         select o.OrganizationName).ToList();

            var box = sender as ComboBox;
            box.ItemsSource = query;
        }

        private void Create_Grant_Proposal(object sender, RoutedEventArgs e)
        {
            if (SubmissionDueDate.ToString() != "" && Organization.SelectedValue != null && ClassificationCmbx.SelectedValue != null && !string.IsNullOrEmpty(GrantName))
            {
                determineClass(ClassificationCmbx.SelectedIndex);
                string organiz = Organization.SelectedValue.ToString();
                DateTime datet = Convert.ToDateTime(SubmissionDueDate.ToString());
                string classification = ClassificationCmbx.SelectedValue.ToString();

                //MessageBox.Show(organiz + "\n" + datet + "\n" + GrantName + "\n" + "Status is Pending");
                Models.FCS_DBModel db = new Models.FCS_DBModel();
                int DonorID = (from d in db.Donors
                               where d.OrganizationName == organiz
                               select d.DonorID).Distinct().First();

                Models.GrantProposal gp = new Models.GrantProposal();

                gp.DonorID = DonorID;
                gp.GrantName = GrantName;
                gp.SubmissionDueDate = datet;
                gp.GrantStatus = "Pending";
                gp.GrantClass = Classification;

                db.GrantProposals.Add(gp);
                db.SaveChanges();
                
                this.Close();
            }
            else
            {
                MessageBox.Show("All fields must be filled");
            }

        }

		private void useEnterAsTab(object sender, System.Windows.Input.KeyEventArgs e)
		{
			CommonControl.IntepretEnterAsTab(sender, e);
		}

        private void determineClass(int selection)
        {
            switch (selection)
            {
                case 0:
                    Classification = "Insurance";
                    break;
                case 1:
                    Classification = "EAP";
                    break;
                case 2:
                    Classification = "Grant";
                    break;
                case 3:
                    Classification = "Other";
                    break;

            }
        }
    }
}

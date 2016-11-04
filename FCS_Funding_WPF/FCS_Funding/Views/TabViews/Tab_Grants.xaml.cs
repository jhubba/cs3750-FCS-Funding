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
using System.Windows.Navigation;
using System.Windows.Shapes;
using FCS_DataTesting;
using FCS_Funding.Models;

namespace FCS_Funding.Views.TabViews
{
    using Definition;
    using System.Collections.ObjectModel;
    /// <summary>
    /// Interaction logic for Tab_Grants.xaml
    /// </summary>
    public partial class Tab_Grants : UserControl
    {

        public Tab_Grants()
        {
            InitializeComponent();
        }

        private void EditGrant(object sender, MouseButtonEventArgs e)
        {
            var db = new FCS_DBModel();

            try
            {
                DataGrid dg = sender as DataGrid;

                GrantsDataGrid p = (GrantsDataGrid)dg.SelectedItems[0];
                UpdateGrant up = new UpdateGrant(p);

                var expenseTotal = (from ex in db.Expenses
                                    where ex.DonationID == p.DonationID
                                    select ex).Count();
                if (expenseTotal > 0) { up.DonAmount.IsEnabled = false; up.AmountRem.IsEnabled = false; }
                up.DonationDate.SelectedDate = p.DonationDate;
                up.DonationExpiration.SelectedDate = p.ExpirationDate;
                up.ShowDialog();
            }
            catch (Exception error)
            {
            }

            Refresh_GrantGrid(sender, e);
        }

        private void Open_CreateGrantProposal(object sender, RoutedEventArgs e)
        {
            CreateGrantProposal cgp = new CreateGrantProposal();
            cgp.ShowDialog();

            Refresh_GrantGrid(sender, e);
        }

        private void Refresh_GrantGrid(object sender, RoutedEventArgs e)
        {
            Models.FCS_DBModel db = new Models.FCS_DBModel();
            var query = from g in db.GrantProposals
                        join d in db.Donors on g.DonorID equals d.DonorID
                        select new GrantProposalGrid
                        {
                            GrantName = g.GrantName,
                            OrganizationName = d.OrganizationName,
                            SubmissionDueDate = g.SubmissionDueDate,
                            GrantStatus = g.GrantStatus,
                            GrantProposalID = g.GrantProposalID,
                            DonorID = g.DonorID
                        };

            Grant_DataGrid.ItemsSource = query.ToList();
        }

        private void EditGrantProposal(object sender, MouseButtonEventArgs e)
        {
            int index;
            DataGrid dg = sender as DataGrid;
            if (dg.SelectedIndex != -1)
            {
                GrantProposalGrid p = (GrantProposalGrid)dg.SelectedItems[0]; // OR:  Patient p = (Patient)dg.SelectedItem;
                if (p.GrantStatus == "Accepted") { index = 1; }
                else if (p.GrantStatus == "Not Accepted") { index = 2; }
                else { index = 0; }

                Models.FCS_DBModel db = new Models.FCS_DBModel();
                EditGrantProposals dgp = new EditGrantProposals(p);
                dgp.ShowDialog();
                dgp.text_GrantName.IsEnabled = false;
                if (index == 1 || index == 2)
                {
                    dgp.combobox_Status.IsEnabled = false;
                }
                dgp.combobox_Status.SelectedIndex = index;
            }
            Refresh_GrantGrid(sender, e);
        }
    }
}

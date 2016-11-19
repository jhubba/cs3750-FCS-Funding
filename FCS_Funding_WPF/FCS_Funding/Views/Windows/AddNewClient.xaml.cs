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

namespace FCS_Funding.Views.Windows
{
    using FCS_Funding;
    using System.Windows.Controls.Primitives;
    using Definition;
    using FCS_DataTesting;
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
        //public string patientOQ { get; set; }
        public Boolean headOfHouse { get; set; }
        public string PatientGender { get; set; }
        public DateTime date { get; set; }
        public string familyOQNumber { get; set; }
        public string relationToHead { get; set; }
        //	household properties
        private string Income { get; set; }
        public int HouseholdPopulation { get; set; }
        public string County { get; set; }
        private string ageGroup { get; set; }
        private string ethnicGroup { get; set; }

        public AddNewClient()
        {
            InitializeComponent();
        }

        private void useEnterAsTab(object sender, System.Windows.Input.KeyEventArgs e)
        {
            CommonControl.IntepretEnterAsTab(sender, e);
        }

        private void radio_isHead_Checked(object sender, RoutedEventArgs e)
        {
            var rb = sender as RadioButton;
            if (rb.Name.Equals("radio_Dependent"))
            {
                textbox_RelationToHead.IsEnabled = true;
                textbox_SearchHead.IsEnabled = true;
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

        private void AddNewSession(object sender, RoutedEventArgs e)
        {

        }

        private void button_SearchHeadOfHousehold_Click(object sender, RoutedEventArgs e)
        {

        }

        private void txt_NumberOnlyCheck(object sender, TextCompositionEventArgs e)
        {
            CommonControl.NumberOnlyEventCheckNoPeriod(sender, e);
        }

        private void Close_Window(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

       
    }
}

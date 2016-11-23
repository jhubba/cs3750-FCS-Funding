using FCS_Funding.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace FCS_Funding.Views
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        ObservableCollection<Patient> list { get; set; }
        public UserControl1()
        {
            InitializeComponent();
        }
        
        public ObservableCollection<Patient> searchHead (String LastName)
        {
            FCS_Funding.Models.FCS_DBModel db = new FCS_Funding.Models.FCS_DBModel();

            var headID = (from p in db.Patients

                          where p.PatientLastName.Equals(LastName) && p.IsHead == true
                          select p );
            var list2 = new ObservableCollection<Patient>(headID);
            return list2;
        }
       // wpf tutorial use datagrid with radio buttons.
    }
}

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
    /// <summary>
    /// Interaction logic for UpdateClient.xaml
    /// </summary>
    public partial class UpdateClient : Window
    {
        public UpdateClient()
        {
            InitializeComponent();
        }

        private void button_UpdateClient_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Close_Window(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void useEnterAsTab(object sender, System.Windows.Input.KeyEventArgs e)
        {
            CommonControl.IntepretEnterAsTab(sender, e);
        }

        private void check_UpdateHousehold_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Change_UpdateHousehold(object sender, RoutedEventArgs e)
        {

        }

        private void txt_NumberOnlyCheck(object sender, TextCompositionEventArgs e)
        {
            CommonControl.NumberOnlyEventCheckNoPeriod(sender, e);
        }
    }
}

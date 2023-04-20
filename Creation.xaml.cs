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
using EFIGeneratorByPreuty;

namespace EFIGeneratorByPreuty
{
    /// <summary>
    /// Interaction logic for Creation.xaml
    /// </summary>
    public partial class Creation : Window
    {
        int page = 1;
        UserPC pc = new UserPC();

        public Creation()
        {
            InitializeComponent();
            _mainFrame.Navigate(new Page1());
        }

        private void Move(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            pc.CpuType = Page1.cpuType;
            pc.CpuName = Page1.cpuName;
            pc.PCType = Page1.pcType;
            if (!pc.CheckCorrectValuesPage1(Page1.allNames))
            {
                lblError.Content = "⚠ Make sure to fill up everything and that you used the proposed cpu names";
                return;
            }
            lblError.Content = "";
        }
    }
}

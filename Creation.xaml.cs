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
        public static bool stop = false;

        public Creation()
        {
            InitializeComponent();
            _mainFrame.Navigate(new Page1());
        }

        private void MoveOrClose(object sender, MouseButtonEventArgs e)
        {
            string x = e.OriginalSource.GetType().Name;
            if (x != null)
                if ( x != "Ellipse" && x != "TextBlock" && x != "TextBoxView" && x != "Rectangle")
                {
                    if (Page1.CmbBrand.IsMouseOver)
                    {
                        Page1.CmbBrand.IsDropDownOpen = true;
                        return;
                    }
                    else if (Page1.CmbPC.IsMouseOver)
                    {
                        Page1.CmbPC.IsDropDownOpen = true;
                        return;
                    }

                    this.DragMove();
                    Page1.l.Height = 0;
                }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            pc.CpuName = Page1.cpuName;
            pc.PCType = Page1.pcType;
            pc.PCBrand = Page1.pcBrand;
            pc.OCVersion = Page1.opencoreVersion;
            if (!pc.CheckCorrectValuesPage1(Page1.allNames))
            {
                lblError.Content = "⚠ Make sure to fill up everything and that you used the proposed cpu names";
                return;
            }
            stop = true;
            lblError.Content = "";
        }
    }
}

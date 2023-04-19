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

namespace EFIGeneratorByPreuty
{
    /// <summary>
    /// Interaction logic for Creation.xaml
    /// </summary>
    public partial class Creation : Window
    {
        public Creation()
        {
            InitializeComponent();
        }

        private void Move(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}

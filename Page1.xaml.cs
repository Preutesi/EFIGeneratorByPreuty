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
using System.Xml.Linq;
using System.IO;

namespace EFIGeneratorByPreuty
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        List<string> intel = new List<string>();
        List<string> amd = new List<string>();


        public Page1()
        {
            InitializeComponent();
            CheckForIntelUPDATES();
            CheckForAmdUPDATES();
            GetAllCPUNames();
        }
        
        void CheckForIntelUPDATES()
        {
            //fai partire un codice python
        }

        void CheckForAmdUPDATES()
        {
            //fai partire il codice python
        }

        private void txtCPU_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        void GetAllCPUNames()
        {
            intel = IntelCPU();
            amd = AmdCPU();
        }

        List<string> IntelCPU()
        {
            string path = Directory.GetCurrentDirectory() + @"\AllSupported\INTELCPUNames.csv";

            StreamReader sr = new StreamReader(path);
            string line = sr.ReadLine();
            while (true)
            {
                line = sr.ReadLine();
                string[] items = line.Split(";");
                if (int.TryParse(items[1], out int i))
                {

                }
            }
        }

        List<string> AmdCPU()
        {

        }
    }
}

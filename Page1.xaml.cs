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
using System.Windows.Threading;

namespace EFIGeneratorByPreuty
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        List<string> intel = new List<string>();
        List<string> amd = new List<string>();
        List<string> allNames = new List<string>();
        


        public Page1()
        {
            InitializeComponent();
            CheckForIntelUPDATES();
            CheckForAmdUPDATES();
            GetAllCPUNames();
            allNames = intel;
            allNames.AddRange(amd);
            cmbCPU.Items.Clear();
            for (int i = 0; i < allNames.Count; i++)
                cmbCPU.Items.Add(allNames[i]);
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
            cmbCPU.IsDropDownOpen = true;
            for (int i = 0; i < allNames.Count; i++)
                if (allNames[i].Contains(txtCPU.Text))
                {
                    cmbCPU.SelectedIndex = i;
                    break;
                }
            txtCPU.Focus();
        }

        void GetAllCPUNames()
        {
            intel = IntelCPU();
            amd = AmdCPU();
        }

        List<string> IntelCPU()
        {
            string path = Directory.GetCurrentDirectory() + @"\AllSupported\INTELCPUNames.csv";
            List<string> names = new List<string>();

            StreamReader sr = new StreamReader(path);
            string line = sr.ReadLine();
            while (true)
            {
                line = sr.ReadLine();
                if (line == null)
                    break;
                string[] items = line.Split(";");
                if (int.TryParse(items[1], out int i))
                    names.Add(items[0] + "|" + items[items.Length - 1]);
                else 
                    names.Add(items[0] + " " + items[1] + "|" + items[items.Length - 1]);
            }
            return names;
        }

        List<string> AmdCPU()
        {
            string path = Directory.GetCurrentDirectory() + @"\AllSupported\AMDCPUNames.csv";
            List<string> names = new List<string>();

            StreamReader sr = new StreamReader(path);
            string line = sr.ReadLine();
            while (true)
            {
                line = sr.ReadLine();
                if (line == null)
                    break;
                string[] items = line.Split(",");
                items = items.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                for (int i = 0; i < items.Length; i++)
                    items[i] = items[i].Replace("\"", "");
                names.Add(items[1]);
            }
            return names;
        }
    }
}

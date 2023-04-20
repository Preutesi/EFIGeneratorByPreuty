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
using System.Reflection;

namespace EFIGeneratorByPreuty
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        List<string> intel = new List<string>();
        List<string> amd = new List<string>();
        public static List<string> allNames = new List<string>();

        public static string cpuType = "";
        public static string cpuName = "";
        public static string pcType = "";


        public Page1()
        {
            InitializeComponent();
            CheckForIntelUPDATES();
            CheckForAmdUPDATES();
            GetAllCPUNames();
            BkgProcess();
            allNames = intel;
            allNames.AddRange(amd);
            lstCPU.Items.Clear();
            for (int i = 0; i < allNames.Count; i++)
                lstCPU.Items.Add(allNames[i]);
        }

         void BkgProcess()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                cpuName = txtCPU.Text;
                if (!(bool)rbIntel.IsChecked && !(bool)rbAmd.IsChecked)
                    cpuType = "";
                if ((bool)rbIntel.IsChecked)
                    cpuType = "Intel";
                else if ((bool)rbAmd.IsChecked)
                    cpuType = "Amd";
                pcType = cmbPC.Text;
            }));
        }
        
        void CheckForIntelUPDATES()
        {
            //fai partire un codice python per scraping
        }

        void CheckForAmdUPDATES()
        {
            //fai partire il codice python per scraping
        }

        int before = 0;

        private void txtCPU_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (before == 0 && txtCPU.Text.Length == 1)
                lstCPU.Height = 135;
            for (int i = 0; i < allNames.Count; i++)
                if (allNames[i].ToLower().Contains(txtCPU.Text.ToLower()))
                {
                    var item = lstCPU.Items.GetItemAt(i);
                    lstCPU.ScrollIntoView(item);
                    ListBoxItem lbi = (ListBoxItem)lstCPU.ItemContainerGenerator.ContainerFromIndex(i);
                    lbi.IsSelected = true;
                    break;
                }
            txtCPU.Focus();
        }

        private void SetName(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                txtCPU.Text = allNames[lstCPU.SelectedIndex];
                lstCPU.Height = 0;
            }
            else if (e.Key == Key.Up)
            {
                if (lstCPU.SelectedIndex - 1 >= 0)
                {
                    var item = lstCPU.Items.GetItemAt(lstCPU.SelectedIndex - 1);
                    lstCPU.ScrollIntoView(item);
                    ListBoxItem lbi = (ListBoxItem)lstCPU.ItemContainerGenerator.ContainerFromIndex(lstCPU.SelectedIndex - 1);
                    lbi.IsSelected = true;
                }
            }
            else if (e.Key == Key.Down)
            {
                if (lstCPU.SelectedIndex + 1 < allNames.Count)
                {
                    var item = lstCPU.Items.GetItemAt(lstCPU.SelectedIndex + 1);
                    lstCPU.ScrollIntoView(item);
                    ListBoxItem lbi = (ListBoxItem)lstCPU.ItemContainerGenerator.ContainerFromIndex(lstCPU.SelectedIndex + 1);
                    lbi.IsSelected = true;
                }
            }
            txtCPU.Focus();
        }

        private void SetNameOnclick(object sender, MouseButtonEventArgs e)
        {
            txtCPU.Text = allNames[lstCPU.SelectedIndex];
            lstCPU.Height = 0;
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
                    names.Add(items[0] + " | " + items[items.Length - 2]);
                else 
                    names.Add(items[0] + " " + items[1] + " | " + items[items.Length - 1]);
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

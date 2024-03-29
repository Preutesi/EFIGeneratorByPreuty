﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Threading;
using System.Threading.Tasks;

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
        public static string pcBrand = "";
        public static string opencoreVersion = "";

        public static ComboBox CmbPC, CmbBrand;

        public static ListBox l;

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
            l = lstCPU;
            CmbPC = cmbPC;
            CmbBrand = cmbBrand;
        }

        void cht()
        {
            while (true)
            {
                txtCPU.Dispatcher.BeginInvoke(new Action(() =>
                {
                    cpuName = txtCPU.Text;
                }));

                cmbPC.Dispatcher.BeginInvoke(new Action(() =>
                {
                    pcType = cmbPC.Text;
                }));

                cmbBrand.Dispatcher.BeginInvoke(new Action(() =>
                {
                    pcBrand = cmbBrand.Text;
                }));

                rbDebug.Dispatcher.BeginInvoke(new Action(() =>
                {
                    rbRelease.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (!(bool)rbDebug.IsChecked && !(bool)rbRelease.IsChecked)
                            opencoreVersion = "";
                        if ((bool)rbRelease.IsChecked)
                            opencoreVersion = "Release";
                        else if ((bool)rbDebug.IsChecked)
                            opencoreVersion = "Debug";
                    }));
                }));
                Thread.Sleep(100);
                if (Creation.stop)
                    break;
            }
        }

        void BkgProcess()
        {
            Thread t = new Thread(() =>
            {
                cht();
            });
            t.Start();
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

        void txtCPU_TextChanged(object sender, TextChangedEventArgs e)
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

        void SetName(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                txtCPU.Text = allNames[lstCPU.SelectedIndex];
                lstCPU.Height = 0;
            }
            else if (e.Key == Key.Up)
            {
                try
                {
                    if (lstCPU.SelectedIndex - 1 >= 0)
                    {
                        var item = lstCPU.Items.GetItemAt(lstCPU.SelectedIndex - 1);
                        lstCPU.ScrollIntoView(item);
                        ListBoxItem lbi = (ListBoxItem)lstCPU.ItemContainerGenerator.ContainerFromIndex(lstCPU.SelectedIndex - 1);
                        lbi.IsSelected = true;
                    }
                }
                catch
                {
                    return;
                }
            }
            else if (e.Key == Key.Down)
            {
                try
                {
                    if (lstCPU.SelectedIndex + 1 < allNames.Count)
                    {
                        var item = lstCPU.Items.GetItemAt(lstCPU.SelectedIndex + 1);
                        lstCPU.ScrollIntoView(item);
                        ListBoxItem lbi = (ListBoxItem)lstCPU.ItemContainerGenerator.ContainerFromIndex(lstCPU.SelectedIndex + 1);
                        lbi.IsSelected = true;
                    }
                }
                catch
                {
                    return;
                }
            }
            txtCPU.Focus();
        }

        void SetNameOnclick(object sender, MouseButtonEventArgs e)
        {
            string x = e.OriginalSource.GetType().Name;
            if (lstCPU.Height != 0 && x != "Rectangle")
            {
                txtCPU.Text = allNames[lstCPU.SelectedIndex];
                lstCPU.Height = 0;
            }
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
                    names.Add("Intel " + items[0] + " | " + items[items.Length - 2]);
                else
                    names.Add("Intel " + items[0] + " " + items[1] + " | " + items[items.Length - 1]);
            }
            return names;
        }

        List<string> AmdCPU()
        {
            string path = Directory.GetCurrentDirectory() + @"\AllSupported\AMDCPUNames.csv";
            List<string> names = new List<string>();
            List<string> finalNames = new List<string>();

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
                if (!items[1].Contains("AMD"))
                    names.Add("AMD " + items[1].Replace("™", "") + "|" + items[2].Replace("™", ""));
                else
                    names.Add(items[1].Replace("™", "") + "|" + items[2].Replace("™", ""));
            }
            List<string> x = BetterAMD();

            for (int i = 0; i < names.Count; i++)
                for (int j = 0; j < x.Count; j++)
                {
                    if (names[i].Contains(x[j].Split("|")[1]))
                        finalNames.Add(names[i].Split("|")[0] + "|" + x[j].Split("|")[0]);
                }

            return finalNames;
        }

        List<string> BetterAMD()
        {
            string path = Directory.GetCurrentDirectory() + @"\AllSupported\BetterAMD.csv";
            StreamReader sr = new StreamReader(path);
            List<string> names = new List<string>();
            int counter = 0;

            while (true)
            {
                string line = sr.ReadLine();
                if (line == null)
                    break;
                string[] items = line.Split(";");
                if (items[0] == "" && items[1] == "" && counter == 1)
                    names.Add(names[counter - 1]);
                if (items[0] == "" && items[1] == "" && counter != 1)
                    names.Add(names[counter - 2]);
                else if (items[0] == "" && items[1] != "")
                {
                    string lilItem = names[counter - 2].Split("|")[0];
                    names.Add(lilItem + "|" + items[1]);
                }
                else if (items[0] != "" && items[1] != "")
                    names.Add(items[0] + "|" + items[1]);
                counter++;
            }
            return names;
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            cmbBrand.IsDropDownOpen = true;
        }

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            cmbPC.IsDropDownOpen = true;
        }
    }
}

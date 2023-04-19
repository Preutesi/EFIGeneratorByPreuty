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
using System.Net;
using System.Net.Http;
using System.IO;
using System.IO.Compression;
using System.IO.Packaging;
using System.Linq.Expressions;
using System.Threading;

namespace EFIGeneratorByPreuty
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CheckForUpdates();
        }

        private void Move(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        void CheckForUpdates()
        {
            CheckForOpencore();
        }

        void DeleteUselessFilesOpencore()
        {
            ExecutionLog.Text = "Deleting useless files...";

            string pathBOOT = Directory.GetCurrentDirectory() + @"\Files\Debug\OPENCORE\EFI\BOOT\";

            foreach (var f in Directory.GetFiles(pathBOOT))
                if (f.Replace(pathBOOT, "") != "BOOTx64.efi")
                    File.Delete(f);

            string pathDRIVERS = Directory.GetCurrentDirectory() + @"\Files\Debug\OPENCORE\EFI\OC\Drivers\";
            foreach (var f in Directory.GetFiles(pathDRIVERS))
            {
                string t = f.Replace(pathDRIVERS, "");
                if (t != "OpenUsbKbDxe.efi" && t != "OpenPartitionDxe.efi" && t != "ResetNvramEntry.efi" && t != "OpenRuntime.efi")
                    File.Delete(f);
            }

            string pathTOOLS = Directory.GetCurrentDirectory() + @"\Files\Debug\OPENCORE\EFI\OC\Tools\";
            foreach (var f in Directory.GetFiles(pathTOOLS))
            {
                string t = f.Replace(pathTOOLS, "");
                if (t != "OpenShell.efi")
                    File.Delete(f);
            }

            string pathBASE = Directory.GetCurrentDirectory() + @"\Files\Debug\OPENCORE\EFI\OC\";
            foreach (var f in Directory.GetFiles(pathBASE))
            {
                string t = f.Replace(pathBASE, "");
                if (t != "OpenCore.efi")
                    File.Delete(f);
            }
            ExecutionLog.Text = "Deleting useless files... DONE";
        }

        async void CheckForOpencore()
        {
            ExecutionLog.Text = "Checking for opencore folder updates... DEBUG";
            string pathDEBUG = Directory.GetCurrentDirectory() + @"\Files\Debug\OPENCORE\Debug.zip";
            string debug = Directory.GetCurrentDirectory() + @"\Files\Debug\OPENCORE\";
            string pathRELEASE = Directory.GetCurrentDirectory() + @"\Files\Release\OPENCORE\Release.zip";
            string release = Directory.GetCurrentDirectory() + @"\Files\Release\OPENCORE\";

            var httpClient = new HttpClient();
            File.Delete(pathDEBUG);
            File.Delete(pathRELEASE);

            ExecutionLog.Text = "Checking for opencore folder updates... RELEASE";

            using (var stream = await httpClient.GetStreamAsync("https://github.com/acidanthera/OpenCorePkg/releases/download/0.9.1/OpenCore-0.9.1-DEBUG.zip"))
            {
                using (var fileStream = new FileStream(pathDEBUG, FileMode.CreateNew))
                {
                    await stream.CopyToAsync(fileStream);
                }
            }

            using (var stream = await httpClient.GetStreamAsync("https://github.com/acidanthera/OpenCorePkg/releases/download/0.9.1/OpenCore-0.9.1-RELEASE.zip"))
            {
                using (var fileStream = new FileStream(pathRELEASE, FileMode.CreateNew))
                {
                    await stream.CopyToAsync(fileStream);
                }
            }

            ExecutionLog.Text = "Checking for opencore folder updates... UNIZPPING";

            DeleteDirs(debug);
            DeleteDirs(release);

            ZipFile.ExtractToDirectory(pathDEBUG, debug);
            ZipFile.ExtractToDirectory(pathRELEASE, release);

            File.Delete(pathDEBUG);
            File.Delete(pathRELEASE);

            Directory.Delete(debug + @"EFI", true);
            Directory.Delete(release + @"EFI", true);

            Directory.Move(debug + @"X64\EFI\", debug + "EFI");
            Directory.Move(release + @"X64\EFI\", release + "EFI");
            DeleteDirs(release);
            DeleteDirs(debug);

            ExecutionLog.Text = "Checking for opencore folder updates... DONE";
            DeleteUselessFilesOpencore();
            OpenWindow();
        }

        void OpenWindow()
        {
            Creation c = new Creation();
            c.Show();
            this.Close();
        }

        void DeleteDirs(string path)
        {
            foreach (string item in Directory.GetDirectories(path))
                if (!item.Replace(Directory.GetCurrentDirectory(), "").Contains("EFI"))
                    Directory.Delete(item, true);
        }

    }
}

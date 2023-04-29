using HtmlAgilityPack;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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

        Task<string> ReturnResponse(string url)
        {
            HttpClient client = new HttpClient();
            var response = client.GetStringAsync(url);
            return response;
        }

        string latestValue = "";

        int Latest()
        {
            var r = ReturnResponse("https://github.com/acidanthera/OpenCorePkg/releases").Result;

            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(r);

            var version = htmlDoc.DocumentNode.Descendants("span")
                .Where(node => !node.GetAttributeValue("class", "").Contains("f1 text-bold d-inline mr-3"))
                .ToList();
            foreach (var v in version)
                if (int.TryParse(v.InnerHtml.Replace(".", ""), out int i) && v.InnerHtml.Contains("."))
                {
                    latestValue = v.InnerHtml.Replace(" ", "");
                    latestValue = Regex.Replace(latestValue, @"\n", "");
                    return int.Parse(v.InnerHtml.Replace(" ", "").Replace(@"\n", "").Replace(".", ""));
                }
            return -1;
        }

        string[]? LatestLinks()
        {
            string[] retStr = new string[2];
            //Debug=0   |   Release=1
            string bu = "https://github.com/acidanthera/OpenCorePkg/releases";
            retStr[0] = bu + $"/download/{latestValue}/OpenCore-{latestValue}-DEBUG.zip";
            retStr[1] = bu + $"/download/{latestValue}/OpenCore-{latestValue}-RELEASE.zip";

            string path = Directory.GetCurrentDirectory() + @"\Versions\versions.txt";

            string[] items = File.ReadAllLines(path);
            items[0] = "Opencore=" + latestValue;

            StreamWriter sw = new StreamWriter(path, false);
            foreach (string x in items)
                sw.WriteLine(x.ToString());
            sw.Close();

            return retStr;
        }

        string[]? GetLatestVersion()
        {
            ExecutionLog.Text = "Checking latest version...";
            string path = Directory.GetCurrentDirectory() + @"\Versions\versions.txt";
            StreamReader sr = new StreamReader(path);

            int currentVersion = int.Parse(sr.ReadLine().Split("=")[1].Replace(".", ""));
            sr.Close();
            int latestVersion = Latest();

            if (currentVersion == latestVersion)
                return null;
            else
                return LatestLinks();
        }

        void CheckForUpdates()
        {
            if (IsConnectedToInternet())
            {
                string[]? links = GetLatestVersion();
                if (links != null)
                    CheckForOpencore(links);
                else
                    OpenWindow();
            }
            else
                OpenWindow();
        }

        void DeleteUselessFilesOpencore()
        {
            ExecutionLog.Text = "Deleting useless files...";

            string pathBOOT = Directory.GetCurrentDirectory() + @"\Files\Debug\OPENCORE\EFI\BOOT\";

            foreach (var f in Directory.GetFiles(pathBOOT))
                if (f.Replace(pathBOOT, "") != "BOOTx64.efi")
                    File.Delete(f);
            //AGGIUNGI SCRAPING
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

        async void CheckForOpencore(string[] links)
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

            using (var stream = await httpClient.GetStreamAsync(links[0]))
            {
                using (var fileStream = new FileStream(pathDEBUG, FileMode.CreateNew))
                {
                    await stream.CopyToAsync(fileStream);
                }
            }

            using (var stream = await httpClient.GetStreamAsync(links[1]))
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

        bool IsConnectedToInternet()
        {
            try
            {
                Ping myPing = new Ping();
                String host = "google.com";
                byte[] buffer = new byte[32];
                int timeout = 1000;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                return (reply.Status == IPStatus.Success);
            }
            catch { return false; }
        }

    }
}

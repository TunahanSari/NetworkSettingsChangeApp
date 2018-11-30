using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using Ozgurtek.Analyst.AnalistBase.Logging;

namespace NetworkSettingsChangeApp
{
    public partial class MainForm : Form
    {

        [DllImport("wininet.dll")]
        public static extern bool InternetSetOption
          (IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);
        public const int INTERNET_OPTION_SETTINGS_CHANGED = 39;
        public const int INTERNET_OPTION_REFRESH = 37;
        public static MainForm form;

        private Dictionary<string, NetworkInterface> networkInterfaces = new Dictionary<string, NetworkInterface>();
        public MainForm()
        {
            InitializeComponent();
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();

            Logger.Current.LogChanged += _OnLogChanged;

            form = this;

            foreach (NetworkInterface adapter in nics)
            {
                networkInterfaces.Add(adapter.Description, adapter);
            }
        }
        


        private void EnableProxy(object sender, EventArgs e)
        {
            disableButtons();
            progressBar1.Value = 0;

            string proxyAdapter = ConfigurationManager.AppSettings["ProxyAdapter"];
            string normalAdapter = ConfigurationManager.AppSettings["NormalAdapter"];
            Task TaskOne = Task.Factory.StartNew(() => DisableAdapter(normalAdapter));
            Task TaskTwo = Task.Factory.StartNew(() => EnableAdapter(proxyAdapter));
            setProxy(true);
            progressBar1.Value++;

        }

        private void disableButtons()
        {
            ProxyOnBtn.Enabled = false;
            ProxyOffBtn.Enabled = false;
        }

        private void DisableProxy(object sender, EventArgs e)
        {
            disableButtons();
            progressBar1.Value = 0;

            string proxyAdapter = ConfigurationManager.AppSettings["ProxyAdapter"];
            string normalAdapter = ConfigurationManager.AppSettings["NormalAdapter"];
            Task TaskOne = Task.Factory.StartNew(() => EnableAdapter(normalAdapter));
            Task TaskTwo = Task.Factory.StartNew(() => DisableAdapter(proxyAdapter));
            setProxy(false);
            progressBar1.Value++;
        }

        private void _OnLogChanged(object sender, LogChangedEventArgs e)
        {
            //AddItemThreadSafe(progressTextLabel, e.Time + " | " + e.Log);
        }

        public static void AddItemThreadSafe(Label lbl, object item)
        {
            if (lbl.InvokeRequired)
            {
                lbl.Invoke(new MethodInvoker(delegate
                {
                    lbl.Text = item.ToString();
                }));
            }
            else
            {
                    lbl.Text = item.ToString();
            }

            //while (lbl.Items.Count > 500)
            //{
            //    lbl.Invoke(new MethodInvoker(delegate
            //    {
            //        lbl.Items.RemoveAt(0);
            //    }));
            //}
        }
        
        void setProxy(bool enabled)
        {
            bool settingsReturn, refreshReturn;

            string l = enabled ? "Proxy aktive ediliyor" : "Proxy deaktif ediliyor";
            Logger.Current.Log(l, LogType.Info);
            try
            {

                RegistryKey registry = Registry.CurrentUser.OpenSubKey
                    ("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);

                string proxy = ConfigurationManager.AppSettings["proxyAddress"] + ":" +
                               ConfigurationManager.AppSettings["proxyPort"];

                if (enabled)
                {
                    registry.SetValue("ProxyEnable", 1);
                    registry.SetValue
                        ("ProxyServer", proxy);
                    if ((int) registry.GetValue("ProxyEnable", 0) == 0)
                        Logger.Current.Log("Unable to enable the proxy.",LogType.Info);
                    else
                        Logger.Current.Log("The proxy has been turned on.",LogType.Info);
                }
                else
                {
                    registry.SetValue("ProxyEnable", 0);
                    registry.SetValue("ProxyServer", 0);
                    if ((int) registry.GetValue("ProxyEnable", 1) == 1)
                        Logger.Current.Log("Unable to disable the proxy.",LogType.Info);
                    else
                        Logger.Current.Log("The proxy has been turned off.",LogType.Info);

                }
                registry.Close();
                settingsReturn = InternetSetOption
                    (IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
                refreshReturn = InternetSetOption
                    (IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);
                Logger.Current.Log($"settings Return: {settingsReturn} | refresh Return: {refreshReturn}",LogType.Info);
            }
            catch (Exception ex)
            {
                Logger.Current.LogException(ex);
            }

            progressBar1.Value += 33;
            CheckFinished();
        }

        static void EnableAdapter(string interfaceName)
        {
            Logger.Current.Log("adaptör açılıyor: " + interfaceName, LogType.Info);
            ProcessStartInfo psi = new ProcessStartInfo("netsh", "interface set interface \"" + interfaceName + "\" enable");
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            Process p = new Process();
            p.StartInfo = psi;
            p.EnableRaisingEvents = true;

            //p.OutputDataReceived += ProcOutputDataReceivedHandler;
            //p.ErrorDataReceived += ProcErrorDataReceivedHandler;
            p.Exited += ProcExitedHandler;
            p.Disposed += ProcDisposedHandler;

            p.Start();
        }
        static void DisableAdapter(string interfaceName)
        {
            Logger.Current.Log("adaptör kapanıyor: " + interfaceName, LogType.Info);
            ProcessStartInfo psi = new ProcessStartInfo("netsh", "interface set interface \"" + interfaceName + "\" disable");
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            Process p = new Process();
            p.StartInfo = psi;
            p.EnableRaisingEvents = true;
            //p.OutputDataReceived += ProcOutputDataReceivedHandler;
            //p.ErrorDataReceived += ProcErrorDataReceivedHandler;
            p.Exited += ProcExitedHandler;
            p.Disposed += ProcDisposedHandler;

            p.Start();
        }

        private static void ProcDisposedHandler(object sender, EventArgs e)
        {
            Logger.Current.Log("Adaptör toggle bitti" + sender, LogType.Info);


            form.progressBar1.Invoke(new MethodInvoker(delegate
            {
                form.progressBar1.Value += 33;
            }));
            CheckFinished();
        }

        private static void ProcExitedHandler(object sender, EventArgs e)
        {
            Logger.Current.Log("Adaptör toggle bitti", LogType.Info);
            form.progressBar1.Invoke(new MethodInvoker(delegate
            {
                form.progressBar1.Value += 33;
            }));
            CheckFinished();
        }
       

        private static void CheckFinished()
        {
            if (form.progressBar1.Value == 100)
                form.Invoke(new MethodInvoker(delegate
                {
                    form.EnableButtons();
                }));

        }

        private void EnableButtons()
        {
            ProxyOffBtn.Enabled = true;
            ProxyOnBtn.Enabled = true;
        }


        private void ProxyOnBtn_MouseEnter(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = Color.Green;
            Cursor = Cursors.Hand;

        }

        private void ProxyOnBtn_MouseLeave(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = Color.DarkGreen;
            Cursor = Cursors.Default;

        }

        private void ProxyOffBtn_MouseEnter(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = Color.Red;
            Cursor = Cursors.Hand;

        }

        private void ProxyOffBtn_MouseLeave(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = Color.DarkRed;
            Cursor = Cursors.Hand;
        }
    }
}

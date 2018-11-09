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
    public partial class Form1 : Form
    {

        [DllImport("wininet.dll")]
        public static extern bool InternetSetOption
          (IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);
        public const int INTERNET_OPTION_SETTINGS_CHANGED = 39;
        public const int INTERNET_OPTION_REFRESH = 37;


        private Dictionary<string, NetworkInterface> networkInterfaces = new Dictionary<string, NetworkInterface>();
        public Form1()
        {
            InitializeComponent();
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();

            foreach (NetworkInterface adapter in nics)
            {
                networkInterfaces.Add(adapter.Description, adapter);
                //proxyAdapterCombo.Items.Add(adapter.Description);
                //normalAdapterCombo.Items.Add(adapter.Description);
            }
        }
        


        private void EnableProxy(object sender, EventArgs e)
        {
            string proxyAdapter = proxyAdapterTB.Text;
            string normalAdapter = normalAdapterTB.Text;
            Task TaskOne = Task.Factory.StartNew(() => DisableAdapter(normalAdapter));
            Task TaskTwo = Task.Factory.StartNew(() => EnableAdapter(proxyAdapter));
            setProxy(true);
        }



        private void DisableProxy(object sender, EventArgs e)
        {
            string proxyAdapter = proxyAdapterTB.Text;
            string normalAdapter = normalAdapterTB.Text;
            Task TaskOne = Task.Factory.StartNew(() => EnableAdapter(normalAdapter));
            Task TaskTwo = Task.Factory.StartNew(() => DisableAdapter(proxyAdapter));
            setProxy(false);
        }

        private void _OnLogChanged(object sender, LogChangedEventArgs e)
        {
            AddItemThreadSafe(eventListBox, e.Time + " | " + e.Log);
        }

        public static void AddItemThreadSafe(ListBox lb, object item)
        {
            if (lb.InvokeRequired)
            {
                lb.Invoke(new MethodInvoker(delegate
                {
                    lb.Items.Add(item);
                }));
            }
            else
            {
                lb.Items.Add(item);
            }

            while (lb.Items.Count > 500)
            {
                lb.Invoke(new MethodInvoker(delegate
                {
                    lb.Items.RemoveAt(0);
                }));
            }
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
        }



        static void EnableAdapter(string interfaceName)
        {
            Logger.Current.Log("adaptör açılıyor: " + interfaceName, LogType.Info);
            ProcessStartInfo psi = new ProcessStartInfo("netsh", "interface set interface \"" + interfaceName + "\" enable");
            Process p = new Process();
            p.StartInfo = psi;
            p.Start();
        }

        static void DisableAdapter(string interfaceName)
        {
            Logger.Current.Log("adaptör kapanıyor: " + interfaceName, LogType.Info);
            ProcessStartInfo psi = new ProcessStartInfo("netsh", "interface set interface \"" + interfaceName + "\" disable");
            Process p = new Process();
            p.StartInfo = psi;
            p.Start();
        }
    }
}

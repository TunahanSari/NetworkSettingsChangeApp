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
        KeyboardHook enableHook = new KeyboardHook();
        KeyboardHook disableHook = new KeyboardHook();


        [DllImport("wininet.dll")]
        public static extern bool InternetSetOption
          (IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);
        public const int INTERNET_OPTION_SETTINGS_CHANGED = 39;
        public const int INTERNET_OPTION_REFRESH = 37;
        public static MainForm form;
        private static NotifyIcon notifyIconstatic;
        private static bool isBusy = false;

        private Dictionary<string, NetworkInterface> networkInterfaces = new Dictionary<string, NetworkInterface>();
        public MainForm()
        {
            this.KeyPreview = true;


            InitializeComponent();
            notifyIconstatic = notifyIcon1;
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();

            Logger.Current.LogChanged += _OnLogChanged;

            form = this;

            foreach (NetworkInterface adapter in nics)
            {
                networkInterfaces.Add(adapter.Description, adapter);
            }


            enableHook.KeyPressed +=
            new EventHandler<KeyPressedEventArgs>(EnableProxy);
            enableHook.RegisterHotKey(NetworkSettingsChangeApp.ModifierKeys.Control, 
                Keys.F9);

            disableHook.KeyPressed +=
            new EventHandler<KeyPressedEventArgs>(DisableProxy);
            disableHook.RegisterHotKey(NetworkSettingsChangeApp.ModifierKeys.Control,
                Keys.F10);
        }



        private void EnableProxy(object sender, EventArgs e)
        {
            if (isBusy)
            {
                notifyIcon1.ShowBalloonTip(300, "bekleyin", "işlem devam ediyor lütfen bekleyin", ToolTipIcon.Error);
                return;
            }
            isBusy = true;
            notifyIcon1.ShowBalloonTip(300, null, "Proxy Açılıyor", ToolTipIcon.Info);

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
            if (isBusy)
            {
                notifyIcon1.ShowBalloonTip(300, "", "işlem devam ediyor lütfen bekleyin", ToolTipIcon.Error);
                return;
            }
            isBusy = true;
            notifyIcon1.ShowBalloonTip(300, null, "Proxy kapanıyor", ToolTipIcon.Info);

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
                    notifyIconstatic.ShowBalloonTip(1000, "işlem tamalandı.", "işlem tamamlandı", ToolTipIcon.None);
                    isBusy = false;
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


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.F10))
            {
                MessageBox.Show("What the Ctrl+F10?");
                return true;
            }
            if (keyData == (Keys.Control | Keys.F9))
            {
                MessageBox.Show("What the Ctrl+F9?");
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {

            //notifyIcon1.BalloonTipTitle = "Akom Analist";
            //notifyIcon1.BalloonTipText = "Görev Çubuğunda Çalışmaya Devam Ediyor...";

            if (FormWindowState.Minimized == WindowState)
            {
                notifyIcon1.Visible = true;
                //notifyIcon1.ShowBalloonTip(500);
                Hide();
            }
            else if (FormWindowState.Normal == WindowState)
            {
                notifyIcon1.Visible = false;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }
    }


    public sealed class KeyboardHook : IDisposable
    {
        // Registers a hot key with Windows.
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        // Unregisters the hot key with Windows.
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        /// <summary>
        /// Represents the window that is used internally to get the messages.
        /// </summary>
        private class Window : NativeWindow, IDisposable
        {
            private static int WM_HOTKEY = 0x0312;

            public Window()
            {
                // create the handle for the window.
                this.CreateHandle(new CreateParams());
            }

            /// <summary>
            /// Overridden to get the notifications.
            /// </summary>
            /// <param name="m"></param>
            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);

                // check if we got a hot key pressed.
                if (m.Msg == WM_HOTKEY)
                {
                    // get the keys.
                    Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
                    ModifierKeys modifier = (ModifierKeys)((int)m.LParam & 0xFFFF);

                    // invoke the event to notify the parent.
                    if (KeyPressed != null)
                        KeyPressed(this, new KeyPressedEventArgs(modifier, key));
                }
            }

            public event EventHandler<KeyPressedEventArgs> KeyPressed;

            #region IDisposable Members

            public void Dispose()
            {
                this.DestroyHandle();
            }

            #endregion
        }

        private Window _window = new Window();
        private int _currentId;

        public KeyboardHook()
        {
            // register the event of the inner native window.
            _window.KeyPressed += delegate (object sender, KeyPressedEventArgs args)
            {
                if (KeyPressed != null)
                    KeyPressed(this, args);
            };
        }

        /// <summary>
        /// Registers a hot key in the system.
        /// </summary>
        /// <param name="modifier">The modifiers that are associated with the hot key.</param>
        /// <param name="key">The key itself that is associated with the hot key.</param>
        public void RegisterHotKey(ModifierKeys modifier, Keys key)
        {
            // increment the counter.
            _currentId = _currentId + 1;

            // register the hot key.
            if (!RegisterHotKey(_window.Handle, _currentId, (uint)modifier, (uint)key))
                throw new InvalidOperationException("Couldn’t register the hot key.");
        }

        /// <summary>
        /// A hot key has been pressed.
        /// </summary>
        public event EventHandler<KeyPressedEventArgs> KeyPressed;

        #region IDisposable Members

        public void Dispose()
        {
            // unregister all the registered hot keys.
            for (int i = _currentId; i > 0; i--)
            {
                UnregisterHotKey(_window.Handle, i);
            }

            // dispose the inner native window.
            _window.Dispose();
        }

        #endregion
    }

    /// <summary>
    /// Event Args for the event that is fired after the hot key has been pressed.
    /// </summary>
    public class KeyPressedEventArgs : EventArgs
    {
        private ModifierKeys _modifier;
        private Keys _key;

        internal KeyPressedEventArgs(ModifierKeys modifier, Keys key)
        {
            _modifier = modifier;
            _key = key;
        }

        public ModifierKeys Modifier
        {
            get { return _modifier; }
        }

        public Keys Key
        {
            get { return _key; }
        }
    }

    /// <summary>
    /// The enumeration of possible modifiers.
    /// </summary>
    [Flags]
    public enum ModifierKeys : uint
    {
        Alt = 1,
        Control = 2,
        Shift = 4,
        Win = 8
    }


}

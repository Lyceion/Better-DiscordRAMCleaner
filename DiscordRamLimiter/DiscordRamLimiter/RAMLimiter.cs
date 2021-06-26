using System;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DiscordRamLimiter
{
    public class WinAPI
    {
        [DllImport("kernel32.dll")]
        public static extern bool SetProcessWorkingSetSize(IntPtr proc, int min, int max); //https://pinvoke.net/default.aspx/kernel32/SetProcessWorkingSetSize.html
    }
    public class RAMLimiter
    {
        public bool isRunning = false;
        private static Process selectedProcess;
        public RAMLimiter(string processName) { selectedProcess = FindProcessByName(processName); }
        private Process FindProcessByName(string name)
        {
            Process tempProcess = Process.GetProcessesByName(name).FirstOrDefault();
            if (tempProcess.ProcessName == name)
                return tempProcess;
            else
                return new Process();
        }
        private static void LimitRam()
        {
            while(true)
            {
                if(selectedProcess.Handle != IntPtr.Zero)
                {
                    // For local app
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    // For app
                    if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                        WinAPI.SetProcessWorkingSetSize(selectedProcess.Handle, -1, -1);
                }
            }
        }
        private static Thread ramLimiterThread = new Thread(new ThreadStart(LimitRam));
        public void Start() { if (!isRunning) { ramLimiterThread.Start(); isRunning = true; } }
        public void Stop() 
        { 
            if (isRunning) 
            { 
                ramLimiterThread.Abort(); 
                isRunning = false; 
                ramLimiterThread = new Thread(new ThreadStart(LimitRam));
            } 
        }
    }
}

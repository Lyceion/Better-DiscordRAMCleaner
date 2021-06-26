using System;
using System.Linq;
using System.Windows.Forms;

namespace DiscordRamLimiter
{
    public partial class MainForm : Form
    {
        private RAMLimiter DiscordLimiter;
        public MainForm()
        {
            InitializeComponent();
            if(System.Diagnostics.Process.GetProcessesByName("Discord").FirstOrDefault().Handle == IntPtr.Zero)
            {
                MessageBox.Show("Unable to find Discord. Exiting...");
                Environment.Exit(0);
            }
            else
                DiscordLimiter = new RAMLimiter("Discord");
        }

        private void Main_Resize(object sender, EventArgs e)
        {
            if(WindowState == FormWindowState.Minimized)
            {
                Hide();
                trayBar.Visible = true;
                trayBar.ShowBalloonTip(1000);
            }
        }

        private void StartButton_Click(object sender, EventArgs e) 
        {
            if (!DiscordLimiter.isRunning)
            {
                DiscordLimiter.Start();
                startButton.Text = "Stop";
            }
            else
            {
                DiscordLimiter.Stop();
                startButton.Text = "Start";
            }
        }

        private void ExitButton_Click(object sender, EventArgs e) { DiscordLimiter.Stop(); Environment.Exit(0); }

        private void Main_FormClosing(object sender, FormClosingEventArgs e){ Environment.Exit(0); }

        private void MainForm_Load(object sender, EventArgs e) { }
        private void trayBar_DoubleClick(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            trayBar.Visible = false;
        }

        private void trayBar_BalloonTipClicked(object sender, EventArgs e) { trayBar_DoubleClick(sender, e); }
    }
}

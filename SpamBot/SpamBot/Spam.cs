using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpamBot
{
    public partial class Form1 : Form
    {
        private List<Process> notepadProcesses = new List<Process>();
        private string spamMessage = "Yes you are getting spammed\n";

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        public Form1()
        {
            InitializeComponent();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            IntPtr foregroundWindow = GetForegroundWindow();
            GetWindowThreadProcessId(foregroundWindow, out int processId);

            foreach (Process process in notepadProcesses)
            {
                if (process.Id == processId)
                {
                    IntPtr hwnd = process.MainWindowHandle;
                    SendKeys.SendWait(spamMessage);
                    SendKeys.SendWait("{Enter}");
                    Thread.Sleep(50); 
                }
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 5; i++) 
            {
                Process notepadProcess = Process.Start("notepad.exe");
                if (notepadProcess != null)
                {
                    notepadProcesses.Add(notepadProcess);
                }
            }
            timer.Start();
            lblSpam.Text = "Spamming..."; 
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            foreach (Process process in notepadProcesses)
            {
                process.CloseMainWindow(); 
            }
            notepadProcesses.Clear();
            timer.Stop();
            lblSpam.Text = "Spamming stopped.";
            MessageBox.Show("The SPAM has stopped.");
        }
    }
}

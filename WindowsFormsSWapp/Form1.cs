using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace SWapp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SldWorks swApp;        
        // directory
        string Directory = String.Empty;
        // files
        string[] files;
        // target file
        string targetFile = String.Empty;
        string calcFile = String.Empty;
        // timer
        System.Windows.Forms.Timer timer;

        private void Form1_Load(object sender, EventArgs e)
        {
            SetFormText();
            swProgressBar.Step = 1;
            SetButtonsEnableState(true, false, false);            
        }

        private void SetButtonsEnableState(bool browse, bool process, bool cancel)
        {
            swCancel.Enabled = cancel;
            swBrowse.Enabled = browse;
            swProcess.Enabled = process;
        }

        private void SetFormText()
        {
            if (string.IsNullOrEmpty(Directory))
            {
                Directory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            }

            double percent = 0;

            if (files == null || files.Length == 0)
            percent = 0;
            else
            percent = swProgressBar.Value * 1.0 / files.Length * 1.0;

            var FormTitle = string.Format("{0}% {1} - {2}", (percent * 100).ToString("F2"), Directory, "Mates Generation");
            this.Text = FormTitle;
        }

        private void swBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowseDialog = new FolderBrowserDialog();
            if (folderBrowseDialog.ShowDialog() == DialogResult.OK)
            {
                Directory = folderBrowseDialog.SelectedPath;
            }

            // set the form title

            SetFormText();
            SetButtonsEnableState(false, true, false);
        }

        private void ResetStatusStrip()
        {
            swProgressBar.Value = 0;
            if (files != null)
            {
                swProgressBar.Maximum = files.Length;
            }            
        }

        private void CompleteStatusStrip()
        {
            swProgressBar.Value = files.Length;
        }

        private Task<bool> ProcessModelAsync(CancellationToken Token, string filename, string targetFile, string calcFile)
        {
            return Task<bool>.Run(() => {
                return Helper.processModel(swApp, filename, targetFile, calcFile, Token);
                //return interference.interferenceDir(swApp, filename, Token);
                });       
        }

        // start time 
        DateTime startTime;
        Task<bool> processModelAsycTask;
        // declare cancellation source
        CancellationTokenSource CancelSource;
        private async void swProcess_Click(object sender, EventArgs e)
        {
            files = Helper.getCADFilesFromDirectory(Directory);
            if (files == null)
                return;
            if (files.Length == 0)
                return;    
            
            
            try
            {
                startTime = DateTime.Now;
                timer = new System.Windows.Forms.Timer();
                timer.Interval = 1000;
                timer.Tick += Timer_Tick;
                timer.Start();
                swFileName.Text = "Working";
                SetButtonsEnableState(false, false, true);

                swApp = await SWsingleton.getApplicationAsync();
                swApp.Visible = true;
                //swApp.Visible = true;
                // declare cancellation source
                var CancelSource = new CancellationTokenSource();
                // reset the status strip
                ResetStatusStrip();                

                
                foreach (string file in files)
                {
                    targetFile = string.Format(@"{0}.txt", file);
                    targetFile = targetFile.Replace(".sldasm","_read");
                    targetFile = targetFile.Replace(".SLDASM", "_read");
                    calcFile = targetFile.Replace("read", "calc");
                    // invoke processModel
                    processModelAsycTask = ProcessModelAsync(CancelSource.Token, file, targetFile, calcFile);
                    bool ret = await processModelAsycTask;
                    swFileName.Text = file;
                    swProgressBar.Value += 1;

                    SetFormText();
                }

                timer.Stop();
                timer.Tick -= Timer_Tick;
                SetButtonsEnableState(false, false, false);
                CompleteStatusStrip();
                swFileName.Text = "Your file is in the directory of your assembly. You can now close the App.";
                SWsingleton.Dispose();
                
            }
            catch (Exception ex)
            {
                swFileName.Text = "An error har occured." + ex.Message;
                if (swApp != null)
                {                    
                    SWsingleton.Dispose();
                    SetButtonsEnableState(true, false, false);
                }

                timer.Stop();
                timer.Tick -= Timer_Tick;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeSpan difference = DateTime.Now - startTime;
            var hours = difference.Hours.ToString("00");
            var mins = difference.Minutes.ToString("00");
            var secs = difference.Seconds.ToString("00");

            var time = string.Format("{0}:{1}:{2}", hours, mins, secs);
            swTime.Text = time;
        }

        private void swCancel_Click(object sender, EventArgs e)
        {
            if (processModelAsycTask != null)
            {
                switch (processModelAsycTask.Status)
                {
                    case TaskStatus.Created:
                        break;
                    case TaskStatus.WaitingForActivation:
                        break;
                    case TaskStatus.WaitingToRun:
                        break;
                    case TaskStatus.Running:
                        {
                            if (CancelSource != null)
                                CancelSource.Cancel();
                        }
                        break;
                    case TaskStatus.WaitingForChildrenToComplete:
                        break;
                    case TaskStatus.RanToCompletion:
                        break;
                    case TaskStatus.Canceled:
                        break;
                    case TaskStatus.Faulted:
                        break;
                    default:
                        break;
                }
            }
        }        
    }
}

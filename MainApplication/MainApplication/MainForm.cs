using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace MainApplication
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            //set this first
            backgroundWorker.WorkerReportsProgress = true;
        }

        //first lets create a function to check if number is prime
        public bool IsPrime(int number)
        {
            bool isPrime = true;

            int limit = (int)Math.Sqrt(number);

            for (int i = 2; i <= limit; i++)
            {
                if (number % i == 0)
                    return false;
            }

            return isPrime;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            //clear the textbox first
            txtResults.Clear();

            //lets run the thread
            if (!backgroundWorker.IsBusy)
                backgroundWorker.RunWorkerAsync();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //perform work

            int number = 2; //lets start on 2 since 1 is not prime
            int maxPrimeCount = 1000; //our target
            int currentPrimeCount = 0;
            int percentageOfWork = 0;

            while (true)
            {
                if (IsPrime(number))
                {
                    //if number is prime lets add our count
                    currentPrimeCount++;

                    //lets compute the amount of work done
                    float result = (float)currentPrimeCount / (float)maxPrimeCount;
                    percentageOfWork = (int)(result * 100);

                    //set the progress
                    backgroundWorker.ReportProgress(percentageOfWork);

                    //perform the execution on the thread where txtResults was created, which is probably the main/UI thread
                    txtResults.Invoke(new Action(() =>
                    {
                        if (String.IsNullOrWhiteSpace(txtResults.Text))
                            txtResults.Text = number.ToString();
                        else
                            txtResults.AppendText(Environment.NewLine + number.ToString());
                    }));
                }

                //exit the loop once we reached our target
                if (currentPrimeCount == maxPrimeCount)
                    break;

                number++;
            }
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //if task is complete show a message
            //MessageBox.Show("Done!");
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //lets update the progress bar
            customProgressBar1.Value = e.ProgressPercentage;
            customProgressBar1.PerformStep();
        }
        //It works!
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stripe;

namespace ELDERLY_HEALTH_MANAGEMENT_SYSTEM
{
    public partial class Payment : Form
    {
        Database jcov = new Database();
        private int ID;
        private int PaymentID;
        private Timer medicalReminderTimer;
        private string mail;
        private string Staffmail;
        int PreID;
        private bool emailSent = false;
        private bool emailSent1 = false;
        private DateTime lastCheckedDateTime; // Store the last checked date and time

        public Payment()
        {
            InitializeComponent();
            lastCheckedDateTime = DateTime.Now; // Initialize to current system time
            InitializeTimer();
        }

        public int PatientID
        {
            get { return ID; }
            set { ID = value; }
        }
  

        public string FirstNameText
        {
            get { return txtFirstname.Text; }
            set { txtFirstname.Text = value; }
        }

        public string StaffEmailText
        {
            get { return Staffmail; }
            set { Staffmail = value; }
        }

        public string EmailText
        {
            get { return txtEmail.Text; }
            set { txtEmail.Text = value; }
        }

        public string LastNameText
        {
            get { return txtLastname.Text; }
            set { txtLastname.Text = value; }
        }
        public string LocationText
        {
            get { return txtLoc.Text; }
            set { txtLoc.Text = value; }
        }

        public string PhoneText
        {
            get { return txtPhone.Text; }
            set { txtPhone.Text = value; }
        }

        public string GenderText
        {
            get { return txtGender.Text; }
            set { txtGender.Text = value; }
        }

        public string DoBText
        {
            get { return txtDOB.Text; }
            set { txtDOB.Text = value; }
        }


        private void Payment_Load(object sender, EventArgs e)
        {
            txtFirstname.ReadOnly = true;
            txtLastname.ReadOnly = true;
        }


        private decimal ConvertToUSD(decimal amountInMWK)
        {
            decimal exchangeRate = 0.00057m;

            // Convert the amount to USD
            decimal amountInUSD = amountInMWK * exchangeRate;

            return amountInUSD;
        }

        private void StartMedicalReminderTimer()
        {
            // Create a timer with a 30-second interval for medical reminder
            medicalReminderTimer = new Timer();
            medicalReminderTimer.Interval = 30000; // 30 seconds
            medicalReminderTimer.Tick += (sender, e) => MedicalReminderTimer_Tick();
            medicalReminderTimer.Start();
        }

        private void MedicalReminderTimer_Tick()
        {
            SendMedicalReminder();
            medicalReminderTimer.Stop();
            StartMedicalReminderTimer();
        }

        private void SendMedicalReminder()
        {
            string query = "SELECT Meds FROM Prescription WHERE PatientID = " + PatientID;
            DataSet result = jcov.select(query);

            if (result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
            {
                string prescription = result.Tables[0].Rows[0]["Meds"].ToString();

                try
                {
                    MailMessage mail = new MailMessage();
                    SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");

                    // Set sender's email address
                    mail.From = new MailAddress("atves141@gmail.com");

                    mail.To.Add(StaffEmailText);


                    // Set email subject
                    mail.Subject = "Medication Reminder";

                    // Set email body
                    mail.Body = $"Dear nursing Staff/Caretaker, please remember to provide: {prescription} to {FirstNameText} {LastNameText}";

                    // Configure SMTP server
                    smtpServer.Port = 587;
                    smtpServer.Host = "smtp.gmail.com";
                    smtpServer.EnableSsl = true;
                    smtpServer.UseDefaultCredentials = false;
                    smtpServer.Credentials = new NetworkCredential("atves141@gmail.com", "rchq vjbj loqw fstb");

                    // Send email
                    smtpServer.Send(mail);
                }
                catch (Exception ex)
                {
                    // Handle the exception
                    MessageBox.Show($"Failed to send medical reminder: {ex.Message}");
                }
                emailSent = true;


            }
            else
            {
                MessageBox.Show("No prescription found for the patient.");
            }
        }

        private void StartCheckupReminderTimer()
        {
            string query = "SELECT CheckupDates FROM Payment WHERE PrescriptionID = " + PreID;
            DataSet result = jcov.select(query);

            if (result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
            {
                DateTime checkupDate = Convert.ToDateTime(result.Tables[0].Rows[0]["CheckupDates"]).Date;
                DateTime today = DateTime.Today;

                // Calculate the difference between the checkup date and today's date
                TimeSpan timeDifference = today - checkupDate;

                // Check if the difference is exactly 2 days
                if (timeDifference.Days >= 2)
                {
                     CheckupReminderTimer_Tick();
                    
                }
                else
                {
                    // The difference is not 7 days
                    MessageBox.Show("The checkup date is not 7 days from today.");
                }
            }
        }





        private void CheckupReminderTimer_Tick()
        {
            SendCheckupReminder();
            //checkupReminderTimer.Stop();
            //StartCheckupReminderTimer();
        }

        private void SendCheckupReminder()
        {

            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");

              
                mail.From = new MailAddress("atves141@gmail.com");


                mail.To.Add(StaffEmailText);


                mail.Subject = "Check Up Reminder";

                mail.Body = $"Dear nursing Staff/Caretaker, please remember that the checkup date for {FirstNameText} {LastNameText} has arrived";

            
                smtpServer.Port = 587;
                smtpServer.Host = "smtp.gmail.com";
                smtpServer.EnableSsl = true;
                smtpServer.UseDefaultCredentials = false;
                smtpServer.Credentials = new NetworkCredential("atves141@gmail.com", "rchq vjbj loqw fstb");

        
                smtpServer.Send(mail);
            }
            catch (Exception ex)
            {
           
                MessageBox.Show($"Failed to send medical reminder: {ex.Message}");
            }
            emailSent1 = true;
        }

            private void InitializeTimer()
        {
            Timer timer = new Timer();
            timer.Interval = 30000; // Set interval to 1 minute (adjust as needed)
            timer.Tick += Timer_Tick; // Attach event handler
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DateTime currentDateTime = DateTime.Now;

            // Compare current date and time with the last checked value
            if (currentDateTime.Date != lastCheckedDateTime.Date)
            {
                // Date has changed, update application behavior
                MessageBox.Show("Date has changed!");
                // Add your code to handle date change here...
                StartCheckupReminderTimer();

                // Update the last checked date and time
                lastCheckedDateTime = currentDateTime;
            }
         
        }

        private void txtCardNO_TextChanged(object sender, EventArgs e)
        {

        }

        private async void txtSave_Click_1(object sender, EventArgs e)
        {
            // Get user input (amount in MWK)
            decimal amountMWK = decimal.Parse(txtAmount.Text);

            // Convert amount to USD
            decimal amountUSD = ConvertToUSD(amountMWK);

         
            StripeConfiguration.ApiKey = "sk_test_51P6tzVFTXe707lQiHdCL74xruB66FhiLjp9M1mqAgY1ifnVwBnXRiql84OTy9cEqIIIgYZSWpbLNb95HpLF7Xw2j00NdO5aLlm";

        
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(amountUSD * 100),
                Currency = "usd", 
                PaymentMethodTypes = new List<string> { "card" },
                PaymentMethod = "pm_card_visa", 
                Confirm = true
            };
            var service = new PaymentIntentService();
            try
            {
                PaymentIntent paymentIntent = await service.CreateAsync(options);
                
                MessageBox.Show($"Payment successful! Amount in MWK: {amountMWK}");

                string query2 = "SELECT ID FROM Prescription WHERE PatientID = " + PatientID;
                DataSet result = jcov.select(query2);

                if (result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
                {
                    string preID = result.Tables[0].Rows[0]["ID"].ToString();
                    PreID = Convert.ToInt32(preID);

                    PaymentID = jcov.IDAutoGenerate("Payment");
                    string query = "insert into Payment Values(" + PaymentID + ",'" + amountMWK + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "'," + PreID + ")";
                    jcov.Manipulate(query);

                    string query1 = "update Patient set PaymentStatus='Already Paid' where ID='" + PatientID + "'";
                    jcov.Manipulate(query1);

                    StartMedicalReminderTimer();
                }
            }
            catch (StripeException ex)
            {
                // Payment failed
                MessageBox.Show($"Payment failed: {ex.Message}");
            }
        }
    }
}

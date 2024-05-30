using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ELDERLY_HEALTH_MANAGEMENT_SYSTEM
{
    public partial class PaymentDashboard : Form
    {
                Database database = new Database();
        Patient patient = new Patient();
        Payment payment = new Payment();
        private string users = Login.user;
        private int PreID;

        int PatientID;
        string firstName;
        string lastName;
        string gender;
        string email;
        string phone;
        string location;
        string dob;

        public PaymentDashboard()
        {
            InitializeComponent();
            timer1.Start();
        }

        private void PaymentDashboard_Load(object sender, EventArgs e)
        {

            dgvPatients.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvPatients.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Filldatagrids();
            dgvPatients.Columns[0].Visible = false;
            lblUsername.Text = users;
            string query = "SELECT Email FROM Staffs WHERE Username = '" + users + "'";
            DataSet result = database.select(query);

            if (result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
            {
                string userEmail = result.Tables[0].Rows[0]["Email"].ToString();
                payment.StaffEmailText = userEmail;
            }
        }

        private void Filldatagrids()
        {
            var getPatient = new Staff();
            dgvPatients.DataSource = getPatient.ReadStatus();

        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            if (dgvPatients.SelectedRows.Count > 0)
            {

                payment.PatientID = Convert.ToInt32(dgvPatients.SelectedRows[0].Cells[0].Value);
                PatientID = Convert.ToInt32(dgvPatients.SelectedRows[0].Cells[0].Value);
                firstName = dgvPatients.SelectedRows[0].Cells[1].Value?.ToString(); // Assuming first name is in the 2nd column (index 1)
                lastName = dgvPatients.SelectedRows[0].Cells[2].Value?.ToString(); // Assuming last name is in the 3rd column (index 2)
                gender = dgvPatients.SelectedRows[0].Cells[4].Value?.ToString();
                email = dgvPatients.SelectedRows[0].Cells[5].Value?.ToString();
                phone = dgvPatients.SelectedRows[0].Cells[6].Value?.ToString();
                location = dgvPatients.SelectedRows[0].Cells[7].Value?.ToString();
                dob = dgvPatients.SelectedRows[0].Cells[3].Value?.ToString();
                payment.FirstNameText = firstName;
                payment.LastNameText = lastName;
                payment.EmailText = email;
                payment.GenderText = gender;
                payment.PhoneText= phone;
                payment.LocationText = location;
                payment.DoBText= dob;
                payment.ShowDialog(); 
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime dateTime = DateTime.Now;
            this.lblTime.Text = dateTime.ToString();
        }
    }
}

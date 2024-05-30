using Stripe;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace ELDERLY_HEALTH_MANAGEMENT_SYSTEM
{
    public partial class PatientRegistration : Form
    {
        AutoCompleteStringCollection coll = new AutoCompleteStringCollection();
        Database database = new Database();
        Patient patient = new Patient();
        private string users = Login.user;
        private int PreID;

        int PatientID;
        string firstName;
        string lastName;


        public PatientRegistration()
        {
            InitializeComponent();
            timer1.Start();
        }

        private void txtSave_Click(object sender, EventArgs e)
        {
            if (txtFirstname.Text != "" && txtLastname.Text != "" && txtLocation.Text != "" && txtEmail.Text != "" && txtPhone.Text != "")
            {
                if (rbtnMale.Checked == true)
                {
                    patient.aGender = rbtnMale.Text;
                }
                else
                {
                    patient.aGender = rbtnFemale.Text;
                }

                patient.aFirstname = txtFirstname.Text;
                patient.aLastname = txtLastname.Text;
                patient.aPhone = txtPhone.Text;
                patient.aLocation = txtLocation.Text;
                patient.aDob = dtpDateofBirth.Value;
                patient.aLocation = txtLocation.Text;
                patient.aEmail = txtEmail.Text;
                patient.AddPatient();
                txtPhone.Clear();
                txtEmail.Clear();
                rbtnMale.Checked= false;
                rbtnFemale.Checked = false;
                txtLocation.Clear();
                txtLastname.Clear();
                dtpDateofBirth.CustomFormat = "yyyy-MM-dd";
                Filldatagrids();
            }
            else
            {
                MessageBox.Show("Please Fill In All Details", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }


        private void Filldatagrids()
        {
            var getPatient = new Staff();
            dgvPatients.DataSource = getPatient.Read();

        }

        private void PatientRegistration_Load(object sender, EventArgs e)
        {
            dgvPatients.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvPatients.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Filldatagrids();
            dgvPatients.Columns[0].Visible = false;
            lblUsername.Text = users;
           /* string query = "SELECT Email FROM Staffs WHERE Username = '"+ users +"'";
            DataSet result = database.select(query);

            if (result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
            {
                string userEmail = result.Tables[0].Rows[0]["Email"].ToString();
                payment.StaffEmailText = userEmail;
            }*/
           
        }



        private void btnDisplay_Click(object sender, EventArgs e)
        {
            if (dgvPatients.SelectedRows.Count > 0)
            {

                PatientID = Convert.ToInt32(dgvPatients.SelectedRows[0].Cells[0].Value);
                firstName = dgvPatients.SelectedRows[0].Cells[1].Value?.ToString(); // Assuming first name is in the 2nd column (index 1)
                lastName = dgvPatients.SelectedRows[0].Cells[2].Value?.ToString(); // Assuming last name is in the 3rd column (index 2)
                txtFN.Text = firstName;
                txtLN.Text = lastName;
                tabControl1.SelectedTab = tabPage1;
            }
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            DateTime dateTime = DateTime.Now;
            this.lblTime.Text = dateTime.ToString();
        }

        private void btnSubmitPre_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to proceed?", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (result == DialogResult.OK)
            {
                PreID = database.IDAutoGenerate("Prescription");
                string query = "insert into Prescription Values(" + PreID + ",'" + txtDesc.Text + "','" + txtMeds.Text + "'," + PatientID + ")";
                database.Manipulate(query);
            }
        }

        private void tbnBack_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            this.Hide();
            login.Show();
        }
    }
}

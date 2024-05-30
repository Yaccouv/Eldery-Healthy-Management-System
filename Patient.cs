using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ELDERLY_HEALTH_MANAGEMENT_SYSTEM
{
    class Patient
    {
        Database jcov = new Database();

        private int PatientID;
        private string Firstname;
        private DateTime dob;
        private string Gender;
        private string Lastname;
        private string Email;
        private string Phone;
        private string Location;
        string status = "Pending";



        public int aPatientID
        {
            get { return PatientID; }
            set { PatientID = value; }
        }


        public String aFirstname
        {
            get { return Firstname; }
            set { Firstname = value; }
        }

        public String aLastname
        {
            get { return Lastname; }
            set { Lastname = value; }
        }

        public String aGender
        {
            get { return Gender; }
            set { Gender = value; }
        }

        public string aEmail
        {
            get { return Email; }
            set { Email = value; }
        }

        public DateTime aDob
        {
            get { return dob; }
            set { dob = value; }
        }

        public string aPhone
        {
            get { return Phone; }
            set { Phone = value; }
        }
        public string aLocation
        {
            get { return Location; }
            set { Location = value; }
        }

        public void AddPatient()
        {
            SqlDataAdapter adapt = new SqlDataAdapter("select count(*) from Patient where Email ='" + Email + "'", jcov.Con);
            DataTable table = new DataTable();
            adapt.Fill(table);
            if (table.Rows[0][0].ToString() == "1")
            {
                MessageBox.Show("That Email is available. Enter another Email to continue", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                PatientID = jcov.IDAutoGenerate("patient");
                string query = "insert into patient Values(" + PatientID + ",'" + Firstname + "','" + Lastname + "','" + dob + "', '" + Gender + "', '" + Email + "', '" + Phone + "', '" + Location + "', '" + status + "')";
                jcov.Manipulate(query);
                MessageBox.Show("New patient Saved Successfully", "Message", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
        }
    }
}

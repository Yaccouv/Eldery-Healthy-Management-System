using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ELDERLY_HEALTH_MANAGEMENT_SYSTEM
{
    public partial class Login : Form
    {
        Staff Log = new Staff();
        public static string user;

        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click_1(object sender, EventArgs e)
        {
            if (txtUsername.Text != "" && txtPassword.Text != "")
            {
                Log.userUsername = txtUsername.Text;
                Log.userPassword = txtPassword.Text;
                user = txtUsername.Text;

                int userType = Log.StaffLogin();
                switch (userType)
                {
                    case 0:
                        // Navigate to PatientRegistration form
                        this.Hide();
                        PatientRegistration pr = new PatientRegistration();
                        pr.Show();
                        break;
                    case 1:
                        // Navigate to PaymentDashboard form
                        this.Hide();
                        PaymentDashboard pd = new PaymentDashboard();
                        pd.Show();
                        break;
                    case 2:
                        // Navigate to AdminDashboard form
                        this.Hide();
                        AdminDashboard ad = new AdminDashboard();
                        ad.Show();
                        break;
                    default:
                        MessageBox.Show("Invalid username or password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                }

                txtUsername.Clear();
                txtPassword.Clear();
            }
            else
            {
                MessageBox.Show("Please fill in all the details correctly", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }



        private void txtPassword_TextChanged_1(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = '*';
        }

        private void chbPassword_CheckedChanged(object sender, EventArgs e)
        {
            this.txtPassword.PasswordChar = this.chbPassword.Checked ? char.MinValue : '*';
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}


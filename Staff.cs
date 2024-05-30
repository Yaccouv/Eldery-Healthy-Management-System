using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ELDERLY_HEALTH_MANAGEMENT_SYSTEM
{
    class Staff
    {
        Database cov = new Database();

        private int ID;
        private string Username;
        private string Password;


        public int StaffID
        {
            get { return ID; }
            set { ID = value; }
        }

   
        public String userUsername
        {
            get { return Username; }
            set { Username = value; }
        }

        public String userPassword
        {
            get { return Password; }
            set { Password = value; }
        }



        public int StaffLogin()
        {
            string query = "SELECT Type FROM Staffs WHERE Username = '" + userUsername + "' AND Password = '" + userPassword + "'";
            DataSet ds = cov.select(query);

            if (ds.Tables[0].Rows.Count > 0)
            {
                return Convert.ToInt32(ds.Tables[0].Rows[0]["Type"]);
            }
            else
            {
                return -1; // Return -1 if no user found
            }
        }



        public DataTable Read()
        {
            var dt = new DataTable();
            using (var cn = cov.Con)
            {
                using (var cmd = new SqlCommand() { Connection = cn })
                {
                    cmd.CommandText = " select * from Patient";
                    cn.Open();
                    dt.Load(cmd.ExecuteReader());
                }

            }
            return dt;
        }

        public DataTable ReadStatus()
        {
            var dt = new DataTable();
            using (var cn = cov.Con)
            {
                using (var cmd = new SqlCommand() { Connection = cn })
                {
                    cmd.CommandText = "SELECT * FROM Patient WHERE PaymentStatus = 'pending'";
                    cn.Open();
                    dt.Load(cmd.ExecuteReader());
                }
            }
            return dt;
        }

    }

}


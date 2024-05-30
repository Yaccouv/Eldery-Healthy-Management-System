using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace ELDERLY_HEALTH_MANAGEMENT_SYSTEM
{
    class Database
    {
        private SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\T590\source\repos\ELDERLY_HEALTH_MANAGEMENT_SYSTEM\ELDERLY_HEALTH_MANAGEMENT.mdf;Integrated Security=True;");

        public SqlConnection Con
        {
            get
            {
                return con;
            }
        }

        public void openConnection()
        {
            con.Open();
        }

        public void closeConnection()
        {
            con.Close();
        }

        public void Manipulate(string query)
        {
            openConnection();
            SqlCommand cmd = new SqlCommand(query, con);
            if (cmd.ExecuteNonQuery() == 1)
            {
                closeConnection();
            }
            else
            {
                closeConnection();
            }
        }

        public DataSet select(string query)
        {
            openConnection();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            closeConnection();
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        public int IDAutoGenerate(string TableName)
        {
            int ID = 0;
            try
            {
                int x = 0;
                int y = 0;

                string Query = "SELECT count(*) FROM " + TableName;
                con.Open();
                SqlCommand cmd = new SqlCommand(Query, con);
                x = Convert.ToInt32(cmd.ExecuteScalar());

                Query = "SELECT ID FROM " + TableName + " ORDER BY ID ASC";
                SqlDataAdapter sda = new SqlDataAdapter(Query, con);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                if (dt.Rows.Count == 0 && x == 0)
                {
                    ID = 1; // If table is empty, start with 1
                }
                else if (dt.Rows.Count > 0 && x > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        y = Convert.ToInt32(row["ID"]);
                    }

                    if (x > y)
                    {
                        x = x + 1;
                        ID = x;
                    }
                    else if (y > x)
                    {
                        y = y + 1;
                        ID = y;
                    }
                    else if (x == y)
                    {
                        ID = y + 1;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                con.Close();
            }

            return ID;
        }


    }
}

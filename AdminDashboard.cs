using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ELDERLY_HEALTH_MANAGEMENT_SYSTEM
{
    public partial class AdminDashboard : Form
    {
        private string users = Login.user;
        public AdminDashboard()
        {
            InitializeComponent();
            timer1.Start();
        }

        private void AdminDashboard_Load(object sender, EventArgs e)
        {
            dgvPayment.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvPayment.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Filldatagrids();
            // Assuming you have a TabControl named "tabControl1"
            // Assuming you have a TabControl named "tabControl1"
            tabControl1.Size = Screen.PrimaryScreen.Bounds.Size;


            tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;
        }

        private void Filldatagrids()
        {
            var getPatient = new PaymentClass();
            dgvPayment.DataSource = getPatient.ReadPayments();
            lblUsername.Text = users;
        }


        private void PopulateChart()
        {
            string query = @"SELECT 
    FORMAT(pm.CheckupDates, 'MMM') AS Month,
    pr.Meds AS Medication,
    pr.[Desc] AS Disease,
    SUM(pm.Amount) AS TotalPayment
FROM Payment pm
INNER JOIN Prescription pr ON pm.PrescriptionID = pr.ID
GROUP BY FORMAT(pm.CheckupDates, 'MMM'), pr.Meds, pr.[Desc]
ORDER BY Month, Medication
";

            DataTable dt = GetData(query);

            var months = (from p in dt.AsEnumerable()
                          orderby p.Field<string>("Month") ascending
                          select p.Field<string>("Month")).Distinct().ToArray();

            string[] medications = (from p in dt.AsEnumerable()
                                    orderby p.Field<string>("Medication") ascending
                                    select p.Field<string>("Medication")).Distinct().ToArray();

            chart1.Series.Clear();
            foreach (var medication in medications)
            {
                chart1.Series.Add(new Series(medication));
                chart1.Series[medication].ChartType = SeriesChartType.Bar;
                chart1.Series[medication].IsValueShownAsLabel = true;
            }

            foreach (var month in months)
            {
                foreach (var medication in medications)
                {
                    var payment = dt.AsEnumerable()
                .Where(row => row.Field<string>("Month") == month && row.Field<string>("Medication") == medication)
                .Select(row =>
                {
                    var totalPayment = row.Field<object>("TotalPayment");
                    if (totalPayment != null && totalPayment != DBNull.Value)
                    {
                        if (totalPayment is float || totalPayment is double || totalPayment is decimal)
                        {
                            return Convert.ToSingle(totalPayment);
                        }
                        else
                        {
                            // Log the problematic row
                            Console.WriteLine($"Invalid data type for TotalPayment: {totalPayment.GetType()}");

                            // Return a default value
                            return 0.0f;
                        }
                    }
                    else
                    {
                        // Handle null values
                        return 0.0f;
                    }
                })
                .FirstOrDefault();


                    if (payment != null)
                    {
                        chart1.Series[medication].Points.AddXY(month, payment);
                        // Include the disease information in the data point label
                        chart1.Series[medication].Points.Last().Label = $"{medication} ({GetDisease(month, medication, dt)})";
                    }
                }
            }

            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.ChartAreas[0].AxisX.LabelStyle.Font = new System.Drawing.Font("Arial", 10);
            chart1.ChartAreas[0].AxisY.LabelStyle.Font = new System.Drawing.Font("Arial", 10);
            chart1.ChartAreas[0].AxisY.Title = "Payments (MWK)";
            chart1.ChartAreas[0].AxisX.Title = "Month";
            chart1.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Arial", 12);
            chart1.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Arial", 12);
        }

        private string GetDisease(string month, string medication, DataTable dt)
        {
            var row = dt.AsEnumerable()
                        .FirstOrDefault(r => r.Field<string>("Month") == month && r.Field<string>("Medication") == medication);
            return row != null ? row.Field<string>("Disease") : "";
        }

        private static DataTable GetData(string query)
        {
            string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\T590\source\repos\ELDERLY_HEALTH_MANAGEMENT_SYSTEM\ELDERLY_HEALTH_MANAGEMENT.mdf;Integrated Security=True;";
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter(query, con))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Check if the selected tab is the one containing the chart
            if (tabControl1.SelectedTab == tabPage3)
            {
                PopulateChart();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime dateTime = DateTime.Now;
            this.lblTime.Text = dateTime.ToString();
        }
    }
}

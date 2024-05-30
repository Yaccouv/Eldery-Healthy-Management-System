using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELDERLY_HEALTH_MANAGEMENT_SYSTEM
{
    class PaymentClass
    {
         Database cov = new Database();

        public DataTable ReadPayments()
        {
            var dt = new DataTable();
            using (var cn = cov.Con)
            {
                using (var cmd = new SqlCommand() { Connection = cn })
                {
                    cmd.CommandText = " SELECT CONCAT(pa.Firstname, ' ', pa.Lastname) AS Name,\r\n                       pr.Meds AS Medication,\r\n                       pr.[Desc] AS Disease,\r\n                       FORMAT(pm.CheckupDates, 'MMM') AS Month,\r\n                       SUM(pm.Amount) AS TotalAmount\r\n                FROM Payment pm\r\n                INNER JOIN Prescription pr ON pm.PrescriptionID = pr.ID\r\n                INNER JOIN Patient pa ON pa.ID = pr.PatientID\r\n                GROUP BY CONCAT(pa.Firstname, ' ', pa.Lastname), FORMAT(pm.CheckupDates, 'MMM'), pr.Meds, pr.[Desc]\r\n                ORDER BY Name, Month, Medication";
                    cn.Open();
                    dt.Load(cmd.ExecuteReader());
                }

            }
            return dt;
        }

    }
}

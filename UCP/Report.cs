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

namespace UCP
{
    public partial class Report : Form
    {
        static string connectionString = "Data Source=10.69.9.211,1433\\DEPA15;Initial Catalog=DB_HasilPanen;User ID=sa;Password=123;Integrated Security=False;TrustServerCertificate=True;";
        SqlConnection conn = new SqlConnection(connectionString);
        SqlDataAdapter da;
        DataTable dtHasilPanen;

        CrystalReport1 listHasilPanen = new CrystalReport1();
        string namaPetaniTarget { get; set; }
        string tahunPanen { get; set; }
        public Report(string namaPetani, string tahun)
        {
            InitializeComponent();
            namaPetaniTarget = namaPetani;
            tahunPanen = tahun;

            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                SqlCommand cmd = new SqlCommand("sp_ReportPanen", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@inNamaPetani", namaPetaniTarget);
                cmd.Parameters.AddWithValue("@inTahunPanen", tahunPanen);

                da = new SqlDataAdapter(cmd);
                dtHasilPanen = new DataTable();
                da.Fill(dtHasilPanen);
                conn.Close();

                listHasilPanen.SetDataSource(dtHasilPanen);
                crystalReportViewer1.ReportSource = listHasilPanen;
                crystalReportViewer1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal load data: " + ex.Message);
            }
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}

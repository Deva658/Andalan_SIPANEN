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
    public partial class FormRekapPanen : Form
    {
        static string connectionString = "Data Source=DEVA\\DEPA15;Initial Catalog=DB_HasilPanen;Integrated Security=True";
        SqlConnection conn = new SqlConnection(connectionString);
        SqlDataAdapter da;
        DataTable dtHasilPanen;
        DataTable dtPetani;
        private string namaPetaniAktif = "";
        private string hakAkses = "";
        public FormRekapPanen(string idPetani)
        {
            InitializeComponent();
            hakAkses = "Petani";
            DapatkanNamaPetani(Convert.ToInt32(idPetani));
        }

        public FormRekapPanen()
        {
            InitializeComponent();
            hakAkses = "Admin";
        }

        private void DapatkanNamaPetani(int id)
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT nama_petani FROM Petani WHERE id_petani = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                namaPetaniAktif = cmd.ExecuteScalar()?.ToString();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal mengidentifikasi profil petani: " + ex.Message);
            }
        }

        private void FormRekapPanen_Load(object sender, EventArgs e)
        {
            btnCetak.Enabled = false;
            cmbNama.DropDownStyle = ComboBoxStyle.DropDownList;

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT nama_petani FROM Petani", conn);
                dtPetani = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dtPetani);
                conn.Close();

                if (hakAkses == "Petani")
                {
                    cmbNama.DataSource = dtPetani;
                    cmbNama.DisplayMember = "nama_petani";
                    cmbNama.ValueMember = "nama_petani";
                    cmbNama.SelectedValue = namaPetaniAktif;
                    cmbNama.Enabled = false;
                }
                else if (hakAkses == "Admin")
                {
                    DataRow row = dtPetani.NewRow();
                    row["nama_petani"] = "Semua";
                    dtPetani.Rows.InsertAt(row, 0);

                    cmbNama.DataSource = dtPetani;
                    cmbNama.DisplayMember = "nama_petani";
                    cmbNama.ValueMember = "nama_petani";
                    cmbNama.SelectedIndex = 0;
                    cmbNama.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat filter nama: " + ex.Message);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                SqlCommand cmd = new SqlCommand("sp_ReportPanen", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@inNamaPetani", cmbNama.SelectedValue.ToString());
                cmd.Parameters.Add("@inTahunPanen", SqlDbType.Char, 4).Value = DateTime.Now.Year.ToString();

                da = new SqlDataAdapter(cmd);
                dtHasilPanen = new DataTable();
                da.Fill(dtHasilPanen);
                dataGridView1.DataSource = dtHasilPanen;

                if (dtHasilPanen.Rows.Count > 0)
                {
                    btnCetak.Enabled = true;
                }
                else
                {
                    btnCetak.Enabled = false;
                    MessageBox.Show("Data rekap tidak ditemukan.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal Load data rekap: " + ex.Message);
            }
        }

        private void btnCetak_Click(object sender, EventArgs e)
        {
            Report frm2 = new Report(cmbNama.SelectedValue.ToString(), DateTime.Now.Year.ToString());
            frm2.Show();
            this.Hide();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cmbNama_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

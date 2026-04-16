using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace UCP
{
    public partial class FormAdmin : Form
    {
        private readonly SqlConnection conn;
        private readonly string connectionString = "Data Source=DEVA\\DEPA15;Initial Catalog=DB_HasilPanen;Integrated Security=True";

        public FormAdmin()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);

            this.Load += FormAdmin_Load;
            btnLoad.Click += btnLoad_Click;
            btnDelete.Click += btnDelete_Click;
            btnLogout.Click += btnLogout_Click;
            txtCari.TextChanged += txtCari_TextChanged;
        }

        private void FormAdmin_Load(object sender, EventArgs e)
        {
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("ID", "ID Panen");
            dataGridView1.Columns.Add("Petani", "Nama Petani");
            dataGridView1.Columns.Add("NoTelp", "No. Telepon");
            dataGridView1.Columns.Add("Tanaman", "Nama Tanaman");
            dataGridView1.Columns.Add("Tanggal", "Tanggal Panen");
            dataGridView1.Columns.Add("Jumlah", "Jumlah Hasil");
            dataGridView1.Columns.Add("Satuan", "Satuan");
            dataGridView1.Columns.Add("Kualitas", "Kualitas");
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed) conn.Open();

                dataGridView1.Rows.Clear();

                string queryAdmin = @"SELECT h.id_panen, p.nama_petani, p.no_telp, t.nama_tanaman, h.tanggal_panen, h.jumlah_hasil, t.satuan_hasil, h.kualitas 
                              FROM Hasil_Panen h
                              JOIN Petani p ON h.id_petani = p.id_petani
                              JOIN Tanaman t ON h.id_tanaman = t.id_tanaman";

                SqlCommand cmd = new SqlCommand(queryAdmin, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    dataGridView1.Rows.Add(
                        reader["id_panen"].ToString(),
                        reader["nama_petani"].ToString(),
                        reader["no_telp"].ToString(),
                        reader["nama_tanaman"].ToString(),
                        Convert.ToDateTime(reader["tanggal_panen"]).ToShortDateString(),
                        reader["jumlah_hasil"].ToString(),
                        reader["satuan_hasil"].ToString(),
                        reader["kualitas"].ToString()
                    );
                }
                reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal Menampilkan Data: " + ex.Message);
            }
        }

        
    }
}
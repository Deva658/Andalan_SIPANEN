using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace UCP
{
    public partial class FormPetani : Form
    {
        private readonly SqlConnection conn;
        private readonly string connectionString = "Data Source=DEVA\\DEPA15;Initial Catalog=DB_HasilPanen;Integrated Security=True";

        private string idPetaniLogin;

        public FormPetani(string idPetani, string namaPetani)
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);

            idPetaniLogin = idPetani;

            txtNamaPetani.Text = namaPetani;

            this.Load += FormPetani_Load;
            btnLoad.Click += btnLoad_Click;
            btnInsert.Click += btnInsert_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnLogout.Click += btnLogout_Click;
        }

        private void FormPetani_Load(object sender, EventArgs e)
        {
            cmbKualitas.Items.Clear();
            cmbKualitas.Items.Add("Grade A (Sangat Bagus)");
            cmbKualitas.Items.Add("Grade B (Bagus)");
            cmbKualitas.Items.Add("Grade C (Standar)");

            LoadDataKeComboBox("SELECT id_tanaman, nama_tanaman FROM Tanaman", cmbTanaman, "nama_tanaman", "id_tanaman");

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.CellClick += dataGridView1_CellClick;
        }

        private void LoadDataKeComboBox(string query, ComboBox cmb, string display, string value)
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cmb.DataSource = dt;
                cmb.DisplayMember = display;
                cmb.ValueMember = value;
                cmb.SelectedIndex = -1;
                conn.Close();
            }
            catch (Exception ex) { MessageBox.Show("Gagal load combobox: " + ex.Message); }
        }

        private void ClearForm()
        {
            txtID.Clear();
            cmbTanaman.SelectedIndex = -1;
            txtJumlah.Clear();
            cmbKualitas.SelectedIndex = -1;
            dtpTanggal.Value = DateTime.Now;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();
                dataGridView1.Columns.Add("ID", "ID Panen");
                dataGridView1.Columns.Add("Petani", "Nama Petani");
                dataGridView1.Columns.Add("NoTelp", "No. Telepon");
                dataGridView1.Columns.Add("Tanaman", "Nama Tanaman");
                dataGridView1.Columns.Add("Tanggal", "Tanggal Panen");
                dataGridView1.Columns.Add("Jumlah", "Jumlah Hasil");
                dataGridView1.Columns.Add("Satuan", "Satuan");
                dataGridView1.Columns.Add("Kualitas", "Kualitas");

                string query = @"SELECT h.id_panen, p.nama_petani, p.no_telp, t.nama_tanaman, h.tanggal_panen, h.jumlah_hasil, t.satuan_hasil, h.kualitas 
                         FROM Hasil_Panen h
                         JOIN Petani p ON h.id_petani = p.id_petani
                         JOIN Tanaman t ON h.id_tanaman = t.id_tanaman
                         WHERE h.id_petani = @IDLogin";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IDLogin", idPetaniLogin);

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
            catch (Exception ex) { MessageBox.Show("Gagal Menampilkan Data: " + ex.Message); }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbTanaman.SelectedValue == null || txtJumlah.Text == "")
                {
                    MessageBox.Show("Tanaman dan Jumlah Hasil harus diisi!");
                    return;
                }
                if (conn.State == ConnectionState.Closed) conn.Open();

                string query = @"INSERT INTO Hasil_Panen (id_petani, id_tanaman, tanggal_panen, jumlah_hasil, kualitas) 
                                 VALUES (@IdPetani, @IdTanaman, @Tanggal, @Jumlah, @Kualitas)";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@IdPetani", idPetaniLogin);
                cmd.Parameters.AddWithValue("@IdTanaman", cmbTanaman.SelectedValue);
                cmd.Parameters.AddWithValue("@Tanggal", dtpTanggal.Value);
                cmd.Parameters.AddWithValue("@Jumlah", float.Parse(txtJumlah.Text));
                cmd.Parameters.AddWithValue("@Kualitas", cmbKualitas.Text);

                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    MessageBox.Show("Data Panen berhasil dicatat!");
                    ClearForm();
                    btnLoad.PerformClick();
                }
                conn.Close();
            }
            catch (Exception ex) { MessageBox.Show("Pastikan Jumlah Hasil diisi dengan angka! Error: " + ex.Message); }
        }

        
    }
}
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace UCP
{
    public partial class FormPetani : Form
    {
        private BindingSource bindingSource1 = new BindingSource();
        private DataTable dtPanen = new DataTable();
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
            ClearForm();
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
            dtpTanggal.MinDate = DateTime.Now.Date.AddDays(-6);
            dtpTanggal.MaxDate = DateTime.Now.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            dtpTanggal.Value = DateTime.Now;

            txtSatuan.Clear();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                string query = "SELECT * FROM vw_RiwayatPanen WHERE id_petani = @IDLogin";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IDLogin", idPetaniLogin);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                dtPanen.Clear();
                da.Fill(dtPanen);

                bindingSource1.DataSource = dtPanen;
                dataGridView1.DataSource = bindingSource1;
                bindingNavigator1.BindingSource = bindingSource1;

                conn.Close();
            }
            catch (Exception ex) { MessageBox.Show("Gagal Menampilkan Data: " + ex.Message); }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbTanaman.SelectedValue == null || txtJumlah.Text == "" || cmbKualitas.Text == "")
                {
                    MessageBox.Show("Tanaman, Kualitas, dan Jumlah Hasil harus diisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!float.TryParse(txtJumlah.Text, out float jumlahPanen) || jumlahPanen <= 0)
                {
                    MessageBox.Show("Jumlah panen harus berupa angka dan tidak boleh 0 atau negatif!", "Peringatan Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtJumlah.Focus();
                    return;
                }

                DateTime tanggalPilih = dtpTanggal.Value.Date;
                DateTime hariIni = DateTime.Now.Date;
                DateTime batasMin = hariIni.AddDays(-7);

                if (tanggalPilih > hariIni || tanggalPilih < batasMin)
                {
                    MessageBox.Show($"Tanggal panen tidak valid!\nHarus antara {batasMin.ToShortDateString()} sampai {hariIni.ToShortDateString()}.", "Peringatan Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (conn.State == ConnectionState.Closed) conn.Open();

                string query = @"INSERT INTO Hasil_Panen (id_petani, id_tanaman, tanggal_panen, jumlah_hasil, kualitas) 
                                 VALUES (@IdPetani, @IdTanaman, @Tanggal, @Jumlah, @Kualitas)";
                SqlCommand cmd = new SqlCommand("sp_InsertHasilPanen", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdPetani", idPetaniLogin);
                cmd.Parameters.AddWithValue("@IdTanaman", cmbTanaman.SelectedValue);
                cmd.Parameters.AddWithValue("@TanggalPanen", dtpTanggal.Value);
                cmd.Parameters.AddWithValue("@JumlahHasil", jumlahPanen);
                cmd.Parameters.AddWithValue("@Kualitas", cmbKualitas.Text);

                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    MessageBox.Show("Data Panen berhasil dicatat!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearForm();
                    btnLoad.PerformClick();
                }
                conn.Close();
            }
            catch (Exception ex) { MessageBox.Show("Pastikan Jumlah Hasil diisi dengan angka! Error: " + ex.Message); }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtID.Text == "")
                {
                    MessageBox.Show("Pilih data di tabel dulu yang ingin diubah!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!float.TryParse(txtJumlah.Text, out float jumlahPanen) || jumlahPanen <= 0)
                {
                    MessageBox.Show("Jumlah panen harus berupa angka dan tidak boleh 0 atau negatif!", "Peringatan Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtJumlah.Focus();
                    return;
                }

                DateTime tanggalPilih = dtpTanggal.Value.Date;
                DateTime hariIni = DateTime.Now.Date;
                DateTime batasMin = hariIni.AddDays(-7);

                if (tanggalPilih > hariIni || tanggalPilih < batasMin)
                {
                    MessageBox.Show($"Tanggal panen tidak valid!\nHarus antara {batasMin.ToShortDateString()} sampai {hariIni.ToShortDateString()}.", "Peringatan Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult dialog = MessageBox.Show("Apakah Anda yakin ingin menyimpan perubahan pada data panen ini?", "Konfirmasi Ubah Data", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialog == DialogResult.Yes)
                {
                    if (conn.State == System.Data.ConnectionState.Closed) conn.Open();

                    SqlCommand cmd = new SqlCommand("sp_UpdateHasilPanen", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdPanen", txtID.Text);
                    cmd.Parameters.AddWithValue("@IdPetani", idPetaniLogin);
                    cmd.Parameters.AddWithValue("@IdTanaman", cmbTanaman.SelectedValue);
                    cmd.Parameters.AddWithValue("@TanggalPanen", dtpTanggal.Value);
                    cmd.Parameters.AddWithValue("@JumlahHasil", jumlahPanen);
                    cmd.Parameters.AddWithValue("@Kualitas", cmbKualitas.Text);

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Data Panen berhasil diupdate!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearForm();
                        btnLoad.PerformClick();
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi Kesalahan: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                txtID.Text = row.Cells["id_panen"].Value.ToString();
                cmbTanaman.Text = row.Cells["nama_tanaman"].Value.ToString();

                DateTime tanggalDiTabel = Convert.ToDateTime(row.Cells["tanggal_panen"].Value);
                DateTime batasBawahH7 = DateTime.Now.Date.AddDays(-6);
                DateTime batasAtasHariIni = DateTime.Now.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

                dtpTanggal.MinDate = new DateTime(1900, 1, 1);
                dtpTanggal.MaxDate = new DateTime(2100, 12, 31);
                dtpTanggal.Value = tanggalDiTabel;

                if (tanggalDiTabel < batasBawahH7)
                    dtpTanggal.MinDate = tanggalDiTabel;
                else
                    dtpTanggal.MinDate = batasBawahH7;

                if (tanggalDiTabel > batasAtasHariIni)
                    dtpTanggal.MaxDate = tanggalDiTabel;
                else
                    dtpTanggal.MaxDate = batasAtasHariIni;

                txtJumlah.Text = row.Cells["jumlah_hasil"].Value.ToString();
                cmbKualitas.Text = row.Cells["kualitas"].Value.ToString();
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Apakah Anda yakin ingin Log Out?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialog == DialogResult.Yes)
            {
                this.Hide();
                Form1 loginForm = new Form1();
                loginForm.Show();
            }
        }

        private void cmbTanaman_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTanaman.SelectedIndex == -1 || cmbTanaman.SelectedValue == null) return;

            try
            {
                if (conn.State == System.Data.ConnectionState.Closed) conn.Open();

                string query = "SELECT satuan_hasil FROM Tanaman WHERE id_tanaman = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", cmbTanaman.SelectedValue.ToString());

                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    txtSatuan.Text = result.ToString();
                }
                conn.Close();
            }
            catch (Exception)
            {

            }
        }

        private void btnTestInjection_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                string query = "UPDATE Hasil_Panen SET kualitas='HACKED!!!', jumlah_hasil=0 WHERE jumlah_hasil='" + txtJumlah.Text + "'";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    int result = cmd.ExecuteNonQuery();
                    MessageBox.Show(result + " baris data panen telah berhasil di-hack!", "System Compromised", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                btnLoad.PerformClick();

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Injeksi: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string query = @"
                    IF OBJECT_ID('dbo.Hasil_Panen_Backup') IS NOT NULL
                    BEGIN
                        DELETE FROM dbo.Hasil_Panen;
                        SET IDENTITY_INSERT dbo.Hasil_Panen ON;
                        INSERT INTO dbo.Hasil_Panen (id_panen, id_petani, id_tanaman, tanggal_panen, jumlah_hasil, kualitas)
                        SELECT id_panen, id_petani, id_tanaman, tanggal_panen, jumlah_hasil, kualitas 
                        FROM dbo.Hasil_Panen_Backup;
                        SET IDENTITY_INSERT dbo.Hasil_Panen OFF;
                    END";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Data Hasil Panen berhasil direset ke kondisi awal!", "Recovery Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnLoad.PerformClick();

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Reset gagal: Pastikan tabel Backup sudah dibuat di SQL Server. Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRekap_Click(object sender, EventArgs e)
        {
            FormRekapPanen frmRekap = new FormRekapPanen(idPetaniLogin);
            frmRekap.Show();
            this.Hide();
        }

        private void btnGrafik_Click(object sender, EventArgs e)
        {
            string namaPetani = "";
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT nama_petani FROM Petani WHERE id_petani = @id", conn);
                cmd.Parameters.AddWithValue("@id", idPetaniLogin);
                object result = cmd.ExecuteScalar();
                if (result != null) namaPetani = result.ToString();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal mengambil nama profil: " + ex.Message);
            }

            Dashboard petaniChart = new Dashboard(namaPetani);
            petaniChart.Show();
        }
    }
}

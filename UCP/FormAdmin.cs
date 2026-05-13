using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace UCP
{
    public partial class FormAdmin : Form
    {
        private readonly SqlConnection conn;
        private readonly string connectionString = "Data Source=DEVA\\DEPA15;Initial Catalog=DB_HasilPanen;Integrated Security=True";

        private BindingSource bindingSource1 = new BindingSource();
        private DataTable dtPanen = new DataTable();
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
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed) conn.Open();

                string queryAdmin = "SELECT * FROM vw_RiwayatPanen";
                SqlCommand cmd = new SqlCommand(queryAdmin, conn);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                dtPanen.Clear(); 
                da.Fill(dtPanen);

                bindingSource1.DataSource = dtPanen;
                dataGridView1.DataSource = bindingSource1;
                bindingNavigator1.BindingSource = bindingSource1;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal Menampilkan Data: " + ex.Message);
            }
        }

        private void txtCari_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed) conn.Open();

                SqlCommand cmd = new SqlCommand("sp_SearchPanen", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Keyword", txtCari.Text);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dtSearch = new DataTable();
                da.Fill(dtSearch);

                bindingSource1.DataSource = dtSearch;
                dataGridView1.DataSource = bindingSource1;

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal Mencari Data: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow == null || dataGridView1.CurrentRow.Index == -1)
                {
                    MessageBox.Show("Pilih baris data di tabel terlebih dahulu!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string idPanenYangDipilih = dataGridView1.CurrentRow.Cells["id_panen"].Value.ToString();
                DialogResult dialog = MessageBox.Show("Yakin ingin menghapus data panen ini?", "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialog == DialogResult.Yes)
                {
                    if (conn.State == System.Data.ConnectionState.Closed) conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_DeleteHasilPanen", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdPanen", idPanenYangDipilih);

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Data panen berhasil dihapus secara permanen!");
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
    }
}
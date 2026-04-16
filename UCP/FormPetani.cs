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

        
    }
}
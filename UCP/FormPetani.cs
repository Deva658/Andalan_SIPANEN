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

        
    }
}
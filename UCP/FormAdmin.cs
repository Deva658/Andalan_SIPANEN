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

        
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace UCP
{

    public partial class Form1 : Form
    {
        private readonly SqlConnection conn;
        private readonly string connectionString = "Data Source = DEVA\\DEPA15; Initial Catalog = DB_HasilPanen; Integrated Security=True";
        public Form1()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();

                if (txtUsername.Text == "" || txtPassword.Text == "")
                {
                    MessageBox.Show("Username atau Password tidak boleh kosong!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    conn.Close();
                    return;
                }

                string query = "SELECT 'Admin' AS Role, id_admin AS UserID, nama_lengkap AS Nama FROM Admin WHERE username=@u AND password=@p " +
                               "UNION " +
                               "SELECT 'Petani' AS Role, id_petani AS UserID, nama_petani AS Nama FROM Petani WHERE username=@u AND password=@p";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@u", txtUsername.Text);
                cmd.Parameters.AddWithValue("@p", txtPassword.Text);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string role = reader["Role"].ToString();
                    string idUser = reader["UserID"].ToString();
                    string namaUser = reader["Nama"].ToString();

                    MessageBox.Show("Login Berhasil sebagai " + role, "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Hide();

                    if (role == "Admin")
                    {
                        FormAdmin adminForm = new FormAdmin();
                        adminForm.Show();
                    }
                    else
                    {
                        FormPetani petaniForm = new FormPetani(idUser, namaUser);
                        petaniForm.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Username atau Password salah!", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}

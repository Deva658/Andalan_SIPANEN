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

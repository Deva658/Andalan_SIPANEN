using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCP
{
    internal class DAL
    {
        private static string connectionString = "Data Source=DEVA\\DEPA15;Initial Catalog=DB_HasilPanen;Integrated Security=True";
        private SqlConnection conn = new SqlConnection(connectionString);
        private SqlDataAdapter da;
        private DataTable dtResult;

        public string GetConnectionString()
        {
            return connectionString;
        }

        public DataTable GetNamaPetani()
        {
            if (conn.State == ConnectionState.Closed) conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT nama_petani FROM Petani", conn);
            dtResult = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dtResult);
            conn.Close();
            return dtResult;
        }

        public DataTable GetDataRekap(string namaPetani, string tahun)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();
            SqlCommand cmd = new SqlCommand("sp_ReportPanen", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@inNamaPetani", namaPetani);
            cmd.Parameters.AddWithValue("@inTahunPanen", tahun);

            da = new SqlDataAdapter(cmd);
            dtResult = new DataTable();
            da.Fill(dtResult);
            conn.Close();
            return dtResult;
        }

        public int DeleteHasilPanen(string idPanen)
        {
            int result = 0;
            if (conn.State == ConnectionState.Closed) conn.Open();
            SqlCommand cmd = new SqlCommand("sp_DeleteHasilPanen", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@IdPanen", idPanen);

            result = cmd.ExecuteNonQuery();
            conn.Close();
            return result;
        }
        public void InsertHasilPanenMassal(string idPetani, string idTanaman, DateTime tanggal, string kualitas, double jumlah)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();

            SqlCommand command = new SqlCommand("sp_InsertHasilPanen", conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@IdPetani", idPetani);
            command.Parameters.AddWithValue("@IdTanaman", idTanaman);
            command.Parameters.AddWithValue("@TanggalPanen", tanggal);
            command.Parameters.AddWithValue("@Kualitas", kualitas);
            command.Parameters.AddWithValue("@JumlahHasil", jumlah);

            command.ExecuteNonQuery();
            conn.Close();
        }
    }
}

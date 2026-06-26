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
        private static string connectionString = "Data Source=10.69.9.211,1433\\DEPA15;Initial Catalog=DB_HasilPanen;User ID=sa;Password=123;Integrated Security=False;TrustServerCertificate=True;";

        public string GetConnectionString()
        {
            return connectionString;
        }

        public DataTable GetNamaPetani()
        {
            DataTable dtResult = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT nama_petani FROM Petani", conn);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResult);
                    }
                }
                catch (Exception)
                {
                    return dtResult;
                }
            }
            return dtResult;
        }

        public DataTable GetDataRekap(string namaPetani, string tahun)
        {
            DataTable dtResult = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_ReportPanen", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@inNamaPetani", namaPetani);
                    cmd.Parameters.AddWithValue("@inTahunPanen", tahun);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResult);
                    }
                }
                catch (Exception)
                {
                    return dtResult;
                }
            }
            return dtResult;
        }

        public int DeleteHasilPanen(string idPanen)
        {
            int result = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlCommand cmd = new SqlCommand("sp_DeleteHasilPanen", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdPanen", idPanen);

                result = cmd.ExecuteNonQuery();
            }
            return result;
        }

        public void InsertHasilPanenMassal(string idPetani, string idTanaman, DateTime tanggal, string kualitas, double jumlah)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
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
            }
        }
    }
}
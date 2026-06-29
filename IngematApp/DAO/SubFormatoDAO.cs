using IngematApp.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace IngematApp.DAO
{
    public class SubFormatoDAO
    {
        private readonly string _cadenaConexion;
        public SubFormatoDAO(IConfiguration config) { _cadenaConexion = config.GetConnectionString("CadenaConexion") ?? throw new InvalidOperationException(); }

        public List<SubFormato> ListarSubFormatos()
        {
            var lista = new List<SubFormato>();
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_ListarSubFormatos", cnn) { CommandType = CommandType.StoredProcedure };
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read()) lista.Add(new SubFormato { IdSubFormato = dr.GetInt32(0), NomSubFormato = dr.GetString(1), IdFormato = dr.GetInt32(2), NombreFormato = dr.GetString(3), NombreCategoria = dr.GetString(4) });
                }
            }
            return lista;
        }

        public List<SubFormato> ListarSubFormatosPorNombre(string pNombre)
        {
            var lista = new List<SubFormato>();
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_ListarSubFormatosPorNombre", cnn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@nombre", (object)pNombre ?? DBNull.Value);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read()) lista.Add(new SubFormato { IdSubFormato = dr.GetInt32(0), NomSubFormato = dr.GetString(1), IdFormato = dr.GetInt32(2), NombreFormato = dr.GetString(3), NombreCategoria = dr.GetString(4) });
                }
            }
            return lista;
        }

        public SubFormato ObtenerSubFormatoPorId(int id)
        {
            var obj = new SubFormato();
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_ObtenerSubFormatoPorId", cnn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@id", id);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read()) { obj.IdSubFormato = dr.GetInt32(0); obj.NomSubFormato = dr.GetString(1); obj.IdFormato = dr.GetInt32(2); }
                }
            }
            return obj;
        }

        public void InsertarSubFormato(SubFormato sub)
        {
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_InsertarSubFormato", cnn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@nombre", sub.NomSubFormato); cmd.Parameters.AddWithValue("@idformato", sub.IdFormato);
                cmd.ExecuteNonQuery();
            }
        }

        public void ActualizarSubFormato(SubFormato sub)
        {
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_ActualizarSubFormato", cnn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@id", sub.IdSubFormato); cmd.Parameters.AddWithValue("@nombre", sub.NomSubFormato); cmd.Parameters.AddWithValue("@idformato", sub.IdFormato);
                cmd.ExecuteNonQuery();
            }
        }

        public void EliminarSubFormato(int id)
        {
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_EliminarSubFormato", cnn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
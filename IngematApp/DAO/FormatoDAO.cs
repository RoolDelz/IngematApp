using IngematApp.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace IngematApp.DAO
{
    public class FormatoDAO
    {
        private readonly string _cadenaConexion;
        public FormatoDAO(IConfiguration configuracion) { _cadenaConexion = configuracion.GetConnectionString("CadenaConexion") ?? throw new InvalidOperationException(); }

        public List<Formato> ListarFormatos()
        {
            var lista = new List<Formato>();
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_ListarFormatos", cnn) { CommandType = CommandType.StoredProcedure };
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read()) lista.Add(new Formato { IdFormato = dr.GetInt32(0), NomFormato = dr.GetString(1), PrecioFormato = dr.GetDecimal(2), IdCategoria = dr.GetInt32(3), NombreCategoria = dr.GetString(4) });
                }
            }
            return lista;
        }

        public List<Formato> ListarFormatosPorNombre(string pNombre)
        {
            var lista = new List<Formato>();
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_ListarFormatosPorNombre", cnn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@nombre", (object)pNombre ?? DBNull.Value);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read()) lista.Add(new Formato { IdFormato = dr.GetInt32(0), NomFormato = dr.GetString(1), PrecioFormato = dr.GetDecimal(2), IdCategoria = dr.GetInt32(3), NombreCategoria = dr.GetString(4) });
                }
            }
            return lista;
        }

        public Formato ObtenerFormatoPorId(int id)
        {
            var obj = new Formato();
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_ObtenerFormatoPorId", cnn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@id", id);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read()) { obj.IdFormato = dr.GetInt32(0); obj.NomFormato = dr.GetString(1); obj.PrecioFormato = dr.GetDecimal(2); obj.IdCategoria = dr.GetInt32(3); }
                }
            }
            return obj;
        }

        public void InsertarFormato(Formato f)
        {
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_InsertarFormato", cnn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@nombre", f.NomFormato); cmd.Parameters.AddWithValue("@precio", f.PrecioFormato); cmd.Parameters.AddWithValue("@idcat", f.IdCategoria);
                cmd.ExecuteNonQuery();
            }
        }

        public void ActualizarFormato(Formato f)
        {
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_ActualizarFormato", cnn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@id", f.IdFormato); cmd.Parameters.AddWithValue("@nombre", f.NomFormato); cmd.Parameters.AddWithValue("@precio", f.PrecioFormato); cmd.Parameters.AddWithValue("@idcat", f.IdCategoria);
                cmd.ExecuteNonQuery();
            }
        }

        public void EliminarFormato(int id)
        {
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_EliminarFormato", cnn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
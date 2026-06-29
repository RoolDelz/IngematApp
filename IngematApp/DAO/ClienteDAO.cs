using IngematApp.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace IngematApp.DAO
{
    public class ClienteDAO
    {
        private readonly string _cadenaConexion;
        public ClienteDAO(IConfiguration config) { _cadenaConexion = config.GetConnectionString("CadenaConexion") ?? throw new InvalidOperationException(); }

        public List<Cliente> ListarClientes()
        {
            var lista = new List<Cliente>();
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_ListarClientes", cnn) { CommandType = CommandType.StoredProcedure };
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read()) lista.Add(new Cliente
                    {
                        IdCliente = dr.GetInt32(0),
                        NomCliente = dr.GetString(1),
                        Documento = dr.GetString(2),
                        NDocumento = dr.GetString(3),
                        TelefonoCliente = dr.GetString(4),
                        CorreoCliente = dr.GetString(5)
                    });
                }
            }
            return lista;
        }


        // ==========================================
        // BUSCADOR POR NOMBRE
        // ==========================================
        public List<Cliente> ListarClientesPorNombre(string pNombre)
        {
            var lista = new List<Cliente>();
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_ListarClientesPorNombre", cnn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@nombre", (object)pNombre ?? DBNull.Value);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read()) lista.Add(new Cliente
                    {
                        IdCliente = dr.GetInt32(0),
                        NomCliente = dr.GetString(1),
                        Documento = dr.GetString(2),
                        NDocumento = dr.GetString(3),
                        TelefonoCliente = dr.GetString(4),
                        CorreoCliente = dr.GetString(5)
                    });
                }
            }
            return lista;
        }

        public Cliente ObtenerClientePorId(int id)
        {
            var obj = new Cliente();
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_ObtenerClientePorId", cnn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@id", id);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        obj.IdCliente = dr.GetInt32(0); obj.NomCliente = dr.GetString(1); obj.Documento = dr.GetString(2);
                        obj.NDocumento = dr.GetString(3); obj.TelefonoCliente = dr.GetString(4); obj.CorreoCliente = dr.GetString(5);
                    }
                }
            }
            return obj;
        }

        public void ActualizarCliente(Cliente c)
        {
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_ActualizarCliente", cnn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@id", c.IdCliente); cmd.Parameters.AddWithValue("@nom", c.NomCliente);
                cmd.Parameters.AddWithValue("@doc", c.Documento ?? ""); cmd.Parameters.AddWithValue("@ndoc", c.NDocumento ?? "");
                cmd.Parameters.AddWithValue("@tel", c.TelefonoCliente ?? ""); cmd.Parameters.AddWithValue("@correo", c.CorreoCliente ?? "");
                cmd.ExecuteNonQuery();
            }
        }
    }
}
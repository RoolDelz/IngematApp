using IngematApp.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace IngematApp.DAO
{
    public class CategoriaDAO
    {
        private readonly string _cadenaConexion;

        public CategoriaDAO(IConfiguration configuracion)
        {
            _cadenaConexion = configuracion.GetConnectionString("CadenaConexion")
                              ?? throw new InvalidOperationException("Cadena de Conexión no configurada");
        }

        public List<Categoria> ListarCategorias()
        {
            var lista = new List<Categoria>();
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_ListarCategorias", cnn);
                cmd.CommandType = CommandType.StoredProcedure; // <-- El cambio clave

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Categoria
                        {
                            IdCategoria = dr.GetInt32(0),
                            NomCategoria = dr.GetString(1)
                        });
                    }
                }
            }
            return lista;
        }

        public List<Categoria> ListarCategoriasPorNombre(string pNombre)
        {
            var lista = new List<Categoria>();
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_ListarCategoriasPorNombre", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nombre", (object)pNombre ?? DBNull.Value);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Categoria
                        {
                            IdCategoria = dr.GetInt32(0),
                            NomCategoria = dr.GetString(1)
                        });
                    }
                }
            }
            return lista;
        }

        public Categoria ObtenerCategoriaPorId(int id)
        {
            var obj = new Categoria();
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_ObtenerCategoriaPorId", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        obj.IdCategoria = dr.GetInt32(0);
                        obj.NomCategoria = dr.GetString(1);
                    }
                }
            }
            return obj;
        }

        public void InsertarCategoria(Categoria categoria)
        {
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_InsertarCategoria", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nombre", categoria.NomCategoria);
                cmd.ExecuteNonQuery();
            }
        }

        public void ActualizarCategoria(Categoria categoria)
        {
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_ActualizarCategoria", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", categoria.IdCategoria);
                cmd.Parameters.AddWithValue("@nombre", categoria.NomCategoria);
                cmd.ExecuteNonQuery();
            }
        }

        public void EliminarCategoria(int id)
        {
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_EliminarCategoria", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
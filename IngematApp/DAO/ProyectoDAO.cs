using IngematApp.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace IngematApp.DAO
{
    public class ProyectoDAO
    {
        private readonly string _cadenaConexion;

        public ProyectoDAO(IConfiguration config)
        {
            _cadenaConexion = config.GetConnectionString("CadenaConexion") ?? "";
        }

        public List<ProyectoViewModel> ListarActivos()
        {
            var lista = new List<ProyectoViewModel>();
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_ListarProyectosActivos", cnn) { CommandType = CommandType.StoredProcedure };
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new ProyectoViewModel
                        {
                            IdProyecto = dr.GetInt32(0),
                            NomProyecto = dr.GetString(1),
                            FechaCreacion = dr.GetDateTime(2),
                            Estado = dr.GetString(3),
                            TotalOTs = dr.GetInt32(4),
                            OTsCompletadas = dr.GetInt32(5)
                        });
                    }
                }
            }
            return lista;
        }

        public List<ProyectoViewModel> ListarFinalizados()
        {
            var lista = new List<ProyectoViewModel>();
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_ListarProyectosFinalizados", cnn) { CommandType = CommandType.StoredProcedure };
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new ProyectoViewModel
                        {
                            IdProyecto = dr.GetInt32(0),
                            NomProyecto = dr.GetString(1),
                            FechaCreacion = dr.GetDateTime(2),
                            Estado = dr.GetString(3),
                            TotalOTs = dr.GetInt32(4),
                            OTsCompletadas = dr.GetInt32(5)
                        });
                    }
                }
            }
            return lista;
        }

        public List<ProyectoOTViewModel> ListarOTsPorProyecto(int idProyecto)
        {
            var lista = new List<ProyectoOTViewModel>();
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_ListarOTsPorProyecto", cnn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@IdProyecto", idProyecto);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new ProyectoOTViewModel
                        {
                            IdOT = dr.GetInt32(0),
                            N_OT = dr.GetString(1),
                            NomOT = dr.GetString(2),
                            FechaCreacion = dr.GetDateTime(3),
                            Estado = dr.GetString(4),
                            TecnicoAsignado = dr.GetString(5)
                        });
                    }
                }
            }
            return lista;
        }
    }
}

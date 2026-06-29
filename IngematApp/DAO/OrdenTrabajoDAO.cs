using IngematApp.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace IngematApp.DAO
{
    public class OrdenTrabajoDAO
    {
        private readonly string _cadenaConexion;

        public OrdenTrabajoDAO(IConfiguration config)
        {
            _cadenaConexion = config.GetConnectionString("CadenaConexion") ?? "";
        }

        public List<OrdenTrabajoViewModel> ListarParaGerente()
        {
            var lista = new List<OrdenTrabajoViewModel>();
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_ListarOrdenesTrabajoGerente", cnn) { CommandType = CommandType.StoredProcedure };
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new OrdenTrabajoViewModel
                        {
                            IdOT = dr.GetInt32(0),
                            N_OT = dr.GetString(1),
                            NomOT = dr.GetString(2),
                            NomProyecto = dr.GetString(3),
                            FechaCreacion = dr.GetDateTime(4),
                            Estado = dr.GetString(5),
                            TecnicoAsignado = dr.GetString(6)
                        });
                    }
                }
            }
            return lista;
        }

        public List<OrdenTrabajoViewModel> ListarParaTecnico(int idEmpleado)
        {
            var lista = new List<OrdenTrabajoViewModel>();
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_ListarOrdenesTrabajoTecnico", cnn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@IdEmpleado", idEmpleado);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new OrdenTrabajoViewModel
                        {
                            IdOT = dr.GetInt32(0),
                            N_OT = dr.GetString(1),
                            NomOT = dr.GetString(2),
                            NomProyecto = dr.GetString(3),
                            FechaCreacion = dr.GetDateTime(4),
                            Estado = dr.GetString(5)
                        });
                    }
                }
            }
            return lista;
        }

        public List<EmpleadoViewModel> ListarTecnicos()
        {
            var lista = new List<EmpleadoViewModel>();
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_ListarAyudantesTecnicos", cnn) { CommandType = CommandType.StoredProcedure };
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new EmpleadoViewModel
                        {
                            IdEmpleado = dr.GetInt32(0),
                            NombreEmpleado = dr.GetString(1)
                        });
                    }
                }
            }
            return lista;
        }

        public void AsignarTecnico(int idOt, int idEmpleado)
        {
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_AsignarTecnicoOrdenTrabajo", cnn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@IdOT", idOt);
                cmd.Parameters.AddWithValue("@IdEmpleado", idEmpleado);
                cmd.ExecuteNonQuery();
            }
        }

        public void InsertarReporte(int idOt, string nombreArchivo, string extension, string rutaArchivo)
        {
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_InsertarReporteOT", cnn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@IdOT", idOt);
                cmd.Parameters.AddWithValue("@NombreArchivo", nombreArchivo);
                cmd.Parameters.AddWithValue("@Extension", extension);
                cmd.Parameters.AddWithValue("@RutaArchivo", rutaArchivo);
                cmd.ExecuteNonQuery();
            }
        }
        public WorkspaceViewModel ObtenerWorkspace(int idOt)
        {
            var model = new WorkspaceViewModel();
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_ObtenerOTParaWorkspace", cnn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@IdOT", idOt);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        model.IdOT = dr.GetInt32(0);
                        model.N_OT = dr.GetString(1);
                        model.NomOT = dr.GetString(2);
                        model.IdProyecto = dr.GetInt32(3);
                        model.NomProyecto = dr.GetString(4);
                        model.Estado = dr.GetString(5);
                    }
                }
            }
            return model;
        }
        public List<ReporteViewModel> ListarReportes(int idOt)
        {
            var lista = new List<ReporteViewModel>();
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_ListarReportesPorOT", cnn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@IdOT", idOt);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new ReporteViewModel
                        {
                            IdReporte = dr.GetInt32(0),
                            NombreArchivo = dr.GetString(1),
                            Extension = dr.GetString(2),
                            RutaArchivo = dr.GetString(3),
                            FechaSubida = dr.GetDateTime(4)
                        });
                    }
                }
            }
            return lista;
        }

        public void EliminarReporte(int idReporte)
        {
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_EliminarReporteOT", cnn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@IdReporte", idReporte);
                cmd.ExecuteNonQuery();
            }
        }

        public void FinalizarOrdenTrabajo(int idOt)
        {
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_FinalizarOrdenTrabajo", cnn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@IdOT", idOt);
                cmd.ExecuteNonQuery();
            }
        }
    }
}

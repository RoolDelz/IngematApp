using IngematApp.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace IngematApp.DAO
{
    public class OrdenServicioDAO
    {
        private readonly string _cadenaConexion;
        public OrdenServicioDAO(IConfiguration config) { _cadenaConexion = config.GetConnectionString("CadenaConexion") ?? throw new InvalidOperationException(); }

        public List<EmpresaDropdown> ListarEmpresas()
        {
            var lista = new List<EmpresaDropdown>();
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_ListarEmpresas", cnn) { CommandType = CommandType.StoredProcedure };
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read()) lista.Add(new EmpresaDropdown { IdEmpresa = dr.GetInt32(0), NomEmpresa = dr.GetString(1) });
                }
            }
            return lista;
        }

        public List<OrdenServicioListado> ListarOrdenesServicio()
        {
            var lista = new List<OrdenServicioListado>();
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_ListarOrdenesServicio", cnn) { CommandType = CommandType.StoredProcedure };
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read()) lista.Add(new OrdenServicioListado
                    {
                        IdOS = dr.GetInt32(0),
                        FechaOS = dr.GetDateTime(1),
                        NomCliente = dr.GetString(2),
                        Motivo = dr.GetString(3),
                        NomEmpresa = dr.GetString(4),
                        EstadoOS = dr.GetString(5),
                        PrecioFinal = dr.GetDecimal(6)
                    });
                }
            }
            return lista;
        }

        public List<ProformaListado> ListarProformasSinOS()
        {
            var lista = new List<ProformaListado>();
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_ListarProformasSinOS", cnn) { CommandType = CommandType.StoredProcedure };
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read()) lista.Add(new ProformaListado
                    {
                        IdProforma = dr.GetInt32(0),
                        NomCliente = dr.GetString(1),
                        Motivo = dr.GetString(2),
                        FechaP = dr.GetDateTime(3),
                        PrecioFinal = dr.GetDecimal(4)
                    });
                }
            }
            return lista;
        }

        public DatosProformaParaOS ObtenerDatosProformaParaOS(int idProforma)
        {
            var obj = new DatosProformaParaOS();
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_ObtenerDatosProformaParaOS", cnn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@idProforma", idProforma);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        obj.IdProforma = dr.GetInt32(0); obj.NomCliente = dr.GetString(1); obj.NDocumento = dr.GetString(2);
                        obj.Motivo = dr.GetString(3); obj.DireccionProforma = dr.GetString(4); obj.PrecioFinal = dr.GetDecimal(5);
                        obj.NomCategoria = dr.GetString(6); obj.NomFormato = dr.GetString(7);
                    }
                }
            }
            return obj;
        }

        public void InsertarOrdenServicio(OrdenServicioViewModel modelo)
        {
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_InsertarOrdenServicio", cnn) { CommandType = CommandType.StoredProcedure };

                cmd.Parameters.AddWithValue("@IdProforma", modelo.IdProforma);
                cmd.Parameters.AddWithValue("@NomEmpresa", modelo.NomEmpresa);
                cmd.Parameters.AddWithValue("@Ruc", modelo.Ruc ?? "");
                cmd.Parameters.AddWithValue("@Telefono", modelo.TelefonoEmpresa ?? "");
                cmd.Parameters.AddWithValue("@Correo", modelo.CorreoEmpresa ?? "");
                cmd.Parameters.AddWithValue("@Direccion", modelo.DireccionEmpresa ?? "");

                cmd.ExecuteNonQuery();
            }
        }

        public List<OrdenServicioListado> ListarOSPendientes()
        {
            var lista = new List<OrdenServicioListado>();
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_ListarOSPendientes", cnn) { CommandType = CommandType.StoredProcedure };
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read()) lista.Add(new OrdenServicioListado
                    {
                        IdOS = dr.GetInt32(0),
                        FechaOS = dr.GetDateTime(1),
                        NomCliente = dr.GetString(2),
                        Motivo = dr.GetString(3),
                        NomEmpresa = dr.GetString(4),
                        EstadoOS = dr.GetString(5)
                    });
                }
            }
            return lista;
        }

        public void AprobarOrdenServicio(int idOS)
        {
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_AprobarOrdenServicio", cnn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@IdOS", idOS);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
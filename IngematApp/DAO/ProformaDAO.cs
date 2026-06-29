using IngematApp.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace IngematApp.DAO
{
    public class ProformaDAO
    {
        private readonly string _cadenaConexion;
        public ProformaDAO(IConfiguration config) { _cadenaConexion = config.GetConnectionString("CadenaConexion") ?? throw new InvalidOperationException(); }

        public void RegistrarCotizacionCompleta(ProformaViewModel modelo, decimal precioFormatoBase)
        {
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_RegistrarCotizacion", cnn);
                cmd.CommandType = CommandType.StoredProcedure;

                // Parámetros del Cliente
                cmd.Parameters.AddWithValue("@NomCliente", modelo.NomCliente);
                cmd.Parameters.AddWithValue("@Documento", modelo.Documento ?? "RUC");
                cmd.Parameters.AddWithValue("@NDocumento", modelo.NDocumento ?? "");
                cmd.Parameters.AddWithValue("@TelefonoCliente", modelo.TelefonoCliente ?? "");
                cmd.Parameters.AddWithValue("@CorreoCliente", modelo.CorreoCliente ?? "");

                // Parámetros de la Proforma
                cmd.Parameters.AddWithValue("@Motivo", modelo.Motivo);
                cmd.Parameters.AddWithValue("@DireccionProforma", modelo.DireccionProforma ?? "");
                cmd.Parameters.AddWithValue("@IdCategoria", modelo.IdCategoria);
                cmd.Parameters.AddWithValue("@IdFormato", modelo.IdFormato);

                // Cálculos
                decimal total = precioFormatoBase;
                decimal precioFinal = total * 1.18m;
                cmd.Parameters.AddWithValue("@Total", total);
                cmd.Parameters.AddWithValue("@PrecioFinal", precioFinal);

                cmd.ExecuteNonQuery(); // ¡El SP en SQL hace el BeginTransaction y el Insert doble!
            }
        }

        public List<ProformaListado> ListarProformas()
        {
            var lista = new List<ProformaListado>();
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_ListarProformas", cnn) { CommandType = CommandType.StoredProcedure };
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read()) lista.Add(new ProformaListado { IdProforma = dr.GetInt32(0), NomCliente = dr.GetString(1), Motivo = dr.GetString(2), FechaP = dr.GetDateTime(3), Total = dr.GetDecimal(4), PrecioFinal = dr.GetDecimal(5) });
                }
            }
            return lista;
        }

        public List<ProformaListado> ListarProformasPorCliente(string pNombre)
        {
            var lista = new List<ProformaListado>();
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_ListarProformasPorCliente", cnn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@nombre", (object)pNombre ?? DBNull.Value);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read()) lista.Add(new ProformaListado { IdProforma = dr.GetInt32(0), NomCliente = dr.GetString(1), Motivo = dr.GetString(2), FechaP = dr.GetDateTime(3), Total = dr.GetDecimal(4), PrecioFinal = dr.GetDecimal(5) });
                }
            }
            return lista;
        }

        public ProformaImpresion ObtenerProformaParaImpresion(int idProforma)
        {
            var obj = new ProformaImpresion();
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_ObtenerProformaImpresion", cnn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@id", idProforma);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        obj.IdProforma = dr.GetInt32(0); obj.FechaP = dr.GetDateTime(1); obj.Motivo = dr.GetString(2); obj.DireccionProforma = dr.GetString(3); obj.Total = dr.GetDecimal(4); obj.PrecioFinal = dr.GetDecimal(5);
                        obj.NomCliente = dr.GetString(6); obj.Documento = dr.GetString(7); obj.NDocumento = dr.GetString(8); obj.TelefonoCliente = dr.GetString(9); obj.CorreoCliente = dr.GetString(10);
                        obj.NomCategoria = dr.GetString(11); obj.NomFormato = dr.GetString(12);
                    }
                }
            }
            return obj;
        }
    }
}
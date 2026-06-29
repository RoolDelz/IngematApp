using IngematApp.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace IngematApp.DAO
{
    public class FacturaDAO
    {
        private readonly string _cadenaConexion;

        public FacturaDAO(IConfiguration config)
        {
            _cadenaConexion = config.GetConnectionString("CadenaConexion") ?? "";
        }

        // Método reutilizable para leer una lista de facturas desde un SP
        private List<FacturaViewModel> EjecutarListado(string nombreSP)
        {
            var lista = new List<FacturaViewModel>();
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand(nombreSP, cnn) { CommandType = CommandType.StoredProcedure };
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new FacturaViewModel
                        {
                            IdFactura = dr.GetInt32(0),
                            NumFactura = dr.GetString(1),
                            FechaFactura = dr.GetDateTime(2),
                            PrecioBase = dr.GetDecimal(3),
                            PrecioFactura = dr.GetDecimal(4),
                            NomProyecto = dr.GetString(5),
                            NomCliente = dr.GetString(6),
                            Motivo = dr.GetString(7),
                            Estado = dr.GetString(8),
                            MetodoPago = dr.GetString(9)
                        });
                    }
                }
            }
            return lista;
        }

        public List<FacturaViewModel> ListarPendientes()
        {
            return EjecutarListado("SP_ListarFacturasPendientes");
        }

        public List<FacturaViewModel> ListarRealizadas()
        {
            return EjecutarListado("SP_ListarFacturasRealizadas");
        }

        public List<FacturaViewModel> ListarPagadas()
        {
            return EjecutarListado("SP_ListarFacturasPagadas");
        }

        public FacturaImpresion ObtenerParaImpresion(int idFactura)
        {
            var obj = new FacturaImpresion();
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_ObtenerFacturaImpresion", cnn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@IdFactura", idFactura);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        obj.IdFactura = dr.GetInt32(0);
                        obj.NumFactura = dr.GetString(1);
                        obj.FechaFactura = dr.GetDateTime(2);
                        obj.PrecioBase = dr.GetDecimal(3);
                        obj.PrecioFactura = dr.GetDecimal(4);
                        obj.MetodoPago = dr.GetString(5);
                        obj.Estado = dr.GetString(6);
                        obj.NomProyecto = dr.GetString(7);
                        obj.NomCliente = dr.GetString(8);
                        obj.Documento = dr.GetString(9);
                        obj.NDocumento = dr.GetString(10);
                        obj.TelefonoCliente = dr.GetString(11);
                        obj.CorreoCliente = dr.GetString(12);
                        obj.NomEmpresa = dr.GetString(13);
                        obj.Ruc = dr.GetString(14);
                        obj.DireccionEmpresa = dr.GetString(15);
                        obj.Motivo = dr.GetString(16);
                        obj.NomCategoria = dr.GetString(17);
                        obj.NomFormato = dr.GetString(18);
                    }
                }
            }
            return obj;
        }

        public void GuardarMetodoPago(int idFactura, string metodoPago)
        {
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_GuardarMetodoPago", cnn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@IdFactura", idFactura);
                cmd.Parameters.AddWithValue("@MetodoPago", metodoPago);
                cmd.ExecuteNonQuery();
            }
        }

        public void RealizarFactura(int idFactura)
        {
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_RealizarFactura", cnn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@IdFactura", idFactura);
                cmd.ExecuteNonQuery();
            }
        }

        public void PagarFactura(int idFactura)
        {
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_PagarFactura", cnn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@IdFactura", idFactura);
                cmd.ExecuteNonQuery();
            }
        }
    }
}

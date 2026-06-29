using IngematApp.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace IngematApp.DAO
{
    public class EmpleadoDAO
    {
        private readonly string _cadenaConexion;
        public EmpleadoDAO(IConfiguration config) { _cadenaConexion = config.GetConnectionString("CadenaConexion") ?? throw new InvalidOperationException(); }

        // (Mantén aquí tu método ValidarLogin si ya lo habías copiado)


        // ==========================================
        // MÉTODO PARA VALIDAR EL LOGIN DE USUARIOS
        // ==========================================
        public DataTable ValidarLogin(string correo, string dni)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_ValidarEmpleado", cnn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@Correo", correo ?? "");
                cmd.Parameters.AddWithValue("@Dni", dni ?? "");

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
            return dt; // Retorna los datos si las credenciales coinciden
        }



        public List<Empleado> ListarEmpleados()
        {
            var lista = new List<Empleado>();
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_ListarEmpleados", cnn) { CommandType = CommandType.StoredProcedure };
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read()) lista.Add(new Empleado
                    {
                        IdEmpleado = dr.GetInt32(0),
                        NombreEmpleado = dr.GetString(1),
                        Dni = dr.GetString(2),
                        TelefonoEmpleado = dr.IsDBNull(3) ? "" : dr.GetString(3),
                        CorreoEmpleado = dr.GetString(4),
                        Cargo = dr.GetString(5),
                        Estado = dr.GetBoolean(6)
                    });
                }
            }
            return lista;
        }

        public List<Empleado> ListarEmpleadosPorNombre(string pNombre)
        {
            var lista = new List<Empleado>();
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_ListarEmpleadosPorNombre", cnn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@nombre", (object)pNombre ?? DBNull.Value);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read()) lista.Add(new Empleado
                    {
                        IdEmpleado = dr.GetInt32(0),
                        NombreEmpleado = dr.GetString(1),
                        Dni = dr.GetString(2),
                        TelefonoEmpleado = dr.IsDBNull(3) ? "" : dr.GetString(3),
                        CorreoEmpleado = dr.GetString(4),
                        Cargo = dr.GetString(5),
                        Estado = dr.GetBoolean(6)
                    });
                }
            }
            return lista;
        }

        public Empleado ObtenerEmpleadoPorId(int id)
        {
            var obj = new Empleado();
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_ObtenerEmpleadoPorId", cnn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@id", id);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        obj.IdEmpleado = dr.GetInt32(0); obj.NombreEmpleado = dr.GetString(1); obj.Dni = dr.GetString(2);
                        obj.TelefonoEmpleado = dr.IsDBNull(3) ? "" : dr.GetString(3); obj.CorreoEmpleado = dr.GetString(4);
                        obj.Cargo = dr.GetString(5); obj.Estado = dr.GetBoolean(6);
                    }
                }
            }
            return obj;
        }

        public void InsertarEmpleado(Empleado e)
        {
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_InsertarEmpleado", cnn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@nombre", e.NombreEmpleado);
                cmd.Parameters.AddWithValue("@dni", e.Dni);
                cmd.Parameters.AddWithValue("@telefono", e.TelefonoEmpleado ?? "");
                cmd.Parameters.AddWithValue("@correo", e.CorreoEmpleado);
                cmd.Parameters.AddWithValue("@cargo", e.Cargo);
                cmd.ExecuteNonQuery();
            }
        }

        public void ActualizarEmpleado(Empleado e)
        {
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_ActualizarEmpleado", cnn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@id", e.IdEmpleado);
                cmd.Parameters.AddWithValue("@nombre", e.NombreEmpleado);
                cmd.Parameters.AddWithValue("@dni", e.Dni);
                cmd.Parameters.AddWithValue("@telefono", e.TelefonoEmpleado ?? "");
                cmd.Parameters.AddWithValue("@correo", e.CorreoEmpleado);
                cmd.Parameters.AddWithValue("@cargo", e.Cargo);
                cmd.Parameters.AddWithValue("@estado", e.Estado);
                cmd.ExecuteNonQuery();
            }
        }

        public void DesactivarEmpleado(int id)
        {
            using (SqlConnection cnn = new SqlConnection(_cadenaConexion))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_DesactivarEmpleado", cnn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
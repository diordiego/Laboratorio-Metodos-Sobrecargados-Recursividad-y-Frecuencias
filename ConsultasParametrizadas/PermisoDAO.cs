// ============================================================
// Clase 2: PermisoDAO
// Operaciones sobre la tabla 'permisos' con CONSULTAS
// PARAMETRIZADAS / PREPARADAS (defensa contra inyeccion SQL).
//
// IMPORTANTE: en MySql.Data los parametros se agregan ANTES
// de llamar a Prepare(), si no da el error
// "Parameter '@...' was not found during prepare".
// ============================================================

using System;
using MySql.Data.MySqlClient;

namespace ConsultasParametrizadas
{
    public class PermisoDAO
    {
        private readonly Conexion conexion = new Conexion();

        // --------------------------------------------------------
        // INSERT PARAMETRIZADO
        // Aunque el usuario escriba  ' OR '1'='1  o  root'; --
        // se guarda LITERALMENTE como texto, sin alterar el SQL.
        // --------------------------------------------------------
        public void InsertarPermiso(string usuario, string contrasena, string correo)
        {
            using (MySqlConnection conn = conexion.ObtenerConexion())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;

                    // Marcadores (@...), NO concatenacion de cadenas
                    cmd.CommandText =
                        "INSERT INTO permisos (Usuario, Contrasena, Correo, Administrador) " +
                        "VALUES (@usuario, @contrasena, @correo, 0)";

                    // 1ro los parametros (entran como DATO, no como codigo)
                    cmd.Parameters.AddWithValue("@usuario", usuario);
                    cmd.Parameters.AddWithValue("@contrasena", contrasena);
                    cmd.Parameters.AddWithValue("@correo", correo);

                    // 2do preparar (ya existen los parametros)
                    cmd.Prepare();

                    int filas = cmd.ExecuteNonQuery();
                    Console.WriteLine("Registro insertado. Filas afectadas: " + filas);
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Error " + ex.Number + ": " + ex.Message);
                }
            }
        }

        // --------------------------------------------------------
        // UPDATE PARAMETRIZADO
        // El WHERE tambien va parametrizado (@usuario), por lo que
        // no se puede inyectar ' OR '1'='1 para afectar toda la tabla.
        // --------------------------------------------------------
        public void ActualizarCorreo(string usuario, string nuevoCorreo)
        {
            using (MySqlConnection conn = conexion.ObtenerConexion())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;

                    cmd.CommandText =
                        "UPDATE permisos SET Correo = @correo, " +
                        "FechaModificacion = CURDATE() " +
                        "WHERE Usuario = @usuario";

                    // Parametros antes de preparar
                    cmd.Parameters.AddWithValue("@correo", nuevoCorreo);
                    cmd.Parameters.AddWithValue("@usuario", usuario);

                    cmd.Prepare();

                    int filas = cmd.ExecuteNonQuery();
                    if (filas > 0)
                        Console.WriteLine("Correo actualizado. Filas afectadas: " + filas);
                    else
                        Console.WriteLine("No existe un usuario con ese nombre.");
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Error " + ex.Number + ": " + ex.Message);
                }
            }
        }
    }
}
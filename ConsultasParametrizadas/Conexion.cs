using MySql.Data.MySqlClient;
 
namespace ConsultasParametrizadas
{
    public class Conexion
    {
        // Ajusta estos valores a tu entorno de MySQL
        private readonly string strConnection =
            "server=localhost;database=lab_hpa;uid=root;pwd=Xeneize217-";
 
        // Devuelve una conexion nueva (sin abrir todavia)
        public MySqlConnection ObtenerConexion()
        {
            return new MySqlConnection(strConnection);
        }
    }
}
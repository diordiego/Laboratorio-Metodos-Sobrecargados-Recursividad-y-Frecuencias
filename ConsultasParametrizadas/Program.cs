using System;

namespace ConsultasParametrizadas
{
    class Program
    {
        static void Main(string[] args)
        {
            PermisoDAO dao = new PermisoDAO();

            Console.WriteLine("=== INSERT parametrizado ===");
            // Insercion normal
            dao.InsertarPermiso("dsanjur", "clave123", "dsanjur@correo.com");

            // Prueba de seguridad: aunque mandemos un texto malicioso,
            // se guarda literalmente como nombre de usuario, NO se ejecuta.
            dao.InsertarPermiso("' OR '1'='1", "x", "prueba@correo.com");

            Console.WriteLine();
            Console.WriteLine("=== UPDATE parametrizado ===");
            dao.ActualizarCorreo("dsanjur", "nuevo_correo@correo.com");

            Console.WriteLine();
            Console.WriteLine("Listo. Verifica la tabla 'permisos' en MySQL Workbench.");
            Console.ReadKey();
        }
    }
}
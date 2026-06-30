using System;
using MySql.Data.MySqlClient; 

namespace Progra3Card.Administrativo
{
    class Program
    {
        private static string connectionString = "Server=localhost;Database=mi_banco_db;Uid=root;Pwd=Ragnarlothdrok11;";

        static void Main(string[] args)
        {
            bool salir = false;
            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine("    SISTEMA ADMINISTRATIVO PROGRA3CARD   ");
                Console.WriteLine("========================================");
                Console.WriteLine("1. Emitir Nueva Tarjeta (Alta de Cliente)");
                Console.WriteLine("2. Listar Tarjetas");
                Console.WriteLine("3. Ver Detalle de una Tarjeta / Cliente");
                Console.WriteLine("4. Eliminar Tarjeta (Baja de Sistema)");
                Console.WriteLine("5. Emitir Nueva Liquidación Mensual");
                Console.WriteLine("6. Salir");
                Console.WriteLine("========================================");
                Console.Write("Seleccione una opción: ");

                switch (Console.ReadLine())
                {
                    case "1": MenuEmitirTarjeta(); break;
                    case "2": MenuListarTarjetas(); break;
                    case "3": MenuVerDetalleTarjeta(); break;
                    case "4": MenuEliminarTarjeta(); break;
                    case "5": MenuEmitirLiquidacion(); break;
                    case "6": salir = true; break;
                    default:
                        Console.WriteLine("Opción no válida. Presione una tecla para continuar...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        // Funciones a completar:
        static void MenuEmitirTarjeta()
        {
            Console.Clear();
            Console.WriteLine("--- EMITIR NUEVA TARJETA (ALTA CLIENTE) ---");

            //Datos personales del Cliente
            Console.Write("Nombre: "); string nombre = Console.ReadLine();
            Console.Write("Apellido: "); string apellido = Console.ReadLine();
            Console.Write("Tipo Doc (DNI/PASAPORTE): "); string tipoDoc = Console.ReadLine().ToUpper();
            Console.Write("Nro Documento: "); string documento = Console.ReadLine();
            Console.Write("Email: "); string email = Console.ReadLine();
            Console.Write("Fecha Nacimiento (AAAA-MM-DD): "); string fechaNac = Console.ReadLine();

            //Datos del Plástico/Tarjeta
            Console.Write("Nro Cuenta (Entero Único): "); int numCuenta = Convert.ToInt32(Console.ReadLine());
            Console.Write("Nro Tarjeta (16 dígitos): "); string nroTarjeta = Console.ReadLine();
            Console.Write("Banco Emisor (Ej: Banco Nación): "); string banco = Console.ReadLine();

            bool exito = RegistrarClienteYTarjeta(nombre, apellido, tipoDoc, documento, email, fechaNac, numCuenta, nroTarjeta, banco);

            if (exito)
                Console.WriteLine("\n¡Cliente registrado y tarjeta emitida con éxito en la base de datos!");
            else
                Console.WriteLine("\n❌ Error al emitir la tarjeta. Verifique si el documento o número de cuenta ya existen.");

            Console.WriteLine("\nPresione una tecla para volver al menú...");
            Console.ReadKey();
        }
        static void MenuListarTarjetas()
        {
            Console.Clear();
            Console.WriteLine("--- LISTADO GENERAL DE TARJETAS ---");
            Console.WriteLine("{0,-12} {1,-18} {2,-20} {3,-15}", "Nro Cuenta", "Nro Tarjeta", "Banco Emisor", "DNI Titular");
            Console.WriteLine("----------------------------------------------------------------------");

            // === A realizar ===
            // Aquí deben implementar un SELECT sobre la tabla 'tarjetas'
            // para recorrer las filas e imprimirlas en la consola.
            
            ObtenerYMostrarTarjetas();

            Console.WriteLine("\nPresione una tecla para volver al menú...");
            Console.ReadKey();
        }

        static void MenuVerDetalleTarjeta()
        {
            Console.Clear();
            Console.WriteLine("--- DETALLE DE TARJETA Y CLIENTE ---");
            Console.Write("Ingrese el Número de Cuenta a consultar: ");
            int numCuenta = Convert.ToInt32(Console.ReadLine());

            // === A realizar ===
            // Aquí deben realizar un SELECT con un JOIN entre 'tarjetas' y 'usuarios' 
            // filtrando por el numCuenta para traer todos los campos (Nombre, Apellido, Email, Saldo, etc.)
            
            MostrarDetalleCompleto(numCuenta);

            Console.WriteLine("\nPresione una tecla para volver al menú...");
            Console.ReadKey();
        }

        static void MenuEliminarTarjeta()
        {
            Console.Clear();
            Console.WriteLine("--- ELIMINAR TARJETA DEL SISTEMA ---");
            Console.Write("Ingrese el Número de Cuenta de la tarjeta a dar de baja: ");
            int numCuenta = Convert.ToInt32(Console.ReadLine());

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n⚠️ ADVERTENCIA: Se eliminará la tarjeta, sus liquidaciones y los datos de acceso web vinculados.");
            Console.ResetColor();
            Console.Write("¿Está seguro de continuar? (S/N): ");
            
            if (Console.ReadLine().ToUpper() == "S")
            {
                // === A realizar ===
                // Aquí deben ejecutar un DELETE sobre la tabla 'tarjetas' donde num_cuenta = numCuenta.
                // Como definimos ON DELETE CASCADE en la base de datos, las liquidaciones se borrarán solas.
                // Opcional: Evaluar si también eliminan al usuario de la tabla 'usuarios' o si lo mantienen.
                
                bool exito = DarDeBajaTarjeta(numCuenta);

                if (exito)
                    Console.WriteLine("\nTarjeta eliminada correctamente del sistema.");
                else
                    Console.WriteLine("\nError al intentar eliminar la tarjeta. Verifique el número de cuenta.");
            }
            else
            {
                Console.WriteLine("\nOperación cancelada.");
            }

            Console.WriteLine("\nPresione una tecla para volver al menú...");
            Console.ReadKey();
        }
        static void MenuEmitirLiquidacion()
        {
            Console.Clear();
            Console.WriteLine("--- EMITIR NUEVA LIQUIDACIÓN MENSUAL ---");
            Console.Write("Ingrese el Número de Cuenta del cliente: ");
            int numCuenta = Convert.ToInt32(Console.ReadLine());
            Console.Write("Período (YYYY-MM): "); string periodo = Console.ReadLine();
            Console.Write("Fecha Vencimiento (AAAA-MM-DD): "); string vencimiento = Console.ReadLine();
            Console.Write("Pago Mínimo ($): "); decimal pagoMin = Convert.ToDecimal(Console.ReadLine());
            Console.Write("Total a Pagar ($): "); decimal total = Convert.ToDecimal(Console.ReadLine());

            bool exito = InsertarLiquidacion(numCuenta, periodo, vencimiento, pagoMin, total);

            if (exito)
                Console.WriteLine("\n¡Liquidación mensual generada e impactada con éxito!");
            else
                Console.WriteLine("\n❌ Error al emitir la liquidación. Verifique si el Número de Cuenta existe.");

            Console.WriteLine("\nPresione una tecla para volver al menú...");
            Console.ReadKey();
        }

        // =========================================================================
        // MÉTODOS BASE QUE DEBEN COMPLETAR CON LA LÓGICA 
        // =========================================================================
        static bool RegistrarClienteYTarjeta(string nom, string ape, string tDoc, string doc, string mail, string fNac, 
        int cuenta, string tarjeta, string banco){
            using (MySqlConnection conn = new MySqlConnection(connectionString)){
                try
                {
                    conn.Open();
                    // Usamos una transacción para asegurarnos de que se guarden ambos o ninguno
                    using (MySqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            //Insertar el cliente (usuario e ingreso web inician en NULL)
                            string queryUser = "INSERT INTO usuarios (documento, nombre, apellido, tipo_doc, email, fecha_nacimiento, usuario, password) " +
                                               "VALUES (@doc, @nom, @ape, @tDoc, @mail, @fNac, NULL, NULL)";

                            using (MySqlCommand cmdUser = new MySqlCommand(queryUser, conn, trans))
                            {
                                cmdUser.Parameters.AddWithValue("@doc", doc);
                                cmdUser.Parameters.AddWithValue("@nom", nom);
                                cmdUser.Parameters.AddWithValue("@ape", ape);
                                cmdUser.Parameters.AddWithValue("@tDoc", tDoc);
                                cmdUser.Parameters.AddWithValue("@mail", mail);
                                cmdUser.Parameters.AddWithValue("@fNac", fNac);
                                cmdUser.ExecuteNonQuery();
                            }

                            //Insertar la tarjeta apuntando al DNI del cliente recién creado
                            string queryTarjeta = "INSERT INTO tarjetas (num_cuenta, numero_tarjeta, banco_emisor, dni_titular, estado, saldo) " +
                                                  "VALUES (@cuenta, @tarjeta, @banco, @doc, 'Activa', 0.00)";

                            using (MySqlCommand cmdTarj = new MySqlCommand(queryTarjeta, conn, trans))
                            {
                                cmdTarj.Parameters.AddWithValue("@cuenta", cuenta);
                                cmdTarj.Parameters.AddWithValue("@tarjeta", tarjeta);
                                cmdTarj.Parameters.AddWithValue("@banco", banco);
                                cmdTarj.Parameters.AddWithValue("@doc", doc);
                                cmdTarj.ExecuteNonQuery();
                            }

                            trans.Commit(); // Confirmamos los dos inserts
                            return true;
                        }
                        catch
                        {
                            trans.Rollback(); // Si algo falla, cancelamos todo el proceso
                            throw;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error de Base de Datos: {ex.Message}");
                    return false;
                }
            }
        }
        static void ObtenerYMostrarTarjetas()
        {
            string query = "SELECT num_cuenta, numero_tarjeta, banco_emisor, dni_titular FROM tarjetas";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                try
                {
                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("{0,-12} {1,-18} {2,-20} {3,-15}",
                                reader["num_cuenta"],
                                reader["numero_tarjeta"],
                                reader["banco_emisor"],
                                reader["dni_titular"]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al listar tarjetas: {ex.Message}");
                }
            }
        }

        static void MostrarDetalleCompleto(int cuenta)
        {
            // JOIN clásico para unificar al titular de la cuenta
            string query = "SELECT u.nombre, u.apellido, u.email, t.numero_tarjeta, t.banco_emisor, t.saldo, t.estado " +
                           "FROM tarjetas t " +
                           "INNER JOIN usuarios u ON t.dni_titular = u.documento " +
                           "WHERE t.num_cuenta = @cuenta";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@cuenta", cuenta);
                try
                {
                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Console.WriteLine($"Titular:     {reader["apellido"]}, {reader["nombre"]}");
                            Console.WriteLine($"Email:       {reader["email"]}");
                            Console.WriteLine($"Nro Tarjeta: {reader["numero_tarjeta"]}");
                            Console.WriteLine($"Banco:       {reader["banco_emisor"]}");
                            Console.WriteLine($"Estado:      {reader["estado"]}");
                            Console.WriteLine($"Saldo:       ${Convert.ToDouble(reader["saldo"]):N2}");
                        }
                        else
                        {
                            Console.WriteLine("❌ No se encontró ninguna tarjeta vinculada a ese Número de Cuenta.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al consultar detalle: {ex.Message}");
                }
            }
        }

        static bool DarDeBajaTarjeta(int cuenta)
        {
            string query = "DELETE FROM tarjetas WHERE num_cuenta = @cuenta";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@cuenta", cuenta);
                try
                {
                    conn.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas > 0; // Si borró al menos una fila, devuelve true
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al eliminar plástico: {ex.Message}");
                    return false;
                }
            }
        }
        static bool InsertarLiquidacion(int cuenta, string periodo, string vencimiento, decimal pagoMin, decimal total)
        {
            string query = "INSERT INTO liquidaciones (num_cuenta, periodo, fecha_vencimiento, pago_minimo, total_a_pagar) " +
                           "VALUES (@cuenta, @periodo, @vencimiento, @pagoMin, @total)";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@cuenta", cuenta);
                cmd.Parameters.AddWithValue("@periodo", periodo);
                cmd.Parameters.AddWithValue("@vencimiento", vencimiento);
                cmd.Parameters.AddWithValue("@pagoMin", pagoMin);
                cmd.Parameters.AddWithValue("@total", total);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al insertar liquidación: {ex.Message}");
                    return false;
                }
            }
        }
    }
}
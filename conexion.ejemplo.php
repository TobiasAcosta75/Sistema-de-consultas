<?php
// Copiá este archivo, renombralo a 'conexion.php' y poné tus credenciales locales
$host    = 'localhost';
$db      = 'mi_banco_db';
$user    = 'root';
$pass    = 'TU_CONTRASEÑA_AQUÍ';
$charset = 'utf8mb4';
// Data Source Name: La cadena de configuración que necesita PDO
$dsn = "mysql:host=$host;dbname=$db;charset=$charset";

// Opciones avanzadas de configuración para PDO
$options = [
    PDO::ATTR_ERRMODE            => PDO::ERRMODE_EXCEPTION, // Hace que PHP tire errores graves si falla la query
    PDO::ATTR_DEFAULT_FETCH_MODE => PDO::FETCH_ASSOC,       // Devuelve los datos de la BD como arrays asociativos
    PDO::ATTR_EMULATE_PREPARES   => false,                  // Usa consultas preparadas reales para máxima seguridad
];

try {
    // Intentamos crear la conexión creando una instancia del objeto PDO
    $pdo = new PDO($dsn, $user, $pass, $options);

    // Si querés probar que funciona, podés descomentar la línea de abajo:
    echo "Conexión exitosa a la base de datos.";
} catch (\PDOException $e) {
    // Si algo sale mal (contraseña incorrecta, servidor apagado), el bloque catch frena el programa
    die("Error crítico de conexión a la base de datos: " . $e->getMessage());
}
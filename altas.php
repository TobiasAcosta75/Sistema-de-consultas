<?php
require_once 'conexion.php';

if ($_SERVER["REQUEST_METHOD"] == "POST") {
    
    //Capturar los datos que vienen de registro.html
    $nro_doc   = $_POST['documento'] ?? ''; 
    $usuario   = $_POST['usuario'] ?? '';
    $password  = $_POST['passwordA'] ?? '';
    $password_conf = $_POST['passwordB'] ?? '';

    //Validaciones básicas
    if (empty($nro_doc) || empty($usuario) || empty($password)) {
        die("Error: Todos los campos son obligatorios.");
    }

    if ($password !== $password_conf) {
        die("Error: Las contraseñas ingresadas no coinciden.");
    }

    try {
        //Verificar si el cliente ya fue precargado por consola y si tiene tarjeta
        // Hacemos un JOIN rápido para asegurarnos de que el cliente existe Y tiene tarjeta asignada
        $sql_verificar = "SELECT u.documento, t.numero_tarjeta 
                          FROM usuarios u
                          INNER JOIN tarjetas t ON u.documento = t.dni_titular
                          WHERE u.documento = :documento";
        
        $stmt_verificar = $pdo->prepare($sql_verificar);
        $stmt_verificar->execute([':documento' => $nro_doc]);
        $cliente = $stmt_verificar->fetch(); //$cliente se convierte en array[document, num_tarjeta], si no se le retorno ningun dato, su valor es false

        if (!$cliente) {
            die("Error: El documento ingresado no se encuentra registrado en el sistema financiero o no posee una tarjeta emitida.");
        }

        //Si existe, actualizamos sus credenciales para activar la cuenta
        $sql_activar = "UPDATE usuarios 
                        SET usuario = :usuario, password = :password 
                        WHERE documento = :documento";
        
        $stmt_activar = $pdo->prepare($sql_activar);
        $stmt_activar->execute([
            ':usuario'   => $usuario,
            ':password'  => $password,
            ':documento' => $nro_doc
        ]);

        echo "¡Activación digital exitosa! Tu cuenta ha sido activada. Ya podés ingresar al HomeBanking.";

    } catch (\PDOException $e) {
        die("Error crítico en el proceso de activación: " . $e->getMessage());
    }

} else {
    header("Location: registro.html");
    exit();
}
?>
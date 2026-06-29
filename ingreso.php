<?php
//nos brinda un lugar de memoria privado en el servidor bajo la variable global $_SESSION, 
//todo lo guardado ahi estara disponible en cualquier archivo php mientras el user no cierre la session/navegador
session_start(); 
require_once 'conexion.php';
if ($_SERVER["REQUEST_METHOD"] == "POST") {

    //Capturar las credenciales del formulario de ingreso.html
    $usuario  = $_POST['usuario'] ?? '';
    $password = $_POST['password'] ?? '';

    if (empty($usuario) || empty($password)) {
        die("Error: Por favor, complete todos los campos.");
    }

    try {
        //Consultar a la base de datos si existe el usuario con esa contraseña
        $sql = "SELECT documento, nombre, apellido FROM usuarios 
                WHERE usuario = :usuario AND password = :password";

        $stmt = $pdo->prepare($sql);
        $stmt->execute([
            ':usuario'  => $usuario,
            ':password' => $password
        ]);

        $user_data = $stmt->fetch();

        //Validar el resultado
        if ($user_data) {
            // ¡Credenciales correctas! Guardamos los datos clave en la sesión
            $_SESSION['usuario_dni']    = $user_data['documento'];
            $_SESSION['usuario_nombre'] = $user_data['nombre'] . " " . $user_data['apellido'];
            $_SESSION['logeado']        = true;

            // Redirigimos al panel del cliente (resumen)
            header("Location: resumen.html");
            exit();
        } else {
            // Credenciales incorrectas
            die("Error: El usuario o la contraseña son incorrectos.");
        }
    } catch (\PDOException $e) {
        die("Error crítico en el inicio de sesión: " . $e->getMessage());
    }
} else {
    header("Location: ingreso.html");
    exit();
}
?>
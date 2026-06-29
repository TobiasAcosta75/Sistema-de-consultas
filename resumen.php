<?php
session_start();

// Control de acceso implícito para la API
if (!isset($_SESSION['logeado']) || $_SESSION['logeado'] !== true) {
    http_response_code(401); // No autorizado
    echo json_encode(['error' => 'No autorizado']);
    exit();
}

require_once 'conexion.php';
$dni_usuario = $_SESSION['usuario_dni'];

try {
    // Consulta A: Tarjeta
    $sql_tarjeta = "SELECT numero_tarjeta, banco_emisor, saldo, estado 
                    FROM tarjetas WHERE dni_titular = :dni";
    $stmt_t = $pdo->prepare($sql_tarjeta);
    $stmt_t->execute([':dni' => $dni_usuario]);
    $tarjeta = $stmt_t->fetch();

    // Consulta B: Liquidaciones (JOIN)
    $sql_liquidaciones = "SELECT l.periodo, l.fecha_vencimiento, l.pago_minimo, l.total_a_pagar 
                          FROM liquidaciones l
                          INNER JOIN tarjetas t ON l.num_cuenta = t.num_cuenta
                          WHERE t.dni_titular = :dni
                          ORDER BY l.periodo DESC";
    $stmt_l = $pdo->prepare($sql_liquidaciones);
    $stmt_l->execute([':dni' => $dni_usuario]);
    $liquidaciones = $stmt_l->fetchAll();

    // Estructuramos la respuesta final
    $respuesta = [
        'nombre_cliente' => $_SESSION['usuario_nombre'],
        'dni' => $dni_usuario,
        'tarjeta' => $tarjeta ? [
            'banco' => $tarjeta['banco_emisor'],
            'ultimos_cuatro' => substr($tarjeta['numero_tarjeta'], -4),
            'saldo' => (float)$tarjeta['saldo'],
            'estado' => $tarjeta['estado']
        ] : null,
        'liquidaciones' => $liquidaciones
    ];

    // Le avisamos al navegador que le enviamos un JSON
    header('Content-Type: application/json');
    echo json_encode($respuesta);
} catch (\PDOException $e) {
    http_response_code(500);
    echo json_encode(['error' => $e->getMessage()]);
}

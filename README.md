# Sistema-de-consultas
Sistema de gestión y consultas para la tarjeta de crédito
# documentacion 
1. Analizando el Formulario
action="altas.php": Le dice al navegador: "Cuando el usuario haga clic en el botón de Enviar, mandá todos los datos a este archivo PHP".

method="POST": Significa que los datos van a viajar de forma oculta en el cuerpo de la petición HTTP (no visibles en la URL), lo cual es obligatorio para manejar contraseñas y datos sensibles.

El valor del name será la "llave" para capturar los datos ingresados en el backend.

Dentro de toda la información que tiene el servidor, guardó una clave llamada "REQUEST_METHOD". El valor de esta clave te dice el método HTTP que se usó para acceder a la página.

El método header() es una función nativa de PHP que se utiliza para enviar encabezados HTTP sin procesar (raw HTTP headers) directamente al navegador del usuario.

$nro_doc = $_POST['nro_doc'] ?? '';
El operador ?? se llama operador de fusión de nulos (Null Coalescing Operator). Funciona como un escudo protector: le dice a PHP que intente leer $_POST['nro_doc'], pero que si por alguna razón ese campo no vino en el formulario o está vacío, en lugar de tirar un error de advertencia (Warning: Undefined array key), le asigne por defecto un texto vacío ('').

PARA CONECTAR PHP CON MYSQL: Vamos a usar PDO (PHP Data Objects), que es la librería orientada a objetos estándar de PHP. Es súper segura y te protege de ataques comunes como la inyección SQL.

¿Qué es $pdo->prepare() y por qué lo usamos?
Con $pdo->prepare($sql) le enviamos la estructura limpia de la query al motor de MySQL con "marcardores vacíos" (:nombre, :apellido).
El motor compila la estructura de la consulta de forma segura.
Con $stmt->execute([...]) le mandamos los datos puros. MySQL sabe que esos datos son texto y jamás los va a ejecutar como comandos SQL, protegiendo tu sistema por completo.

[resumen.html] 
      │
      ▼ (Al cargar la página, JS hace un Fetch)
[resumen.php] (Backend: Verifica sesión, hace el JOIN en la BD y devuelve un JSON)
      │
      ▼ (Devuelve los datos)
[resumen.html] (JS recibe el JSON e inyecta los datos en los ID del HTML)
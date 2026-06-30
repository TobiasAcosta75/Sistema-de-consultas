# 💳 Sistema de Consulta de Liquidaciones "Mis Tarjetas"
### UTN FRH - Tecnicatura Universitaria en Programación (Programación III)

Este proyecto consiste en un ecosistema integrado por dos plataformas distintas que comparten una misma base de datos relacional MySQL (`mi_banco_db`). Simula el circuito financiero real de la tarjeta **Progra3card**, dividiéndose en una fase administrativa de escritorio y un portal web para el cliente.

---

## 🗺️ Arquitectura del Sistema y Flujo de Información

El proyecto implementa una arquitectura desacoplada donde los datos persisten de manera centralizada:

1. **Fase Administrativa (C# - Consola):** El personal bancario da de alta a los clientes (dejando las credenciales web vacías) y emite plásticos. Además, genera de forma mensual las liquidaciones financieras correspondientes.
2. **Fase del Cliente (PHP / HTML / JS - Portal Web):** El usuario realiza su activación digital (Onboarding) validando su DNI y creando su cuenta. Posteriormente, inicia sesión para consumir de forma asrincrónica (mediante una API propia) sus datos de tarjeta e historial de resúmenes.

---

## 🛠️ Funcionalidades Implementadas

### 🖥️ 1. Módulo Administrativo (C#)
Desarrollado en .NET utilizando el paradigma de programación orientada a objetos y conectividad ADO.NET con `MySql.Data`.
* **Emitir Nueva Tarjeta (Alta de Cliente):** Registra los datos personales en la tabla `usuarios` y, mediante una **Transacción SQL**, asegura la creación en simultáneo de su tarjeta vinculada (relación 1:1) manteniendo sus credenciales de acceso web en `NULL`.
* **Listado General:** Recorre la tabla `tarjetas` mediante un `MySqlDataReader` para mostrar de forma formateada los plásticos emitidos en el sistema.
* **Detalle Completo:** Realiza un `INNER JOIN` entre clientes y tarjetas para auditar los saldos, estados y datos de contacto de una cuenta en específico.
* **Eliminación (Baja de Sistema):** Remueve la tarjeta seleccionada de la base de datos aplicando borrado lógico o en cascada (`ON DELETE CASCADE`) para sus liquidaciones asociadas.
* **Emisión de Liquidaciones:** Permite cargar los totales financieros (período, vencimiento, pago mínimo y total) impactando directamente en el historial financiero del cliente.

### 🌐 2. Portal del Cliente (PHP & Arquitectura SPA con JavaScript)
Diseñado con interfaces modernas, responsivas y estilizadas mediante **Tailwind CSS**.
* **Activación Digital (`registro.html` -> `altas.php`):** Permite al cliente realizar su onboarding digital. El backend valida que el DNI ya haya sido precargado por el banco, y mediante un `UPDATE` reemplaza los valores de usuario y contraseña (en texto plano según requerimiento).
* **Control de Acceso (`ingreso.html` -> `ingreso.php`):** Autentica las credenciales ingresadas, iniciando una sesión segura en el servidor a través de la superglobal `$_SESSION`.
* **Consumo de Datos Asincrónico (`resumen.html` -> `resumen.js`):** Al cargar la vista principal, JavaScript realiza una petición asrincrónica (`fetch`) de forma oculta protegiendo la navegación.
* **API de Consultas Relacionales (`resumen.php`):** Actúa como un endpoint seguro que solo responde en formato **JSON**. Realiza un `INNER JOIN` entre las tablas `tarjetas` y `liquidaciones` filtrando por el DNI del usuario logueado.
<!-- * **Seguridad y Cierre (`logout.php`):** Destruye por completo las variables de sesión del servidor evitando que usuarios no autorizados manipulen el historial dándole al botón "Atrás" del navegador. Este ultimo punto es a realizar!!. -->

---

## 🗄️ Estructura de la Base de Datos
Este proyecto consiste en un ecosistema integrado por dos plataformas distintas que comparten una misma base de datos relacional MySQL (`mi_banco_db`). Toda la infraestructura del portal web y el motor de base de datos se ejecutan localmente utilizando el entorno de desarrollo **XAMPP**.

El esquema relacional `mi_banco_db` cuenta con las siguientes entidades principales:
* **`usuarios`**: Almacena información de identidad (PK: `documento`). Las columnas `usuario` y `password` inician vacías hasta el registro web.
* **`tarjetas`**: Contiene la información de los plásticos y saldos. Vinculada de forma estricta 1:1 mediante la FK `dni_titular`.
* **`liquidaciones`**: Almacena las cabeceras de resúmenes financieros mensuales agrupados mediante la FK `num_cuenta`.

---

## 🚀 Tecnologías Utilizadas
* **Backend Core:** C# (.NET SDK) & PHP 8
* **Frontend:** HTML5, JavaScript Moderno (Async/Await, Fetch API, Array.map)
* **Estilos:** Tailwind CSS v4 (vía CDN)
* **Base de Datos y Servidor:** MySQL / MariaDB & Apache (Servidor mediante **XAMPP**)
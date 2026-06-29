document.addEventListener("DOMContentLoaded", async () => {
  try {
    //Pedir los datos al backend de PHP
    const response = await fetch("resumen.php");

    //Si el backend responde que no está logueado (401), lo sacamos de acá
    if (response.status === 401) {
      window.location.href = "ingreso.html";
      return;
    }

    const data = await response.json();

    //Inyectar datos del Cliente y Tarjeta en los IDs del HTML
    document.getElementById("nombre-cliente").textContent = data.nombre_cliente;
    document.getElementById("nombre-titular").textContent = data.nombre_cliente;
    document.getElementById("dni-titular").textContent = data.dni;

    if (data.tarjeta) {
      document.getElementById("banco-emisor").textContent = data.tarjeta.banco;
      document.getElementById("numero-tarjeta").textContent =
        `XXXX XXXX XXXX ${data.tarjeta.ultimos_cuatro}`;
      document.getElementById("estado-tarjeta").textContent =
        data.tarjeta.estado;
      document.getElementById("saldo-tarjeta").textContent =
        new Intl.NumberFormat("es-AR", {
          style: "currency",
          currency: "ARS",
        }).format(data.tarjeta.saldo);
    }

    //Renderizar las filas de la tabla de liquidaciones usando .map()
    const tabla = document.getElementById("tabla-liquidaciones");

    if (data.liquidaciones.length === 0) {
      tabla.innerHTML = `<tr><td colspan="4" class="px-6 py-8 text-center text-gray-400">No hay liquidaciones cargadas.</td></tr>`;
      return;
    }

    const filasHTML = data.liquidaciones
      .map((liq) => {
        //Formatear fechas y monedas de forma local en pesos argentinos
        const fechaFormateada = new Date(
          liq.fecha_vencimiento + "T00:00:00",
        ).toLocaleDateString("es-AR");
        const min = new Intl.NumberFormat("es-AR", {
          style: "currency",
          currency: "ARS",
        }).format(liq.pago_minimo);
        const total = new Intl.NumberFormat("es-AR", {
          style: "currency",
          currency: "ARS",
        }).format(liq.total_a_pagar);

        return `
                <tr class="hover:bg-gray-50 transition">
                    <td class="px-6 py-4 font-mono font-bold">${liq.periodo}</td>
                    <td class="px-6 py-4">${fechaFormateada}</td>
                    <td class="px-6 py-4 text-right font-mono text-gray-600">${min}</td>
                    <td class="px-6 py-4 text-right font-mono font-bold text-blue-600">${total}</td>
                </tr>
            `;
      })
      .join(""); // Unimos el array de strings en un solo bloque de texto HTML

    tabla.innerHTML = filasHTML;
  } catch (error) {
    console.error("Error al obtener el resumen financiero:", error);
  }
});

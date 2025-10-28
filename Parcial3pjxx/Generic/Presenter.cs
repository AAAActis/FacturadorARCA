using Parcial3pjxx.CodeFirst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial3pjxx.Generic
{
    public class Presenter
    {
        public void MostrarTitulo(string titulo)
        {
            string borde = "==================================================";
            string tituloCentrado = $"-- {titulo} --";

            Console.Clear(); // ¡Importante! Limpia la pantalla
            Console.WriteLine(borde);
            // Centramos el título (asumiendo un ancho de 50)
            int espacios = (50 - tituloCentrado.Length) / 2;
            Console.WriteLine(tituloCentrado.PadLeft(espacios + tituloCentrado.Length));
            Console.WriteLine(borde);
            Console.WriteLine(); // Espacio extra
        }
        public void MostrarMenuPrincipal()
        {
            MostrarTitulo("Bienvenidos al Facturador WEB de ARCA");
            Console.WriteLine("   1. Gestionar Clientes");
            Console.WriteLine("   2. Gestionar Facturas");
            Console.WriteLine();
            Console.WriteLine("   3. Salir");
            Console.WriteLine("==================================================");
            Console.Write("   Que opcion desea ingresar? -> ");
        }

        public void MostrarGestionarClientes()
        {
            MostrarTitulo("Gestión de Clientes");
            Console.WriteLine("   1. Crear cliente");
            Console.WriteLine("   2. Modificar cliente");
            Console.WriteLine("   3. Eliminar cliente");
            Console.WriteLine("   4. Consultar cliente");
            Console.WriteLine();
            Console.WriteLine("   5. Volver al Menú Principal");
            Console.WriteLine("==================================================");
            Console.Write("   Que opcion desea ingresar? -> ");
        }

        public void MostrarGestionarFacturas()
        {
            MostrarTitulo("Gestión de Facturas");
            Console.WriteLine("   1. Emitir Factura");
            Console.WriteLine("   2. Consultar Factura");
            Console.WriteLine("   3. Listar Facturas");
            Console.WriteLine();
            Console.WriteLine("   4. Volver al Menú Principal");
            Console.WriteLine("==================================================");
            Console.Write("   Que opcion desea ingresar? -> ");
        }

        public void MostrarSubMenuListarFacturas()
        {
            MostrarTitulo("Listar Facturas");
            Console.WriteLine("   1. Listar todas las facturas");
            Console.WriteLine("   2. Listar facturas por cliente");
            Console.WriteLine();
            Console.WriteLine("   3. Volver al Menú de Gestión de Facturas");
            Console.WriteLine("==================================================");
            Console.Write("   Que opcion desea ingresar? -> ");
        }
        public void MostrarMensaje(string v)
        {
            Console.WriteLine(v);
        }

        public void MostrarMensajeAlLado(string v)
        {
            Console.Write(v);
        }

        public void MostrarCliente(Cliente cliente)
        {
            Console.WriteLine($"ID: {cliente.IdCliente}");
            Console.WriteLine($"CUIL/CUIT: {cliente.CuilCuit}");
            Console.WriteLine($"Razon Social: {cliente.RazonSocial}");
            Console.WriteLine($"Domicilio: {cliente.Domicilio}");
        }

        public void MostrarCrearCliente()
        {
            MostrarTitulo("Crear Nuevo Cliente");
        }

        public void MostrarModificarCliente()
        {
            MostrarTitulo("Modificar Cliente");
        }

        public void MostrarEliminarCliente()
        {
            MostrarTitulo("Eliminar Cliente");
        }

        public void MostrarConsultarCliente()
        {
            MostrarTitulo("Consultar Cliente");
        }

        public void MostrarClienteCreado(Cliente cliente)
        {
            Console.WriteLine();
            Console.WriteLine("==================================================");
            Console.WriteLine($"   ¡Cliente dado de alta con éxito!");
            Console.WriteLine($"   ID Asignado: {cliente.IdCliente}");
            Console.WriteLine("==================================================");
            Console.WriteLine();
            Console.Write("   Presione [Enter] para volver al menú...");
        }

        public void MostrarMenuModificarCliente(Cliente cliente)
        {
            // Usamos el método MostrarTitulo que ya creamos
            MostrarTitulo("Datos del Cliente a Modificar");

            // Mostramos los datos actuales del cliente
            Console.WriteLine($"   ID:           {cliente.IdCliente}");
            Console.WriteLine($"   Razón Social: {cliente.RazonSocial}");
            Console.WriteLine($"   CUIT/CUIL:    {cliente.CuilCuit}");
            Console.WriteLine($"   Domicilio:    {cliente.Domicilio}");
            Console.WriteLine();
            Console.WriteLine("   ¿Qué dato deseas cambiar?");
            Console.WriteLine("   1. Domicilio");
            Console.WriteLine("   2. Razón Social");
            Console.WriteLine("   3. CUIT/CUIL");
            Console.WriteLine("==================================================");
            Console.Write("   Que opcion desea ingresar? -> ");
        }

        public void MostrarConsultaCliente(Cliente cliente)
        {
            // Usamos el método MostrarTitulo
            MostrarTitulo("Consulta de Cliente");

            // Mostramos los datos actuales del cliente
            Console.WriteLine($"   ID:           {cliente.IdCliente}");
            Console.WriteLine($"   Razón Social: {cliente.RazonSocial}");
            Console.WriteLine($"   CUIT/CUIL:    {cliente.CuilCuit}");
            Console.WriteLine($"   Domicilio:    {cliente.Domicilio}");
            Console.WriteLine("==================================================");
            Console.WriteLine();
            Console.Write("   Presione [Enter] para volver al menú...");
        }

        public void MostrarConfirmacionEliminar(Cliente cliente)
        {
            // Usamos el método MostrarTitulo
            MostrarTitulo("Datos del Cliente a Eliminar");

            // Mostramos los datos actuales del cliente
            Console.WriteLine($"   ID:           {cliente.IdCliente}");
            Console.WriteLine($"   Razón Social: {cliente.RazonSocial}");
            Console.WriteLine($"   CUIT/CUIL:    {cliente.CuilCuit}");
            Console.WriteLine($"   Domicilio:    {cliente.Domicilio}");
            Console.WriteLine();
            Console.WriteLine("   !! ADVERTENCIA: Esta acción es permanente !!");
            Console.WriteLine("==================================================");
            Console.Write("   ¿Desea eliminar este cliente? (SI/NO) -> ");
        }

        public void MostrarFacturaSimple(Factura factura)
        {
            string id = $"ID: {factura.IdFactura}".PadRight(10);
            string Fecha = $"Fecha: {factura.Fecha:dd/MM/yyyy}".PadRight(20);

            string cliente = $"Cliente: (N/A)".PadRight(30);
            if (factura.Cliente != null)
            {
                cliente = $"Cliente: {factura.Cliente.RazonSocial}".PadRight(30);
            }

            string total = $"Total: $ {factura.Total:N2}";

            Console.WriteLine($"{id} {Fecha} {cliente} {total}");
        }

        public void MostrarListaFacturas(List<Factura> facturas)
        {

            if (facturas == null || !facturas.Any())
            {
                Console.WriteLine("No hay facturas para mostrar.");
            }
            else
            {
                foreach (var factura in facturas)
                {
                    MostrarFacturaSimple(factura);
                }
            }

            Console.WriteLine("==================================================");
            Console.WriteLine();
            Console.Write("   Presione [Enter] para volver al menú...");
        }

        
            
        public void MostrarFactura(Factura factura)
        {
            string separador = "---------------------------------------------";
            Console.WriteLine(separador);

            string numeroFormateado = $"0001-{factura.Numero.ToString("D8")}";

            Console.WriteLine($"Factura:       {factura.Tipo}");
            Console.WriteLine($"Numero:        {numeroFormateado}");
            Console.WriteLine($"Fecha:         {factura.Fecha.ToString("dd/MM/yyyy")}");
            Console.WriteLine(); // espacio

            if (factura.Cliente != null)
            {
                Console.WriteLine($"Razon Social:  {factura.Cliente.RazonSocial}");
                Console.WriteLine($"Cuil/Cuit:     {factura.Cliente.CuilCuit}");
                Console.WriteLine($"Domicilio:     {factura.Cliente.Domicilio}");
            }
            else
            {
                Console.WriteLine("Cliente: (Datos no cargados)");
            }

            Console.WriteLine(separador);
            Console.WriteLine(); //espacio

            // Mostramos los items
            if (factura.Items != null && factura.Items.Any())
            {
                // Ajustamos el encabezado para que coincida con los nuevos anchos
                Console.WriteLine("Cant.  Descripcion                  P. Unit.     Subtotal");
                foreach (var item in factura.Items)
                {
                    string cant = item.Cantidad.ToString().PadLeft(6);

                    string desc = item.Descripcion.Length > 25 ? item.Descripcion.Substring(0, 25) : item.Descripcion.PadRight(25);

                    string pUnit = $"$ {item.ImporteUnitario.ToString("N2")}".PadLeft(14); // "C" es formato Moneda

                    string sub = $"$ {item.SubTotal.ToString("N2")}".PadLeft(14);

                    Console.WriteLine($"{cant}  {desc} {pUnit} {sub}");
                }
            }
            else
            {
                Console.WriteLine("La factura no tiene items.");
            }

            Console.WriteLine(separador);

            Console.WriteLine($"TOTAL: $ {factura.Total.ToString("N2")}");
            Console.WriteLine(separador); 
        }
    }
}
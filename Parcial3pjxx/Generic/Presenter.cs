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
        public void MostrarMenuPrincipal()
        {
            // Esto es es metodo MostrarMenuPrincipal de Presenter
            Console.WriteLine(".::Bienvenidos al Facturador WEB de ARCA::.");
            Console.WriteLine("1. Gestionar Clientes");
            Console.WriteLine("2. Gestionar Facturas");
            Console.WriteLine("3. Salir");
            Console.WriteLine("Que opcion desea ingresar? ");
            //......
        }

        public void MostrarGestionarClientes()
        {
            Console.WriteLine(".::Gestion de Clientes::.");
            Console.WriteLine("1. Crear cliente");
            Console.WriteLine("2. Modificar cliente");
            Console.WriteLine("3. Eliminar cliente");
            Console.WriteLine("4. Consultar cliente");
            //en vez de salir del programa podriamos hacer que vuelva al menu principal o no?
            Console.WriteLine("5. Salir del Programa");
        }

        public void MostrarGestionarFacturas()
        {
            Console.WriteLine(".::Gestion de Facturas::.");
            Console.WriteLine("1. Emitir Factura");
            Console.WriteLine("2. Consultar Factura");
            //en vez de salir del programa podriamos hacer que vuelva al menu principal o no?
            Console.WriteLine("3. Salir del Programa");

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

                    string pUnit = item.ImporteUnitario.ToString("C").PadLeft(12); // "C" es formato Moneda

                    string sub = item.SubTotal.ToString("C").PadLeft(12);

                    Console.WriteLine($"{cant}  {desc} {pUnit} {sub}");
                }
            }
            else
            {
                Console.WriteLine("La factura no tiene items.");
            }

            Console.WriteLine(separador);

            Console.WriteLine($"TOTAL: {factura.Total.ToString("C")}");
            Console.WriteLine(separador); 
        }
    }
}
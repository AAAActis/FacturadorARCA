using Microsoft.EntityFrameworkCore;
using Parcial3pjxx.Generic;
using System;
using System.Linq;

namespace Parcial3pjxx.CodeFirst
{
    public class Manager
    {
        private readonly Presenter presenter;
        private readonly Reader reader;
        private readonly FacturadorDbContext context;

        public Manager(FacturadorDbContext _context, Presenter presenter, Reader reader)
        {
            context = _context;
            this.presenter = presenter;
            this.reader = reader;
        }

        public void Iniciar()
        {
            bool repetir = true;
            int opc = 0;

            do
            {
                Console.Clear();
                presenter.MostrarMenuPrincipal();
                try
                {
                    string input = Console.ReadLine();
                    if (!string.IsNullOrEmpty(input))
                        opc = int.Parse(input);
                    else
                        opc = -1;

                    switch (opc)
                    {
                        case 1: GestionarClientes(); break;
                        case 2: GestionarFacturas(); break;
                        case 3:
                            presenter.MostrarMensaje("Ha salido del programa!");
                            repetir = false;
                            System.Threading.Thread.Sleep(1000);
                            break;
                        default:
                            presenter.MostrarMensaje("\n   Opción inválida. Presione Enter para reintentar.");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    presenter.MostrarMensaje($"\n   Error: {ex.Message}. Presione Enter.");
                    Console.ReadKey();
                }

            } while (repetir);
        }

        // ------------------------------ LOGICA DE CLIENTES -------------------------------------
        public void GestionarClientes()
        {
            bool repetir = true;
            do
            {
                Console.Clear();
                presenter.MostrarGestionarClientes();
                int opc = 0;
                try
                {
                    string input = Console.ReadLine();
                    opc = !string.IsNullOrEmpty(input) ? int.Parse(input) : -1;

                    switch (opc)
                    {
                        case 1: CrearCliente(); break;
                        case 2: ModificarCliente(); break;
                        case 3: EliminarCliente(); break;
                        case 4: ConsultarCliente(); break;
                        case 5: repetir = false; break;
                        default:
                            presenter.MostrarMensaje("\n   Opción inválida.");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    presenter.MostrarMensaje($"\n   Error: {ex.Message}");
                    Console.ReadKey();
                }
            } while (repetir);
        }

        public void CrearCliente()
        {
            Console.Clear();
            presenter.MostrarCrearCliente();
            Cliente cte = new Cliente();

            presenter.MostrarMensajeAlLado("   Ingrese la razon social del cliente: ");
            cte.RazonSocial = reader.LeerCadena();

            presenter.MostrarMensajeAlLado("   Ingrese el CUIL / CUIT del cliente: ");
            cte.CuilCuit = reader.LeerLong();

            presenter.MostrarMensajeAlLado("   Ingrese el domicilio del cliente: ");
            cte.Domicilio = reader.LeerCadena();

            try
            {
                context.Clientes.Add(cte);
                context.SaveChanges();
                presenter.MostrarClienteCreado(cte);
                reader.LeerCadena();
            }
            catch (Exception ex)
            {
                presenter.MostrarMensaje($"Ocurrio un error: {ex.Message}");
                // En SQLite los errores de unicidad son diferentes a SQL Server
                if (ex.InnerException != null && ex.InnerException.Message.Contains("UNIQUE"))
                {
                    presenter.MostrarMensaje("Posible duplicado de clave única.");
                }
                presenter.MostrarMensaje("\n   Presione [Enter] para volver...");
                reader.LeerCadena();
            }
        }

        private void ModificarCliente()
        {
            Console.Clear();
            presenter.MostrarTitulo("Modificar Cliente");
            presenter.MostrarMensajeAlLado("Ingrese el ID del Cliente a Modificar: ");
            int id = reader.LeerEntero();

            var cte = context.Clientes.Find(id);
            if (cte == null)
            {
                presenter.MostrarMensaje("Cliente no encontrado!");
                Console.ReadKey();
                return;
            }

            presenter.MostrarMenuModificarCliente(cte);
            bool repetirModificacion = true;
            do
            {
                int opc = reader.LeerEntero();
                Console.Clear();
                switch (opc)
                {
                    case 1:
                        presenter.MostrarTitulo("Modificacion Domicilio");
                        presenter.MostrarMensaje($"Domicilio actual ({cte.Domicilio})");
                        presenter.MostrarMensajeAlLado($"Ingrese el nuevo: ");
                        cte.Domicilio = reader.LeerCadena();
                        break;
                    case 2:
                        presenter.MostrarTitulo("Modificacion Razon Social");
                        presenter.MostrarMensaje($"Razon social actual ({cte.RazonSocial})");
                        presenter.MostrarMensajeAlLado($"Ingrese el nuevo: ");
                        cte.RazonSocial = reader.LeerCadena();
                        break;
                    case 3:
                        presenter.MostrarTitulo("Modificacion CUIT/CUIL");
                        presenter.MostrarMensaje($"CUIT/CUIL actual ({cte.CuilCuit})");
                        presenter.MostrarMensajeAlLado($"Ingrese el nuevo: ");
                        cte.CuilCuit = reader.LeerLong();
                        break;
                }

                presenter.MostrarMensaje("Desea modificar algo mas? (SI/NO)");
                repetirModificacion = reader.LeerCadena().ToUpper().Trim() == "SI";
                Console.Clear();
                if (repetirModificacion) presenter.MostrarMenuModificarCliente(cte);

            } while (repetirModificacion);

            context.SaveChanges();
            presenter.MostrarMensaje("Cliente actualizado con exito!");
            Console.ReadKey();
        }

        private void EliminarCliente()
        {
            presenter.MostrarTitulo("Eliminar Cliente");
            presenter.MostrarMensajeAlLado("ID del cliente a eliminar: ");
            int id = reader.LeerEntero();
            var cte = context.Clientes.Include(c => c.Facturas).FirstOrDefault(c => c.IdCliente == id);

            if (cte == null)
            {
                presenter.MostrarMensaje("Cliente no encontrado!");
                reader.LeerCadena();
                return;
            }

            if (cte.Facturas != null && cte.Facturas.Any())
            {
                presenter.MostrarMensaje("No se puede eliminar el cliente porque tiene facturas asociadas.");
                reader.LeerCadena();
                return;
            }

            presenter.MostrarConfirmacionEliminar(cte);
            if (reader.LeerCadena().ToUpper().Trim() == "SI")
            {
                context.Clientes.Remove(cte);
                context.SaveChanges();
                presenter.MostrarMensaje("¡Cliente eliminado correctamente!");
            }
            else
            {
                presenter.MostrarMensaje("Operación cancelada.");
            }
            reader.LeerCadena();
        }

        private void ConsultarCliente()
        {
            presenter.MostrarTitulo("Consultar Cliente");
            presenter.MostrarMensajeAlLado("ID del cliente a consultar: ");
            int id = reader.LeerEntero();
            var cte = context.Clientes.Find(id);
            if (cte == null)
            {
                presenter.MostrarMensaje("Cliente no encontrado!");
                reader.LeerCadena();
                return;
            }
            presenter.MostrarConsultaCliente(cte);
            reader.LeerCadena();
        }

        // ------------------------------ LOGICA DE FACTURAS -------------------------------------
        public void GestionarFacturas()
        {
            bool repetir = true;
            do
            {
                Console.Clear();
                presenter.MostrarGestionarFacturas();
                int opc = 0;
                try
                {
                    string input = Console.ReadLine();
                    opc = !string.IsNullOrEmpty(input) ? int.Parse(input) : -1;

                    switch (opc)
                    {
                        case 1: EmitirFactura(); break;
                        case 2: ConsultarFactura(); break;
                        case 3: MenuListarFacturas(); break;
                        case 4: repetir = false; break;
                        default:
                            presenter.MostrarMensaje("\n   Opción inválida.");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    presenter.MostrarMensaje($"\n   Error: {ex.Message}");
                    Console.ReadKey();
                }
            } while (repetir);
        }

        public void MenuListarFacturas()
        {
            bool repetir = true;
            while (repetir)
            {
                Console.Clear();
                presenter.MostrarSubMenuListarFacturas();
                int opc = 0;
                try
                {
                    string input = Console.ReadLine();
                    opc = !string.IsNullOrEmpty(input) ? int.Parse(input) : -1;
                    Console.Clear();
                    switch (opc)
                    {
                        case 1: ListarTodasFacturas(); break;
                        case 2: ListarFacturasPorCliente(); break;
                        case 3: repetir = false; break;
                        default:
                            presenter.MostrarMensaje("Opción inválida.");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception) { }
            }
        }

        private void ListarTodasFacturas()
        {
            presenter.MostrarTitulo("Listado de Todas las Facturas");
            var facturas = context.Facturas.Include(f => f.Cliente).OrderByDescending(f => f.Fecha).ToList();
            presenter.MostrarListaFacturas(facturas);
            Console.WriteLine("\nPresione Enter para continuar...");
            reader.LeerCadena();
        }

        private void ListarFacturasPorCliente()
        {
            presenter.MostrarTitulo("Listado de Facturas por Cliente");
            presenter.MostrarMensajeAlLado("Ingrese el ID del Cliente: ");
            int idCliente = reader.LeerEntero();

            var cliente = context.Clientes.Find(idCliente);
            if (cliente == null)
            {
                presenter.MostrarMensaje("Cliente no encontrado!");
                reader.LeerCadena();
                return;
            }

            presenter.MostrarTitulo($"Facturas de: {cliente.RazonSocial}");
            var facturas = context.Facturas
                .Where(f => f.IdCliente == idCliente)
                .Include(f => f.Cliente)
                .Include(f => f.Items)
                .OrderByDescending(f => f.Fecha)
                .ToList();

            if (facturas.Count == 0)
                presenter.MostrarMensaje("No se encontraron facturas.");
            else
                presenter.MostrarListaFacturas(facturas);

            Console.WriteLine("\nPresione Enter para continuar...");
            reader.LeerCadena();
        }

        public void EmitirFactura()
        {
            Console.Clear();
            presenter.MostrarTitulo("Emitir Factura");

            if (!context.Clientes.Any())
            {
                presenter.MostrarMensaje("\n   ERROR: No hay clientes registrados.");
                reader.LeerCadena();
                return;
            }

            presenter.MostrarMensajeAlLado("Tipo de Factura a Emitir (A / B / C): ");
            char tipo = reader.LeerChar();
            presenter.MostrarMensaje("");
            presenter.MostrarMensajeAlLado("Cliente (Razón Social): ");
            string razon = reader.LeerCadena();

            var cte = context.Clientes.FirstOrDefault(c => c.RazonSocial == razon);
            if (cte == null)
            {
                presenter.MostrarMensaje("Cliente no encontrado.");
                Console.ReadKey();
                return;
            }

            // Calculo de numero para SQLite (funciona igual)
            int ultimoNumero = 0;
            if (context.Facturas.Any(f => f.Tipo == tipo))
            {
                ultimoNumero = context.Facturas.Where(f => f.Tipo == tipo).Max(f => f.Numero);
            }

            Factura factura = new Factura
            {
                Tipo = tipo,
                Numero = ultimoNumero + 1,
                Fecha = DateTime.Now,
                IdCliente = cte.IdCliente,
                Cliente = cte
            };

            presenter.MostrarFactura(factura);
            Console.ReadKey();
            Console.Clear();

            bool seguirAgregando = true;
            do
            {
                presenter.MostrarTitulo(".::Ingreso de Items::.");
                Item nuevoItem = SolicitarItem();
                factura.Items.Add(nuevoItem);

                presenter.MostrarMensaje("Desea ingresar otro item? (SI/NO)");
                string opc = reader.LeerCadena().ToUpper().Trim();
                seguirAgregando = (opc == "SI");
                Console.Clear();
            } while (seguirAgregando);

            factura.ImporteTotal = factura.Total;
            Console.Clear();
            presenter.MostrarMensaje("--- VISTA PREVIA ---");
            presenter.MostrarFactura(factura);

            presenter.MostrarMensaje("Desea emitir la factura? (Si / NO):");
            if (reader.LeerCadena().ToUpper().Trim() == "SI")
            {
                try
                {
                    context.Facturas.Add(factura);
                    context.SaveChanges();
                    presenter.MostrarMensaje("¡Factura emitida con éxito!");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    presenter.MostrarMensaje($"Error al guardar: {ex.Message}");
                    Console.ReadKey();
                }
            }
            else
            {
                presenter.MostrarMensaje("Operación cancelada.");
                Console.ReadKey();
            }
        }

        public void ConsultarFactura()
        {
            Console.Clear();
            presenter.MostrarTitulo("Consulta Factura");
            presenter.MostrarMensajeAlLado("ID de la Factura: ");
            int id = reader.LeerEntero();

            var factura = context.Facturas
                 .Include(f => f.Cliente)
                 .Include(f => f.Items)
                 .FirstOrDefault(f => f.IdFactura == id);

            if (factura == null)
            {
                presenter.MostrarMensaje("Factura no encontrada!");
                Console.ReadKey();
                return;
            }
            presenter.MostrarFactura(factura);
            Console.ReadKey();
        }

        public Item SolicitarItem()
        {
            presenter.MostrarMensajeAlLado("Descripcion: ");
            string desp = reader.LeerCadena();
            Console.Write("\n");
            presenter.MostrarMensajeAlLado("Cantidad: ");
            int cant = reader.LeerEntero();
            Console.Write("\n");
            presenter.MostrarMensajeAlLado("Importe: ");
            decimal imp = reader.LeerDecimal();
            Console.Write("\n");

            return new Item { Descripcion = desp, Cantidad = cant, ImporteUnitario = imp };
        }
    }
}
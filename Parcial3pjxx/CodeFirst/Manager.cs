using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Parcial3pjxx.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    {
                        opc = int.Parse(input);
                    }
                    else
                    {
                        opc = -1;
                    }

                    switch (opc)
                    {
                        case 1:
                            GestionarClientes();
                            break;
                        case 2:
                            GestionarFacturas();
                            break;
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
                catch (FormatException)
                {
                    presenter.MostrarMensaje("\n   Entrada no válida (no es un número). Presione Enter para reintentar.");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    presenter.MostrarMensaje($"\n   Error inesperado: {ex.Message}. Presione Enter.");
                    Console.ReadKey();
                }

            }
            while (repetir);
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
                    if (!string.IsNullOrEmpty(input))
                    {
                        opc = int.Parse(input);
                    }
                    else
                    {
                        opc = -1;
                    }

                    switch (opc)
                    {
                        case 1: CrearCliente(); break;
                        case 2: ModificarCliente(); break;
                        case 3: EliminarCliente(); break;
                        case 4: ConsultarCliente(); break;
                        case 5:
                            repetir = false;
                            break;
                        default:
                            presenter.MostrarMensaje("\n   Opción inválida. Presione Enter para reintentar.");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (FormatException)
                {
                    presenter.MostrarMensaje("\n   Entrada no válida (no es un número). Presione Enter para reintentar.");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    presenter.MostrarMensaje($"\n   Error inesperado: {ex.Message}. Presione Enter.");
                    Console.ReadKey();
                }
            } while (repetir);
        }


        //METODOS DE CLIENTE

        public void CrearCliente()
        {
            Console.Clear();
            presenter.MostrarCrearCliente();

            Cliente cte = new Cliente();

            presenter.MostrarMensajeAlLado("   Ingrese la razon social del cliente: ");
            cte.RazonSocial = reader.LeerCadena();

            presenter.MostrarMensajeAlLado("   Ingrese el CUIL / CUIT del cliente: ");
            cte.CuilCuit = reader.LeerCadena();

            presenter.MostrarMensajeAlLado("   Ingrese el domicilio del cliente: ");
            cte.Domicilio = reader.LeerCadena();

            try
            {
                context.Clientes.Add(cte);
                context.SaveChanges();

                presenter.MostrarClienteCreado(cte);
                reader.LeerCadena();
            }
            catch (DbUpdateException dbEx)
            {
                string mensajeError = dbEx.InnerException?.Message ?? dbEx.Message;

                if (mensajeError.Contains("UNIQUE constraint") || mensajeError.Contains("duplicate key"))
                {
                    presenter.MostrarMensaje("Error: Ya existe un cliente con ese CUIL / CUIT.");
                }
                else
                {
                    presenter.MostrarMensaje($"Error de base de datos: {mensajeError}");
                }
                presenter.MostrarMensaje("\n   Presione [Enter] para volver al menú...");
                reader.LeerCadena();
            }
            catch (Exception ex)
            {
                presenter.MostrarMensaje($"Ocurrio un error inesperado: {ex.Message}");
                presenter.MostrarMensaje("\n   Presione [Enter] para volver al menú...");
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
                return;
            }
            presenter.MostrarMenuModificarCliente(cte);
            if (cte == null)
            {
                presenter.MostrarMensaje("Cliente no encontrado!");
                Console.WriteLine($"Pulse ENTER para continuar");
                Console.ReadKey();
                return;
            }

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
                        string dom = reader.LeerCadena();
                        cte.Domicilio = dom;
                        break;
                    case 2:
                        presenter.MostrarTitulo("Modificacion Razon Social");
                        presenter.MostrarMensaje($"Razon social actual ({cte.RazonSocial})");
                        presenter.MostrarMensajeAlLado($"Ingrese el nuevo: ");
                        string RazSoc = reader.LeerCadena();
                        cte.RazonSocial = RazSoc;
                        break;
                    case 3:
                        presenter.MostrarTitulo("Modificacion CUIT/CUIL");
                        presenter.MostrarMensaje($"CUIT/CUIL actual ({cte.CuilCuit})");
                        presenter.MostrarMensajeAlLado($"Ingrese el nuevo: ");
                        string cuil = reader.LeerCadena();
                        cte.CuilCuit = cuil;
                        break;
                }

                presenter.MostrarMensaje("Desea modificar algo mas? (SI/NO)");
                string repetir = reader.LeerCadena().ToUpper().Trim();

                if (repetir == "SI")
                {
                    repetirModificacion = true;
                    Console.Clear();
                }
                else
                {
                    repetirModificacion = false;
                    Console.Clear();
                }

            } while (repetirModificacion);

            context.SaveChanges();
            presenter.MostrarMensaje("Cliente actualizado con exito!");
            Console.WriteLine($"Pulse ENTER para continuar");
            Console.ReadKey();
        }
        private void EliminarCliente()
        {
            presenter.MostrarTitulo("Eliminar Cliente");
            presenter.MostrarMensajeAlLado("ID del cliente a eliminar: ");
            int id = reader.LeerEntero();
            var cte = context.Clientes.Find(id);
            if (cte == null)
            {
                presenter.MostrarMensaje("Cliente no encontrado!");
                reader.LeerCadena();
                return;
            }

            presenter.MostrarConfirmacionEliminar(cte);
            string confirmacion = reader.LeerCadena().ToUpper().Trim();

            if (confirmacion == "SI")
            {
                try
                {
                    context.Clientes.Remove(cte);
                    context.SaveChanges();
                    presenter.MostrarMensaje("¡Cliente eliminado correctamente!");
                }
                catch (Exception ex)
                {
                    presenter.MostrarMensaje($"Error al eliminar: {ex.Message}");
                }
            }
            else
            {
                presenter.MostrarMensaje("Operación cancelada.");
            }
            presenter.MostrarMensaje("\nPresione [Enter] para volver al menú...");
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
                presenter.MostrarMensaje("\nPresione [Enter] para volver al menú...");
                reader.LeerCadena();
                return;
            }
            presenter.MostrarConsultaCliente(cte);
            reader.LeerCadena();
        }



        //----------------------------------- LOGICA DE FACTURA -------------------------------------
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
                    if (!string.IsNullOrEmpty(input))
                    {
                        opc = int.Parse(input);
                    }
                    else
                    {
                        opc = -1;
                    }

                    switch (opc)
                    {
                        case 1: EmitirFactura(); break;
                        case 2: ConsultarFactura(); break;
                        case 3: MenuListarFacturas(); break;
                        case 4:
                            repetir = false;
                            break;
                        default:
                            presenter.MostrarMensaje("\n   Opción inválida. Presione Enter para reintentar.");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (FormatException)
                {
                    presenter.MostrarMensaje("\n   Entrada no válida (no es un número). Presione Enter para reintentar.");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    presenter.MostrarMensaje($"\n   Error inesperado: {ex.Message}. Presione Enter.");
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
                    if (!string.IsNullOrEmpty(input))
                    {
                        opc = int.Parse(input);
                    }
                    else
                    {
                        opc = -1;
                    }

                    Console.Clear();

                    switch (opc)
                    {
                        case 1:
                            ListarTodasFacturas();
                            break;
                        case 2:
                            ListarFacturasPorCliente();
                            break;
                        case 3:
                            repetir = false;
                            break;
                        default:
                            presenter.MostrarMensaje("Opción inválida. Intente nuevamente.");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (FormatException)
                {
                    presenter.MostrarMensaje("\n   Entrada no válida (no es un número). Presione Enter para reintentar.");
                    Console.ReadKey();
                }
            }
        }

        private void ListarTodasFacturas()
        {
            presenter.MostrarTitulo("Listado de Todas las Facturas");
            var facturas = context.Facturas
                .Include(f => f.Cliente)
                .OrderByDescending(f => f.Fecha)
                .ToList();

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
                Console.WriteLine("\nPresione Enter para continuar...");
                reader.LeerCadena();
                return;
            }

            presenter.MostrarTitulo($"Facturas de: {cliente.RazonSocial} (ID: {cliente.IdCliente})");

            var facturas = context.Facturas
                .Where(f => f.IdCliente == idCliente)
                .Include(f => f.Cliente)
                .Include(f => f.Items)
                .OrderByDescending(f => f.Fecha)
                .ToList();

            if (facturas.Count == 0)
            {
                presenter.MostrarMensaje("No se encontraron facturas para el cliente especificado.");
            }
            else
            {
                presenter.MostrarListaFacturas(facturas);
            }

            Console.WriteLine("\nPresione Enter para continuar...");
            reader.LeerCadena();
        }

        //METODOS FACTURA
        public void EmitirFactura()
        {

            Console.Clear();
            presenter.MostrarTitulo("Emitir Factura");

            if (!context.Clientes.Any())
            {
                presenter.MostrarMensaje("\n   ERROR: No hay clientes registrados.");
                presenter.MostrarMensaje("   Debe crear un cliente antes de poder emitir una factura.");
                presenter.MostrarMensaje("\n   Presione [Enter] para volver al menú...");
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
                presenter.MostrarMensaje("Cliente no encontrado. Asegurese de registrarlo antes!");
                Console.WriteLine($"Pulse ENTER para continuar");
                Console.ReadKey();
                return;
            }

            int ultimoNumero = context.Facturas
                .Where(f => f.Tipo == tipo)
                .OrderByDescending(f => f.Numero)
                .Select(f => f.Numero)
                .FirstOrDefault();

            Factura factura = new Factura
            {
                Tipo = tipo,
                Numero = ultimoNumero + 1,
                Fecha = DateTime.Now,
                IdCliente = cte.IdCliente,
                Cliente = cte
            };

            presenter.MostrarFactura(factura);
            presenter.MostrarMensaje("\n");
            Console.WriteLine($"Pulse ENTER para continuar");
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
            presenter.MostrarMensaje("--- VISTA PREVIA DE LA FACTURA ---");
            presenter.MostrarFactura(factura);

            presenter.MostrarMensaje("Desea emitir la factura? (Si / NO):");
            string confirmar = reader.LeerCadena().ToUpper().Trim();

            if (confirmar == "SI")
            {
                try
                {
                    context.Facturas.Add(factura);
                    context.SaveChanges();
                    presenter.MostrarMensaje("¡Factura emitida con éxito!");
                    Console.WriteLine($"Pulse ENTER para continuar");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    presenter.MostrarMensaje($"Ocurrió un error al guardar: {ex.Message}");
                    Console.WriteLine($"Pulse ENTER para continuar");
                    Console.ReadKey();
                }
            }
            else
            {
                presenter.MostrarMensaje("Operación cancelada.");
                Console.WriteLine($"Pulse ENTER para continuar");
                Console.ReadKey();
            }
        }
        public void ConsultarFactura()
        {
            Console.Clear();
            presenter.MostrarTitulo("Consulta Factura");
            presenter.MostrarMensajeAlLado("ID de la Factura a Consultar: ");
            int id = reader.LeerEntero();

            var factura = context.Facturas
                 .Include(f => f.Cliente)
                 .Include(f => f.Items)
                 .FirstOrDefault(f => f.IdFactura == id);
            if (factura == null)
            {
                presenter.MostrarMensaje("Factura no encontrada!");
                Console.WriteLine($"Pulse ENTER para continuar");
                Console.ReadKey();
                return;
            }
            presenter.MostrarFactura(factura);
            Console.WriteLine($"Pulse ENTER para continuar");
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

            Item item = new Item
            {
                Descripcion = desp,
                Cantidad = cant,
                ImporteUnitario = imp
            };

            return item;
        }
    }
}
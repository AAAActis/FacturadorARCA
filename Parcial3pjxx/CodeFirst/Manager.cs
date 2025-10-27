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
        private readonly IServiceProvider _serviceProvider;
        public Manager(IServiceProvider serviceProvider, Presenter presenter, Reader reader)
        {
            _serviceProvider = serviceProvider;
            this.presenter = presenter;
            this.reader = reader;
        }

        public void Iniciar()
        {
            bool band = true;
            bool repetir = true;
            int opc = 0;

            do
            {
                Console.Clear();
                presenter.MostrarMenuPrincipal();
                opc = reader.LeerEntero();

                do
                {
                    try
                    {
                        opc = int.Parse(Console.ReadLine());
                        if (opc > 3 || opc < 1)
                        {
                            band = true;
                            Console.WriteLine("Opcion mal ingresada");
                        }
                        else { band = false; }
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }

                } while (band);


                switch (opc)
                {
                    case 1: GestionarClientes(); break;
                    case 2: GestionarFacturas(); break;
                    case 3:
                        presenter.MostrarMensaje("Ha salido del programa");
                        repetir = false;
                        break;
                    default:
                        presenter.MostrarMensaje("Opción inválida. Intente nuevamente.");
                        break;
                }
            }
            while (repetir);
        }

        // ------------------------------ LOGICA DE CLIENTES -------------------------------------
        public void GestionarClientes()
        {

            bool band = true;
            int opc = 0;

            do
            {
                try
                {
                    opc = int.Parse(Console.ReadLine());
                    if (opc > 4 || opc < 1)
                    {
                        band = true;
                        Console.WriteLine("Opcion mal ingresada");
                    }
                    else { band = false; }
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }

            } while (band);
            switch (opc)
            {
                case 1: CrearCliente(); break;
                case 2: ModificarCliente(); break;
                case 3: EliminarCliente(); break;
                case 4: ConsultarCliente(); break;
                case 5: band = false; break;
                default:
                    presenter.MostrarMensaje("Opcion invalida.");
                    break;
            }

        }


        //METODOS DE CLIENTE
        //pude hacer todos los metdos, queda fijarse si esta bien
        public void CrearCliente()
        {
            using (var context = _serviceProvider.GetRequiredService<FacturadorDbContext>())
            {
                Cliente cte = new Cliente();
                presenter.MostrarMensaje("Ingrese la razon social del cliente");
                cte.RazonSocial = reader.LeerCadena();

                presenter.MostrarMensaje("Ingrese el CUIL / CUIT del cliente");
                cte.CuilCuit = reader.LeerCadena();

                presenter.MostrarMensaje("Ingrese el domicilio del cliente");
                cte.Domicilio = reader.LeerCadena();

                try
                {
                    context.Clientes.Add(cte);
                    context.SaveChanges();
                    presenter.MostrarMensaje("El cliente fue dado de alta con exito");
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
                }
                catch (Exception ex)
                {
                    presenter.MostrarMensaje($"Ocurrio un error inesperado: {ex.Message}");
                }
            }
        }

        private void ModificarCliente()
        {
            using (var context = _serviceProvider.GetRequiredService<FacturadorDbContext>())
            {
                presenter.MostrarMensaje("Ingrese el ID del Cliente a Modificar");
                int id = reader.LeerEntero();
                var cte = context.Clientes.Find(id);
                if (cte == null)
                {
                    presenter.MostrarMensaje("Cliente no encontrado!");
                    return;
                }

                presenter.MostrarMensaje("Que desea cambiar?");
                presenter.MostrarMensaje("1. Domicilio");
                presenter.MostrarMensaje("2. Razon Social");
                presenter.MostrarMensaje("3. CUIT/CUIL");

                int opc = reader.LeerEntero();

                switch (opc)
                {
                    case 1:
                        presenter.MostrarMensaje($"Domicilio actual ({cte.Domicilio}):");
                        presenter.MostrarMensaje($"Ingrese el nuevo: ");
                        string dom = reader.LeerCadena();
                        cte.Domicilio = dom;
                        break;
                    case 2:
                        presenter.MostrarMensaje($"Razon social actual ({cte.RazonSocial}):");
                        presenter.MostrarMensaje($"Ingrese el nuevo: ");
                        string RazSoc = reader.LeerCadena();
                        cte.RazonSocial = RazSoc;
                        break;
                    case 3:
                        presenter.MostrarMensaje($"CUIT/CUIL actual ({cte.CuilCuit}):");
                        presenter.MostrarMensaje($"Ingrese el nuevo: ");
                        string cuil = reader.LeerCadena();
                        cte.Domicilio = cuil;
                        break;
                }

                context.SaveChanges();
                presenter.MostrarMensaje("Cliente actualizado con exito!");
            }

        }
        private void EliminarCliente()
        {
            using (var context = _serviceProvider.GetRequiredService<FacturadorDbContext>())
            {
                presenter.MostrarMensaje("ID del cliente a eliminar:");
                int id = reader.LeerEntero();
                var cte = context.Clientes.Find(id);
                if (cte == null)
                {
                    presenter.MostrarMensaje("Cliente no encontrado!");
                    return;
                }

                context.Clientes.Remove(cte);
                context.SaveChanges();
                presenter.MostrarMensaje("Cliente eliminado correctamente!");
            }
        }
        private void ConsultarCliente()
        {
            using (var context = _serviceProvider.GetRequiredService<FacturadorDbContext>())
            {
                presenter.MostrarMensaje("ID del cliente a consultar:");
                int id = reader.LeerEntero();
                var cte = context.Clientes.Find(id);
                if (cte == null)
                {
                    presenter.MostrarMensaje("Cliente no encontrado!");
                    return;
                }
                presenter.MostrarCliente(cte);
            }
        }


        //----------------------------------- LOGICA DE FACTURA -------------------------------------
        //aca literal copie y pegue lo que hiciste en lo de gestionar cliente -- si pq no servis ni para bosta por eso
        public void GestionarFacturas()
        {

            bool band = true;
            int opc = 0;

            do
            {
                try
                {
                    opc = int.Parse(Console.ReadLine());
                    if (opc > 3 || opc < 1)
                    {
                        band = true;
                        Console.WriteLine("Opcion mal ingresada");
                    }
                    else { band = false; }
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }

            } while (band);
            switch (opc)
            {
                case 1: EmitirFactura(); break;
                case 2: ConsultarFactura(); break;
                case 3: Console.WriteLine("Ha salido del programa"); break;
            }

        }

        //METODOS FACTURA
        public void EmitirFactura()
        {
            using (var context = _serviceProvider.GetRequiredService<FacturadorDbContext>())
            {
                presenter.MostrarMensajeAlLado("Tipo de Factura a Emitir (A / B / C): ");
                char tipo = reader.LeerChar();
                presenter.MostrarMensaje("");
                presenter.MostrarMensajeAlLado("Cliente: ");
                string razon = reader.LeerCadena();

                //esto me lo tiro chatgepetaje JUJU
                var cte = context.Clientes.FirstOrDefault(c => c.RazonSocial == razon);
                if (cte == null)
                {
                    presenter.MostrarMensaje("Cliente no encontrado. Asegurese de registrarlo antes!");
                    return;
                }

                // Obtener ultimo numero para ese tipo
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

                bool seguirAgregando = true;
                do
                {
                    Item nuevoItem = SolicitarItem();
                    factura.Items.Add(nuevoItem);

                    presenter.MostrarMensaje("Desea ingresar otro item? (SI/NO)");
                    string opc = reader.LeerCadena().ToUpper().Trim();
                    seguirAgregando = (opc == "SI");

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
                        // EF es suficientemente inteligente para guardar la factura
                        // y todos sus 'Items' asociados en una sola transacción.
                        context.Facturas.Add(factura);
                        context.SaveChanges();
                        presenter.MostrarMensaje("¡Factura emitida con éxito!");
                    }
                    catch (Exception ex)
                    {
                        presenter.MostrarMensaje($"Ocurrió un error al guardar: {ex.Message}");
                    }
                }
                else
                {
                    presenter.MostrarMensaje("Operación cancelada.");
                }
            }
        }
        public void ConsultarFactura()
        {
            using (var context = _serviceProvider.GetRequiredService<FacturadorDbContext>())
            {
                presenter.MostrarMensaje("ID de la Factura a Consultar:");
                int id = reader.LeerEntero();

                //esto me lo tiro el chatgepetaje
                var factura = context.Facturas
                     .Include(f => f.Cliente)
                     .Include(f => f.Items)
                     .FirstOrDefault(f => f.IdFactura == id);
                if (factura == null)
                {
                    presenter.MostrarMensaje("Factura no encontrada!");
                    return;
                }
                presenter.MostrarFactura(factura);
            }
        }

        public Item SolicitarItem()
        {
            presenter.MostrarMensajeAlLado("Descripcion:");
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

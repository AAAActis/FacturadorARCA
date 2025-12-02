using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Parcial3pjxx.Generic
{
    public class Reader
    {
       
        public int LeerEntero()
        {
            int i = 0;
            bool band;
            do
            {      
               try
                {
                    i = int.Parse(Console.ReadLine());
                    band = false;

                }
                catch (Exception e) { Console.WriteLine("Asegurese de ingresar una opcion correcta!"); band = true; }
            } while (band);
            return i;
        }

        public long LeerLong()
        {
            long l = 0;
            bool band;
            do
            {
                try
                {
                    l = long.Parse(Console.ReadLine());
                    band = false;

                }
                catch (Exception e) { Console.WriteLine("Asegurese de ingresar una opcion correcta!"); band = true; }
            } while (band);
            return l;
        }
        public string LeerCadena()
        {
            string str = "";
            bool band;
            do
            {
                
                try
                {
                    str = Console.ReadLine();
                    band = false;

                }
                catch (Exception e) { Console.WriteLine("Asegurese de ingresar una opcion correcta!"); band = true; }            
            } while (band);
            return str;
        }
        public float LeerFloat()
        {
            float f = 0.0f;
            bool band;
            do
            {
                try
                {
                    f = float.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                    band = false;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error: \"Asegurese de ingresar una opcion correcta!\". Intente de nuevo.");
                    band = true;
                }
            } while (band);
            return f;
        }

        public decimal LeerDecimal()
        {
            decimal d = 0.0m;
            bool band;
            do
            {
                try
                {
                    // Usamos CultureInfo.InvariantCulture para asegurar que el punto decimal sea '.'
                    d = decimal.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                    band = false;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error: \"Asegurese de ingresar una opcion correcta!\". Intente de nuevo.");
                    band = true;
                }
            } while (band);
            return d;
        }

        public char LeerChar()
        {
            char c = ' ';
            bool band;
            do
            {  
                try
                {
                    c = char.Parse(Console.ReadLine());
                    band = false;
                }
                catch (Exception e) { Console.WriteLine("Asegurese de ingresar una opcion correcta!"); band = true; }    
            } while (band);
            return c;
        }

        public DateTime LeerFecha(string formato = "dd/MM/yyyy")
        {
            DateTime fecha = new DateTime();
            bool band = true;

            do
            {
                Console.WriteLine($"Ingrese una fecha (formato: {formato}):");
                string input = Console.ReadLine();

                try
                {
                    fecha = DateTime.ParseExact(input, formato, CultureInfo.InvariantCulture);
                    band = false;
                }
                catch (FormatException)
                {
                    Console.WriteLine($"Error: El formato no es correcto. Debe ser {formato}.");
                    band = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error: \"Asegurese de ingresar una opcion correcta!\"");
                    band = true;
                }

            } while (band);

            return fecha;
        }

    }
}
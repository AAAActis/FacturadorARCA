using System;
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
                catch (Exception) { Console.WriteLine("Asegurese de ingresar una opcion correcta!"); band = true; }
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
                catch (Exception) { Console.WriteLine("Asegurese de ingresar una opcion correcta!"); band = true; }
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
                catch (Exception) { Console.WriteLine("Asegurese de ingresar una opcion correcta!"); band = true; }
            } while (band);
            return str;
        }

        public decimal LeerDecimal()
        {
            decimal d = 0.0m;
            bool band;
            do
            {
                try
                {
                    d = decimal.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                    band = false;
                }
                catch (Exception)
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
                    string input = Console.ReadLine();
                    if (!string.IsNullOrEmpty(input))
                    {
                        c = input[0];
                        band = false;
                    }
                    else
                    {
                        Console.WriteLine("No ingreso nada.");
                        band = true;
                    }
                }
                catch (Exception) { Console.WriteLine("Asegurese de ingresar una opcion correcta!"); band = true; }
            } while (band);
            return c;
        }
    }
}
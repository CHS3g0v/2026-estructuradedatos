using System;

namespace RecursividadEjemplo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Iniciando cuenta regresiva:");
            ContarRegresiva(5);

            Console.WriteLine("\nPresiona cualquier tecla para salir...");
            Console.ReadKey();
        }

        // Método recursivo para cuenta regresiva
        static void ContarRegresiva(int n)
        {
            // 1. Caso base: condición que detiene la recursión
            if (n <= 0)
            {
                Console.WriteLine("¡Despegue!");
                return;
            }

            // 2. Acción actual
            Console.WriteLine($"Número: {n}");

            // 3. Llamada recursiva con un valor menor
            ContarRegresiva(n - 1);
        }
    }
}
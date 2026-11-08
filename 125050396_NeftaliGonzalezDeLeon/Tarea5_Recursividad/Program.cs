using System;

namespace Tarea5_Recursividad
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Tarea 5: Ejemplos de Recursividad ===\n");

            // Ejemplo 1: Factorial
            Console.WriteLine("Ejemplo 1: Cálculo de Factorial");
            int numero = 5;
            int resultadoFactorial = CalcularFactorial(numero);
            Console.WriteLine($"El factorial de {numero} es: {resultadoFactorial}\n");

            // Ejemplo 2: Cuenta Regresiva
            Console.WriteLine("Ejemplo 2: Cuenta Regresiva");
            CuentaRegresiva(5);
        }

        // Ejemplo 1: Método recursivo que calcula el factorial de un número
        static int CalcularFactorial(int n)
        {
            // Caso base: el factorial de 0 es 1
            if (n == 0) 
            {
                return 1;
            }
            // Caso recursivo: n * factorial de (n-1)
            else 
            {
                return n * CalcularFactorial(n - 1);
            }
        }

        // Ejemplo 2: Método recursivo que imprime números en orden descendente
        static void CuentaRegresiva(int n)
        {
            // Caso base: cuando el número es menor a 1, termina la recursividad
            if (n < 1) 
            {
                Console.WriteLine("¡Despegue!");
                return;
            }
            
            // Acción y llamada recursiva
            Console.WriteLine(n);
            CuentaRegresiva(n - 1); 
        }
    }
}
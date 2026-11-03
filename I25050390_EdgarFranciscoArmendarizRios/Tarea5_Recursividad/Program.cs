

using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== EJEMPLO 1: FACTORIAL ===");
        int numero = 5;
        int resultado = Factorial(numero);
        Console.WriteLine($"El factorial de {numero} es: {resultado}");

        Console.WriteLine("\n=== EJEMPLO 2: CUENTA REGRESIVA ===");
        CuentaRegresiva(5);
    }

    // Método recursivo para calcular el factorial
    static int Factorial(int n)
    {
        // Caso base: el factorial de 0 o 1 es 1
        if (n <= 1)
        {
            return 1;
        }
        
        // Caso recursivo: n * factorial del número anterior
        return n * Factorial(n - 1);
    }

    // Método recursivo para hacer una cuenta regresiva
    static void CuentaRegresiva(int n)
    {
        // Caso base: si n es menor o igual a 0, terminamos
        if (n <= 0)
        {
            Console.WriteLine("¡Tiempo cumplido!");
            return;
        }

        // Imprimimos el valor actual
        Console.WriteLine(n);

        // Llamada recursiva disminuyendo n en 1
        CuentaRegresiva(n - 1);
    }
}
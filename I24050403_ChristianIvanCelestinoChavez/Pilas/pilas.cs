
using System;
using System.Collections.Generic;

class Program
{
    static Stack<int> pila = new Stack<int>();

    static void Main()
    {
        int opcion;

        do
        {
            Console.Clear();
            Console.WriteLine("===== ESTRUCTURA DE DATOS: PILAS =====");
            Console.WriteLine("1. Insertar elemento (Push)");
            Console.WriteLine("2. Eliminar elemento (Pop)");
            Console.WriteLine("3. Consultar elemento superior (Peek)");
            Console.WriteLine("4. Mostrar pila");
            Console.WriteLine("5. Verificar si está vacía");
            Console.WriteLine("6. Salir");
            Console.Write("Selecciona una opción: ");

            if (!int.TryParse(Console.ReadLine(), out opcion))
                opcion = 0;

            switch (opcion)
            {
                case 1:
                    Console.Write("Ingresa un número: ");
                    if (int.TryParse(Console.ReadLine(), out int numero))
                    {
                        pila.Push(numero);
                        Console.WriteLine("Elemento insertado correctamente.");
                    }
                    else
                    {
                        Console.WriteLine("Número inválido.");
                    }
                    break;

                case 2:
                    if (pila.Count > 0)
                        Console.WriteLine("Elemento eliminado: " + pila.Pop());
                    else
                        Console.WriteLine("La pila está vacía.");
                    break;

                case 3:
                    if (pila.Count > 0)
                        Console.WriteLine("Elemento superior: " + pila.Peek());
                    else
                        Console.WriteLine("La pila está vacía.");
                    break;

                case 4:
                    Console.WriteLine("\nElementos de la pila:");
                    if (pila.Count == 0)
                        Console.WriteLine("La pila está vacía.");
                    else
                    {
                        foreach (int elemento in pila)
                            Console.WriteLine("| " + elemento + " |");
                        Console.WriteLine("-----");
                    }
                    break;

                case 5:
                    Console.WriteLine(pila.Count == 0
                        ? "La pila está vacía."
                        : "La pila contiene elementos.");
                    break;

                case 6:
                    Console.WriteLine("Programa finalizado.");
                    break;

                default:
                    Console.WriteLine("Opción no válida.");
                    break;
            }

            if (opcion != 6)
            {
                Console.WriteLine("\nPresiona una tecla para continuar...");
                Console.ReadKey();
            }

        } while (opcion != 6);
    }
}

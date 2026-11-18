
using System;
using System.Collections.Generic;

namespace EjemploGrafos
{
    class Grafo
    {
        private Dictionary<string, List<string>> vertices;

        public Grafo()
        {
            vertices = new Dictionary<string, List<string>>();
        }

        // Agregar un vertice
        public void AgregarVertice(string nombre)
        {
            if (!vertices.ContainsKey(nombre))
            {
                vertices[nombre] = new List<string>();
                Console.WriteLine("Vertice agregado correctamente.");
            }
            else
            {
                Console.WriteLine("El vertice ya existe.");
            }
        }

        // Agregar una arista
        public void AgregarArista(string origen, string destino)
        {
            if (!vertices.ContainsKey(origen) ||
                !vertices.ContainsKey(destino))
            {
                Console.WriteLine("Primero debes agregar ambos vertices.");
                return;
            }

            if (origen == destino)
            {
                Console.WriteLine("No se permiten conexiones consigo mismo.");
                return;
            }

            if (vertices[origen].Contains(destino))
            {
                Console.WriteLine("La conexion ya existe.");
                return;
            }

            // Grafo no dirigido
            vertices[origen].Add(destino);
            vertices[destino].Add(origen);

            Console.WriteLine("Conexion agregada correctamente.");
        }

        // Mostrar lista de adyacencia
        public void MostrarGrafo()
        {
            Console.WriteLine("\n--- LISTA DE ADYACENCIA ---");

            foreach (var vertice in vertices)
            {
                Console.Write(vertice.Key + " -> ");

                foreach (var vecino in vertice.Value)
                {
                    Console.Write(vecino + " ");
                }

                Console.WriteLine();
            }
        }

        // Busqueda en anchura (BFS)
        public void BFS(string inicio)
        {
            if (!vertices.ContainsKey(inicio))
            {
                Console.WriteLine("El vertice no existe.");
                return;
            }

            Queue<string> cola = new Queue<string>();
            HashSet<string> visitados = new HashSet<string>();

            cola.Enqueue(inicio);
            visitados.Add(inicio);

            Console.WriteLine("\nRecorrido BFS:");

            while (cola.Count > 0)
            {
                string actual = cola.Dequeue();

                Console.Write(actual + " ");

                foreach (string vecino in vertices[actual])
                {
                    if (!visitados.Contains(vecino))
                    {
                        visitados.Add(vecino);
                        cola.Enqueue(vecino);
                    }
                }
            }

            Console.WriteLine();
        }

        // Busqueda en profundidad (DFS)
        public void DFS(string inicio)
        {
            if (!vertices.ContainsKey(inicio))
            {
                Console.WriteLine("El vertice no existe.");
                return;
            }

            HashSet<string> visitados = new HashSet<string>();

            Console.WriteLine("\nRecorrido DFS:");

            DFSRecursivo(inicio, visitados);

            Console.WriteLine();
        }

        private void DFSRecursivo(
            string actual, HashSet<string> visitados)
        {
            visitados.Add(actual);

            Console.Write(actual + " ");

            foreach (string vecino in vertices[actual])
            {
                if (!visitados.Contains(vecino))
                {
                    DFSRecursivo(vecino, visitados);
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Grafo grafo = new Grafo();

            // Crear vertices iniciales
            grafo.AgregarVertice("A");
            grafo.AgregarVertice("B");
            grafo.AgregarVertice("C");
            grafo.AgregarVertice("D");
            grafo.AgregarVertice("E");

            // Crear conexiones
            grafo.AgregarArista("A", "B");
            grafo.AgregarArista("A", "C");
            grafo.AgregarArista("B", "D");
            grafo.AgregarArista("C", "D");
            grafo.AgregarArista("D", "E");

            int opcion;

            do
            {
                Console.WriteLine("\n===== MENU DE GRAFOS =====");
                Console.WriteLine("1. Agregar vertice");
                Console.WriteLine("2. Agregar conexion");
                Console.WriteLine("3. Mostrar grafo");
                Console.WriteLine("4. Recorrido BFS");
                Console.WriteLine("5. Recorrido DFS");
                Console.WriteLine("0. Salir");
                Console.Write("Selecciona una opcion: ");

                if (!int.TryParse(Console.ReadLine(), out opcion))
                {
                    opcion = -1;
                }

                string origen;
                string destino;

                switch (opcion)
                {
                    case 1:
                        Console.Write("Nombre del vertice: ");
                        origen = Console.ReadLine() ?? "";
                        grafo.AgregarVertice(origen);
                        break;

                    case 2:
                        Console.Write("Vertice de origen: ");
                        origen = Console.ReadLine() ?? "";

                        Console.Write("Vertice de destino: ");
                        destino = Console.ReadLine() ?? "";

                        grafo.AgregarArista(origen, destino);
                        break;

                    case 3:
                        grafo.MostrarGrafo();
                        break;

                    case 4:
                        Console.Write("Vertice inicial: ");
                        origen = Console.ReadLine() ?? "";
                        grafo.BFS(origen);
                        break;

                    case 5:
                        Console.Write("Vertice inicial: ");
                        origen = Console.ReadLine() ?? "";
                        grafo.DFS(origen);
                        break;

                    case 0:
                        Console.WriteLine("Programa finalizado.");
                        break;

                    default:
                        Console.WriteLine("Opcion no valida.");
                        break;
                }

            } while (opcion != 0);
        }
    }
}
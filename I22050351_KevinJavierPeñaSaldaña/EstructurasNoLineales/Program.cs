using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace GrafoDirigidoApp
{
    /// <summary>
    /// Grafo dirigido y ponderado implementado con lista de adyacencia.
    /// </summary>
    public class GrafoDirigido<T> where T : notnull
    {
        private readonly Dictionary<T, Dictionary<T, double>> _adyacencia = new();

        public IEnumerable<T> Vertices => _adyacencia.Keys;
        public int NumVertices => _adyacencia.Count;
        public int NumAristas => _adyacencia.Values.Sum(d => d.Count);

        public bool ExisteVertice(T v) => _adyacencia.ContainsKey(v);

        public bool AgregarVertice(T v)
        {
            if (_adyacencia.ContainsKey(v)) return false;
            _adyacencia[v] = new Dictionary<T, double>();
            return true;
        }

        public void AgregarArista(T origen, T destino, double peso = 1)
        {
            AgregarVertice(origen);
            AgregarVertice(destino);
            _adyacencia[origen][destino] = peso;
        }

        public bool EliminarArista(T origen, T destino)
        {
            return _adyacencia.TryGetValue(origen, out var vecinos) && vecinos.Remove(destino);
        }

        public bool EliminarVertice(T v)
        {
            if (!_adyacencia.Remove(v)) return false;
            foreach (var vecinos in _adyacencia.Values)
                vecinos.Remove(v);
            return true;
        }

        public bool ExisteArista(T origen, T destino) =>
            _adyacencia.TryGetValue(origen, out var vecinos) && vecinos.ContainsKey(destino);

        public IEnumerable<T> Sucesores(T v) =>
            _adyacencia.TryGetValue(v, out var vecinos) ? vecinos.Keys : Enumerable.Empty<T>();

        public int GradoSalida(T v) => _adyacencia.TryGetValue(v, out var d) ? d.Count : 0;

        public int GradoEntrada(T v) => _adyacencia.Values.Count(d => d.ContainsKey(v));

        // ---------- Recorridos ----------

        public List<T> BFS(T inicio)
        {
            var resultado = new List<T>();
            if (!_adyacencia.ContainsKey(inicio)) return resultado;

            var visitados = new HashSet<T> { inicio };
            var cola = new Queue<T>();
            cola.Enqueue(inicio);

            while (cola.Count > 0)
            {
                var actual = cola.Dequeue();
                resultado.Add(actual);
                foreach (var vecino in Sucesores(actual))
                {
                    if (visitados.Add(vecino))
                        cola.Enqueue(vecino);
                }
            }
            return resultado;
        }

        public List<T> DFS(T inicio)
        {
            var resultado = new List<T>();
            if (!_adyacencia.ContainsKey(inicio)) return resultado;

            var visitados = new HashSet<T>();
            DFSRec(inicio, visitados, resultado);
            return resultado;
        }

        private void DFSRec(T v, HashSet<T> visitados, List<T> resultado)
        {
            visitados.Add(v);
            resultado.Add(v);
            foreach (var vecino in Sucesores(v))
            {
                if (!visitados.Contains(vecino))
                    DFSRec(vecino, visitados, resultado);
            }
        }

        // ---------- Algoritmos ----------

        public bool TieneCiclo()
        {
            var visitados = new HashSet<T>();
            var enPila = new HashSet<T>();
            return Vertices.Any(v => !visitados.Contains(v) && CicloRec(v, visitados, enPila));
        }

        private bool CicloRec(T v, HashSet<T> visitados, HashSet<T> enPila)
        {
            visitados.Add(v);
            enPila.Add(v);
            foreach (var vecino in Sucesores(v))
            {
                if (!visitados.Contains(vecino))
                {
                    if (CicloRec(vecino, visitados, enPila)) return true;
                }
                else if (enPila.Contains(vecino))
                {
                    return true;
                }
            }
            enPila.Remove(v);
            return false;
        }

        public List<T>? OrdenTopologico()
        {
            var gradoEntrada = Vertices.ToDictionary(v => v, v => GradoEntrada(v));
            var cola = new Queue<T>(gradoEntrada.Where(p => p.Value == 0).Select(p => p.Key));
            var orden = new List<T>();

            while (cola.Count > 0)
            {
                var actual = cola.Dequeue();
                orden.Add(actual);
                foreach (var vecino in Sucesores(actual))
                {
                    if (--gradoEntrada[vecino] == 0)
                        cola.Enqueue(vecino);
                }
            }
            return orden.Count == NumVertices ? orden : null;
        }

        public (double distancia, List<T> camino)? CaminoMasCorto(T origen, T destino)
        {
            if (!_adyacencia.ContainsKey(origen) || !_adyacencia.ContainsKey(destino))
                return null;

            var dist = Vertices.ToDictionary(v => v, _ => double.PositiveInfinity);
            var previo = new Dictionary<T, T>();
            var pq = new PriorityQueue<T, double>();

            dist[origen] = 0;
            pq.Enqueue(origen, 0);

            while (pq.TryDequeue(out var actual, out var d))
            {
                if (d > dist[actual]) continue;
                if (EqualityComparer<T>.Default.Equals(actual, destino)) break;

                foreach (var (vecino, peso) in _adyacencia[actual])
                {
                    var nueva = dist[actual] + peso;
                    if (nueva < dist[vecino])
                    {
                        dist[vecino] = nueva;
                        previo[vecino] = actual;
                        pq.Enqueue(vecino, nueva);
                    }
                }
            }

            if (double.IsPositiveInfinity(dist[destino])) return null;

            var camino = new List<T>();
            var nodo = destino;
            camino.Add(nodo);
            while (previo.TryGetValue(nodo, out var p))
            {
                nodo = p;
                camino.Add(nodo);
            }
            camino.Reverse();
            return (dist[destino], camino);
        }

        // ---------- Visualización ----------

        public void Imprimir()
        {
            if (NumVertices == 0)
            {
                Console.WriteLine("  (el grafo está vacío)");
                return;
            }
            foreach (var (v, vecinos) in _adyacencia)
            {
                var texto = vecinos.Count == 0
                    ? "(sin aristas de salida)"
                    : string.Join(", ", vecinos.Select(a => $"{a.Key}({a.Value.ToString(CultureInfo.InvariantCulture)})"));
                Console.WriteLine($"  {v} -> {texto}");
            }
        }
    }

    public static class Program
    {
        private static readonly GrafoDirigido<string> Grafo = new();

        public static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            bool salir = false;

            while (!salir)
            {
                MostrarMenu();
                string opcion = LeerTexto("Elige una opción: ");
                Console.WriteLine();

                switch (opcion)
                {
                    case "1": AgregarVertice(); break;
                    case "2": AgregarArista(); break;
                    case "3": EliminarVertice(); break;
                    case "4": EliminarArista(); break;
                    case "5": MostrarGrafo(); break;
                    case "6": Recorrido(true); break;
                    case "7": Recorrido(false); break;
                    case "8": DetectarCiclos(); break;
                    case "9": OrdenTopologico(); break;
                    case "10": CaminoMasCorto(); break;
                    case "11": ConsultarGrados(); break;
                    case "12": CargarEjemplo(); break;
                    case "0": salir = true; Console.WriteLine("¡Hasta luego!"); break;
                    default: Console.WriteLine("Opción no válida."); break;
                }

                if (!salir)
                {
                    Console.WriteLine("\nPresiona una tecla para continuar...");
                    Console.ReadKey(true);
                }
            }
        }

        // ---------- Menú ----------

        private static void MostrarMenu()
        {
            Console.Clear();
            Console.WriteLine("========== GRAFO DIRIGIDO ==========");
            Console.WriteLine($" Vértices: {Grafo.NumVertices} | Aristas: {Grafo.NumAristas}");
            Console.WriteLine("------------------------------------");
            Console.WriteLine(" 1. Agregar vértice");
            Console.WriteLine(" 2. Agregar arista");
            Console.WriteLine(" 3. Eliminar vértice");
            Console.WriteLine(" 4. Eliminar arista");
            Console.WriteLine(" 5. Mostrar grafo");
            Console.WriteLine(" 6. Recorrido BFS");
            Console.WriteLine(" 7. Recorrido DFS");
            Console.WriteLine(" 8. Detectar ciclos");
            Console.WriteLine(" 9. Orden topológico");
            Console.WriteLine("10. Camino más corto (Dijkstra)");
            Console.WriteLine("11. Grados de un vértice");
            Console.WriteLine("12. Cargar grafo de ejemplo");
            Console.WriteLine(" 0. Salir");
            Console.WriteLine("====================================");
        }

        // ---------- Opciones ----------

        private static void AgregarVertice()
        {
            string v = LeerTexto("Nombre del vértice: ");
            Console.WriteLine(Grafo.AgregarVertice(v)
                ? $"Vértice '{v}' agregado."
                : $"El vértice '{v}' ya existe.");
        }

        private static void AgregarArista()
        {
            string origen = LeerTexto("Vértice origen: ");
            string destino = LeerTexto("Vértice destino: ");
            double peso = LeerPeso("Peso de la arista (Enter = 1): ");

            bool existia = Grafo.ExisteArista(origen, destino);
            Grafo.AgregarArista(origen, destino, peso);
            Console.WriteLine(existia
                ? $"Arista {origen} -> {destino} actualizada con peso {peso.ToString(CultureInfo.InvariantCulture)}."
                : $"Arista {origen} -> {destino} agregada con peso {peso.ToString(CultureInfo.InvariantCulture)}.");
            Console.WriteLine("(Los vértices que no existían se crearon automáticamente.)");
        }

        private static void EliminarVertice()
        {
            string v = LeerTexto("Vértice a eliminar: ");
            Console.WriteLine(Grafo.EliminarVertice(v)
                ? $"Vértice '{v}' eliminado junto con sus aristas."
                : $"El vértice '{v}' no existe.");
        }

        private static void EliminarArista()
        {
            string origen = LeerTexto("Vértice origen: ");
            string destino = LeerTexto("Vértice destino: ");
            Console.WriteLine(Grafo.EliminarArista(origen, destino)
                ? $"Arista {origen} -> {destino} eliminada."
                : "Esa arista no existe.");
        }

        private static void MostrarGrafo()
        {
            Console.WriteLine("Lista de adyacencia:");
            Grafo.Imprimir();
        }

        private static void Recorrido(bool esBfs)
        {
            string inicio = LeerTexto("Vértice de inicio: ");
            if (!Grafo.ExisteVertice(inicio))
            {
                Console.WriteLine($"El vértice '{inicio}' no existe.");
                return;
            }
            var orden = esBfs ? Grafo.BFS(inicio) : Grafo.DFS(inicio);
            Console.WriteLine($"{(esBfs ? "BFS" : "DFS")} desde {inicio}: {string.Join(" -> ", orden)}");
        }

        private static void DetectarCiclos()
        {
            Console.WriteLine(Grafo.TieneCiclo()
                ? "El grafo SÍ contiene al menos un ciclo."
                : "El grafo NO contiene ciclos (es un DAG).");
        }

        private static void OrdenTopologico()
        {
            var orden = Grafo.OrdenTopologico();
            Console.WriteLine(orden == null
                ? "No existe orden topológico porque el grafo tiene ciclos."
                : $"Orden topológico: {string.Join(", ", orden)}");
        }

        private static void CaminoMasCorto()
        {
            string origen = LeerTexto("Vértice origen: ");
            string destino = LeerTexto("Vértice destino: ");

            if (!Grafo.ExisteVertice(origen) || !Grafo.ExisteVertice(destino))
            {
                Console.WriteLine("Alguno de los vértices no existe.");
                return;
            }

            var res = Grafo.CaminoMasCorto(origen, destino);
            if (res == null)
                Console.WriteLine($"No hay camino de {origen} a {destino}.");
            else
                Console.WriteLine($"Camino: {string.Join(" -> ", res.Value.camino)} (costo total: {res.Value.distancia.ToString(CultureInfo.InvariantCulture)})");
        }

        private static void ConsultarGrados()
        {
            string v = LeerTexto("Vértice: ");
            if (!Grafo.ExisteVertice(v))
            {
                Console.WriteLine($"El vértice '{v}' no existe.");
                return;
            }
            Console.WriteLine($"Grado de entrada: {Grafo.GradoEntrada(v)}");
            Console.WriteLine($"Grado de salida:  {Grafo.GradoSalida(v)}");
            var sucesores = Grafo.Sucesores(v).ToList();
            Console.WriteLine($"Sucesores: {(sucesores.Count == 0 ? "(ninguno)" : string.Join(", ", sucesores))}");
        }

        private static void CargarEjemplo()
        {
            Grafo.AgregarArista("A", "B", 4);
            Grafo.AgregarArista("A", "C", 2);
            Grafo.AgregarArista("B", "D", 5);
            Grafo.AgregarArista("C", "B", 1);
            Grafo.AgregarArista("C", "D", 8);
            Grafo.AgregarArista("D", "E", 3);
            Console.WriteLine("Grafo de ejemplo cargado (A, B, C, D, E).");
        }

        // ---------- Lectura de datos con validación ----------

        private static string LeerTexto(string mensaje)
        {
            while (true)
            {
                Console.Write(mensaje);
                string? entrada = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(entrada)) return entrada;
                Console.WriteLine("La entrada no puede estar vacía. Intenta de nuevo.");
            }
        }

        private static double LeerPeso(string mensaje)
        {
            while (true)
            {
                Console.Write(mensaje);
                string? entrada = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(entrada)) return 1;

                // Acepta tanto "2.5" como "2,5"
                entrada = entrada.Replace(',', '.');
                if (double.TryParse(entrada, NumberStyles.Float, CultureInfo.InvariantCulture, out double peso))
                {
                    if (peso >= 0) return peso;
                    Console.WriteLine("El peso no puede ser negativo (Dijkstra no lo soporta).");
                }
                else
                {
                    Console.WriteLine("Peso no válido. Ingresa un número.");
                }
            }
        }
    }
}
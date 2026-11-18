using System;

namespace Tarea7_EstructurasLineales
{
    // =========================================================================
    // 1. COLA CIRCULAR (Implementada con arreglo fijo)
    // =========================================================================
    /*
     * CARACTERÍSTICAS DE LA ESTRUCTURA DE DATOS:
     * - Estructura lineal FIFO (First-In, First-Out) de tamaño fijo.
     * - El último elemento se conecta lógicamente con el primero formando un anillo,
     *   utilizando aritmética modular (%) para reutilizar espacios liberados.
     * - Evita el desperdicio de memoria de una cola lineal simple sin necesidad de
     *   desplazar elementos al desencolar (operaciones en tiempo O(1)).
     *
     * 3 EJEMPLOS DE APLICACIONES:
     * 1. Planificación de procesos en sistemas operativos (algoritmo Round-Robin).
     * 2. Buffers de transmisión de audio/video en tiempo real (streaming).
     * 3. Gestión de semáforos o turnos cíclicos en sistemas embebidos.
     */
    class ColaCircular
    {
        private int[] elementos;
        private int frente;
        private int final;
        private int capacidad;
        private int contador;

        public ColaCircular(int capacidad)
        {
            this.capacidad = capacidad;
            elementos = new int[capacidad];
            frente = 0;
            final = -1;
            contador = 0;
        }

        public bool EstaVacia() => contador == 0;
        public bool EstaLlena() => contador == capacidad;

        public void Encolar(int dato)
        {
            if (EstaLlena())
            {
                Console.WriteLine("Error: La cola circular está llena.");
                return;
            }
            final = (final + 1) % capacidad;
            elementos[final] = dato;
            contador++;
            Console.WriteLine($"Encolado: {dato}");
        }

        public void Desencolar()
        {
            if (EstaVacia())
            {
                Console.WriteLine("Error: La cola circular está vacía.");
                return;
            }
            int eliminado = elementos[frente];
            frente = (frente + 1) % capacidad;
            contador--;
            Console.WriteLine($"Desencolado: {eliminado}");
        }

        public void VerFrente()
        {
            if (EstaVacia()) Console.WriteLine("La cola está vacía.");
            else Console.WriteLine($"Elemento al frente: {elementos[frente]}");
        }

        public void Mostrar()
        {
            if (EstaVacia())
            {
                Console.WriteLine("Cola circular: [ Vacía ]");
                return;
            }
            Console.Write("Cola circular: [ ");
            for (int i = 0; i < contador; i++)
            {
                int indice = (frente + i) % capacidad;
                Console.Write(elementos[indice] + " ");
            }
            Console.WriteLine("]");
        }
    }

    // =========================================================================
    // 2. BICOLA / COLA DE DOBLE EXTREMO (Deque)
    // =========================================================================
    /*
     * CARACTERÍSTICAS DE LA ESTRUCTURA DE DATOS:
     * - Generalización de una cola y una pila donde se pueden insertar y eliminar
     *   elementos tanto por el extremo frontal (frente) como por el posterior (final).
     * - Puede implementarse mediante arreglos circulares o listas doblemente enlazadas.
     * - Todas las operaciones de inserción y eliminación en los extremos tienen
     *   complejidad temporal O(1).
     *
     * 3 EJEMPLOS DE APLICACIONES:
     * 1. Historial de navegación web (avanzar y retroceder páginas).
     * 2. Funcionalidad "Deshacer / Rehacer" (Undo/Redo) en editores de texto.
     * 3. Algoritmos de planificación de tareas con robo de trabajo (Work-Stealing).
     */
    class Bicola
    {
        private class Nodo
        {
            public int Dato;
            public Nodo Anterior;
            public Nodo Siguiente;
            public Nodo(int dato) { Dato = dato; }
        }

        private Nodo frente;
        private Nodo final;

        public bool EstaVacia() => frente == null;

        public void InsertarFrente(int dato)
        {
            Nodo nuevo = new Nodo(dato);
            if (EstaVacia())
            {
                frente = final = nuevo;
            }
            else
            {
                nuevo.Siguiente = frente;
                frente.Anterior = nuevo;
                frente = nuevo;
            }
            Console.WriteLine($"Insertado al frente: {dato}");
        }

        public void InsertarFinal(int dato)
        {
            Nodo nuevo = new Nodo(dato);
            if (EstaVacia())
            {
                frente = final = nuevo;
            }
            else
            {
                nuevo.Anterior = final;
                final.Siguiente = nuevo;
                final = nuevo;
            }
            Console.WriteLine($"Insertado al final: {dato}");
        }

        public void EliminarFrente()
        {
            if (EstaVacia())
            {
                Console.WriteLine("Error: La bicola está vacía.");
                return;
            }
            int dato = frente.Dato;
            frente = frente.Siguiente;
            if (frente == null) final = null;
            else frente.Anterior = null;
            Console.WriteLine($"Eliminado del frente: {dato}");
        }

        public void EliminarFinal()
        {
            if (EstaVacia())
            {
                Console.WriteLine("Error: La bicola está vacía.");
                return;
            }
            int dato = final.Dato;
            final = final.Anterior;
            if (final == null) frente = null;
            else final.Siguiente = null;
            Console.WriteLine($"Eliminado del final: {dato}");
        }

        public void Mostrar()
        {
            if (EstaVacia())
            {
                Console.WriteLine("Bicola: [ Vacía ]");
                return;
            }
            Console.Write("Bicola: [ ");
            Nodo actual = frente;
            while (actual != null)
            {
                Console.Write(actual.Dato + " ");
                actual = actual.Siguiente;
            }
            Console.WriteLine("]");
        }
    }

    // =========================================================================
    // 3. LISTA DOBLEMENTE ENLAZADA
    // =========================================================================
    /*
     * CARACTERÍSTICAS DE LA ESTRUCTURA DE DATOS:
     * - Colección dinámica de nodos donde cada nodo contiene un dato y dos punteros:
     *   uno hacia el nodo siguiente y otro hacia el nodo anterior.
     * - Permite el recorrido bidireccional (de inicio a fin y de fin a inicio).
     * - Facilita la eliminación e inserción de nodos intermedios en O(1) si se tiene
     *   la referencia al nodo, a costa de un mayor consumo de memoria por puntero extra.
     *
     * 3 EJEMPLOS DE APLICACIONES:
     * 1. Reproductores multimedia (pasar a la canción siguiente o a la anterior).
     * 2. Cachés de memoria con política LRU (Least Recently Used).
     * 3. Gestión de pestañas o ventanas abiertas en entornos gráficos.
     */
    class ListaDoblementeEnlazada
    {
        private class Nodo
        {
            public int Dato;
            public Nodo Anterior;
            public Nodo Siguiente;
            public Nodo(int dato) { Dato = dato; }
        }

        private Nodo cabeza;
        private Nodo cola;

        public void InsertarInicio(int dato)
        {
            Nodo nuevo = new Nodo(dato);
            if (cabeza == null)
            {
                cabeza = cola = nuevo;
            }
            else
            {
                nuevo.Siguiente = cabeza;
                cabeza.Anterior = nuevo;
                cabeza = nuevo;
            }
            Console.WriteLine($"Insertado al inicio: {dato}");
        }

        public void InsertarFinal(int dato)
        {
            Nodo nuevo = new Nodo(dato);
            if (cabeza == null)
            {
                cabeza = cola = nuevo;
            }
            else
            {
                cola.Siguiente = nuevo;
                nuevo.Anterior = cola;
                cola = nuevo;
            }
            Console.WriteLine($"Insertado al final: {dato}");
        }

        public void Eliminar(int dato)
        {
            Nodo actual = cabeza;
            while (actual != null && actual.Dato != dato)
                actual = actual.Siguiente;

            if (actual == null)
            {
                Console.WriteLine($"Elemento {dato} no encontrado.");
                return;
            }

            if (actual.Anterior != null) actual.Anterior.Siguiente = actual.Siguiente;
            else cabeza = actual.Siguiente;

            if (actual.Siguiente != null) actual.Siguiente.Anterior = actual.Anterior;
            else cola = actual.Anterior;

            Console.WriteLine($"Elemento {dato} eliminado.");
        }

        public void Buscar(int dato)
        {
            Nodo actual = cabeza;
            int pos = 0;
            while (actual != null)
            {
                if (actual.Dato == dato)
                {
                    Console.WriteLine($"Elemento {dato} encontrado en la posición {pos}.");
                    return;
                }
                actual = actual.Siguiente;
                pos++;
            }
            Console.WriteLine($"Elemento {dato} no está en la lista.");
        }

        public void MostrarAdelante()
        {
            Console.Write("Inicio -> Fin: null <-> ");
            Nodo actual = cabeza;
            while (actual != null)
            {
                Console.Write($"{actual.Dato} <-> ");
                actual = actual.Siguiente;
            }
            Console.WriteLine("null");
        }

        public void MostrarAtras()
        {
            Console.Write("Fin -> Inicio: null <-> ");
            Nodo actual = cola;
            while (actual != null)
            {
                Console.Write($"{actual.Dato} <-> ");
                actual = actual.Anterior;
            }
            Console.WriteLine("null");
        }
    }

    // =========================================================================
    // 4. LISTA CIRCULAR (Simplemente enlazada)
    // =========================================================================
    /*
     * CARACTERÍSTICAS DE LA ESTRUCTURA DE DATOS:
     * - Lista enlazada en la que el puntero 'Siguiente' del último nodo apunta
     *   nuevamente al primer nodo (cabeza), eliminando referencias nulas al final.
     * - Se puede recorrer toda la lista comenzando desde cualquier nodo dado.
     * - Requiere una condición de parada explícita al recorrerla (volver al nodo inicial)
     *   para evitar ciclos infinitos.
     *
     * 3 EJEMPLOS DE APLICACIONES:
     * 1. Control de turnos en videojuegos multijugador por rondas (ej. juegos de mesa).
     * 2. Carruseles infinitos de imágenes o banners en interfaces de usuario.
     * 3. Asignación cíclica de recursos en servidores (Balanceo de carga Round-Robin).
     */
    class ListaCircular
    {
        private class Nodo
        {
            public int Dato;
            public Nodo Siguiente;
            public Nodo(int dato) { Dato = dato; }
        }

        private Nodo ultimo; // Apuntar al último facilita insertar al inicio y final en O(1)

        public void Insertar(int dato)
        {
            Nodo nuevo = new Nodo(dato);
            if (ultimo == null)
            {
                ultimo = nuevo;
                ultimo.Siguiente = ultimo;
            }
            else
            {
                nuevo.Siguiente = ultimo.Siguiente;
                ultimo.Siguiente = nuevo;
                ultimo = nuevo;
            }
            Console.WriteLine($"Insertado en lista circular: {dato}");
        }

        public void Eliminar(int dato)
        {
            if (ultimo == null)
            {
                Console.WriteLine("La lista circular está vacía.");
                return;
            }

            Nodo actual = ultimo.Siguiente;
            Nodo previo = ultimo;

            do
            {
                if (actual.Dato == dato)
                {
                    if (actual == ultimo && actual.Siguiente == ultimo)
                    {
                        ultimo = null; // Era el único nodo
                    }
                    else
                    {
                        previo.Siguiente = actual.Siguiente;
                        if (actual == ultimo) ultimo = previo;
                    }
                    Console.WriteLine($"Eliminado: {dato}");
                    return;
                }
                previo = actual;
                actual = actual.Siguiente;
            } while (actual != ultimo.Siguiente);

            Console.WriteLine($"Elemento {dato} no encontrado.");
        }

        public void Mostrar()
        {
            if (ultimo == null)
            {
                Console.WriteLine("Lista circular: [ Vacía ]");
                return;
            }
            Console.Write("Lista circular: ");
            Nodo actual = ultimo.Siguiente;
            do
            {
                Console.Write($"[{actual.Dato}] -> ");
                actual = actual.Siguiente;
            } while (actual != ultimo.Siguiente);
            Console.WriteLine($"(vuelve a [{ultimo.Siguiente.Dato}])");
        }
    }

    // =========================================================================
    // 5. LISTA CIRCULAR DOBLE
    // =========================================================================
    /*
     * CARACTERÍSTICAS DE LA ESTRUCTURA DE DATOS:
     * - Combina las propiedades de la lista doblemente enlazada y la lista circular:
     *   cada nodo tiene punteros 'Siguiente' y 'Anterior', y los extremos están conectados.
     * - El puntero 'Siguiente' del último nodo apunta al primero, y el puntero 'Anterior'
     *   del primer nodo apunta al último (ningún puntero es null cuando hay elementos).
     * - Permite avanzar y retroceder de forma continua e ininterrumpida en ambas direcciones.
     *
     * 3 EJEMPLOS DE APLICACIONES:
     * 1. Listas de reproducción musical en modo "Repetir todo" con avance y retroceso.
     * 2. Selector circular de aplicaciones activas (como Alt + Tab en sistemas operativos).
     * 3. Gestión de bloques libres de memoria en asignadores dinámicos (heaps).
     */
    class ListaCircularDoble
    {
        private class Nodo
        {
            public int Dato;
            public Nodo Anterior;
            public Nodo Siguiente;
            public Nodo(int dato) { Dato = dato; }
        }

        private Nodo cabeza;

        public void Insertar(int dato)
        {
            Nodo nuevo = new Nodo(dato);
            if (cabeza == null)
            {
                cabeza = nuevo;
                cabeza.Siguiente = cabeza;
                cabeza.Anterior = cabeza;
            }
            else
            {
                Nodo ultimo = cabeza.Anterior;
                nuevo.Siguiente = cabeza;
                nuevo.Anterior = ultimo;
                ultimo.Siguiente = nuevo;
                cabeza.Anterior = nuevo;
            }
            Console.WriteLine($"Insertado en lista circular doble: {dato}");
        }

        public void Eliminar(int dato)
        {
            if (cabeza == null)
            {
                Console.WriteLine("La lista circular doble está vacía.");
                return;
            }

            Nodo actual = cabeza;
            do
            {
                if (actual.Dato == dato)
                {
                    if (actual.Siguiente == cabeza && actual.Anterior == cabeza)
                    {
                        cabeza = null;
                    }
                    else
                    {
                        actual.Anterior.Siguiente = actual.Siguiente;
                        actual.Siguiente.Anterior = actual.Anterior;
                        if (actual == cabeza) cabeza = actual.Siguiente;
                    }
                    Console.WriteLine($"Eliminado: {dato}");
                    return;
                }
                actual = actual.Siguiente;
            } while (actual != cabeza);

            Console.WriteLine($"Elemento {dato} no encontrado.");
        }

        public void MostrarAdelante()
        {
            if (cabeza == null)
            {
                Console.WriteLine("Lista circular doble: [ Vacía ]");
                return;
            }
            Console.Write("Adelante: ");
            Nodo actual = cabeza;
            do
            {
                Console.Write($"[{actual.Dato}] <-> ");
                actual = actual.Siguiente;
            } while (actual != cabeza);
            Console.WriteLine($"(inicio [{cabeza.Dato}])");
        }

        public void MostrarAtras()
        {
            if (cabeza == null)
            {
                Console.WriteLine("Lista circular doble: [ Vacía ]");
                return;
            }
            Console.Write("Atrás:    ");
            Nodo actual = cabeza.Anterior;
            Nodo inicio = actual;
            do
            {
                Console.Write($"[{actual.Dato}] <-> ");
                actual = actual.Anterior;
            } while (actual != inicio);
            Console.WriteLine($"(fin [{inicio.Dato}])");
        }
    }

    // =========================================================================
    // PROGRAMA PRINCIPAL CON MENÚ DE SELECCIÓN
    // =========================================================================
    class Program
    {
        static void Main(string[] args)
        {
            int opcion;
            do
            {
                Console.Clear();
                Console.WriteLine("=================================================");
                Console.WriteLine("      TAREA 7: ESTRUCTURAS DE DATOS LINEALES     ");
                Console.WriteLine("=================================================");
                Console.WriteLine("1. Cola Circular");
                Console.WriteLine("2. Bicola (Cola de doble extremo)");
                Console.WriteLine("3. Lista Doblemente Enlazada");
                Console.WriteLine("4. Lista Circular");
                Console.WriteLine("5. Lista Circular Doble");
                Console.WriteLine("0. Salir");
                Console.WriteLine("=================================================");
                Console.Write("Selecciona una estructura para probar: ");

                if (!int.TryParse(Console.ReadLine(), out opcion)) opcion = -1;

                switch (opcion)
                {
                    case 1: MenuColaCircular(); break;
                    case 2: MenuBicola(); break;
                    case 3: MenuListaDoble(); break;
                    case 4: MenuListaCircular(); break;
                    case 5: MenuListaCircularDoble(); break;
                    case 0: Console.WriteLine("Saliendo del programa..."); break;
                    default:
                        Console.WriteLine("Opción no válida. Presiona Enter...");
                        Console.ReadLine();
                        break;
                }
            } while (opcion != 0);
        }

        static int LeerEntero(string mensaje)
        {
            int valor;
            Console.Write(mensaje);
            while (!int.TryParse(Console.ReadLine(), out valor))
            {
                Console.Write("Por favor ingresa un número entero válido: ");
            }
            return valor;
        }

        static void MenuColaCircular()
        {
            Console.Clear();
            int cap = LeerEntero("Ingresa la capacidad máxima de la Cola Circular: ");
            ColaCircular cola = new ColaCircular(cap > 0 ? cap : 5);
            int op;
            do
            {
                Console.WriteLine("\n--- COLA CIRCULAR ---");
                Console.WriteLine("1. Encolar (Insertar)");
                Console.WriteLine("2. Desencolar (Eliminar)");
                Console.WriteLine("3. Ver frente");
                Console.WriteLine("4. Mostrar cola");
                Console.WriteLine("0. Volver al menú principal");
                op = LeerEntero("Opción: ");

                switch (op)
                {
                    case 1: cola.Encolar(LeerEntero("Dato a encolar: ")); break;
                    case 2: cola.Desencolar(); break;
                    case 3: cola.VerFrente(); break;
                    case 4: cola.Mostrar(); break;
                }
            } while (op != 0);
        }

        static void MenuBicola()
        {
            Console.Clear();
            Bicola bicola = new Bicola();
            int op;
            do
            {
                Console.WriteLine("\n--- BICOLA (COLA DE DOBLE EXTREMO) ---");
                Console.WriteLine("1. Insertar al frente");
                Console.WriteLine("2. Insertar al final");
                Console.WriteLine("3. Eliminar del frente");
                Console.WriteLine("4. Eliminar del final");
                Console.WriteLine("5. Mostrar bicola");
                Console.WriteLine("0. Volver al menú principal");
                op = LeerEntero("Opción: ");

                switch (op)
                {
                    case 1: bicola.InsertarFrente(LeerEntero("Dato al frente: ")); break;
                    case 2: bicola.InsertarFinal(LeerEntero("Dato al final: ")); break;
                    case 3: bicola.EliminarFrente(); break;
                    case 4: bicola.EliminarFinal(); break;
                    case 5: bicola.Mostrar(); break;
                }
            } while (op != 0);
        }

        static void MenuListaDoble()
        {
            Console.Clear();
            ListaDoblementeEnlazada lista = new ListaDoblementeEnlazada();
            int op;
            do
            {
                Console.WriteLine("\n--- LISTA DOBLEMENTE ENLAZADA ---");
                Console.WriteLine("1. Insertar al inicio");
                Console.WriteLine("2. Insertar al final");
                Console.WriteLine("3. Eliminar dato");
                Console.WriteLine("4. Buscar dato");
                Console.WriteLine("5. Mostrar hacia adelante");
                Console.WriteLine("6. Mostrar hacia atrás");
                Console.WriteLine("0. Volver al menú principal");
                op = LeerEntero("Opción: ");

                switch (op)
                {
                    case 1: lista.InsertarInicio(LeerEntero("Dato al inicio: ")); break;
                    case 2: lista.InsertarFinal(LeerEntero("Dato al final: ")); break;
                    case 3: lista.Eliminar(LeerEntero("Dato a eliminar: ")); break;
                    case 4: lista.Buscar(LeerEntero("Dato a buscar: ")); break;
                    case 5: lista.MostrarAdelante(); break;
                    case 6: lista.MostrarAtras(); break;
                }
            } while (op != 0);
        }

        static void MenuListaCircular()
        {
            Console.Clear();
            ListaCircular lista = new ListaCircular();
            int op;
            do
            {
                Console.WriteLine("\n--- LISTA CIRCULAR ---");
                Console.WriteLine("1. Insertar dato");
                Console.WriteLine("2. Eliminar dato");
                Console.WriteLine("3. Mostrar lista circular");
                Console.WriteLine("0. Volver al menú principal");
                op = LeerEntero("Opción: ");

                switch (op)
                {
                    case 1: lista.Insertar(LeerEntero("Dato a insertar: ")); break;
                    case 2: lista.Eliminar(LeerEntero("Dato a eliminar: ")); break;
                    case 3: lista.Mostrar(); break;
                }
            } while (op != 0);
        }

        static void MenuListaCircularDoble()
        {
            Console.Clear();
            ListaCircularDoble lista = new ListaCircularDoble();
            int op;
            do
            {
                Console.WriteLine("\n--- LISTA CIRCULAR DOBLE ---");
                Console.WriteLine("1. Insertar dato");
                Console.WriteLine("2. Eliminar dato");
                Console.WriteLine("3. Mostrar hacia adelante");
                Console.WriteLine("4. Mostrar hacia atrás");
                Console.WriteLine("0. Volver al menú principal");
                op = LeerEntero("Opción: ");

                switch (op)
                {
                    case 1: lista.Insertar(LeerEntero("Dato a insertar: ")); break;
                    case 2: lista.Eliminar(LeerEntero("Dato a eliminar: ")); break;
                    case 3: lista.MostrarAdelante(); break;
                    case 4: lista.MostrarAtras(); break;
                }
            } while (op != 0);
        }
    }
}
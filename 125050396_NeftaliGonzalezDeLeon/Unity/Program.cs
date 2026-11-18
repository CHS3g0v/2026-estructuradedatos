using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrafoLaberinto : MonoBehaviour
{
    [Header("Modelos 3D")]
    public GameObject prefabPared;
    public GameObject prefabPiso;
    public GameObject prefabMoneda;
    public GameObject prefabJugadorBolita;
    public EnemigoBFS fantasmaBot;

    [Header("Podio y Sprites de Lugar (1ro al 6to)")]
    public Transform puntoCentroPodio;
    public Sprite[] spritesLugares;

    [Header("Configuración")]
    public float tamanoCasilla = 2f;
    public Nodo[,] cuadriculaNodos;
    public int monedasRestantes = 0;
    public bool minijuegoEnCurso = false;

    private Color[] coloresPorFigura = {
        Color.blue,                     // ID 0
        Color.green,                    // ID 1
        Color.yellow,                   // ID 2
        new Color(1f, 0.4f, 0.7f),      // ID 3 (Rosa)
        new Color(1f, 0.5f, 0f),        // ID 4 (Naranja)
        new Color(0.6f, 0.2f, 0.8f)     // ID 5 (Morada)
    };

    private string[] nombresColores = { "Azul", "Verde", "Amarilla", "Rosa", "Naranja", "Morada" };

    private Vector2Int[] puntosAparicion = {
        new Vector2Int(1, 1),
        new Vector2Int(1, 17),
        new Vector2Int(17, 1),
        new Vector2Int(17, 17),
        new Vector2Int(9, 1),
        new Vector2Int(9, 17)
    };

    public List<JugadorLaberinto> jugadoresEnMinijuego = new List<JugadorLaberinto>();
    private List<GameObject> objetosEscenario = new List<GameObject>();
    private List<GameObject> objetosPodio = new List<GameObject>();

    private int[,] mapaLaberinto = {
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        {1,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,1},
        {1,0,1,1,0,1,1,1,0,1,0,1,1,1,0,1,1,0,1},
        {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        {1,0,1,1,0,1,0,1,1,1,1,1,0,1,0,1,1,0,1},
        {1,0,0,0,0,1,0,0,0,1,0,0,0,1,0,0,0,0,1},
        {1,1,1,1,0,1,1,1,0,1,0,1,1,1,0,1,1,1,1},
        {1,0,0,0,0,1,0,0,0,0,0,0,0,1,0,0,0,0,1},
        {1,0,1,1,0,1,0,1,1,0,1,1,0,1,0,1,1,0,1},
        {1,0,0,0,0,0,0,1,0,0,0,1,0,0,0,0,0,0,1},
        {1,0,1,1,0,1,0,1,1,1,1,1,0,1,0,1,1,0,1},
        {1,0,0,0,0,1,0,0,0,0,0,0,0,1,0,0,0,0,1},
        {1,1,1,1,0,1,0,1,1,1,1,1,0,1,0,1,1,1,1},
        {1,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,1},
        {1,0,1,1,0,1,1,1,0,1,0,1,1,1,0,1,1,0,1},
        {1,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1,0,0,1},
        {1,1,0,1,0,1,0,1,1,1,1,1,0,1,0,1,0,1,1},
        {1,0,0,0,0,1,0,0,0,1,0,0,0,1,0,0,0,0,1},
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
    };

    private int filas, columnas;

    void Awake()
    {
        filas = mapaLaberinto.GetLength(0);
        columnas = mapaLaberinto.GetLength(1);
    }

    void Update()
    {
        if (!minijuegoEnCurso) return;

        int vivos = 0;
        foreach (var j in jugadoresEnMinijuego)
        {
            if (j != null && !j.estaEliminado) vivos++;
        }

        if (monedasRestantes <= 0 || vivos == 0)
        {
            minijuegoEnCurso = false;
            if (fantasmaBot != null) fantasmaBot.DetenerFantasma();
            Debug.Log("¡Minijuego terminado! Monedas restantes: " + monedasRestantes + " | Jugadores vivos: " + vivos);
        }
    }

    public void PrepararMinijuego(List<int> idsFigurasEnPartida, List<bool> esIAJugadores)
    {
        LimpiarTodo();
        CrearGrafoYModelos();

        List<Nodo> nodosLibres = new List<Nodo>();
        foreach (Nodo n in cuadriculaNodos)
        {
            if (n != null && !(n.gridX == 9 && n.gridY == 9))
                nodosLibres.Add(n);
        }

        int total = Mathf.Clamp(idsFigurasEnPartida.Count, 1, 6);

        for (int i = 0; i < total; i++)
        {
            int figId = Mathf.Clamp(idsFigurasEnPartida[i], 0, 5);
            Vector2Int coord = puntosAparicion[i];
            Nodo nodoSpawn = cuadriculaNodos[coord.x, coord.y];

            if (nodoSpawn == null)
                nodoSpawn = nodosLibres[(i * (nodosLibres.Count / total)) % nodosLibres.Count];

            GameObject bolita = Instantiate(prefabJugadorBolita, nodoSpawn.posicionMundo + new Vector3(0, 1f, 0), Quaternion.identity, transform);
            bolita.name = "Bolita_" + nombresColores[figId];

            Renderer rend = bolita.GetComponent<Renderer>();
            if (rend != null) rend.material.color = coloresPorFigura[figId];

            JugadorLaberinto ctrl = bolita.GetComponent<JugadorLaberinto>();
            if (ctrl == null) ctrl = bolita.AddComponent<JugadorLaberinto>();

            ctrl.Configurar(this, nodoSpawn, !esIAJugadores[i], nombresColores[figId]);
            ctrl.GetComponent<Renderer>().material.color = coloresPorFigura[figId];
            jugadoresEnMinijuego.Add(ctrl);
        }

        // Arranca congelado para la cuenta regresiva 3, 2, 1
        minijuegoEnCurso = false;
        if (fantasmaBot != null)
        {
            Nodo nodoCentro = cuadriculaNodos[9, 9];
            if (nodoCentro != null)
                fantasmaBot.transform.position = nodoCentro.posicionMundo + new Vector3(0, 1f, 0);
            fantasmaBot.gameObject.SetActive(true);
        }
    }

    public void ArrancarAccionMinijuego()
    {
        minijuegoEnCurso = true;
        if (fantasmaBot != null) fantasmaBot.ActivarFantasma();
    }

    void CrearGrafoYModelos()
    {
        cuadriculaNodos = new Nodo[filas, columnas];
        monedasRestantes = 0;

        for (int x = 0; x < filas; x++)
        {
            for (int y = 0; y < columnas; y++)
            {
                Vector3 posMundo = transform.position + new Vector3(x * tamanoCasilla, 0, y * tamanoCasilla);

                if (prefabPiso != null)
                    objetosEscenario.Add(Instantiate(prefabPiso, posMundo, Quaternion.identity, transform));

                if (mapaLaberinto[x, y] == 0)
                {
                    Nodo nuevoNodo = new Nodo(x, y, posMundo);
                    cuadriculaNodos[x, y] = nuevoNodo;

                    if (prefabMoneda != null && !(x == 9 && y == 9))
                    {
                        Vector3 posMoneda = posMundo + new Vector3(0, 1.0f, 0);
                        GameObject m = Instantiate(prefabMoneda, posMoneda, Quaternion.identity, transform);
                        nuevoNodo.monedaObj = m;
                        objetosEscenario.Add(m);
                        monedasRestantes++;
                    }
                }
                else if (mapaLaberinto[x, y] == 1)
                {
                    if (prefabPared != null)
                    {
                        Vector3 posPared = posMundo + new Vector3(0, 1f, 0);
                        objetosEscenario.Add(Instantiate(prefabPared, posPared, Quaternion.identity, transform));
                    }
                }
            }
        }

        for (int x = 0; x < filas; x++)
        {
            for (int y = 0; y < columnas; y++)
            {
                if (cuadriculaNodos[x, y] != null)
                {
                    if (x > 0 && cuadriculaNodos[x - 1, y] != null) cuadriculaNodos[x, y].vecinos.Add(cuadriculaNodos[x - 1, y]);
                    if (x < filas - 1 && cuadriculaNodos[x + 1, y] != null) cuadriculaNodos[x, y].vecinos.Add(cuadriculaNodos[x + 1, y]);
                    if (y > 0 && cuadriculaNodos[x, y - 1] != null) cuadriculaNodos[x, y].vecinos.Add(cuadriculaNodos[x, y - 1]);
                    if (y < columnas - 1 && cuadriculaNodos[x, y + 1] != null) cuadriculaNodos[x, y].vecinos.Add(cuadriculaNodos[x, y + 1]);
                }
            }
        }
    }

    public IEnumerator MostrarPodioConRuleta(Transform camaraTransform)
    {
        ArbolPodio arbol = new ArbolPodio();
        foreach (var j in jugadoresEnMinijuego)
        {
            arbol.Insertar(j);
        }

        List<JugadorLaberinto> ranking = arbol.ObtenerRankingOrdenado();
        int totalJugadores = ranking.Count;

        int[] indicesSpriteFinal = new int[totalJugadores];
        int lugarActual = 0;
        indicesSpriteFinal[0] = 0;

        for (int i = 1; i < totalJugadores; i++)
        {
            if (ranking[i].monedasRecolectadas < ranking[i - 1].monedasRecolectadas)
            {
                lugarActual++;
            }
            indicesSpriteFinal[i] = Mathf.Clamp(lugarActual, 0, spritesLugares.Length - 1);
        }

        Vector3 centro = puntoCentroPodio != null ? puntoCentroPodio.position : transform.position + new Vector3(18f, 20f, 18f);
        float separacion = 2.5f;
        float inicioX = -((totalJugadores - 1) * separacion) / 2f;

        List<SpriteRenderer> marcadoresCabeza = new List<SpriteRenderer>();

        for (int i = 0; i < totalJugadores; i++)
        {
            Vector3 posBolita = centro + new Vector3(inicioX + (i * separacion), 0, 0);
            GameObject esferaPodio = Instantiate(prefabJugadorBolita, posBolita, Quaternion.identity);
            esferaPodio.GetComponent<Renderer>().material.color = ranking[i].GetComponent<Renderer>().material.color;
            objetosPodio.Add(esferaPodio);

            GameObject objSprite = new GameObject("LugarSprite_" + i);
            objSprite.transform.SetParent(esferaPodio.transform);
            objSprite.transform.localPosition = new Vector3(0, 1.8f, 0);
            objSprite.transform.localScale = Vector3.one * 0.4f;

            SpriteRenderer sr = objSprite.AddComponent<SpriteRenderer>();
            marcadoresCabeza.Add(sr);
        }

        float tiempoRuleta = 0f;
        int maxOpcionesRuleta = Mathf.Min(totalJugadores, spritesLugares.Length);

        while (tiempoRuleta < 2.2f)
        {
            for (int i = 0; i < totalJugadores; i++)
            {
                if (maxOpcionesRuleta > 0)
                {
                    int randomSprite = Random.Range(0, maxOpcionesRuleta);
                    marcadoresCabeza[i].sprite = spritesLugares[randomSprite];
                }
                if (camaraTransform != null)
                    marcadoresCabeza[i].transform.LookAt(camaraTransform);
            }
            tiempoRuleta += 0.08f;
            yield return new WaitForSeconds(0.08f);
        }

        for (int i = 0; i < totalJugadores; i++)
        {
            if (spritesLugares.Length > 0)
                marcadoresCabeza[i].sprite = spritesLugares[indicesSpriteFinal[i]];

            Debug.Log("Podio -> Jugador " + ranking[i].colorNombre + " | Monedas: " + ranking[i].monedasRecolectadas + " | Lugar: " + (indicesSpriteFinal[i] + 1) + "°");
        }

        yield return new WaitForSeconds(3.5f);
    }

    public void LimpiarTodo()
    {
        foreach (var j in jugadoresEnMinijuego) if (j != null) Destroy(j.gameObject);
        jugadoresEnMinijuego.Clear();

        foreach (var obj in objetosEscenario) if (obj != null) Destroy(obj);
        objetosEscenario.Clear();

        foreach (var p in objetosPodio) if (p != null) Destroy(p);
        objetosPodio.Clear();
    }

    public Nodo EncontrarMonedaMasCercana(Nodo inicio)
    {
        if (inicio == null) return null;
        Queue<Nodo> frontera = new Queue<Nodo>();
        frontera.Enqueue(inicio);

        foreach (Nodo n in cuadriculaNodos)
            if (n != null) { n.visitado = false; n.nodoPadre = null; }
        inicio.visitado = true;

        while (frontera.Count > 0)
        {
            Nodo actual = frontera.Dequeue();
            if (actual.monedaObj != null) return actual;

            foreach (Nodo vecino in actual.vecinos)
            {
                if (!vecino.visitado)
                {
                    vecino.visitado = true;
                    vecino.nodoPadre = actual;
                    frontera.Enqueue(vecino);
                }
            }
        }
        return null;
    }

    public List<Nodo> EncontrarRutaBFS(Nodo inicio, Nodo meta)
    {
        if (inicio == null || meta == null) return null;
        Queue<Nodo> frontera = new Queue<Nodo>();
        frontera.Enqueue(inicio);

        foreach (Nodo n in cuadriculaNodos)
            if (n != null) { n.visitado = false; n.nodoPadre = null; }
        inicio.visitado = true;

        while (frontera.Count > 0)
        {
            Nodo actual = frontera.Dequeue();
            if (actual == meta) break;

            foreach (Nodo vecino in actual.vecinos)
            {
                if (!vecino.visitado)
                {
                    vecino.visitado = true;
                    vecino.nodoPadre = actual;
                    frontera.Enqueue(vecino);
                }
            }
        }

        List<Nodo> rutaFinal = new List<Nodo>();
        Nodo rastreo = meta;
        while (rastreo != null)
        {
            rutaFinal.Add(rastreo);
            rastreo = rastreo.nodoPadre;
        }
        rutaFinal.Reverse();
        return rutaFinal;
    }
}
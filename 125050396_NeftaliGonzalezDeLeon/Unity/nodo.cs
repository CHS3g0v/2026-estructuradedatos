using System.Collections.Generic;
using UnityEngine;

public class Nodo
{
    public int gridX;
    public int gridY;
    public Vector3 posicionMundo;
    public GameObject monedaObj; // Referencia directa al objeto moneda en este vértice
    
    public List<Nodo> vecinos;
    public bool visitado;
    public Nodo nodoPadre; 

    public Nodo(int x, int y, Vector3 pos)
    {
        gridX = x;
        gridY = y;
        posicionMundo = pos;
        vecinos = new List<Nodo>();
    }
}
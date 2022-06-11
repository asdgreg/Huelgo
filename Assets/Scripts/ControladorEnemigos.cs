using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorEnemigos : MonoBehaviour
{
    public GameObject enemigo;          
    public float spawnTime = 3f;         
    public Transform[] puntosEnemigos;
    public bool seguir;  


    void Start()
    {
        seguir = true;
        InvokeRepeating("Instanciar", spawnTime, spawnTime);
    }


    void Instanciar()
    {
        if (!seguir){
            return;
        }
      
        int spawnPointIndex = Random.Range(0, puntosEnemigos.Length);
        Instantiate(enemigo, puntosEnemigos[spawnPointIndex].position, puntosEnemigos[spawnPointIndex].rotation);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zona_Muerte : MonoBehaviour

{
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            Debug.Log("El jugador ha muerto");
        }
    }
}
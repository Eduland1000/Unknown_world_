using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camara2D : MonoBehaviour

{
  public Transform personaje;  // El transform del personaje
    public float offsetX = 5f;   // Distancia horizontal desde el personaje
    public float offsetY = 2f;   // Distancia vertical desde el personaje
    public float smoothSpeed = 0.125f; // Velocidad de suavizado de la cámara
    
    void LateUpdate()
    {
        // Establece la nueva posición de la cámara
        Vector3 desiredPosition = new Vector3(personaje.position.x + offsetX, personaje.position.y + offsetY, transform.position.z);
        
        // Interpola suavemente hacia la posición deseada
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        
        // Aplica la nueva posición a la cámara
        transform.position = smoothedPosition;
    }
}

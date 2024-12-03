using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camara2D : MonoBehaviour

{
  public Transform personaje;  
    public float offsetX = 5f;   
    public float offsetY = 2f;   
    public float smoothSpeed = 0.125f; 
    
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

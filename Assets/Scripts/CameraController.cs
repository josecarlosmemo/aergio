using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cameraPositon;

    void Update()
    {
        //* Movemos este gameobject a la misma posición del que recibimoms como referencia (cameraPosition)
        transform.position = cameraPositon.position;
    }
}

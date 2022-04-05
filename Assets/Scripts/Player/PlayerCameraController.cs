using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [Header("Sensibility")]
    //TODO Agregar como ajuste más adelante
    public float sensibility;

    public Transform orientation;

    float xRotation;
    float yRotation;

    bool isCameraDisabled;

    void Start()
    {
        //* Nos permite fijar y ocultar el cursor en la pantalla
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Drone.OnPlayerSpotted += disableCamera;
        PlayerController.onReachedFinish += disableCamera;
        UIManager.onUIStart += disableCamera;
        UIManager.onUIFinish += enableCamera;
    }

    void Update()
    {
        if (!isCameraDisabled)
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensibility;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensibility;
            

            yRotation += mouseX;
            xRotation -= mouseY;

            //* La función Math.Clamp nos permite fijar un valor dentro de un rango.
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);


            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0); // Eje X
            orientation.rotation = Quaternion.Euler(0, yRotation, 0); // Eje y
        }
    }

    void disableCamera()
    {
        isCameraDisabled = true;
    }

    void enableCamera(){
        isCameraDisabled = false;
    }

    void OnDestroy()
    {
        Drone.OnPlayerSpotted -= disableCamera;
        PlayerController.onReachedFinish -= disableCamera;
        UIManager.onUIStart -= disableCamera;
        UIManager.onUIFinish -= enableCamera;


    }

    
}

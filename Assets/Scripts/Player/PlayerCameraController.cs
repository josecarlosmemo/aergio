using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    //TODO FIXME
    [Header("Sensibility")]
    public float sensibility;

    public Transform orientation;

    float xRotation;
    float yRotation;

    bool isCameraDisabled;

    void Start()
    {
        //* Nos permite fijar y ocultar el cursor en la pantalla
        disableCursor();

        if (PlayerPrefs.HasKey("Sensibility"))
        {
            sensibility = PlayerPrefs.GetFloat("Sensibility");
        } else{
             PlayerPrefs.SetFloat("Sensibility",400f);
        }


        Drone.OnPlayerSpotted += disableCamera;
        PlayerController.onReachedFinish += disableCamera;
        CanvasManager.onUIStart += disableCamera;
        CanvasManager.onUIFinish += enableCamera;

        CanvasManager.onPauseStart+= enableCursor;
        CanvasManager.onPauseStart += disableCamera;
        CanvasManager.onPauseEnd += disableCursor;
        CanvasManager.onPauseEnd += enableCamera;
    }

    void Update()
    {
        if (!isCameraDisabled)
        {
            sensibility = PlayerPrefs.GetFloat("Sensibility");

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
        enableCursor();

        Drone.OnPlayerSpotted -= disableCamera;
        PlayerController.onReachedFinish -= disableCamera;
        CanvasManager.onUIStart -= disableCamera;
        CanvasManager.onUIFinish -= enableCamera;
        CanvasManager.onPauseStart -= enableCursor;
        CanvasManager.onPauseEnd -= disableCursor;
        CanvasManager.onPauseStart -= disableCamera;
        CanvasManager.onPauseEnd -= enableCamera;



    }

    void enableCursor(){
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

    }

    void disableCursor(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    
}

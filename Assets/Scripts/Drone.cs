using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    // Evento a correr en caso de que el jugador sea atrapado
    public static event System.Action OnPlayerSpotted;

    

    [Header("Movement")]
    // Camino que debe seguir el dron
    public Transform path;

    // Velocidad en la que el dron recorre el camino
    public float droneSpeed = 5;

    // Velocidad en la que el dron gira para dirigirse de un punto a otro
    public float turnSpeed = 90;

    // Tiempo el cual cada dron espera en un punto antes de moverse al siguiente
    public float waitFor = 0.3f;

    [Header("Light")]
    // Luz del dron
    public Light droneLight;

    // Layer para los Obstaculos que deben impedir la vista del dron
    public LayerMask obstacleLayer;

    [Header("Capture Settings")]
    // Distancia hacia delante que puede ver el dron
    public float droneViewDistance;

    // Tiempo que puede durar el jugador en la vista de un dron, antes de perder.

    public float playerGracePeriod = 0.5f;

    // Color normal de la luz
    private Color defaultDroneLightColor;

    // Timer: Tiempo que el jugador a sido visto
    private float playerInSightTimer;

    // Angulo el cual puede ver el dron
    private float droneViewAngle;

    // Posición del jugador
    private Transform player;

    private bool isMovementEnabled = true;

    

    void Start()
    {
        CanvasManager.onPauseStart += disableMovement;
        CanvasManager.onPauseEnd += enableMovement;


        // Encontramos al jugador
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // Establecemos parametros de la luz del dron
        droneViewAngle = droneLight.spotAngle;
        defaultDroneLightColor = droneLight.color;

        // Obtenemos los puntos del camino del dron
        Vector3[] points = new Vector3[path.childCount];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = path.GetChild(i).position;
            points[i] = new Vector3(points[i].x, transform.position.y, points[i].z);
        }

        // Coomenzamos a seguir el camino
        StartCoroutine(FollowPath(points));
    }

    void Update()
    {
        // Si el dron esta viendo al jugador incrementamos o decrementamos el timer
        if (isPlayerInView())
        {
            playerInSightTimer += Time.deltaTime;
        }
        else
        {
            playerInSightTimer -= Time.deltaTime;
        }

        // Normalizamos los valores del timer entre 0 y playerGracePeriod

        playerInSightTimer = Mathf.Clamp(playerInSightTimer, 0, playerGracePeriod);

        // Cambiamos el color de la luz en base al timer

        droneLight.color = Color.Lerp(
            defaultDroneLightColor,
            Color.red,
            playerInSightTimer / playerGracePeriod
        );
        // Si el jugador fue encontrado ejecutamos nuestro evento
        if (playerInSightTimer >= playerGracePeriod)
        {
            if (OnPlayerSpotted != null)
            {
                OnPlayerSpotted();
            }
        }
    }

    bool isPlayerInView()
    {
        // Si la distancia entre el dron y el jugador esta dentro de la distancia que puede ver el dron
        if (Vector3.Distance(transform.position, player.position) < droneViewDistance)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            float angleDronePlayer = Vector3.Angle(transform.forward, directionToPlayer);
            // Si el jugador se encuentra dentro del angulo en el cual el dron puede ver
            if (angleDronePlayer < droneViewAngle / 2f)
            {
                //  Si no hay un obstaculo entre el  jugador y el dron
                if (!Physics.Linecast(transform.position, player.position, obstacleLayer))
                {
                    return true;
                }
            }
        }
        return false;
    }

    IEnumerator FollowPath(Vector3[] points)
    {
        transform.position = points[0];

        int nextPointIndex = 1;
        Vector3 nextPoint = points[nextPointIndex];
        transform.LookAt(nextPoint);

        while (true)
        {
            if(isMovementEnabled){

                 transform.position = Vector3.MoveTowards(
                transform.position,
                nextPoint,
                droneSpeed * Time.deltaTime
            );

            }
           


            if (transform.position == nextPoint)
            {
                nextPointIndex = (nextPointIndex + 1) % points.Length;
                nextPoint = points[nextPointIndex];
                yield return new WaitForSeconds(waitFor);
                yield return StartCoroutine(Turn(nextPoint));
            }
            yield return null;
        }
    }

    IEnumerator Turn(Vector3 target)
    {
        Vector3 targetDirection = (target - transform.position).normalized;
        float targetAngle = 90 - Mathf.Atan2(targetDirection.z, targetDirection.x) * Mathf.Rad2Deg;

        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f)
        {
            if(isMovementEnabled){
                float angle = Mathf.MoveTowardsAngle(
                transform.eulerAngles.y,
                targetAngle,
                turnSpeed * Time.deltaTime
            );

            transform.eulerAngles = Vector3.up * angle;
                
            }


            
            yield return null;
        }
    }

    void OnDrawGizmos()
    {
        Vector3 start = path.GetChild(0).position;
        Vector3 prev = start;

        foreach (Transform point in path)
        {
            Gizmos.DrawSphere(point.position, .3f);
            Gizmos.DrawLine(prev, point.position);
            prev = point.position;
        }
        Gizmos.DrawLine(prev, start);

        Gizmos.color = Color.red;

        Gizmos.DrawRay(transform.position, transform.forward * droneViewDistance);
    }

    void enableMovement(){
        isMovementEnabled = true;
    }
    void disableMovement(){
        isMovementEnabled = false;
    }

     void OnDestroy() {
        CanvasManager.onPauseStart -= disableMovement;
        CanvasManager.onPauseEnd -= enableMovement;

        
    }
}

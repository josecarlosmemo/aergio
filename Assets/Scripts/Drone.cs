using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    public static event System.Action OnPlayerSpotted;
    public Transform path;
    public Light droneLight;
    private Transform player;

    private Color defaultDroneLightColor;
    public LayerMask obstacleLayer;
    public float droneViewDistance;

    public float playerGracePeriod = 0.5f;

    private float playerInSightTimer;



    private float droneViewAngle;
    public float droneSpeed = 5;
    public float turnSpeed = 90;



    public float waitFor = 0.3f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        droneViewAngle = droneLight.spotAngle;
        defaultDroneLightColor = droneLight.color;
        Vector3[] points = new Vector3[path.childCount];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = path.GetChild(i).position;
            points[i] = new Vector3(points[i].x, transform.position.y, points[i].z);
        }

        StartCoroutine(FollowPath(points));

    }

    void Update()
    {
        if (isPlayerInView())
        {
            playerInSightTimer += Time.deltaTime;
        }
        else
        {
            playerInSightTimer -= Time.deltaTime;

        }

        playerInSightTimer = Mathf.Clamp(playerInSightTimer, 0, playerGracePeriod);

        droneLight.color = Color.Lerp(defaultDroneLightColor, Color.red, playerInSightTimer / playerGracePeriod);
        //! Si el jugador fue encontrado
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
        if (Vector3.Distance(transform.position, player.position) < droneViewDistance)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            float angleDronePlayer = Vector3.Angle(transform.forward, directionToPlayer);
            if (angleDronePlayer < droneViewAngle / 2f)
            {
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

        int targetWaypointIndex = 1;
        Vector3 targetWaypoint = points[targetWaypointIndex];
        transform.LookAt(targetWaypoint);

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, droneSpeed * Time.deltaTime);
            if (transform.position == targetWaypoint)
            {
                targetWaypointIndex = (targetWaypointIndex + 1) % points.Length;
                targetWaypoint = points[targetWaypointIndex];
                yield return new WaitForSeconds(waitFor);
                yield return StartCoroutine(Turn(targetWaypoint));
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
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
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

}

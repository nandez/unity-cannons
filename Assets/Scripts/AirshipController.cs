using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirshipController : MonoBehaviour
{
    public float speed = 3f;
    public Transform[] waypoints;


    public Transform helix;
    public float helixRotationSpeed = 50f;

    private int currentWaypointIdx = 0;

    void Start()
    {
 
    }

    void Update()
    {
        // Rotates the helix on x axis
        helix.Rotate(Vector3.right * helixRotationSpeed * Time.deltaTime);

        // Handles waypoint movement..
        if (currentWaypointIdx < waypoints.Length)
        {
            var waypoint = waypoints[currentWaypointIdx];
            transform.position = Vector3.MoveTowards(transform.position, waypoint.position, Time.deltaTime * speed);

            var targetRotation = Quaternion.LookRotation(waypoint.position - transform.position, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime);


            if (Vector3.Distance(transform.position, waypoint.position) < 0.5f)
            {
                currentWaypointIdx++;

                if (currentWaypointIdx >= waypoints.Length)
                    currentWaypointIdx = 0;
            }
        }
    }
}

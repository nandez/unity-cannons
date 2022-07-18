using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeppelingController : MonoBehaviour
{
    public float speed;
    public Transform[] waypoints;
    
    private int currentWaypointIdx = 0;

    void Start()
    {
 
    }

    void Update()
    {
        if (currentWaypointIdx < waypoints.Length)
        {
            var waypoint = waypoints[currentWaypointIdx];
            transform.position = Vector3.MoveTowards(transform.position, waypoint.position, Time.deltaTime * speed);
            transform.LookAt(waypoint.position);

            if (Vector3.Distance(transform.position, waypoint.position) < 0.5f)
            {
                currentWaypointIdx++;

                if (currentWaypointIdx >= waypoints.Length)
                    currentWaypointIdx = 0;
            }
        }
    }
}

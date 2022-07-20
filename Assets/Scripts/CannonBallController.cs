using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallController : MonoBehaviour
{
    public float speed = 5f;

    private Transform target;

    void Update()
    {
        if(target != null)
        {
            transform.LookAt(target);
            transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * speed);
        }
        else
        {
            // TODO: to see what happens when cannon ball was fired, but target
            // gets out of tower range..
        }
    }

    void OnTriggerEnter(Collider col)
    {
        // Validates the cannon ball hits the desired target..
        if (col.gameObject.transform == target)
        {
            //TODO: to create some particles

            // Finally destroys the cannon ball..
            Destroy(gameObject, 0.1f);
            return;
        }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}

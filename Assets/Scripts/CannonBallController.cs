using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallController : MonoBehaviour
{
    [SerializeField] protected float lifeTime;
    [SerializeField] protected int damage;

    void Start()
    {
        // Destruimos el objeto una vez cumplido el tiempo de vida.
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            // Destruimos el objeto.
            Destroy(gameObject);
        }
    }
}

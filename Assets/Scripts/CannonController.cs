using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{

    private bool targetLocked;
    private 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("enemyBug") && !targetLocked)
        {
            /*twr.target = col.gameObject.transform;
            curTarget = col.gameObject;
            targetLocked = true;*/
        }

    }
}

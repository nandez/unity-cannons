using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapController : MonoBehaviour
{
    [SerializeField] protected Transform player;
    [SerializeField] protected bool rotateWithPlayer = false;

    void LateUpdate()
    {
        // Actualizamos la posición de la cámara de la mini-map.
        if (player != null)
        {
            transform.position = new Vector3(player.position.x, transform.position.y, player.position.z);

            // Actualizamos la rotación de la cámara de la mini-map.
            if (rotateWithPlayer)
                transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
        }
    }
}

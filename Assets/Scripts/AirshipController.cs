using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirshipController : MonoBehaviour
{
    [SerializeField] protected LayerMask navigationLayer;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float rotationSpeed;
    [SerializeField] protected float movementMinDistance = 0.25f;

    [SerializeField] protected Transform helix;
    [SerializeField] protected float helixRotationSpeed = 100f;

    protected Vector3 target;
    protected bool shouldMove = false;

    //public Transform[] waypoints;
    //private int currentWaypointIdx = 0;

    void Start()
    {

    }

    void Update()
    {
        // Rotates the helix on x axis
        helix.Rotate(Vector3.right * helixRotationSpeed * Time.deltaTime);

        // Proyectamos un rayo desde la cámara hasta el punto donde se hizo click
        // y verificamos la colisión con la capa de navegación..
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f, navigationLayer))
        {
            // Si el jugador presiona el click izquierdo, guardamos la posición para
            // movernos.
            if (Input.GetMouseButtonDown(0))
            {
                target = hitInfo.point;
                shouldMove = true;
            }
            else if (Input.GetMouseButtonDown(1))
            {
                // TODO: implementar el ataque..
            }
        }

        // Mantenemos la misma altura en todo momento..
        target.y = transform.position.y;

        if (shouldMove)
        {
            // Calculamos la dirección y la distancia.
            var direction = target - transform.position;
            var distance = direction.magnitude;

            // Si la distancia es mayor al mínimo preestablecido,
            // giramos y nos movemos hacia el punto objetivo.
            if (distance > movementMinDistance)
            {
                var rotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
            }
            else
            {
                // En este caso, ya estamos cerca del punto objetivo, por lo que
                // cancelamos el movimiento
                shouldMove = false;
            }
        }
    }
}

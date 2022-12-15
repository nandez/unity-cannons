using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] protected string navigationLayerName;
    [SerializeField] protected string destroyablesLayerName;
    [SerializeField] protected string destroyableTag;
    [SerializeField] protected float raycastDistance = 250f;
    [SerializeField] private MouseCursorController mouseCursorCtrl;
    [SerializeField] private Camera mainCamera;

    [Header("Movement Settings")]
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float rotationSpeed;
    [SerializeField] protected float movementMinDistance = 0.25f;

    [Header("Attack Settings")]
    [SerializeField] protected float attackRange;
    [SerializeField] protected CannonController cannonCtrl;
    [SerializeField] protected bool drawAttackRangeGizmo;

    [Header("References")]
    [SerializeField] protected HealthController playerHealthCtrl;
    [SerializeField] protected BarController playerHealthBarCtrl;

    protected Vector3 destination; // Indica la posición a la que nos queremos mover.
    protected Transform target; // Indica el objetivo al que queremos atacar.
    protected bool shouldMove = false;
    protected int interactionLayer;

    void Start()
    {
        // Calculamos la mascara de interacción para determinar a donde podemos movernos y/o atacar.
        interactionLayer = LayerMask.GetMask(navigationLayerName, destroyablesLayerName);

        playerHealthCtrl.OnHealthUpdated += OnPlayerHealthUpdatedHandler;
    }

    void Update()
    {

        // Proyectamos un rayo desde la cámara hasta el punto donde se hizo clicky verificamos la colisión con las capas de navegación y la de objetos destruibles
        // para determinar a donde movernos y/o atacar.
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, raycastDistance, interactionLayer))
        {
            // Verificamos la capa con la que colisionó el rayo para determina que tipo de cursor mostrar.
            mouseCursorCtrl.Type = hitInfo.transform.gameObject.CompareTag(destroyableTag)
                 ? MouseCursorController.CursorType.Attack
                 : MouseCursorController.CursorType.Normal;

            // Cuando el jugador hace click derecho, nos movemos hacia el punto objetivo.
            if (Input.GetMouseButton(1))
            {
                destination = hitInfo.point;
                shouldMove = true;

                // Si el objeto es destruible, seteamos el objetivo para atacarlo
                target = hitInfo.transform.gameObject.CompareTag(destroyableTag) ? hitInfo.transform : null;
            }
        }


        // Mantenemos la misma altura en todo momento..
        destination.y = transform.position.y;

        if (shouldMove)
        {
            // Calculamos la dirección y la distancia.
            var direction = destination - transform.position;
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

        // Verificamos si tenemos un objetivo para atacar.
        if (target != null)
        {
            // Si estamos en el rango de ataque, invocamos al controlador de cañon
            // para setear el objetivo y disparar.
            var distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget <= attackRange)
            {
                this.cannonCtrl.SetTarget(target.position, gameObject);
                shouldMove = false;
            }
        }
    }

    private void OnPlayerHealthUpdatedHandler(int currentHp, int maxHp)
    {
        playerHealthBarCtrl.UpdateValue(currentHp, maxHp);
    }

    private void OnDrawGizmos()
    {
        // Dibujamos el rango de ataque..
        if (drawAttackRangeGizmo)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}

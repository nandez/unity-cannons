using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TowerController : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] protected float attackRange;


    [Header("Component References")]
    [SerializeField] protected HealthController healthCtrl;
    [SerializeField] protected BarController healthBarCtrl;
    [SerializeField] protected CannonController cannonCtrl;

    [Header("Events")]
    public UnityAction OnTowerDestroyed;

    [Header("Misc")]
    [SerializeField] protected bool drawAttackRangeGizmo = true;


    protected PlayerController player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();

        // Inicializamos las referencias a los componentes que controla los puntos de vida
        // y asignamos los handlers para los eventos.
        healthCtrl.OnDeath += OnEnemyDeathHandler;
        healthCtrl.OnHealthUpdated += OnEnemyHealthUpdatedHandler;
    }

    void Update()
    {
        // Si detectamos al jugador en el rango de ataque, seteamos la posición como objetivo..
        var distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= attackRange)
            cannonCtrl.SetTarget(player.transform.position + player.transform.forward, gameObject);
    }

    protected virtual void OnEnemyDeathHandler()
    {
        // Cuando el enemigo muere, invocamos el evento OnTowerDestroyed para notificar a los listeners.
        OnTowerDestroyed?.Invoke();

        // TODO: particulas de destrucción?
        Destroy(gameObject, 0.5f);
    }

    protected virtual void OnEnemyHealthUpdatedHandler(int currentHitPoints, int maxHitPoints)
    {
        // Cuando el enemigo recibe daño, actualizamos la barra de vida.
        healthBarCtrl?.UpdateValue(currentHitPoints, maxHitPoints);
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

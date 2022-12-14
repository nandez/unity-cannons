using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallController : MonoBehaviour
{
    [SerializeField] protected float lifeTime;
    [SerializeField] protected int damage;

    protected Rigidbody rbComponent;

    void Start()
    {
        rbComponent = GetComponent<Rigidbody>();

        // Destruimos el objeto una vez cumplido el tiempo de vida.
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision col)
    {
        DealDamage(col.collider);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            DealDamage(col);
        }
        else if (col.gameObject.CompareTag("Water"))
        {
            // Cuando la bala de cañon impacta con el agua, detenemos el movimiento
            // y dejamos que la gravedad actue sobre el objeto.
            rbComponent.velocity = Vector3.zero;
        }
    }

    protected void DealDamage(Collider col)
    {
        // Obtenemos el componente HealthController del objeto con el que colisionamos.
        var healthCtrl = col.gameObject.GetComponent<HealthController>();

        // Si el objeto tiene un componente HealthController, le aplicamos daño.
        if (healthCtrl != null)
        {
            healthCtrl.TakeDamage(damage);
            damage = 0; // Seteamos en 0 el daño para evitar que se aplique más de una vez.
        }

        Destroy(gameObject);
    }
}

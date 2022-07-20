using System.Collections;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    public GameObject cannonPivot;
    public GameObject firingPoint;

    public float rotationSpeed = 5f;
    public GameObject cannonBallPrefab;
    public float shootCooldown = 5f;

    private GameObject target;
    private bool isShooting;

    private void Start()
    {
    }

    private void Update()
    {
        if (target != null)
        {
            var direction = target.transform.position - cannonPivot.transform.position;
            var rotation = Quaternion.LookRotation(direction);
            cannonPivot.transform.rotation = Quaternion.Slerp(cannonPivot.transform.rotation, rotation, rotationSpeed * Time.deltaTime);

            if (!isShooting)
                StartCoroutine(Shoot());
        }
        else
        {
            isShooting = false;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        Debug.Log($"OnTriggerEnter: {col.name} - {col.tag}");

        if (col.gameObject.CompareTag("Airship"))
            target = col.gameObject;
    }

    private void OnTriggerExit(Collider col)
    {
        Debug.Log("OnTriggerExit");
        Debug.Log($"OnTriggerEnter: {col.name} - {col.tag}");

        if (col.gameObject.CompareTag("Airship"))
            target = null;
    }

    private IEnumerator Shoot()
    {
        isShooting = true;
        yield return new WaitForSeconds(shootCooldown);

        if (target != null)
        {
            // Creates a cannon ball from prefab on firing point..
            var cannonBall = Instantiate(cannonBallPrefab, firingPoint.transform.position, Quaternion.identity);
            cannonBall.GetComponent<CannonBallController>().SetTarget(target.transform);
        }

        isShooting = false;
    }
}
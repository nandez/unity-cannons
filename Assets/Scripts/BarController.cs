using UnityEngine;
using UnityEngine.UI;

public class BarController : MonoBehaviour
{
    [SerializeField] protected Image foregroundImg;
    [SerializeField] protected bool invertFilling = false; // Indica si el relleno de la barra debe invertirse.
    [SerializeField] protected bool followCamera = false; // Indica si la barra debe seguir la cámara.
    [SerializeField] protected Camera cameraToFollow; // Cámara a la que seguir.

    public void UpdateValue(float amount, float maxAmount)
    {
        if (maxAmount <= 0f)
            maxAmount = 1f;

        var fillAmount = Mathf.Lerp(0f, 1f, amount / maxAmount);

        if (invertFilling)
            fillAmount = 1f - fillAmount;

        foregroundImg.fillAmount = fillAmount;
    }

    private void LateUpdate()
    {
        if (followCamera)
        {
            transform.LookAt(cameraToFollow.transform);
            transform.Rotate(0, 180, 0);
        }
    }
}

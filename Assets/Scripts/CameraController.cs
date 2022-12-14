using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 10f;
    [SerializeField] protected float borderThickness = 10f;
    [SerializeField] protected float zoomSpeed = 20f;
    [SerializeField] protected float maxHeight = 30f;
    [SerializeField] protected float minHeight = 10f;
    [SerializeField] protected Vector2 mapSize;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        var position = transform.position;

        if (Input.GetKey(KeyCode.W) || Input.mousePosition.y >= Screen.height - borderThickness)
            position.z += moveSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.S) || Input.mousePosition.y <= borderThickness)
            position.z -= moveSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.D) || Input.mousePosition.x >= Screen.width - borderThickness)
            position.x += moveSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.A) || Input.mousePosition.x <= borderThickness)
            position.x -= moveSpeed * Time.deltaTime;

        var sccroll = Input.GetAxis("Mouse ScrollWheel");
        position.y -= sccroll * zoomSpeed * 100f * Time.deltaTime;

        position.x = Mathf.Clamp(position.x, -mapSize.x, mapSize.x);
        position.y = Mathf.Clamp(position.y, minHeight, maxHeight);
        position.z = Mathf.Clamp(position.z, -mapSize.y, mapSize.y);

        transform.position = position;
    }
}

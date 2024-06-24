using UnityEngine;

[RequireComponent(typeof(Camera))]
public class TopDownCameraBehaviour : MonoBehaviour
{
    public float panSpeed = 10f;
    public float panBorderThickness = 10f;
    public float scrollSpeed = 5f;
    public float minX;
    public float maxX = 10f;
    public float minY = 10f;
    public float maxY = 80f;
    public float minZ = -100f;
    public float maxZ = -1f;
    public float zoomSpeed = 10f;
    public MapGenerator Map;


    private void Start()
    {
        minX = 0;
        maxX = Map.Width;
        minY = 0;
        maxY = Map.Height;
        minZ = -Mathf.Min(Map.Height, Map.Width);
        maxZ = -Mathf.Min(Map.Height, Map.Width) / 4;

        var pos = transform.position;
        pos.z = -10;
    }

    private void Update()
    {
        var pos = transform.position;

        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            pos.y += panSpeed * Time.deltaTime;
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
        }

        if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
        {
            pos.y -= panSpeed * Time.deltaTime;
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
        }

        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            pos.x += panSpeed * Time.deltaTime;
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
        }

        if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
        {
            pos.x -= panSpeed * Time.deltaTime;
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
        }

        var scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.z -= -scroll * 1000 * scrollSpeed * Time.deltaTime;
        pos.z = Mathf.Clamp(pos.z, minZ, maxZ);

        transform.position = pos;
    }
}
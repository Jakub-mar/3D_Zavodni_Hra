using UnityEngine;

public class CarRotateMouse : MonoBehaviour
{
    public float rotationSpeed = 5f;

    private float lastMouseX;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMouseX = Input.mousePosition.x;
        }

        if (Input.GetMouseButton(0))
        {
            float deltaX = Input.mousePosition.x - lastMouseX;
            lastMouseX = Input.mousePosition.x;

            transform.Rotate(Vector3.up, -deltaX * rotationSpeed * Time.deltaTime, Space.World);
        }
    }
}

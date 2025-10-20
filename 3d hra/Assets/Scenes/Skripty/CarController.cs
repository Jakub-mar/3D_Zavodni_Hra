using UnityEngine;
using UnityEngine.InputSystem; // důležité!

public class CarController : MonoBehaviour
{
    public float speed = 10f;
    public float rotationSpeed = 100f;

    private Vector2 moveInput;

    void Update()
    {
        // čtení WASD z nového Input Systemu
        moveInput = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) moveInput.y += 1;
        if (Keyboard.current.sKey.isPressed) moveInput.y -= 1;
        if (Keyboard.current.aKey.isPressed) moveInput.x -= 1;
        if (Keyboard.current.dKey.isPressed) moveInput.x += 1;

        // pohyb dopředu/dozadu
        float move = moveInput.y * speed * Time.deltaTime;
        transform.Translate(0, 0, move);

        // otáčení
        float turn = moveInput.x * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, turn, 0);
    }
}
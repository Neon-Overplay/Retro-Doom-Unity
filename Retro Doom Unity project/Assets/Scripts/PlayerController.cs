using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 6f;
    public float acceleration = 20f;
    public float deceleration = 25f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 3f;

    [Header("Cursor")]
    public bool lockAndHideCursorOnStart = true;

    CharacterController controller;

    Vector3 currentVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (lockAndHideCursorOnStart)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        Move();
        Look();
    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 inputDir = (transform.right * h + transform.forward * v).normalized;
        Vector3 targetVelocity = inputDir * walkSpeed;

        if (inputDir.sqrMagnitude > 0.01f)
        {
            currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, acceleration * Time.deltaTime);
        }
        else
        {
            currentVelocity = Vector3.MoveTowards(currentVelocity, Vector3.zero, deceleration * Time.deltaTime);
        }

        controller.Move(currentVelocity * Time.deltaTime);
    }

    void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * 100f * Time.deltaTime;
        transform.Rotate(0f, mouseX, 0f);
    }
}
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 6f;
    public float groundDistance = 1.1f;
    public LayerMask groundLayer;

    public float sprintMultiplier = 2f;

    Rigidbody rb;
    bool grounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        CheckGround();
        Move();
        Jump();
    }

    void CheckGround()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, groundDistance, groundLayer);
    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = (transform.forward * v + transform.right * h).normalized;

        float speed = moveSpeed;

        // Velocidades por direcci�n
        if (v > 0) speed *= 1f;          // adelante
        if (v < 0) speed *= 0.5f;        // atr�s
        if (h != 0) speed *= 0.75f;      // lados

        // Sprint
        if (grounded && Input.GetKey(KeyCode.LeftShift) && v > 0)
            speed *= sprintMultiplier;

        // Aire
        if (!grounded)
            speed *= 0.5f;

        Vector3 vel = new Vector3(moveDir.x * speed, rb.linearVelocity.y, moveDir.z * speed);
        rb.linearVelocity = vel;
    }

    void Jump()
    {
        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        }
    }
}

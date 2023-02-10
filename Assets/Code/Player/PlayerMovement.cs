using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f;

    private Rigidbody2D rb;
    private Vector2 movement;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        movement = new Vector2(horizontal, vertical);
    }

    private void FixedUpdate()
    {
        rb.velocity = movement * speed;
    }
}

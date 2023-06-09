using UnityEngine;

public class HalfGravity : MonoBehaviour
{
    [SerializeField] private float jumpForce;

    private AudioSource jumpSound;

    private Vector2 initialGravity;
    private Vector2 halfGravity;

    private void Awake()
    {
        initialGravity = Physics2D.gravity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        jumpSound = collision.GetComponent<AudioSource>();
        halfGravity = Physics2D.gravity / 2;

        if (collision.CompareTag("Player") && rb.velocity.y < 0)
        {
            Physics2D.gravity = halfGravity;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpSound.PlayOneShot(jumpSound.clip);
            Destroy(gameObject);
        }
    }
}

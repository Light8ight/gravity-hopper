using UnityEngine;

public class DefaultAndSpring : MonoBehaviour
{
    [SerializeField] private float jumpForce;

    private AudioSource jumpSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        jumpSound = collision.GetComponent<AudioSource>();
        if (collision.CompareTag("Player") && rb.velocity.y < 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.deltaTime);
            jumpSound.PlayOneShot(jumpSound.clip);
        }
    }
}

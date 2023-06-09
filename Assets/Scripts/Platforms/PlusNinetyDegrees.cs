using UnityEngine;

public class PlusNinetyDegrees : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    [SerializeField] private float targetPosition;

    private GameObject player;
    private AudioSource jumpSound;

    private float initialOrthographicSize = 5f;
    private float targetOrthographicSize = 8f;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        jumpSound = collision.GetComponent<AudioSource>();

        if (collision.CompareTag("Player") && rb.velocity.y < 0)
        {
            Camera.main.transform.position = new Vector3(0f, player.transform.position.y + targetPosition, -10f);
            Camera.main.orthographicSize = targetOrthographicSize;

            float randomRotation = Random.Range(0, 2) == 0 ? -90f : 90f;
            Camera.main.transform.rotation = Quaternion.Euler(0f, 0f, Camera.main.transform.rotation.eulerAngles.z + randomRotation);

            if (Mathf.Abs(Camera.main.transform.rotation.eulerAngles.z) < 1f || Mathf.Abs(Camera.main.transform.rotation.eulerAngles.z - 180f) < 1f)
            {
                Camera.main.transform.position = new Vector3(0f, player.transform.position.y, -10f);
                Camera.main.orthographicSize = initialOrthographicSize;
            }
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpSound.PlayOneShot(jumpSound.clip);
            Destroy(gameObject);
        }
    }
}

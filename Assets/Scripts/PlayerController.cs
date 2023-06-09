using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Text scoreText;

    public float topScore = 0.0f;
    public int score;

    private float moveInput;
    private bool faceRight = true;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (rb.velocity.y > 0 && transform.position.y > topScore)
        {
            topScore = transform.position.y;
        }

        score = (int)Mathf.Round(topScore);
        scoreText.text = "Score: " + score.ToString();
    }

    private void FixedUpdate()
    {
        MoveHorizontally();
        Flip();
        GoingBeyondTheScreen();
    }

    private void MoveHorizontally()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
    }

    private void Flip()
    {
        if (moveInput > 0 && !faceRight || moveInput < 0 && faceRight)
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            faceRight = !faceRight;
        }
    }

    private void GoingBeyondTheScreen()
    {
        if (transform.position.x <= -9)
        {
            transform.position = new Vector2(7, transform.position.y);
        }
        else if (transform.position.x >= 9)
        {
            transform.position = new Vector2(-7, transform.position.y);
        }
    }
}

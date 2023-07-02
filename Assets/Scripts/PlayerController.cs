using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float score = 0.0f;
    public int topScore;

    public GameObject leftButton;
    public GameObject rightButton;
    public Text scoreText;

    private float moveInput;
    private bool faceRight = true;
    private bool isMoving = false;
    private bool isMobilePlatform;

    private Rigidbody2D rb;

    private void Awake()
    {
        isMobilePlatform = Application.isMobilePlatform;
        CheckTheBuildPlatform();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (rb.velocity.y > 0 && transform.position.y > score)
        {
            score = transform.position.y;
        }

        topScore = (int)Mathf.Round(score);
        scoreText.text = string.Format("Score: {0}", topScore);

        if (PlayerPrefs.GetInt("highestScore") < topScore)
        {
            PlayerPrefs.SetInt("highestScore", topScore);
        }
    }

    private void FixedUpdate()
    {
        MoveHorizontally();
        Flip();
        GoingBeyondTheScreen();
    }

    private void CheckTheBuildPlatform()
    {
        if (isMobilePlatform)
        {
            leftButton.SetActive(true);
            rightButton.SetActive(true);
        }
        else
        {
            leftButton.SetActive(false);
            rightButton.SetActive(false);
        }
    }

    private void MoveHorizontally()
    {
        float keyboardInput = Input.GetAxisRaw("Horizontal");
        if (isMoving)
        {
            rb.velocity = new Vector2(moveInput * speed * Time.deltaTime, rb.velocity.y);
        }
        else if (keyboardInput != 0)
        {
            moveInput = keyboardInput;
            rb.velocity = new Vector2(moveInput * speed * Time.deltaTime, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
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

    public void OnRightButtonDown()
    {
        moveInput = 1f;
        isMoving = true;
    }

    public void OnLeftButtonDown()
    {
        moveInput = -1f;
        isMoving = true;
    }

    public void OnButtonUp()
    {
        isMoving = false;
    }
}

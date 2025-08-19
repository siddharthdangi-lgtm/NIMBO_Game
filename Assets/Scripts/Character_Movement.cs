using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Character_Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isMove;
    private bool isGrounded;
    private bool isOver;
    private static int lives;
    private bool isJump;
    private Data_Saver saver;


    public float speed;
    public float jump_height;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    public string platformTag;
    public string obstaclesTag;
    public string finishScreenTag;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menuScreenManager.instance.UIManager(menuScreenManager.instance.gameScreen);
        menuScreenManager.instance.StartHearts();
        rb = GetComponent<Rigidbody2D>();
        saver = new Data_Saver();
        isOver = false;
        isMove = true;
        groundCheckRadius = 0.2f;
        
        lives = 3;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isOver)
        {
            if (isMove)
            {
                rb.gravityScale = 3f;
                float move = menuScreenManager.instance.direction;
                //move = Input.GetAxis("Horizontal");
                rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);
            }

            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }
        

    }

    private void Update()
    {
        if (!isOver)
        {
            if (isGrounded) isMove = true;

            isJump = menuScreenManager.instance.isJump;
            if (isJump && isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump_height);
                isGrounded = false;
                isJump = false;
            }
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(platformTag))
        {
            isMove = false;
            rb.gravityScale = 5f;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(obstaclesTag)) Damage();

        if (collision.CompareTag(finishScreenTag))
        {
            FinishGame();
        }
    }

    private void Damage()
    {
        lives--;
        if(lives < 1)
        {
            isOver = true;
            menuScreenManager.instance.UIManager(menuScreenManager.instance.gameOverScreen);
        }
        else
        {
            menuScreenManager.instance.HeartMechanism(lives);
        }
    }

    private void FinishGame()
    {
        isOver = true;
        menuScreenManager.instance.UIManager(menuScreenManager.instance.finishScreen);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;

        string key = SceneManager.GetActiveScene().name;

        saver.openLevels.Add(key, true);
        menuScreenManager.instance.LevelOpen();
    }
}

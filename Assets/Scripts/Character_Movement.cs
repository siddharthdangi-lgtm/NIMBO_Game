using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Character_Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    public static Character_Movement instance;
    private bool isMove;
    private bool isGrounded;
    private bool isOver;
    private Data_Saver saver;
    Animator animator;


    public float speed;
    public float jump_height;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    public string platformTag;
    public string obstaclesTag;
    public string finishScreenTag;
    public string fallTag;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        saver = new Data_Saver();
        isOver = false;
        isMove = true;
        groundCheckRadius = 0.2f;
        menuScreenManager.instance.direction = 0;
        

    }
    void Update()
    {
        if (!isOver)
        {
            if (isMove)
            {
                rb.gravityScale = 3f;
                float move = menuScreenManager.instance.direction;
                //move = Input.GetAxis("Horizontal");
                rb.linearVelocity = new Vector2(move * speed, rb.linearVelocityY);
                animator.SetFloat("xVelocity", 0);
            }
            else animator.SetFloat("xVelocity", 1);

            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

            if (isGrounded) isMove = true;
        }
    }

    public void IsJump()
    {
        if (instance.isGrounded)
        {
            rb.AddForce(new Vector2(rb.linearVelocityX, jump_height));
            instance.isGrounded = false;
            animator.SetBool("isJumping", !instance.isGrounded);
            animator.SetFloat("yVelocity", 0);
        }
        animator.SetBool("isJumping", instance.isGrounded);
        animator.SetFloat("yVelocity", -1);
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
        if (collision.CompareTag(fallTag)) Fall();

        if (collision.CompareTag(finishScreenTag)) FinishGame();
    }

    private void Damage()
    {
        if(menuScreenManager.lives <= 1)
        {
            isOver = true;
            menuScreenManager.instance.UIManager(menuScreenManager.instance.gameOverScreen);
            menuScreenManager.instance.StartHearts();
        }
        else
        {
            menuScreenManager.instance.HeartMechanism();
        }
    }

    public void Fall()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Damage();
    }

    private void FinishGame()
    {
        isOver = true;
        menuScreenManager.instance.UIManager(menuScreenManager.instance.finishScreen);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;

        string key = SceneManager.GetActiveScene().name;

        saver.openLevels.Add(key, true);
        menuScreenManager.instance.OpenLevels();
    }
}

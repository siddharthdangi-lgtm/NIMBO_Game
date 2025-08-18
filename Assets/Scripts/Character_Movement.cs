using UnityEngine;

public class Character_Movement : MonoBehaviour
{
    public float speed;
    public float jump_height;
    private float move;
    private Rigidbody2D rb;
    private bool isMove;
    private bool isGrounded;
    private bool isOver;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    public string platformTag;
    public string obstaclesTag;
    public string finishTag;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isOver = false;
        isMove = true;
        groundCheckRadius = 0.2f;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isOver)
        {
            if (isMove)
            {
                rb.gravityScale = 3f;
                move = Input.GetAxis("Horizontal");
                rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);
            }

            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
            Debug.Log(isGrounded);
        }
        

    }

    private void Update()
    {
        if (!isOver)
        {
            if (isGrounded) isMove = true;

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump_height);
                isGrounded = false;
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
        if (collision.CompareTag(obstaclesTag))
        {
            isOver = true;
            MenuManager.instance.UIManager(MenuManager.instance.gameOver);
        }
        else if (collision.CompareTag(finishTag))
        {
            isOver = true;
            MenuManager.instance.UIManager(MenuManager.instance.finish);
        }
    }
}

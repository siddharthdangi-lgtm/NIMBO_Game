using UnityEngine;

public class collider_Ground1_L1 : MonoBehaviour
{
    public GameObject GameObject;
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnTriggerEnter2D(Collider2D collision)
    {
        rb = GameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 5f;
        gameObject.SetActive(false);
    }
}

using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMagnetController : MonoBehaviour
{
    public enum MagnetMode
    {
    Neutral,
    North,
    South
    }

    public MagnetMode currentMode = MagnetMode.Neutral;
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 8f;

    [Header("Magnet")]
    public float magnetRadius = 5f;
    public float magnetForce = 20f;

    [Header("Pole")]
    private Rigidbody2D rb;
    private bool isGrounded;

    [Header("Characteristics")]
    public float mass = 1f;
    public int health = 100;
    public bool isAlive = true;
    public bool isLevelFinished = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Prevent player from falling over
        rb.freezeRotation = true;
        
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandlePolarity();
        ApplyMagnetism();
        HandleHealth();
        HandleLevelCompletion();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            health = 0;
        }
        if (collision.gameObject.CompareTag("Finish"))
        {
            isLevelFinished = true;
        }
    }
    
    void HandleLevelCompletion()
    {
        if (isLevelFinished)
        {
            
            print("Level Completed!");
        
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
        }
    }

    void HandleMovement()
    {
        float move = Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector2(
            move * moveSpeed,
            rb.linearVelocity.y
        );
    }

    void HandleHealth()
    {
        if (health <= 0)
        {
            isAlive = false;
            Destroy(gameObject);
        }
        
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(
                Vector2.up * jumpForce,
                ForceMode2D.Impulse
            );
        }
    }

    void HandlePolarity()
    {
    if (Input.GetKey(KeyCode.N))
    {
        currentMode = MagnetMode.North;
    }
    else if (Input.GetKey(KeyCode.S))
    {
        currentMode = MagnetMode.South;
    }
    else
    {
        currentMode = MagnetMode.Neutral;
    }
}

    void ApplyMagnetism()
{
    if (currentMode == MagnetMode.Neutral)
        return;

    Collider2D[] nearbyObjects =
        Physics2D.OverlapCircleAll(
            transform.position,
            magnetRadius
        );

    foreach (Collider2D col in nearbyObjects)
    {
        MagnBox box = col.GetComponent<MagnBox>();

        if (box == null)
            continue;

        Rigidbody2D boxRb = box.GetComponent<Rigidbody2D>();

        if (boxRb == null)
            continue;

        Vector2 direction =
            ((Vector2)transform.position - boxRb.position)
            .normalized;

        bool playerIsNorth =
            currentMode == MagnetMode.North;

        bool samePole =
            playerIsNorth == box.isNorth;

        if (samePole)
        {
            boxRb.linearVelocity = -direction * 1.5f;
        }
        else
        {
            boxRb.linearVelocity = direction * 1.5f;
        }
    }
}

    private void OnCollisionStay2D(Collision2D collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        Gizmos.DrawWireSphere(
            transform.position,
            magnetRadius
        );
    }
}
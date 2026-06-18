using UnityEngine;
using System.Collections;

// Ensures the Player always has a Rigidbody2D attached
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMagnetController : MonoBehaviour
{
    public enum MagnetMode
    {
        Neutral,
        North,
        South
    }

    [Header("State")]
    public MagnetMode currentMode = MagnetMode.Neutral;
    public bool isAlive = true;
    public bool isLevelFinished = false;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 8f;

    [Header("Magnetism")]
    public float magnetRadius = 5f;
    public float magnetForce = 20f;

    [Header("Characteristics")]
    public float mass = 1f;
    public int health = 100;

    // Internal references
    private Rigidbody2D rb;
    private bool isGrounded;
    private Lever activeLever = null; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; 
        rb.mass = mass;           
    }

    void Update()
    {
        
        if (!isAlive || isLevelFinished) 
            return;

        HandleMovement();
        HandleJump();
        HandlePolarity();
        ApplyMagnetism();
        HandleInteraction();
    }

    // mechanics t3 LKHRA LI 3YAWNI W KSROLI RASSI W NCHLH YKONO YMCHO  ---

    private void HandleMovement()
    {
        
        float move = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);
    }

    private void HandleJump()
    {
        
        
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded || Input.GetKeyDown(KeyCode.JoystickButton1) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void HandlePolarity()
    {
        
        if (Input.GetKey(KeyCode.N) || Input.GetKey(KeyCode.JoystickButton2))
        {
            currentMode = MagnetMode.North;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.JoystickButton3))
        {
            currentMode = MagnetMode.South;
        }
        else
        {
            currentMode = MagnetMode.Neutral;
        }
    }

    private void ApplyMagnetism()
    {
        
        if (currentMode == MagnetMode.Neutral) 
            return;

        
        Collider2D[] nearbyObjects = Physics2D.OverlapCircleAll(transform.position, magnetRadius);

        foreach (Collider2D col in nearbyObjects)
        {
            
            MagnBox box = col.GetComponent<MagnBox>();
            if (box == null) continue;

            Rigidbody2D boxRb = box.GetComponent<Rigidbody2D>();
            if (boxRb == null) continue;

            
            Vector2 direction = ((Vector2)transform.position - boxRb.position).normalized;

            bool playerIsNorth = (currentMode == MagnetMode.North);
            bool samePole = (playerIsNorth == box.isNorth);

            
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

    private void HandleInteraction()
    {
        
        if (Input.GetKeyDown(KeyCode.E) && activeLever != null || Input.GetKeyDown(KeyCode.JoystickButton0) && activeLever != null)
        {
            activeLever.ActivateLever();
        }
    }

    

    private void Die()
    {
        health = 0;
        isAlive = false;
        Destroy(gameObject);
    }

    private void CompleteLevel()
    {
        isLevelFinished = true;
        print("Level Completed!");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Die();
        }
        
        else if (collision.gameObject.CompareTag("Finish"))
        {
            CompleteLevel();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Lever"))
        {
            activeLever = collision.GetComponent<Lever>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Lever"))
        {
            activeLever = null;
        }
    }

    

    void OnDrawGizmosSelected()
    {
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, magnetRadius);
    }
}
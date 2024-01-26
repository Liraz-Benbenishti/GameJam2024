using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float velocity = 5f;
    public float jumpForce = 10f;
    public bool isGrounded = true;

    public Transform groundChecker;
    public float groundCheckerDistance = 0.1f;
    public LayerMask groundLayer;
    public bool isDoubleJumpPossible = false;
    public VoidEventChannel gameOverEvent;
    public VoidEventChannel winEvent;
    public int maxHealth = 3;
    public int currentHealth;
    public event UnityAction onObsticle;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // Get input from the player
        float horizontalInput = Input.GetAxis("Horizontal");

        // Calculate movement vector
        Vector2 movement = new Vector2(horizontalInput, 0f);

        // Apply movement to the Rigidbody2D
        rb.velocity = new Vector2(movement.x * velocity, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded || isDoubleJumpPossible)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                isDoubleJumpPossible = !isDoubleJumpPossible;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "obstacle")
        {
            Debug.Log("hurting playyer");
            currentHealth--;
            onObsticle?.Invoke();

            if (currentHealth == 0)
            {
                gameOverEvent.raiseEvent();
            }
        }

        else if (collision.gameObject.tag == "Finish")
        {
            winEvent.raiseEvent();
        }
    }

    public void ApplyPowerUp(PowerUp p)
    {
        if (p is SpeedPowerUp speedPowerUp)
        {
            Debug.Log("Apply speed boost to player");
            velocity += speedPowerUp.speedBoost;
        }
        else if (p is JumpPowerUp jumpPowerUp)
        {
            jumpForce += jumpPowerUp.jumpBonus;
        }
    }

    public IEnumerator RemovePowerUp(PowerUp p)
    {
        yield return new WaitForSeconds(p.duration);

        if (p is SpeedPowerUp speedPowerUp)
        {
            velocity -= speedPowerUp.speedBoost;
            Debug.Log("Remove speed boost from player");
        }
        else if (p is JumpPowerUp jumpPowerUp)
        {
            jumpForce -= jumpPowerUp.jumpBonus;
        }
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.Raycast(groundChecker.position, Vector2.down, groundCheckerDistance, groundLayer);
    }

    // Draw Gizmo to visualize the ground check
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(groundChecker.position, groundChecker.position + Vector3.down * groundCheckerDistance);
    }
}
using System;
using System.Threading.Tasks;
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

    public AudioCueEventChannelSO playSfxEvent;
    public AudioConfigurationSO sfxConfig;
    public AudioCueSO playerMotorSfx;
    public AudioCueSO playerJumpSfx;
    public AudioCueSO playerLandSfx;
    public AudioCueSO winSfx;
    public AudioCueSO collectPowerUpSfx;
    public AudioCueSO fadePowerUpSfx;

    private float lastHitTime;
    public float invinsibleTime = 0.5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        playSfxEvent.RaisePlayEvent(playerMotorSfx, sfxConfig);
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

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded || isDoubleJumpPossible)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                isDoubleJumpPossible = !isDoubleJumpPossible;
                playSfxEvent.RaisePlayEvent(playerJumpSfx, sfxConfig);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("obstacle") && Time.time > lastHitTime + invinsibleTime)
        {
            Debug.Log($"hurting playyer by { collision.gameObject.name }");
            currentHealth--;
            onObsticle?.Invoke();

            lastHitTime = Time.time;

            if (currentHealth == 0)
            {
                gameOverEvent.raiseEvent();
            }
        }

        else if (collision.gameObject.CompareTag("Finish"))
        {
            playSfxEvent.RaisePlayEvent(winSfx, sfxConfig);
            winEvent.raiseEvent();
        }
    }

    public void ApplyPowerUp(PowerUp p)
    {
        Debug.Log($"Apply power up of type {p.name} to player");
        playSfxEvent.RaisePlayEvent(collectPowerUpSfx, sfxConfig);
        if (p is SpeedPowerUp speedPowerUp)
        {
            velocity += speedPowerUp.speedBoost;
        }
        else if (p is JumpPowerUp jumpPowerUp)
        {
            jumpForce += jumpPowerUp.jumpBonus;
        }
    }

    public async Task RemovePowerUp(PowerUp p)
    {
        await Task.Delay(TimeSpan.FromSeconds(p.duration));

        Debug.Log($"Removing power up of type {p.name} from player");
        playSfxEvent.RaisePlayEvent(fadePowerUpSfx, sfxConfig);
        if (p is SpeedPowerUp speedPowerUp)
        {
            velocity -= speedPowerUp.speedBoost;
        }
        else if (p is JumpPowerUp jumpPowerUp)
        {
            jumpForce -= jumpPowerUp.jumpBonus;
        }
    }

    private void FixedUpdate()
    {
        var previous = isGrounded;
        isGrounded = Physics2D.Raycast(groundChecker.position, Vector2.down, groundCheckerDistance, groundLayer);
        if (previous == false && isGrounded)
        {
            playSfxEvent.RaisePlayEvent(playerLandSfx, sfxConfig);
        }
    }

    // Draw Gizmo to visualize the ground check
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(groundChecker.position, groundChecker.position + Vector3.down * groundCheckerDistance);
    }
}
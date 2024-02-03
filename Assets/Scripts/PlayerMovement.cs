using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    public float velocity = 5f;
    public float jumpForce = 10f;
    public bool isGrounded = true;
    public ScrollingBackground bg;
    public MoveObject route;
    public Transform groundChecker;
    public float groundCheckerDistance = 0.1f;
    public LayerMask groundLayer;
    public bool isDoubleJumpPossible = false;
    public VoidEventChannel gameOverEvent;
    public VoidEventChannel winEvent;
    public int maxHealth = 5;
    public int currentHealth;
    public event UnityAction onObsticle;

    public AudioCueEventChannelSO playSfxEvent;
    public AudioConfigurationSO sfxConfig;
    public AudioCueSO playerMotorSfx;
    public AudioCueSO playerJumpSfx;
    public AudioCueSO playerLandSfx;
    public AudioCueSO winSfx;
    public AudioCueSO loseSfx;
    public AudioCueSO collectPowerUpSfx;
    public AudioCueSO fadePowerUpSfx;

    private float lastHitTime;
    public float invinsibleTime = 0.5f;

    public ParticleSystem powerUpVfx1;
    public ParticleSystem powerUpVfx2;
    public ParticleSystem hurtVfx;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
        Vector3 movement = new Vector3(horizontalInput, 0f, 0f);

        // Apply movement to the Rigidbody
        rb.velocity = new Vector3(movement.x * velocity, rb.velocity.y, 0);

        if (Input.GetKey(KeyCode.LeftControl))
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationZ;
            rb.rotation = Quaternion.Slerp(rb.rotation, Quaternion.LookRotation(new Vector3(0, 0, 0)), 0.007f);
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            rb.constraints = RigidbodyConstraints.None;
        }

        if (!Input.GetKey(KeyCode.LeftControl))
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationX;
            rb.constraints = RigidbodyConstraints.FreezeRotationY;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded || isDoubleJumpPossible)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, 0f);
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isDoubleJumpPossible = !isDoubleJumpPossible;
                playSfxEvent.RaisePlayEvent(playerJumpSfx, sfxConfig);
            }
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer==3)
        {
            isGrounded = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 3)
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Hole"))
        {
            gameOverEvent.raiseEvent();
        }
        if (collision.gameObject.CompareTag("obstacle") && Time.time > lastHitTime + invinsibleTime)
        {
            Debug.Log($"hurting player by { collision.gameObject.name }");
            hurtVfx.Play();
            currentHealth--;
            onObsticle?.Invoke();

            lastHitTime = Time.time;

            lastHitTime = Time.time;

            if (currentHealth == 0)
            {
                playSfxEvent.RaisePlayEvent(loseSfx, sfxConfig);
                gameOverEvent.raiseEvent();
            }
        }

        else if (collision.gameObject.CompareTag("Finish"))
        {
            playSfxEvent.RaisePlayEvent(winSfx, sfxConfig);
            winEvent.raiseEvent();
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("obstacle"))
        {
            hurtVfx.Stop();
        }
    }      
    
    public void ApplyPowerUp(PowerUp p)
    {
        Debug.Log($"Apply power up of type {p.name} to player");
        playSfxEvent.RaisePlayEvent(collectPowerUpSfx, sfxConfig);
        //rb.freezeRotation = true;
        if (p is SpeedPowerUp speedPowerUp)
        {
            velocity += speedPowerUp.speedBoost;
            route.speed += 1;
            bg.speed += 0.05f;
            powerUpVfx1.Play();
        }
        else if (p is JumpPowerUp jumpPowerUp)
        {
            jumpForce += jumpPowerUp.jumpBonus;
            powerUpVfx2.Play();
        }
    }

    public async Task RemovePowerUp(PowerUp p)
    {
        await Task.Delay(TimeSpan.FromSeconds(p.duration));
        Debug.Log($"Removing power up of type {p.name} from player");
        playSfxEvent.RaisePlayEvent(fadePowerUpSfx, sfxConfig);
        powerUpVfx1.Stop();
        powerUpVfx2.Stop();
        if (p is SpeedPowerUp speedPowerUp)
        {
            velocity -= speedPowerUp.speedBoost;
            route.speed -= 1;
            bg.speed -= 0.05f;
        }
        else if (p is JumpPowerUp jumpPowerUp)
        {
            jumpForce -= jumpPowerUp.jumpBonus;
        }
    }

    private void FixedUpdate()
    {
        var previous = isGrounded;
        //isGrounded = Physics.Raycast(groundChecker.position, Vector3.down, groundCheckerDistance, groundLayer);
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
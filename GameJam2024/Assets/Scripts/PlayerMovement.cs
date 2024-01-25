using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    // Start is called before the first frame update
    void Start()
    {
        
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
                rb.velocity.y = 0f;
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                //isGrounded = false;
                isDoubleJumpPossible = !isDoubleJumpPossible;
                //if (isDoubleJumpPossible)
                //{
                //    isDoubleJumpPossible = false;
                //}
                //else
                //{
                //    isDoubleJumpPossible = true;
                //}
            }
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

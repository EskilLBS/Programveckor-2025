using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public LayerMask groundLayer;

    Rigidbody2D rb;
    [SerializeField] float speed = 5;
    [SerializeField] float jumpHeight = 10;

    [SerializeField] bool topDownMovement;
    bool grounded;
    [SerializeField] GameObject groundCheck;

    Animator animator;

    public bool pauseMovement { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!pauseMovement)
        {
            if (topDownMovement)
            {
                // Get the input on the horizontal and vertical axis and use that as a movement vector
                rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * speed;
            }
            else
            {
                animator.SetBool("Walking", true);

                // Get the input on the horizontal axis and use that as a movement vector
                rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed, rb.velocity.y);

                if(Input.GetAxisRaw("Horizontal") == 0)
                {
                    animator.SetBool("Walking", false);
                }

                if (Input.GetKeyDown(KeyCode.Space) && GroundCheck())
                {
                    // Set grounded to false to ensure that the player can't jump again, and then add force upwards
                    rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);
                }
            }
        }
        else
        {
            // Make the player stand still if movement should be paused, ex. in combat
            rb.velocity = new Vector2(0, rb.velocity.y);
        }


        // Check if the player is on the ground
        
    }

    bool GroundCheck()
    {
        RaycastHit2D hit = Physics2D.CircleCast(groundCheck.transform.position, 0.2f, groundCheck.transform.position, 0.3f, groundLayer);
        if (hit.collider)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetPauseMovement(bool state)
    {
        pauseMovement = state;
    }
}

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

    [SerializeField] Animator animator;

    [SerializeField] GameObject walkEffect;

    public bool pauseMovement { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
                walkEffect.GetComponent<ParticleSystem>().enableEmission = true;

                // Get the input on the horizontal axis and use that as a movement vector
                rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed, rb.velocity.y);

                if(rb.velocity.x < 0)
                {
                    animator.gameObject.transform.localScale = new Vector3(-1, 1, 1);
                    walkEffect.transform.rotation = new Quaternion(0, 180, 0,0 );
                }
                else if(rb.velocity.x > 0)
                {
                    animator.gameObject.transform.localScale = new Vector3(1, 1, 1);
                    walkEffect.transform.rotation = new Quaternion(0, 0, 0, 0);
                }

                if(Input.GetAxisRaw("Horizontal") == 0)
                {
                    animator.SetBool("Walking", false);
                    walkEffect.GetComponent<ParticleSystem>().enableEmission = false;
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
            walkEffect.GetComponent<ParticleSystem>().enableEmission = false;
            animator.SetBool("Walking", false);
        }

        if (!GroundCheck())
        {
            walkEffect.GetComponent<ParticleSystem>().enableEmission = false;
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

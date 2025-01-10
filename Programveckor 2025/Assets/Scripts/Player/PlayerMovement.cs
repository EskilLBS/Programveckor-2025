using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public LayerMask groundLayer;

    Rigidbody2D rigidbody;
    public int speed = 5;

    [SerializeField] bool topDownMovement;
    bool grounded;
    [SerializeField] GameObject groundCheck;

    public bool pauseMovement { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!pauseMovement)
        {
            if (topDownMovement)
            {
                // Get the input on the horizontal and vertical axis and use that as a movement vector
                rigidbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * speed;
            }
            else
            {
                // Get the input on the horizontal axis and use that as a movement vector
                rigidbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed, rigidbody.velocity.y);

                if (Input.GetKeyDown(KeyCode.Space) && grounded)
                {
                    // Set grounded to false to ensure that the player can't jump again, and then add force upwards
                    grounded = false;
                    rigidbody.AddForce(transform.up * speed, ForceMode2D.Impulse);
                }
            }
        }
        else
        {
            // Make the player stand still if movement should be paused, ex. in combat
            rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
        }


        // Check if the player is on the ground
        RaycastHit2D hit = Physics2D.CircleCast(groundCheck.transform.position, 0.2f, groundCheck.transform.position, 0.3f, groundLayer);
        if (hit.collider)
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
        
        /*if (Input.GetKey(KeyCode.RightArrow))
        {
            //Moves player to the right
            rigidbody.velocity = new Vector2(1, rigidbody.velocity.y).normalized * speed;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //Moves the player to the left
            rigidbody.velocity = new Vector2(-1, rigidbody.velocity.y).normalized * speed;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            //Moves player up
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, 1).normalized * speed;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            //Moves player down
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, - 1).normalized * speed;
        }*/
    }

    public void SetPauseMovement(bool state)
    {
        pauseMovement = state;
    }
}

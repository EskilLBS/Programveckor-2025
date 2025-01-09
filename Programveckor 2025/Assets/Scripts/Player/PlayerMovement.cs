using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rigidbody;
    public int speed = 5;

    public bool pauseMovement { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * speed;
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

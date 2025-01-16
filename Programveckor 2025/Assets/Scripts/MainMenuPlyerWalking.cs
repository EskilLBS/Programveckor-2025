using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Walking : MonoBehaviour
{
    Rigidbody2D rigidbody;
    public int speed;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = new Vector2(1, 0) * speed;
    }

    // Update is called once per frame
    void Update()
    {

        if(transform.position.x >= 30)
        {
            transform.position = new Vector2(-31.14f, transform.position.y);
        }
    }
}

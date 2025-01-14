using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // The object that should be tracked and the speed that it should be tracked at
    [SerializeField] Transform trackedObject;
    [SerializeField] float trackingSpeed;

    // Update is called once per frame
    void Update()
    {
        Vector2 velocity = Vector2.zero;

        // Smoothdamp the position of the camera so that if follows the player smoothly
        transform.position = Vector2.SmoothDamp(transform.position, trackedObject.position, ref velocity, trackingSpeed);
        transform.position += new Vector3(0, 0, -10);
    }
}

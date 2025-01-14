using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform trackedObject;
    [SerializeField] float trackingSpeed;

    // Update is called once per frame
    void Update()
    {
        Vector2 velocity = Vector2.zero;

        transform.position = Vector2.SmoothDamp(transform.position, trackedObject.position, ref velocity, trackingSpeed);
        transform.position += new Vector3(0, 0, -10);
    }
}

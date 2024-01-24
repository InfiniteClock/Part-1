using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    float steering;
    float acceleration;
    public float steeringSpeed = 100;
    public float forwardSpeed = 500;
    public float maxSpeed = 1000;
    Rigidbody2D rigidBody;
    // Start is called before the first frame update
    void Start()
    {
        if (rigidBody == null)
        {
            rigidBody = GetComponent<Rigidbody2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        steering = Input.GetAxis("Horizontal");
        acceleration = Input.GetAxis("Vertical");
    }
    private void FixedUpdate()
    {
        rigidBody.AddTorque(steeringSpeed * -steering * Time.deltaTime);
        Vector2 force = transform.up * acceleration * forwardSpeed * Time.deltaTime;
        if (rigidBody.velocity.magnitude < maxSpeed)
        {
            rigidBody.AddForce(force);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 100;               // Actual speed of player
    public float steeringSpeed = 100;       // Actual roational speed of player
    public float acceleration;              // Value for speed input (+ is forward, - is backwards)
    public float steering;                  // Value for steering input (+ is right, - is left);
    public float maxSpeed = 500;            // Max speed possible for player
    public float firingSpeed = 1;           // Max spawning speed of projectiles. Determines max timer for firingTimer
    private float firingTimer;              // Timer before able to spawn another projectile
    public float invincibilityWindow = 3;   // Max timer for invincibility after being hit 
    public float invincibility;             // Timer for invincibility
    public float bounce = 15;               // Multiplier value for the ricochet effect after getting hit
        
    public int health = 3;                  // Current health value
    public int maxHealth = 3;               // Max value for health

    public GameObject projectile;           // Prefab for projectiles
    public Transform spawnPoint;            // Transform for child object that determines spawn point for projectiles

    private Rigidbody2D rb;                 // Rigidbody for this object
    private SpriteRenderer spr;             // Sprite renderer for this object

    // Start is called before the first frame update
    void Start()
    {
        // Get components for this object's rigidbody and sprite renderer
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();

        // If the spawnpoint isn't given a value, default to this object's transform instead
        if (spawnPoint == null) { spawnPoint = transform; }
    }

    // Update is called once per frame
    void Update()
    {
        // Set input values to acceleration and steering based on player input
        acceleration = Input.GetAxis("Vertical");
        steering = Input.GetAxis("Horizontal");

        // Count down on the firing timer if it isn't already at/less than 0
        if (firingTimer > 0)
        {
            firingTimer -= Time.deltaTime;
        }
        // If timer isn't counting down and space is pressed, set the timer and spawn a projectile
        else if (Input.GetKey(KeyCode.Space))
        {
            firingTimer = 1 / firingSpeed;
            Instantiate(projectile, spawnPoint.position, transform.rotation);
        }

        
        if (health < 1)
        {
            gameObject.SetActive(false);
        }
        else if (health == 1)
        {
            spr.color = Color.red;
        }
        else if (health < (maxHealth / 2))
        {
            spr.color = Color.yellow;
        }
        else if (health < maxHealth)
        {
            spr.color = Color.green;
        }
        else
        {
            spr.color = Color.blue;
        }

        if (invincibility > 0)
        {
            invincibility -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        // Torque is added to player to turn them. Steering must be inversed for correct value
        rb.AddTorque(steeringSpeed * -steering * Time.deltaTime);

        // Force is determined based on object's upwards (front-facing) direction
        Vector2 force = transform.up * acceleration * speed * Time.deltaTime;

        // If not already at maxSpeed, apply force to object
        if (rb.velocity.magnitude < maxSpeed)
        {
            rb.AddForce(force);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Enemy collision layer is 10. If colliding with something from this layer, bounce away from it on collision
        if (collision.gameObject.layer == 10)
        {
            // Direction is the normalized magnitude of the difference between this object and the collider
            Vector2 direction = (transform.position - collision.transform.position).normalized;

            // Force added is equal to maxSpeed times bounce value
            rb.AddForce(direction * maxSpeed * bounce * Time.deltaTime);

            // If player is not in invincibility timer, set the timer and reduce health by 1
            if (invincibility <= 0)
            {
                invincibility = invincibilityWindow;
                health--;
            }
        }
    }
}

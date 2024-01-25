using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 100;               // actual speed of player
    public float steeringSpeed = 100;       // actual roational speed of player
    public float acceleration;              // value for speed input (+ is forward, - is backwards)
    public float steering;                  // value for steering input (+ is right, - is left);
    public float maxSpeed = 500;            // max speed possible for player
    public float firingSpeed = 1;           // max spawning speed of projectiles. Determines max timer for firingTimer
    private float firingTimer;              // timer before able to spawn another projectile
    public float invincibilityWindow = 1;   // max timer for invincibility after being hit 
    public float invincibility;             // timer for invincibility
    public float bounce = 15;               // multiplier value for the ricochet effect after getting hit
        
    public int health = 3;                  // current health value
    public int maxHealth = 3;               // max value for health

    public GameObject projectile;           // prefab for projectiles
    public Transform spawnPoint;            // transform for child object that determines spawn point for projectiles

    private Rigidbody2D rb;                 // rigidbody for this object
    private SpriteRenderer spr;             // sprite renderer for this object

    // Start is called before the first frame update
    void Start()
    {
        // get components for this object's rigidbody and sprite renderer
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        // if the spawnpoint isn't given a value, default to this object's transform instead
        if (spawnPoint == null) { spawnPoint = transform; }
    }

    // Update is called once per frame
    void Update()
    {
        // set input values to acceleration and steering based on player input
        acceleration = Input.GetAxis("Vertical");
        steering = Input.GetAxis("Horizontal");
        // Count down on the firing timer if it isn't already at/less than 0
        if (firingTimer > 0)
        {
            firingTimer -= Time.deltaTime;
        }
        // if timer isn't counting down and space is pressed, set the timer and spawn a projectile
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            firingTimer = 1 / firingSpeed;
            Instantiate(projectile, spawnPoint.position, transform.rotation);
        }

        // return to this when enemies are implemented properly
        /*
        if (health < 1)
        {
            Destroy(gameObject);
        }
        else if (health == 1)
        {
            spr.color = Color.red;
        }
        else if (health < maxHealth)
        {
            spr.color = Color.yellow;
        }
        else
        {
            spr.color = Color.green;
        }
        */

    }

    private void FixedUpdate()
    {
        // torque is added to player to turn them. steering must be inversed for correct value
        rb.AddTorque(steeringSpeed * -steering * Time.deltaTime);
        // force is determined based on object's upwards (front-facing) direction
        Vector2 force = transform.up * acceleration * speed * Time.deltaTime;
        // if not already at maxSpeed, apply force to object
        if (rb.velocity.magnitude < maxSpeed)
        {
            rb.AddForce(force);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // enemy collision layer is 10. If colliding with something from this layer, bounce away from it on collision
        if (collision.gameObject.layer == 10)
        {
            // direction is the normalized magnitude of the difference between this object and the collider
            Vector2 direction = (transform.position - collision.transform.position).normalized;
            Debug.Log(direction);
            // force added is equal to maxSpeed times bounce value
            rb.AddForce(direction * maxSpeed * bounce * Time.deltaTime);
        }
    }
}

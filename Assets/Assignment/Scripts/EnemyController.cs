using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float rotation;              // Rotation direction
    public float rotationSpeed = 100;   // Rotation speed
    public float firingSpeed = 1;       // Spawning speed of projectiles
    private float firingTimer;          // Timer before being able to fire another projectile
    public float healthMax = 3;         // Health total
    public float health = 3;            // Health current
    public bool canShoot = false;       // Boolean for whether able to shoot or not
    public float bounce = 15;           // Multiplier for collision bounces

    public GameObject projectile;       // Prefab for projectile to fire
    public Transform spawnPoint;        // Spawn position for projectiles

    public Transform target;            // Holds the transform of the target (player) to look at them

    private Rigidbody2D rb;             // This object's rigidbody

    // Start is called before the first frame update
    void Start()
    {
        // get components for this object's rigidbody
        rb = GetComponent<Rigidbody2D>();
        // if the spawnpoint isn't given a value, default to this object's transform instead
        if (spawnPoint == null) { spawnPoint = transform; }
    }

    // Update is called once per frame
    void Update()
    {
        // If health is dropped to 0, destroy this object
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        // Count down on the firing timer if it isn't already at/less than 0
        if (firingTimer > 0)
        {
            firingTimer -= Time.deltaTime;
        }

        // If angled to shoot properly, fire away
        else if (canShoot)
        {
            // Greater firing speed creates diminishingly lower timer
            firingTimer = 1 / firingSpeed;

            // Instantiates using the spawnpoint's rotation instead on account of this object being off center
            Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
        }
    }

    private void FixedUpdate()
    {
        // Calculates the direction to target as a vector with values between -1 and 1
        Vector2 dirToTarget = (target.position - transform.position).normalized;

        // Converts dirToTarget to an angle in rads, then to degrees between -180 and 180, then to degrees between 0 and 360
        float targetAngle = Mathf.Atan2(dirToTarget.y, dirToTarget.x) * Mathf.Rad2Deg + 180f;

        // Ensures that the rotation value of the rigidbody is never negative 
        if (rb.rotation < 0)
        {
            rb.rotation += 360;
        }

        // Collects the current rotation of the object (using remainder operation for numbers above 360)
        float selfAngle = rb.rotation%360;

        // Determines which direction to turn to reach the target fastest
        if (selfAngle - targetAngle >= targetAngle - selfAngle)
        {
            // Turns right if it is closer, or left if the "0=360" axis has been crossed
            if (selfAngle - targetAngle > 180)
            {
                rotation = -1;
            }
            else
            {
                rotation = 1;
            }
        }
        else
        {
            // Turns left if it is closer, or right if the "0=360" axis has been crossed
            if (selfAngle - targetAngle < -180)
            {
                rotation = 1;
            }
            else
            {
                rotation = -1;
            }
        }

        // If the target is within a specified cone, slow rotation and enable firing
        // This uses the absolute value of the Angles' difference
        // due to rapid switching between positive and negative values while rotating
        if (Mathf.Abs(selfAngle - targetAngle) < 190 && Mathf.Abs(selfAngle - targetAngle) > 170)
        {
            canShoot = true;
            rotation = rotation / 10f;
        }
        else
        {
            canShoot = false;
        }

        // Apply torque to object based on above calculations
        rb.AddTorque(rotationSpeed * rotation * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Player projectiles collision layer is 11. If colliding with something from this layer, bounce away from it on collision
        if (collision.gameObject.layer == 11)
        {
            // direction is the normalized magnitude of the difference between this object and the collider
            Vector2 direction = (transform.position - collision.transform.position).normalized;

            // force added is equal to maxSpeed times bounce value
            rb.AddForce(direction * rotationSpeed * bounce * Time.deltaTime);

            // Reduce health by 1 on collision with player projectile
            health--;
        }
    }
}

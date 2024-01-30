using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    Rigidbody2D rb;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    private void FixedUpdate()
    {
        // Sets direction to be the forward facing direction, multiplied by speed over time
        Vector2 direction = new Vector2 (transform.up.x, transform.up.y) * speed * Time.deltaTime;
        rb.MovePosition(rb.position + direction);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Destroys projectile on collision
        Destroy(gameObject);
    }
}

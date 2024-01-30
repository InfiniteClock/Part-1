using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float speed = 1;
    public float timerMax = 5;
    public float timer;
    public bool isActive = false;

    public GameObject enemy;

    public Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        if (direction.x == 0 && direction.y == 0)
        {
            direction = new Vector2(speed, speed);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (timer <= 0)
            {
                Instantiate(enemy, -transform.position, transform.rotation);
                timer = timerMax;
            }
        }

        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }
    private void FixedUpdate()
    {
        if(transform.position.x > 8)
        {
            direction.x = -speed;
            speed *= 1.1f;
        }
        else if (transform.position.x < -8)
        {
            direction.x = speed;speed *= 1.1f;
            speed *= 1.1f;
        }

        if (transform.position.y > 4)
        {
            direction.y = -speed;
            speed *= 1.1f;
        }
        else if (transform.position.y < -4)
        {
            direction.y = speed;
            speed *= 1.1f;
        }

        transform.Translate(direction * Time.deltaTime);
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( collision.gameObject.layer == 9)
        {
            isActive = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            isActive = false;
        }
    }
}

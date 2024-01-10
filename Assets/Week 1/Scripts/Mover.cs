using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public float speed = 5f;
    public GameObject prefab;
    public Transform barrel;

    // Update is called once per frame
    void Update()
    {
        float keyboardInput = Input.GetAxis("Horizontal");
        transform.Translate(keyboardInput * speed * Time.deltaTime, 0, 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(prefab, barrel.position, barrel.rotation);
        }
    }
}

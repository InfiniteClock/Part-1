using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{

    public GameObject door;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (door.activeInHierarchy)
        {
            door.SetActive(false);
        }
        else {
            door.SetActive(true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<PlayerHealth>()) 
        {
            OnPickup(collision.gameObject);
        }
    }

    protected virtual void OnPickup(GameObject obj) 
    {
        Destroy(gameObject);
    }
}
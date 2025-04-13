using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlane : MonoBehaviour
{
    [SerializeField] Vector3 originPoint;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerHealth>().gameObject;
        originPoint = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y < -50) 
        {
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            player.transform.position = originPoint;
        }
    }
}

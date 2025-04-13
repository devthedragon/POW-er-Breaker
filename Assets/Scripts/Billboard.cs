using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{

    GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 safeTarget = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
        transform.LookAt(safeTarget);
    }
}

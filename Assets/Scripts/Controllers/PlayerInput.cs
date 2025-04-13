using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // Movement Info
    [SerializeField] Vector3 rotSpeed = new Vector3(0, 90, 0);
    [SerializeField] float acceleration = 0.1f;
    [SerializeField] float maxSpeed = 1;

    GameObject playerCam;
    protected AudioPlayer audioPlayer;
    Vector3 camStart;
    Vector3 currentSpeed = new Vector3(0, 0, 0);
    KeyCode moveFront = KeyCode.W;
    KeyCode moveBack = KeyCode.S;
    KeyCode rotLeft = KeyCode.A;
    KeyCode rotRight = KeyCode.D;
    KeyCode moveLeft = KeyCode.Q;
    KeyCode moveRight = KeyCode.E;

    //Bad Implementation
    [SerializeField] GameObject footstepSource;

    // Start is called before the first frame update
    void Start()
    {
        playerCam = GetComponentInChildren<Camera>().gameObject;
        camStart = playerCam.transform.localPosition;
        footstepSource.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Movement code
        if (Input.GetKey(moveFront))
        {
            if (currentSpeed.z < 0)
            {
                AccelerateForward(1);
            }
            AccelerateForward(1);
        }
        if (Input.GetKey(moveBack)) 
        {
            if (currentSpeed.z > 0) 
            {
                AccelerateForward(-1);
            }
            AccelerateForward(-1);
        }
        if(Input.GetKey(moveFront) == false && Input.GetKey(moveBack) == false) 
        {
            AccelerateForward(0);
        }

        if (Input.GetKey(moveRight))
        {
            if (currentSpeed.x < 0)
            {
                AccelerateSide(1);
            }
            AccelerateSide(1);
        }
        if (Input.GetKey(moveLeft))
        {
            if (currentSpeed.x > 0)
            {
                AccelerateSide(-1);
            }
            AccelerateSide(-1);
        }
        if(Input.GetKey(moveLeft) == false && Input.GetKey(moveRight) == false)
        {
            AccelerateSide(0);
        }

        if (Input.GetKey(rotLeft)) 
        {
            transform.eulerAngles -= rotSpeed * Time.deltaTime;
        }
        if (Input.GetKey(rotRight))
        {
            transform.eulerAngles += rotSpeed * Time.deltaTime;
        }
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, 0.25f, transform.position + Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up) * currentSpeed, out hit, 1) == false || hit.transform.gameObject.tag != "Enemy" || hit.transform.gameObject.tag != "Pickup")
        {
            transform.position += Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up) * currentSpeed * Time.deltaTime;

            PlayerUIManager.pum.playerSpeed = currentSpeed.magnitude;
            playerCam.transform.localPosition = camStart + new Vector3(0, (Mathf.Sin(1.5f * Time.time * Mathf.PI) - 1) * 0.02f, 0) * currentSpeed.magnitude;
        }

        if (currentSpeed.magnitude > 0.5f)
        {
            footstepSource.SetActive(true);
        }
        else
        {
            footstepSource.SetActive(false);
        }
    }

    void AccelerateForward(int direction)
    {
        if (direction > 0)
        {
            currentSpeed.z += acceleration * Time.deltaTime;

            if (currentSpeed.z > maxSpeed)
            {
                currentSpeed.z = maxSpeed;
            }
        }
        else if (direction < 0)
        {
            currentSpeed.z -= acceleration * Time.deltaTime;

            if (currentSpeed.z < maxSpeed * -1)
            {
                currentSpeed.z = maxSpeed * -1;
            }
        }
        else 
        {
            if (currentSpeed.z > 0)
            {
                currentSpeed.z -= acceleration * Time.deltaTime;

                if (currentSpeed.z < 0)
                {
                    currentSpeed.z = 0;
                }
            }
            else if (currentSpeed.z < 0) 
            {
                currentSpeed.z += acceleration * Time.deltaTime;

                if (currentSpeed.z > 0)
                {
                    currentSpeed.z = 0;
                }
            }
        }
    }

    void AccelerateSide(int direction) 
    {
        if (direction > 0)
        {
            currentSpeed.x += acceleration * Time.deltaTime;

            if (currentSpeed.x > maxSpeed)
            {
                currentSpeed.x = maxSpeed;
            }
        }
        else if (direction < 0)
        {
            currentSpeed.x -= acceleration * Time.deltaTime;

            if (currentSpeed.x < maxSpeed * -1)
            {
                currentSpeed.x = maxSpeed * -1;
            }
        }
        else
        {
            if (currentSpeed.x > 0)
            {
                currentSpeed.x -= acceleration * Time.deltaTime;

                if (currentSpeed.x < 0)
                {
                    currentSpeed.x = 0;
                }
            }
            else if (currentSpeed.x < 0)
            {
                currentSpeed.x += acceleration * Time.deltaTime;

                if (currentSpeed.x > 0)
                {
                    currentSpeed.x = 0;
                }
            }
        }
    }
}

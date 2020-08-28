using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private bool trigger = false;
    private Vector2 firstTouchPos;
    private Vector3 firstPos;
    // Start is called before the first frame update
    void Start()
    {
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            if (!trigger)
            {
                trigger = true;
                firstTouchPos = Input.mousePosition;
                firstPos = transform.position;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            trigger = false;                                                                                                                                                                           
        }

        if (trigger)
        {
            Vector2 diff = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - firstTouchPos;
            Vector3 forwardHorizontal = new Vector3(transform.forward.x,0, transform.forward.z);
            Vector3 movement = -transform.forward*diff.y + Vector3.Cross(transform.forward,new Vector3(0,1,0))*diff.x;
            transform.position = firstPos + (movement) *0.01f;
        }

        //Vector3 mouse = Input.mousePosition;
        //Debug.Log("mouse " + mouse.x + " " + mouse.y);

        //yaw += speedH*Input.GetAxis("Mouse X");
        //pitch -= speedV*Input.GetAxis("Mouse Y");

        //transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }
}

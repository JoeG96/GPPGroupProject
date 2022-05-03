using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platforms : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float speed;
    private Rigidbody rb;
    private Vector3 currentPos;

    CharacterController characterController;

    public bool isTriggered = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        //    currentPos = Vector3.Lerp(startPoint.position, endPoint.position,
        //        Mathf.Cos(Time.time / speed * Mathf.PI * 2) * -.5f + .5f);
        //    rb.MovePosition(currentPos);       

        if (isTriggered)
        {
            float step = speed * Time.deltaTime;
            currentPos = Vector3.MoveTowards(transform.position, endPoint.position, step);
            rb.MovePosition(currentPos);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            characterController = other.GetComponent<CharacterController>();
        }

        if (other.gameObject.tag == "EndPoint")
        {
            transform.position = startPoint.transform.position;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            characterController.Move(rb.velocity * Time.deltaTime);
        }
    }
}

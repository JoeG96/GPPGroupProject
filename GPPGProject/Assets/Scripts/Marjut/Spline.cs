using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class Spline : MonoBehaviour
{
    public PathCreator pathCreator;
    public EndOfPathInstruction end;
    public float speed;
    float distanceTravelled;

    // Update is called once per frame
    void Update()
    {
        distanceTravelled += speed * Time.deltaTime;
        transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, end);
        //transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, end);
    }
}

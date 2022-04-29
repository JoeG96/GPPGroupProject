using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPlatforms : MonoBehaviour
{
    [SerializeField] Collider hand;
    public GameObject[] platforms;

    void Start()
    {
        foreach (GameObject platform in platforms)
        {
            MovingPlatforms movingPlatforms;
            movingPlatforms = platform.GetComponent<MovingPlatforms>();
            movingPlatforms.isTriggered = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider == hand)
        {       
            foreach (GameObject platform in platforms)
            {
                MovingPlatforms movingPlatforms;
                movingPlatforms = platform.GetComponent<MovingPlatforms>();
                movingPlatforms.isTriggered = false;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _TriggerPlatforms : MonoBehaviour
{
    [SerializeField] Collider hand;
    public GameObject[] platforms;

    void Start()
    {
        foreach (GameObject platform in platforms)
        {
            BackAndForwardPlatfroms movingPlatforms;
            movingPlatforms = platform.GetComponent<BackAndForwardPlatfroms>();
            movingPlatforms.isTriggered = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider == hand)
        {
            foreach (GameObject platform in platforms)
            {
                BackAndForwardPlatfroms movingPlatforms;
                movingPlatforms = platform.GetComponent<BackAndForwardPlatfroms>();
                movingPlatforms.isTriggered = true;
            }
        }
    }
}

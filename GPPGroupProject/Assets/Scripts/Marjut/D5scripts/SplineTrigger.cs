using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineTrigger : MonoBehaviour
{
    public GameObject cutsceneCam;
    public GameObject mainCam;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            cutsceneCam.SetActive(true);
            mainCam.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            cutsceneCam.SetActive(false);
            mainCam.SetActive(true);
        }
    }
}

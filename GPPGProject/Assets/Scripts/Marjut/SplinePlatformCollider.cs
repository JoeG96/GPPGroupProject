using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplinePlatformCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        other.transform.SetParent(transform, true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        other.transform.SetParent(null);
    }
}

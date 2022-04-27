using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    public Collider player;
    public float damage;

    void Damage()
    {
        HealthController.currentHealth -= damage;
    }        

    private void OnTriggerEnter(Collider other)
    {
        if (other == player)
        {
            Damage();
        }
    }
}

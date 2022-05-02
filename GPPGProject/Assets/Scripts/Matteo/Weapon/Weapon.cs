using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] float damage = 10f;
    [SerializeField] PlayerController playerController;

    public float GetDamage()
    {
        playerController = GetComponent<PlayerController>();
        /*if (playerController._attackActive)
        {
            damage = 100f;
        }
        else
        {
            damage = 10f;
        }*/
            
        return damage;
    }
}

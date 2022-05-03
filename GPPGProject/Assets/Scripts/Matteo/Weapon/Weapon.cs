using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] float damage = 10f;
    [SerializeField] float improvedDamage = 100f;
    [SerializeField] PlayerController playerController;

    private void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public float GetDamage()
    {
        if (playerController._attackActive)
        {
            return improvedDamage;
        }
        else
        {
            return damage;
        }
    }
}

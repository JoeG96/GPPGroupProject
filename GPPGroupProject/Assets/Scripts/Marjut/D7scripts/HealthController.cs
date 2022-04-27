using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public Slider healthSlider;
    public float maxHealth;
    public static float currentHealth;

    public Transform playerPos;
    public Transform respawnPoint;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.value = currentHealth;

        if(currentHealth == 0)
        {
            playerPos.transform.position = respawnPoint.transform.position;
            currentHealth = maxHealth;
        }
    }
}

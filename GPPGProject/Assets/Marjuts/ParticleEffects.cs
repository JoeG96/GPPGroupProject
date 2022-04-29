using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffects : MonoBehaviour
{
    public ParticleSystem electricity;
    ParticleSystem.EmissionModule emissionModule;
    float emissionRate = 50f;

    float speedBoosTimer = 0;
    bool isSpeedBoosting = false;
    public int maxTimeSpeedBoost = 4;

    int jumpCounter = 3;
    bool isDoubleJumping = false;
    public int maxTimeDoubleJump = 6;

    private void Awake()
    {
        electricity = GetComponent<ParticleSystem>();
        emissionModule = electricity.emission;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void handleBoostPowerUp()
    {
        if (isSpeedBoosting)
        {
            speedBoosTimer += Time.deltaTime;
            emissionRate -= Time.deltaTime;
            if (speedBoosTimer >= maxTimeSpeedBoost)
            {
                walkSpeed = 5;
                runSpeed = 9;
                animator.SetFloat("speed", 1);
                speedBoosTimer = 0;
                isSpeedBoosting = false;
                speedParticle.SetActive(false);
            }
            emissionModule.rateOverTime = emissionRate;
        }
    }

    void handleDoubleJumpPowerUp()
    {
        if (isDoubleJumping)
        {
            speedBoosTimer += Time.deltaTime;
            if (speedBoosTimer >= maxTimeDoubleJump)
            {
                speedBoosTimer = 0;
                isDoubleJumping = false;
                doubleJumpParticle.SetActive(false);
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "SpeedBoost")
        {
            walkSpeed = 15;
            runSpeed = 20;
            isSpeedBoosting = true;
            animator.SetFloat("speed", 2);
            speedParticle.SetActive(true);
            speedBoostCollectables.SetActive(true);
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "DoubleJump")
        {
            isDoubleJumping = true;
            doubleJumpParticle.SetActive(true);
            doubleJumpCollectables.SetActive(true);
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Collectable")
        {
            score += 1;
            scoreText.text = "Score " + score.ToString();
            Destroy(other.gameObject);
        }
    }
}

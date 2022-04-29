using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrokenCube : MonoBehaviour
{
    //public GameObject brokenCube;
    public GameObject enemy;
    public Collider sword;
    public Collider player;
    int hitCounter = 0;

    float randomPosX;
    float randomPosZ;
    public Transform spawnPos;
    public Transform pos;

    Vector3 enemySize1;
    Vector3 enemySize2;
    Vector3 enemySize3;

    private void Start()
    {
        randomPosX = Random.Range(1f, 7f);
        randomPosZ = Random.Range(5f, 10f);
        spawnPos.position = pos.position + new Vector3(randomPosX, 0, randomPosZ);

        enemySize1 = new Vector3(4, 4, 4);
        enemySize2 = new Vector3(3, 3, 3);
        enemySize3 = new Vector3(2, 2, 2);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other == sword)
        {
            //Debug.Log("collision");
            hitCounter++;
            if(hitCounter == 5)
            {
                if(enemy.transform.localScale == enemySize1)
                {
                    enemy.transform.localScale = new Vector3(3, 3, 3);
                    Instantiate(enemy, spawnPos.position, Quaternion.identity);
                    Instantiate(enemy, spawnPos.position, Quaternion.identity);
                    Destroy(gameObject);

                    hitCounter = 0;
                }
            }
            if(hitCounter == 3)
            {
                if (enemy.transform.localScale == enemySize2)
                {
                    enemy.transform.localScale = new Vector3(2, 2, 2);
                    Instantiate(enemy, spawnPos.position, Quaternion.identity);
                    Instantiate(enemy, spawnPos.position, Quaternion.identity);
                    Destroy(gameObject);

                    hitCounter = 0;
                }
            }
            if(hitCounter == 2)
            {
                if (enemy.transform.localScale == enemySize3)
                {
                    Destroy(enemy);
                }
            }
        }
    }
}

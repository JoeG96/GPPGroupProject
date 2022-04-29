using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public GameObject door;
    public GameObject cutsceneCam;
    public GameObject player;
    public GameObject button;

    public Material green;

    [SerializeField]Collider hand;

    Animator buttonAnim;
    public Animator doorAnim;
    public Animator playerAnim;

    public CharacterController playerCharacterController;

    int isButtonPressedHash;
    bool isOpen = false;
    float timer = 0;

    private void Start()
    {
        buttonAnim = GetComponent<Animator>();
        doorAnim.GetComponent<Animator>().enabled = false;

        isButtonPressedHash = Animator.StringToHash("isButtonPressed");
    }

    private void Update()
    {
        handleAnimation();
    }

    void handleAnimation()
    {
       bool isButtonPressed = buttonAnim.GetBool(isButtonPressedHash);
     
        if (isOpen)
        {
           buttonAnim.SetBool(isButtonPressedHash, true);
           timer += Time.deltaTime;
            if(timer >= 1)
            {
                doorAnim.GetComponent<Animator>().enabled = true;
            }         
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider == hand)
        {          
            if (!isOpen)
             {
                isOpen = true;
                cutsceneCam.SetActive(true);
                player.SetActive(false);
                playerCharacterController.enabled = false;
                playerAnim.GetComponent<Animator>().enabled = false;
                button.GetComponent<Renderer>().material = green;
                StartCoroutine(FinishCutscene());
            }
        }
    }

    IEnumerator FinishCutscene()
    {
        yield return new WaitForSeconds(3);
        player.SetActive(true);
        cutsceneCam.SetActive(false);
        playerCharacterController.enabled = true;
        playerAnim.GetComponent<Animator>().enabled = true;
    }
}

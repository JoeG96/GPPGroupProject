using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class _CharacterMovement : MonoBehaviour
{
    PlayerControls input;
    Animator animator;
    CharacterController characterController;
    public GameObject speedParticle;
    public GameObject doubleJumpParticle;
    public GameObject speedBoostCollectables;
    public GameObject doubleJumpCollectables;
    public Text scoreText;

    public Transform child;

    public ParticleSystem electricity;
    ParticleSystem.EmissionModule emissionModule;
    float emissionRate = 50f;

    int score = 0;

    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentRunMovement;
    Vector3 appliedMovement;
    Vector3 worldAppliedMovement;
    public float walkSpeed = 4.0f;
    public float runSpeed = 9.0f;
    bool isMovementPressed;
    bool isRunPressed;

    int isWalkingHash;
    int isRunningHash;
    int isJumpingHash;

    float gravity = -9.8f;
    float groundedGravity = -.05f;

    bool isJumpPressed = false;
    float initialJumpVelocity;
    public float maxJumpHeight = 4f;
    public float maxJumpTime = 0.75f;
    bool isJumping = false;
    bool isJumpAnimating = false;

    public float rotateSpeed;
    Vector2 _rotateInput;

    bool isAttackPressed;
    bool isPunchPressed;

    float speedBoosTimer = 0;
    bool isSpeedBoosting = false;
    public int maxTimeSpeedBoost = 4;

    int jumpCounter = 3;
    bool isDoubleJumping = false;
    public int maxTimeDoubleJump = 6;

    float rotationFactorPerFrame = 15.0f;

    private void Awake()
    {
        input = new PlayerControls();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        electricity = GetComponent<ParticleSystem>();
        emissionModule = electricity.emission;

        scoreText.text = "Score " + score.ToString();

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");

        input.Player.Move.started += onMovementInput;
        input.Player.Move.canceled += onMovementInput;
        input.Player.Move.performed += onMovementInput;
        input.Player.Run.started += onRun;
        input.Player.Run.canceled += onRun;
        input.Player.Jump.started += onJump;
        input.Player.Jump.canceled += onJump;
        input.Player.Attack.started += onAttack;
        input.Player.Attack.canceled += onAttack;
        input.Player.Punch.started += onPunch;
        input.Player.Punch.canceled += onPunch;
    
        //input.Player.Camera.started += rotateInput;
        //input.Player.Camera.canceled += rotateInput;

        setupJumpVariables();
    }

    void Update()
    {
        handleRotation();
        //rotateCam(_rotateInput);
        handleAnimation();

        if (isRunPressed)
        {
            //appliedMovement.x = currentRunMovement.x;
            //appliedMovement.z = currentRunMovement.z;
            characterController.Move(currentRunMovement * Time.deltaTime);
        }
        else
        {
            //appliedMovement.x = currentMovement.x;
            //appliedMovement.z = currentMovement.z;
            characterController.Move(currentMovement * Time.deltaTime);
        }

        //worldAppliedMovement = transform.TransformDirection(appliedMovement);
        //characterController.Move(worldAppliedMovement * Time.deltaTime);

        handleGravity();
        resetJump();
        handleBoostPowerUp();
        handleDoubleJumpPowerUp();

        if (!isSpeedBoosting)
        {
            speedParticle.SetActive(false);
            speedBoostCollectables.SetActive(false);
        }

        if (!isDoubleJumping)
        {
            doubleJumpParticle.SetActive(false);
            doubleJumpCollectables.SetActive(false);
        }
    }

   //void rotateInput(InputAction.CallbackContext context)    
   //{
   //     _rotateInput = context.ReadValue<Vector2>();
   //}

   //void rotateCam(Vector2 rotateInput)
   //{
   //    transform.RotateAround(transform.position, new Vector3(0, rotateInput.x, 0), rotateSpeed * Time.deltaTime);
   //}

    void onMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x * walkSpeed;
        currentMovement.z = currentMovementInput.y * walkSpeed;
        currentRunMovement.x = currentMovementInput.x * runSpeed;
        currentRunMovement.z = currentMovementInput.y * runSpeed;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }

    void onAttack(InputAction.CallbackContext context)
    {
        isAttackPressed = context.ReadValueAsButton();

        int attackMode = Random.Range(1, 3);
        if (isAttackPressed)
        {
            animator.SetTrigger("Attack " + attackMode);
        }
    }

    void onPunch(InputAction.CallbackContext context)
    {
        isPunchPressed = context.ReadValueAsButton();

        if (isPunchPressed)
        {
            animator.SetBool("isPunching", true);
        }
        else
        {
            animator.SetBool("isPunching", false);
        }
    }

    void onRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }

    void onJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();

        if (isJumpPressed)
        {
            if (isDoubleJumping)
            {
                handleDoubleJump();
            }
            else
            {
                handleJump();
            }
        }
    }

    void setupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }

    void handleJump()
    {
        if (!isJumping && characterController.isGrounded)
        {
            animator.SetBool(isJumpingHash, true);
            isJumpAnimating = true;
            isJumping = true;
            currentMovement.y = initialJumpVelocity;
            appliedMovement.y = initialJumpVelocity;
        }
    }

    void handleDoubleJump()
    {
        if (!isJumping && characterController.isGrounded && jumpCounter > 0)
        {
            animator.SetBool(isJumpingHash, true);
            isJumpAnimating = true;
            isJumping = true;
            currentMovement.y = initialJumpVelocity;
            appliedMovement.y = initialJumpVelocity;
            jumpCounter -= 1;
        }
        else if (!characterController.isGrounded && isJumping && jumpCounter > 0)
        {
            currentMovement.y = initialJumpVelocity;
            appliedMovement.y = initialJumpVelocity;
            jumpCounter -= 1;
        }
    }

    void resetJump()
    {
        if (isJumping && characterController.isGrounded)
        {
            isJumping = false;
            jumpCounter = 3;
        }
    }

    void handleGravity()
    {
        bool isFalling = currentMovement.y <= 0.0f || !isJumpPressed;
        float fallMultiplier = 2.0f;

        if (characterController.isGrounded)
        {
            if (isJumpAnimating)
            {
                animator.SetBool(isJumpingHash, false);
                isJumpAnimating = false;
            }
            currentMovement.y = groundedGravity;
            appliedMovement.y = groundedGravity;
        }
        else if (isFalling)
        {
            float previousYVelocity = currentMovement.y;
            currentMovement.y = currentMovement.y + (gravity * fallMultiplier * Time.deltaTime);
            appliedMovement.y = (previousYVelocity + currentMovement.y) * .5f;
        }
        else
        {
            float previousYVelocity = currentMovement.y;
            currentMovement.y = currentMovement.y + (gravity * Time.deltaTime);
            appliedMovement.y = (previousYVelocity + currentMovement.y) * .5f;
        }
    }

    void handleRotation()
    {
        Vector3 positionToLookAt;

        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = currentMovement.z;

        Quaternion currentRotation = transform.rotation;

        if(isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }       
    }

    void handleAnimation()
    {
        bool isRunning = animator.GetBool(isRunningHash);
        bool isWalking = animator.GetBool(isWalkingHash);

        if (isMovementPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);
        }

        if (!isMovementPressed && isWalking)
        {
            animator.SetBool(isWalkingHash, false);
        }

        if ((isMovementPressed && isRunPressed) && !isRunning)
        {
            animator.SetBool(isRunningHash, true);
        }

        if ((!isMovementPressed || !isRunPressed) && isRunning)
        {
            animator.SetBool(isRunningHash, false);
        }
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

    private void OnEnable()
    {
        input.Player.Enable();
    }

    private void OnDisable()
    {
        input.Player.Disable();
    }
}

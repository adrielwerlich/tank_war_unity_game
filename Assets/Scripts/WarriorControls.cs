using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class WarriorControls : MonoBehaviour
{
    Animator animator;
    private float speed = 0;
    private bool walking = false;

    private bool speedBoost = false;

    [SerializeField] private int speedRunning = 20;
    [SerializeField] private int speedWalking = 10;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private PlayerInputControls inputControls;

    [SerializeField] private AudioClip laserSound;
    private AudioSource audioSource;
    private void Awake()
    {
        inputControls = new PlayerInputControls();
    }

    private void OnEnable()
    {
        inputControls.Enable();
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        inputControls.PlayerActionMap.FireWeapon.performed += ctx => HandleAttack();

        // inputControls.PlayerActionMap.Movement.performed += ctx =>
        // {
        //     if (ctx.control.device is Keyboard)
        //     {
        //         Debug.Log("Keyboard input");
        //     }
        //     else if (ctx.control.device is Gamepad)
        //     {
        //         Debug.Log("Gamepad input");
        //     }
        // };

        inputControls.PlayerActionMap.SpeedBoost.performed += OnSpeedBoostPerformed;
        inputControls.PlayerActionMap.SpeedBoost.canceled += OnSpeedBoostCanceled;

    }

    private void OnDestroy()
    {
        inputControls.Disable();
        inputControls.PlayerActionMap.FireWeapon.performed -= ctx => HandleAttack();

        inputControls.PlayerActionMap.SpeedBoost.performed -= OnSpeedBoostPerformed;
        inputControls.PlayerActionMap.SpeedBoost.canceled -= OnSpeedBoostCanceled;

    }

    private void OnSpeedBoostPerformed(InputAction.CallbackContext ctx)
    {
        speedBoost = true;
        // Debug.Log("Speed boost start");
    }

    private void OnSpeedBoostCanceled(InputAction.CallbackContext ctx)
    {
        speedBoost = false;
        // Debug.Log("Speed boost end");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movementInput = inputControls.PlayerActionMap.Movement.ReadValue<Vector2>();
        // Debug.Log("movementInput: " + movementInput.x + ", " + movementInput.y);
        HandleMovementForwardBackward(movementInput.y);
        HandleRotate(movementInput.x);
    }

    private void HandleAttack()
    {
        audioSource.PlayOneShot(laserSound);
        var go = Instantiate(bullet, spawnPoint.position + Vector3.up, transform.rotation);
        go.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }


    private void HandleRotate(float x)
    {
        if (x == 0)
        {
            return;
        }
        if (x <= 0.4)
        {
            transform.Rotate(Vector3.up * Time.deltaTime * 50 * x);
        }
        else if (x > 0.4)
        {
            transform.Rotate(Vector3.up * Time.deltaTime * 50 * x);
        }
    }

    private void HandleMovementForwardBackward(float y)
    {
        if (y > 0)
        {
            SetWalking();

            //make movement forward and set animation to walking
            if (speedBoost)
            {
                IncreaseSpeed(5 * y, speedRunning);
                animator.speed = 2f;
            }
            else
            {
                if (speed <= 2)
                {
                    IncreaseSpeed(2 * y, speedWalking);
                }
                else
                {
                    SetAnimatorAndSpeed();
                }
            }

        }
        else if (y < 0)
        {
            SetWalking();

            //make movement backward and set animation to walking
            if (speedBoost)
            {
                IncreaseSpeed(-5, speedRunning);
                animator.speed = 2f;
            }
            else
            {
                if (speed >= -2)
                {
                    IncreaseSpeed(-2, speedWalking);
                }
                else
                {
                    SetAnimatorAndSpeed();
                }
            }
        }
        else if (speed > 0)
        {
            walking = false;
            animator.SetBool("walking", walking);

            speed -= Time.deltaTime;
            if (speed < 0)
            {
                speed = 0;
            }
        }
        else if (speed > 0)
        {
            walking = false;
            animator.SetBool("walking", walking);

            speed -= Time.deltaTime;
            if (speed < 0)
            {
                speed = 0;
            }
        }
        animator.SetFloat("speed", speed);
    }

    private void SetAnimatorAndSpeed()
    {
        animator.speed = 1f;
        speed = 2;
    }

    private void SetWalking()
    {
        walking = true;
        animator.SetBool("walking", walking);
    }

    private void IncreaseSpeed(float incrementBy, int forwardBy)
    {
        if (incrementBy > 0)
        {
            if (speed <= incrementBy)
            {
                speed += Time.deltaTime * incrementBy;
            }
            if (speed >= 1.3f)
            {
                transform.Translate(Vector3.forward * Time.deltaTime * forwardBy);
            }
        }
        else if (incrementBy < 0)
        {
            if (speed >= incrementBy)
            {
                speed += Time.deltaTime * incrementBy;
            }
            if (speed <= 1.3f)
            {
                this.transform.Translate(Vector3.back * Time.deltaTime * forwardBy);
            }
        }
    }
}

using DG.Tweening;
// using Plugins.GeometricVision.TargetingSystem.BaseCode.MainClasses;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MechMovement : MonoBehaviour
{
    Animator animator;
    private float speed = 0;
    private bool walking = false;
    private bool gunOn = false;
    [SerializeField] private int speedRunning = 20;
    [SerializeField] private int speedWalking = 10;

    [SerializeField] private Transform spine;
    [SerializeField] private float rotationSpeed = 5f;

    // private GV_TargetingSystem targetingSystem;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.CrossFade("NextAnimation", 0f);

        // targetingSystem = GetComponent<GV_TargetingSystem>();
        //StartCoroutine(UpdateClosestTarget());
    }

    IEnumerator UpdateClosestTarget()
    {
        while (true)
        {
            // var closestTarget = targetingSystem.GetClosestTargetAsGameObject(false);

            // string json = JsonUtility.ToJson(closestTarget, true);

            // Debug.Log("closestTarget => " + json);
            yield return new WaitForSeconds(2f);
        }
    }

    void Update()
    {
        HandleMovementForward();
        HandleShowWeapon();

        // write a funtion to handle the rotation of the mech while pressing - copilot do not write comments, write a function
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.up * Time.deltaTime * -50);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(Vector3.up * Time.deltaTime * 50);
        }

    }

    private void LateUpdate()
    {
        //float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        //spine.transform.rotation *= Quaternion.Euler(0, mouseX, 0);
        Vector3 centerPoint = spine.position;
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        spine.RotateAround(centerPoint, Vector3.up, mouseX);
        //Camera.main.transform.LookAt(centerPoint);
    }

    private void HandleShowWeapon()
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            gunOn = true;
            animator.SetBool("gunUp", gunOn);
        }
        else
        {
            gunOn = false;
            animator.SetBool("gunUp", gunOn);
        }
    }

    private void HandleMovementForward()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            walking = true;
            animator.SetBool("acelerating", walking);

            //make movement forward and set animation to walking
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                IncreaseSpeed(5, speedRunning);
            }
            else
            {
                if (speed <= 2)
                {
                    IncreaseSpeed(2, speedWalking);
                }
                else
                {
                    speed = 2;
                }
            }

        }
        else if (speed > 0)
        {
            walking = false;
            animator.SetBool("acelerating", walking);

            speed -= Time.deltaTime;
            if (speed < 0)
            {
                speed = 0;
            }
        }
        animator.SetFloat("speed", speed);
    }

    private void IncreaseSpeed(int incrementBy, int forwardBy)
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
}

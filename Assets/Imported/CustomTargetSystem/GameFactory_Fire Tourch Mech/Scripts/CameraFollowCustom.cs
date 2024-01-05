using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class CameraFollowCustom : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;
    [SerializeField] Vector2 clampAxis = new Vector2(20, 80);
    
    [SerializeField] float follow_smoothing = 5;
    [SerializeField] float rotate_Smoothing = 5;
    [SerializeField] float senstivity = 60;


    float rotX, rotY;
    bool cursorLocked = false;
    [SerializeField] private CinemachineVirtualCamera cam;

    public bool lockedTarget;


    [SerializeField] private Transform mechTransform;
    //private Transform pelvis;


    void Start(){
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        offset = new Vector3(2, 2, 2);
        originalPosition = cam.transform.position;
    }
    void Update()
    {
        
        Vector3 target_P= target.position + offset;
        transform.position = Vector3.Lerp(transform.position, target_P, follow_smoothing * Time.deltaTime);

        
        if(!lockedTarget) CameraTargetRotation(); 
        //else LookAtTarget();

        CameraZoom();
        
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(cursorLocked){
                Cursor.visible= true;
                Cursor.lockState = CursorLockMode.None;
            }else{
                Cursor.visible= false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(ShakeCamera());
        }
    }

    [SerializeField] private float shakeTime;
    [SerializeField] private float shakeAmount;
    private Vector3 originalPosition;
    
    private IEnumerator ShakeCamera()
    {
        float elapsedTime = 0;

        while (elapsedTime < shakeTime)
        {
            cam.transform.position = originalPosition + UnityEngine.Random.insideUnitSphere * shakeAmount;
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        cam.transform.position = originalPosition;
    }

    [SerializeField] private float zoomForce = 1.1f;
    private void CameraZoom()
    {
        var force = zoomForce * Time.deltaTime;
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            cam.m_Lens.FieldOfView -= force;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            cam.m_Lens.FieldOfView += force;
        }
    }

    void CameraTargetRotation()
    {
        Vector2 mouseAxis = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        rotX += (mouseAxis.x * senstivity) * Time.deltaTime;
        rotY -= (mouseAxis.y * senstivity) * Time.deltaTime;

        Quaternion localRotation = Quaternion.Euler(rotY, rotX, 0);
        localRotation *= mechTransform.rotation;

        transform.rotation = Quaternion.Slerp(transform.rotation, localRotation, Time.deltaTime * rotate_Smoothing);
    }

    //void LookAtTarget(){
    //    transform.rotation = cam.rotation;
    //    Vector3 r = cam.eulerAngles;
    //    rotX = r.y;
    //    rotY = 1.8f;
    //}
}

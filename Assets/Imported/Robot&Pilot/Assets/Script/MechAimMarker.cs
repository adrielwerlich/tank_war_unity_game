using UnityEngine.UI;
using UnityEngine;
using ChobiAssets.KTP;
using TMPro;
using UnityEngine.Timeline;

public class MechAimMarker : MonoBehaviour
{
    [SerializeField]
    float zPosition;


    [SerializeField] private Canvas canvas;
    [SerializeField] private Transform playerSpine;

    // Update is called once per frame
    void Update()
    {

        this.transform.position = new Vector3(
            playerSpine.position.x, 
            playerSpine.position.y, 
            playerSpine.position.z + zPosition);

    }

}

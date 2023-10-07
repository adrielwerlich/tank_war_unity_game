using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomRoute : MonoBehaviour
{

    private Vector3 targetPosition;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
        SetNewRandomTargetPosition();
    }



    public float patrolSpeed = 15f;
    public float patrolRange = 100f;
    public int rotationSpeed = 5;
    public float minimumAltitude = 20f; // Adjust this value as needed
    public Terrain terrain; // Reference to the terrain object

    private void Awake()
    {
        terrain = FindObjectOfType<Terrain>();
    }


    void Update()
    {
        // Move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, patrolSpeed * Time.deltaTime);

        // Calculate the direction to the target position
        Vector3 direction = targetPosition - transform.position;

        // Check if the target position has been reached
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            SetNewRandomTargetPosition();
        }
        // Rotate the GameObject to face the direction of movement
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Perform raycasting to avoid terrain collision
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, patrolSpeed * Time.deltaTime))
        {
            // Adjust the target position to avoid terrain collision
            float safeHeightAboveTerrain = 10f; // Adjust this value as needed
            targetPosition = new Vector3(hit.point.x, Mathf.Max(hit.point.y + safeHeightAboveTerrain, minimumAltitude), hit.point.z);
        }
    }

    private void SetNewRandomTargetPosition()
    {

        // Get terrain bounds
        Bounds terrainBounds = terrain.GetComponent<Collider>().bounds;

        // Generate random point within the terrain bounds
        Vector3 randomPoint = new Vector3(
            Random.Range(terrainBounds.min.x, terrainBounds.max.x),
            minimumAltitude,
            Random.Range(terrainBounds.min.z, terrainBounds.max.z)
        );

        // Set the new target position
        targetPosition = randomPoint;

        targetPosition.y = Mathf.Max(targetPosition.y, minimumAltitude); // Ensure minimum altitude
    }

}

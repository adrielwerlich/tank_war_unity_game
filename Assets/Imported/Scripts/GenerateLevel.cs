using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevel : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawnedSections = new List<GameObject>();

    [SerializeField] private GameObject[] section;
    [SerializeField] private Transform player;
    [SerializeField] private float spawnDistanceThreshold = 140f; // Distance from the last terrain to spawn a new one
    
    private float zPos = -90.8f;
    private int randomSection;
    //private int destroyCounter = 0;
    string childNameToFind = "Plane";
    private Vector3 lastTerrainEnd;

    private bool spawnningTerrain = false;

    private Transform FindChildRecursive(Transform parent, string childName)
    {
        if (parent == null)
            return null;

        Transform child = parent.Find(childName);

        if (child != null)
            return child;

        foreach (Transform childTransform in parent)
        {
            foreach (Transform subChild in childTransform)
            { 
                child = FindChildRecursive(childTransform, childName);
                if (child != null)
                    return child;
            }
            child = FindChildRecursive(childTransform, childName);
            if (child != null)
                return child;
        }

        return null;
    }

    private void Start()
    {
        GetPlaneAndSetPositions(spawnedSections[spawnedSections.Count - 1].transform);

    }

    private void GetPlaneAndSetPositions(Transform parent)
    {
        Transform plane = FindChildRecursive(parent, childNameToFind);

        if (plane != null)
        {
            GetRendererAndSetPositions(plane);
        }
    }

    private void GetRendererAndSetPositions(Transform plane)
    {
        Renderer r = plane.GetComponent<Renderer>();
        if (r != null)
        {
            Bounds bounds = r.bounds;
            zPos = bounds.center.z;
            lastTerrainEnd = new Vector3(0, 0, bounds.max.z + 59);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.z > zPos - spawnDistanceThreshold && !spawnningTerrain)
        {
            spawnningTerrain = true;
            SpawnNewTerrain();
        }
    }

    private void SpawnNewTerrain()
    {
        // Calculate the position for the new terrain
        Vector3 spawnPosition = lastTerrainEnd;

        randomSection = Random.Range(0,3);

        GameObject terrain = section[randomSection];
        // Instantiate the new terrain
        var newTerrain = Instantiate(
            terrain, 
            spawnPosition, 
            Quaternion.identity,
            this.transform
        );

        newTerrain.transform.localScale = new Vector3(
            6,
            6,
            6
        );

        spawnedSections.Add(newTerrain);

        GetPlaneAndSetPositions(newTerrain.transform);

        StartCoroutine(WaitAndDestroy());
    }

    private IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(2f);
        if (spawnedSections.Count > 5)
        {
            // destroy spawned section
            GameObject spawnedSection = spawnedSections[0];
            Destroy(spawnedSection);
            spawnedSections.Remove(spawnedSection);
            //destroyCounter++;
        }
        spawnningTerrain = false;
    }
}

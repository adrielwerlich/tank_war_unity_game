using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEnemies : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawnedPacks = new List<GameObject>();
    [SerializeField] private Transform player;
    [SerializeField] private GameObject[] enemyPacks;
    private int packNumber;
    private int destroyCounter = 0;

    [SerializeField] private float spawnDistanceThreshold = 50f; // Distance from the last terrain to spawn a new one

    private Vector3 nextPositionForward;

    private void Start()
    {
        InvokeRepeating("SpawnNewEnemyPack", 4f, 4f);
    }


    // Update is called once per frame
    void Update()
    {
        //if (player.position.z > zPosCreate - spawnDistanceThreshold)
        //{
        //    SpawnNewEnemyPack();
        //}
        foreach (GameObject enemy in spawnedPacks)
        {
            if (enemy != null && enemy.transform.position.z < player.position.z) { 
                Destroy(enemy, 5f);
            }
        }
    }

    private void SpawnNewEnemyPack()
    {


        nextPositionForward = new Vector3(
            Random.Range(-14, 14), 
            Random.Range(2.5f, 9), 
            player.position.z + spawnDistanceThreshold);

        // Calculate the position for the new terrain
        Vector3 spawnPosition = nextPositionForward;

        packNumber = Random.Range(0,1);

        GameObject terrain = enemyPacks[packNumber];
        // Instantiate the new terrain
        var newEnemyPack = Instantiate(
            terrain, 
            spawnPosition, 
            Quaternion.identity,
            this.transform
        );

        spawnedPacks.Add( newEnemyPack );


        //StartCoroutine(WaitAndDestroy());
    }

    private IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(10f);
        // destroy spawned section
        GameObject spawnedSection = spawnedPacks[destroyCounter];
        Destroy(spawnedSection);
        destroyCounter++;
    }
}

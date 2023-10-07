using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private float loadDelay = 1f;
    [SerializeField] private ParticleSystem deathExplosion;
    [SerializeField] private GameObject starShip;

    void OnTriggerEnter(Collider other)
    {
        //StartCrashSequence();
    }

    void StartCrashSequence()
    {
        //Time.timeScale = 0f;
        deathExplosion.Play();

        starShip.GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;

        //GetComponent<InputControls>().enabled = false;
        Invoke("ReloadLevel", loadDelay);
    }

    void ReloadLevel()
    {

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        //Time.timeScale = 1f;
    }

}

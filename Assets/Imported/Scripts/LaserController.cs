using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LaserController : MonoBehaviour
{
    private ParticleSystem ps;
    // Start is called before the first frame update
    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        GameObject body;

        if (sceneName == "Town3_crab_player")
        {
            body = GameObject.Find("PA_Warrior_Player");
        }
        else
        {
            body = GameObject.Find("MainBody");
        }

        ps = GetComponent<ParticleSystem>();
        if (body != null)
        {
            ps.collision.AddPlane(body.transform);
        }
    }

    public string targetTag = "Finish";

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("other.tag => " + other.tag + " or name => " + other.name);
        if (other.CompareTag(targetTag))
        {
            // Handle collision with objects having the specified tag
            // For example, destroy the colliding object
            Destroy(other);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("other.name" + other.name);
        Debug.Log("other.name => " + other.name);
    }
}

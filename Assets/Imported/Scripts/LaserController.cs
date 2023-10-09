using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    private ParticleSystem ps;
    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        ps.collision.AddPlane(GameObject.Find("MainBody").transform);
    }

    public string targetTag = "Finish";

    private void OnParticleCollision(GameObject other)
    {
        //Debug.Log("other.tag => " + other.tag + " or name => " + other.name);
        if (other.CompareTag(targetTag))
        {
            // Handle collision with objects having the specified tag
            // For example, destroy the colliding object
            Destroy(other);
        }
    }
}

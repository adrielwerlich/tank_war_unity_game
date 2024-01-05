using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    private float throttle;
    private GameObject bombExplosionEffect;
    private GameObject player;

    private AudioSource missileExplosionSound;

    private void Start()
    {
        player = GameObject.Find("PA_Warrior_Player");
        if (player == null)
        {
            this.gameObject.SetActive(false);
        }
        missileExplosionSound = GameObject.Find("MissileExplosionSound").GetComponent<AudioSource>();
        throttle = Random.Range(30f, 65f);
        bombExplosionEffect = Resources.Load<GameObject>("Explosions/Explosion4");
        
    }

    void Update()
    {
        ProcessThrottle();
    }

    private float closestDistanceToPlayer = float.MaxValue;
    private bool hasPassedPlayer = false;
    private void ProcessThrottle()
    {
        this.transform.Translate(-transform.forward * throttle * Time.deltaTime);
        if (player == null)
        {
            return;
        }
        float currentDistanceToPlayer = Vector3.Distance(this.transform.position, player.transform.position);

        // Update the closest distance to the player
        if (currentDistanceToPlayer < closestDistanceToPlayer)
        {
            closestDistanceToPlayer = currentDistanceToPlayer;
            hasPassedPlayer = false;  // Reset the flag as we are closing in on the player
        }
        else if (currentDistanceToPlayer > closestDistanceToPlayer)
        {
            // Object is moving away from the player
            if (!hasPassedPlayer)
            {
                hasPassedPlayer = true;  // Set the flag the first time it starts moving away
            }
        }

        // Check if the object has passed the player without hitting and trigger explosion
        if (hasPassedPlayer && currentDistanceToPlayer > 30.0f)
        {
            Explosion();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "PA_Warrior_Player")
        {
            Explosion();
        }
        else if (other.gameObject.tag == "PlayerLaser")
        {
            missileExplosionSound.Play();
            Explosion();
            Destroy(other.gameObject);
        }

    }

    private void Explosion()
    {
        var explosion = Instantiate(
                            bombExplosionEffect,
                            this.transform.position,
                            Quaternion.identity
                        );
        explosion.transform.Rotate(-90.0f, 0.0f, 0.0f);
        Destroy(explosion, 8f);
        Destroy(this.gameObject);
    }

    [SerializeField] private float hitForce = .3f; // Adjust this to control the throwing force

    private void ApplyHitEffect(Vector3 hitDirection, Rigidbody rb)
    {
        if (rb != null)
        {
            rb.AddForce(hitDirection * hitForce);
        }

        StartCoroutine(WaitAndReset(rb));
    }

    private IEnumerator WaitAndReset(Rigidbody rb)
    {
        yield return new WaitForSeconds(.05f);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}

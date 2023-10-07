using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomb : MonoBehaviour
{

    [SerializeField] private float throttle;
    private GameObject bombExplosionEffect;

    private void Start()
    {
        throttle = Random.Range(8f, 25f);
        bombExplosionEffect = Resources.Load<GameObject>("Prefabs/Explosion6");
    }

    void Update()
    {
        ProcessThrottle();
    }

    private void ProcessThrottle()
    {
        this.transform.Translate(transform.forward * throttle * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("$$ bomb hit => " + other.name);

        if (other.name == "Star Destroyer")
        {
            Vector3 hitDirection = other.transform.position - transform.position;

            // Normalize the hit direction vector
            hitDirection.Normalize();

            Rigidbody rb = other.GetComponent<Rigidbody>();
            // Apply a hit effect based on the hitDirection vector
            ApplyHitEffect(hitDirection, rb);
        }
        else if (other.gameObject.tag == "Bullet")
        {
            Destroy(this.gameObject);

            var explosion = Instantiate(
                    bombExplosionEffect,
                    this.transform.position,
                    Quaternion.identity
                );
            explosion.transform.Rotate(-90.0f, 0.0f, 0.0f);
            Destroy(explosion, 8f);
        }

    }

    private float hitForce = .05f; // Adjust this to control the throwing force

    private void ApplyHitEffect(Vector3 hitDirection, Rigidbody rb)
    {
        if (rb != null)
        {
            rb.AddForce(hitDirection * hitForce, ForceMode.Impulse);
        }

        StartCoroutine(WaitAndReset(rb));
    }

    private IEnumerator WaitAndReset(Rigidbody rb)
    {
        yield return new WaitForSeconds(.1f);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}

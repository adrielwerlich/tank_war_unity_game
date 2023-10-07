using UnityEngine;
using System.Collections;

public class DestroyEffect : MonoBehaviour
{

    private float hitForce = 3f; // Adjust this to control the throwing force

    private void OnTriggerEnter(Collider other)
    {

        if (other.name == "Star Destroyer")
        {
            Debug.Log("@@@ BOMB EXPLOSION HIT OTHER @@@ => " + other.name);
            Vector3 hitDirection = other.transform.position - transform.position;

            // Normalize the hit direction vector
            hitDirection.Normalize();

            Rigidbody rb = other.GetComponent<Rigidbody>();
            // Apply a hit effect based on the hitDirection vector
            ApplyHitEffect(hitDirection, rb);
        }
    }

    private void ApplyHitEffect(Vector3 hitDirection, Rigidbody rb)
    {
        if (rb != null)
        {
            Debug.Log("hit %%%");
            rb.AddForce(hitDirection * hitForce, ForceMode.Impulse);
        }

        StartCoroutine(WaitAndReset(rb));
    }

    private IEnumerator WaitAndReset(Rigidbody rb)
    {
        yield return new WaitForSeconds(.15f);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}

using ChobiAssets.KTP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    private Rigidbody rb;
    private GameObject hitEffect;
    private Damage_Control_CS damageController;
    private HealthBarController healthBarController;

    private void Start()
    {
        hitEffect = Resources.Load<GameObject>("Explosions/Explosion_Tank");
        rb = GetComponent<Rigidbody>();

        damageController = GetComponentInParent<Damage_Control_CS>();
        //healthBarController = GameObject.Find("HealthBar").GetComponent<HealthBarController>();

        PatrollingAgent.HitPlayer += EnemyHit;
    }

    private void EnemyHit(Vector3 hitDirection)
    {
        ApplyHitEffect(hitDirection);
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.name.Contains("BEAM"))
        {
            Vector3 hitDirection = GetHitDirection(other.transform);
            ApplyHitEffect(hitDirection);

            //Destroy(other.gameObject);
        }
    }

    [SerializeField] private float rangeMin = 1000;
    [SerializeField] private float rangeMax = 5000;
    [SerializeField] private float hitForce = 14f;
    [SerializeField] private bool useHitForce = false;


    private void ApplyHitEffect(Vector3 hitDirection)
    {
        makeHitEffect();
        if (rb != null)
        {
            float forceMultiplier = useHitForce ? hitForce : Random.Range(rangeMin, rangeMax);
            rb.AddForce(hitDirection * forceMultiplier, ForceMode.Impulse);
            float damageValue = Random.Range(.3f, .10f);
            damageController.Get_Damage(damageValue);
            //healthBarController.Damage(damageValue);
        }

        StartCoroutine(WaitAndReset());
    }

    private void makeHitEffect()
    {
        Vector3 position = new Vector3(
                            this.transform.position.x,
                            this.transform.position.y + 1,
                            this.transform.position.z
                            );

        var explosion = Instantiate(
                hitEffect,
                position,
                Quaternion.identity
        );
        Destroy(explosion, 1f);
    }

    private IEnumerator WaitAndReset()
    {
        yield return new WaitForSeconds(.5f);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Laser")
        {
            Vector3 hitDirection = GetHitDirection(other.transform);
            ApplyHitEffect(hitDirection);
            Destroy(other.gameObject);
        }

        if (other.name.Contains("HeartPackage"))
        {
            float healingValue = Random.Range(15f, 95f);
            damageController.Get_Recovery(healingValue);
            //healthBarController.Heal(healingValue);
            Destroy(other.gameObject);
        }
    }

    private Vector3 GetHitDirection(Transform other)
    {
        Vector3 hitDirection = transform.position - other.transform.position;
        hitDirection.Normalize();
        return hitDirection;
    }
}

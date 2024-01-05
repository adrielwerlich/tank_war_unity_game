using ChobiAssets.KTP;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Microlight.MicroBar;
public class CrabCollisionController : MonoBehaviour
{
    [SerializeField] private AudioSource explosionSound;
    private GameObject bombExplosionEffect;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip missileHitSound;
    private AudioSource audioSource;
    private Rigidbody rb;
    private GameObject hitEffect;
    [SerializeField] private MicroBar healthBarController;

    private float _health = 100f;

    public float Health
    {
        get { return _health; }
        set { _health = value; }
    }

    private void Start()
    {
        hitEffect = Resources.Load<GameObject>("Explosions/Explosion_Tank");
        rb = GetComponent<Rigidbody>();

        healthBarController.Initialize(_health);

        isDead = false;
        audioSource = GetComponent<AudioSource>();
        PatrollingAgent.HitPlayer += EnemyHit;

        explosionSound = GameObject.Find("ExplosionSound").GetComponent<AudioSource>();
        bombExplosionEffect = Resources.Load<GameObject>("Explosions/Explosion1");

        InvokeRepeating("CheckRigidBody", 1f, 1f);
    }

    private void CheckRigidBody()
    {
        if (rb != null)
        {
            if (rb.velocity != Vector3.zero || rb.angularVelocity != Vector3.zero)
            {
                // Debug.Log("Rigidbody is moving or rotating.");
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            // else
            // {
            //     Debug.Log("Rigidbody is stationary.");
            // }
        }
    }

    private void EnemyHit(Vector3 hitDirection)
    {
        ApplyHitEffect(hitDirection);
        _health -= 1f;
        healthBarController.UpdateHealthBar(_health);
    }

    [SerializeField] private float rangeMin = 1;
    [SerializeField] private float rangeMax = 5;
    [SerializeField] private float hitForce = 1f;
    [SerializeField] private bool useHitForce = false;

    private void PlayHitSound() {
        audioSource.PlayOneShot(hitSound);
    }

    private void PlayMissileHitSound() {
        audioSource.PlayOneShot(missileHitSound);
    }

    private void ApplyHitEffect(Vector3 hitDirection)
    {
        makeHitEffect();
        if (rb != null)
        {
            float forceMultiplier = useHitForce ? hitForce : Random.Range(rangeMin, rangeMax);
            rb.AddForce(hitDirection * forceMultiplier, ForceMode.Impulse);
            float damageValue = Random.Range(.2f, .8f);
        }

        StartCoroutine(WaitAndReset());
    }

    private void makeHitEffect(bool isDead = false)
    {
        Vector3 position = new Vector3(
                            this.transform.position.x,
                            this.transform.position.y + 1,
                            this.transform.position.z
                            );

        if (!isDead) {
            var explosion = Instantiate(
                    hitEffect,
                    position,
                    Quaternion.identity
            );
            Destroy(explosion, 1f);
        } else {
            var explosion = Instantiate(
                    bombExplosionEffect,
                    position,
                    Quaternion.identity
            );
            Destroy(explosion, 8f);
        }
    }

    private IEnumerator WaitAndReset()
    {
        yield return new WaitForSeconds(.5f);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("player hit by => " + other.name);
        if (other.tag == "Laser")
        {
            Vector3 hitDirection = GetHitDirection(other.transform);
            ApplyHitEffect(hitDirection);
            PlayHitSound();
            Destroy(other.gameObject);
            DecreaseHealth();
        }

        if (other.name.Contains("Rocket"))
        {
            Vector3 hitDirection = GetHitDirection(other.transform);
            ApplyHitEffect(hitDirection);
            PlayMissileHitSound();
            DecreaseHealth();
            makeHitEffect();
        }

        if (other.name.Contains("HeartPackage"))
        {
            float healingValue = Random.Range(4f, 8f);
            _health += healingValue;
            healthBarController.UpdateHealthBar(_health);
            Destroy(other.gameObject);
        }
    }

    public static event System.Action<bool> ShowGameMenu;
    private bool isDead;
    private void DecreaseHealth(float min = 1f, float max = 5f)
    {
        float damageValue = Random.Range(min, max);
        _health -= damageValue;
        healthBarController.UpdateHealthBar(_health);
        if (_health <= 0 && !isDead)
        {
            isDead = true;
            makeHitEffect(true);
            explosionSound.Play();
            Destroy(this.gameObject, .5f);
            ShowGameMenu?.Invoke(true);
        }
    }

    private Vector3 GetHitDirection(Transform other)
    {
        Vector3 hitDirection = transform.position - other.transform.position;
        hitDirection.Normalize();
        return hitDirection;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRunner : MonoBehaviour
{
    public float throttle;
    private Coroutine randomFireWeaponCoroutine;
    private GameObject bombExplosionEffect;
    private GameObject explosionEffect;
    private string[] options = {
        "Blue", "Green", "Grey", "Orange", "Pink", "Red", "Yellow"
    };


    private void Start()
    {

        throttle = Random.Range(.4f, 3f);
        // Start the coroutine to call the function randomly
        randomFireWeaponCoroutine = StartCoroutine(CallFunctionRandomly());
        int randomX = Random.Range(-10, 10);
        int randomY = Random.Range(-5, 4);
        this.transform.position = new Vector3(
                this.transform.position.x + randomX,
                this.transform.position.y + randomY,
                this.transform.position.z
        );

        if (this.transform.position.z < 1.3f)
        {
            this.transform.position = new Vector3(
                this.transform.position.x,
                Random.Range(2.5f, 4.5f),
                this.transform.position.z
            );
        }

        explosionEffect = Resources.Load<GameObject>("Prefabs/EnemyExplosionEffect");
        bombExplosionEffect = Resources.Load<GameObject>("Prefabs/Explosion6");
    }
    private void FireWeapon()
    {
        string randomChoice = options[Random.Range(0, options.Length)];
        int rocketType = Random.Range(0, 30);
        string rocketOption = $"{(rocketType < 10 ? "0" : "")}{rocketType}";
        string path = $"Rockets Missiles and Bombs/Prefabs/{randomChoice}/Rocket{rocketOption}_{randomChoice}";
        
        GameObject prefab = Resources.Load<GameObject>(path);

        if (prefab != null)
        {
            var go = Instantiate(
                prefab, 
                this.transform.position, 
                Quaternion.identity
            );
            // Get the GameObject's initial local scale
            Vector3 initialScale = go.transform.localScale;

            // Calculate the new scale as 10% of the initial scale
            Vector3 newScale = initialScale * 0.05f;

            // Apply the new scale to the GameObject
            go.transform.localScale = newScale;

            // Rotate the GameObject by -90 degrees around the X-axis
            go.transform.Rotate(-90.0f, 0.0f, 0.0f);

            StartCoroutine(DestroyAndExplode(go));
        }
    }

    private IEnumerator DestroyAndExplode(GameObject bomb)
    {
        yield return new WaitForSeconds(6);

        if (bomb != null )
        {
            GameObject.Destroy(bomb);

            var explosion = Instantiate(
                        bombExplosionEffect,
                        bomb.transform.position,
                        Quaternion.identity
                    );
            explosion.transform.Rotate(-90.0f, 0.0f, 0.0f);
            Destroy(explosion, 4f);
        }

    }

    private IEnumerator CallFunctionRandomly()
    {
        while (true)
        {
            // Calculate a random time interval between 1 and 8 seconds
            float randomInterval = Random.Range(.1f, 3f);

            // Wait for the random interval
            yield return new WaitForSeconds(randomInterval);

            // Call the random function
            FireWeapon();
        }
    }

    private void OnDestroy()
    {
        // Stop the coroutine when the object is destroyed
        if (randomFireWeaponCoroutine != null)
        {
            StopCoroutine(randomFireWeaponCoroutine);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrottle();

        
    }

    private void ProcessThrottle()
    {
        this.transform.Translate(Vector3.forward * throttle * Time.deltaTime);
    }

    private void destructionSequence()
    {
        var explosion = Instantiate(
                    explosionEffect,
                    this.transform.position,
                    Quaternion.identity
                );
        //scoreBoard.IncreaseScore(scorePerHit);
        Destroy(gameObject);
        Destroy(explosion, 4f);
    }

    private void OnTriggerEnter(Collider other)
    {
         //Debug.Log("enemy trigger on => " + other.gameObject.tag);

        if (other.gameObject.tag == "Bullet")
        {
            destructionSequence();
        }

    }
}

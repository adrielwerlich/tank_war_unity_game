using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject explosionEffect;
    [SerializeField] int scorePerHit = 1;
    ScoreBoard scoreBoard;

    void Start()
    {
        scoreBoard = FindObjectOfType<ScoreBoard>();
        explosionEffect = Resources.Load<GameObject>("Prefabs/EnemyExplosionEffect");
    }


    void OnParticleCollision(GameObject other)
    {
        destructionSequence();
    }

    private void destructionSequence()
    {
        var explosion = Instantiate(
                    explosionEffect,
                    this.transform.position,
                    Quaternion.identity
                );
        scoreBoard.IncreaseScore(scorePerHit);
        Destroy(gameObject);
        Destroy(explosion, 4f);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("enemy trigger on => " + other.gameObject.tag);

        if (other.gameObject.tag == "Bullet")
        {
            destructionSequence();
        }
        
    }

}

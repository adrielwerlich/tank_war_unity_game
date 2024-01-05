using UnityEngine.SceneManagement;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private AudioSource explosionSound;

    private GameObject bombExplosionEffect1;
    private GameObject bombExplosionEffect2;
    private GameObject healingHeart;

    private int hitsToDestroy;
    private bool isLaserTurret;
    private string sceneName;

    void Start()
    {
        bombExplosionEffect1 = Resources.Load<GameObject>("Explosions/Explosion1");
        bombExplosionEffect2 = Resources.Load<GameObject>("Explosions/Explosion_A");
        healingHeart = Resources.Load<GameObject>("HeartPackage");

        explosionSound = GameObject.Find("ExplosionSound").GetComponent<AudioSource>();

        isLaserTurret = this.name.Contains("Laser_Turret");
        hitsToDestroy = Random.Range(2,8);
        sceneName = SceneManager.GetActiveScene().name;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("enemy colision with => " + other.name);

        if (other.name.Equals("Bullet(Clone)"))
        {
            hitsToDestroy--;
            if (hitsToDestroy == 0)
            {
                DestroySequence();
                MakeHearth();
            }
            else
            {
                makeExplosionEffect(2);
            }
             
        
            if (sceneName == "Town3_crab_player")
            {
                Destroy(other.gameObject);
            }
        }
    }

    private void MakeHearth()
    {
        Instantiate(
            healingHeart,
            GetPosition(),
            Quaternion.identity
        );
    }

    private void DestroySequence()
    {
        GameObject explosion = makeExplosionEffect(1);
        explosionSound.Play();
        Destroy(this.gameObject);
        Destroy(explosion, 8f);
    }

    private GameObject makeExplosionEffect(int type)
    {
        GameObject bombType;
        if (type == 1)
        {
            bombType = bombExplosionEffect1;
        }
        else
        {
            bombType = bombExplosionEffect2;
        }

        var explosion = Instantiate(
            bombType,
            GetPosition(),
            Quaternion.identity
        );
        return explosion;
    }

    private Vector3 GetPosition()
    {
        return new Vector3(
            this.transform.position.x,
            this.transform.position.y + 4,
            this.transform.position.z
        );
    }
}

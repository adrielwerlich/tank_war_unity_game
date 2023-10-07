using System.Collections;
using System.Collections.Generic;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;

public class Target : MonoBehaviour
{
    private GameObject bombExplosionEffect1;
    private GameObject bombExplosionEffect2;
    private GameObject healingHeart;

    private int hitsToDestroy;
    private bool isLaserTurret;

    void Start()
    {
        bombExplosionEffect1 = Resources.Load<GameObject>("Explosions/Explosion1");
        bombExplosionEffect2 = Resources.Load<GameObject>("Explosions/Explosion_A");
        healingHeart = Resources.Load<GameObject>("HeartPackage");

        isLaserTurret = this.name.Contains("Laser_Turret");
        hitsToDestroy = Random.Range(2,8);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(this.name);

        if (other.name.Equals("Bullet(Clone)"))
        {

            //if (!this.name.Contains("Laser_Turret"))
            //{
            //    DestroySequence();
            //}
            //else
            //{
            //}
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

using System.Collections;
using UnityEngine;

public class FireControl_ION_Turret : MonoBehaviour
{
    private Animation _turretAnimation;
    private ParticleSystem _laserBeam;
    [SerializeField] private ParticleSystem[] _laserBeamTypes;

    private Transform player;  // The player's transform
    [SerializeField] private float maxAimRange = 10f;  // Maximum aiming range for the turret
    private Transform gyroscope;
    private Transform rotorBody;

    private bool isFiring = false;
    // Start is called before the first frame update
    void Start()
    {
        _turretAnimation = GetComponent<Animation>();
        int randomType = Random.Range(0, _laserBeamTypes.Length - 1);
        _laserBeam = _laserBeamTypes[randomType].GetComponent<ParticleSystem>();
        player = GameObject.Find("MainBody").transform;

        gyroscope = transform.Find("Gyroscope");
        rotorBody= gyroscope .transform.Find("Rotor_Body");
    }


    // Update is called once per frame
    void Update()
    {
        CheckPlayerDistanceAndAim();
    }

    private void FireWeapon()
    {
        _turretAnimation.Play();
        _laserBeam.Play();
        StartCoroutine(WaitForAnimation(Random.Range(.2f,.6f)));
    }

    private void CheckPlayerDistanceAndAim()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Check if the player is within the maximum aiming range
            if (distanceToPlayer <= maxAimRange)
            {
                AimAtPlayer();
            }
        }
    }

    private void AimAtPlayer()
    {
        // Calculate the direction from the turret to the player
        Vector3 directionToPlayer = player.position - gyroscope.transform.position;

        // Ensure that the turret only rotates on the Y axis to face the player
        directionToPlayer.y = 0f;

        // Rotate the turret to face the player
        if (directionToPlayer != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
            gyroscope.transform.rotation = Quaternion.Slerp(
                gyroscope.transform.rotation, 
                lookRotation, 
                10 * Time.deltaTime);

            rotorBody.transform.rotation = Quaternion.Slerp(
                rotorBody.transform.rotation, lookRotation, 10 * Time.deltaTime);

        } 
        if (!isFiring)
        {
            isFiring = true;
            StartCoroutine(WaitForAndFire(Random.Range(.9f, 1.5f)));
        }
    }

    private IEnumerator WaitForAnimation(float timeToWait)
    {
        // Wait until the animation finishes
        yield return new WaitForSeconds(timeToWait);

        // Call the method to stop the animation or perform other actions
        _turretAnimation.Stop();
    }

    private IEnumerator WaitForAndFire(float timeToWait)
    {
        // Wait until the animation finishes
        yield return new WaitForSeconds(timeToWait);

        FireWeapon();
        isFiring = false;
    }

}

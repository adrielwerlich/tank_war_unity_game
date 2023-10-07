using UnityEngine;

public class LaserMovement : MonoBehaviour
{
    [SerializeField] private float speed;

    void Start()
    {
        speed = Random.Range(70f, 100f);
    }

    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * speed;

    }
}

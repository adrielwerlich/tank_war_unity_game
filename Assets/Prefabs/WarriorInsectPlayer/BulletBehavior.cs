using UnityEngine;
using System.Collections;

public class BulletBehavior : MonoBehaviour {

	[SerializeField] private float speed = 1000f;
	void Start () {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = new Color(Random.value, Random.value, Random.value);
        }
        Destroy(this.gameObject, 4f);
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += transform.forward * Time.deltaTime * speed;
	}

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("bullet colision with => " + other.name);
        //if (other.name.Equals("Enemy"))
        //{
        //    Destroy(other.gameObject);
        //}
    }
}

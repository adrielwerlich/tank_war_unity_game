// using Plugins.GeometricVision.TargetingSystem.BaseCode.MainClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mech : MonoBehaviour {

	public LineRenderer BigCanon01L;
	public LineRenderer BigCanon01R;
	public LineRenderer BigCanon02L;
	public LineRenderer BigCanon02R;

	public LineRenderer SmallCanon01L;
	public LineRenderer SmallCanon01R;
	public LineRenderer SmallCanon02L;
	public LineRenderer SmallCanon02R;

	public AudioClip audioBigCanon;
	public AudioClip audioSmallCanon;

	Animator animator;
	Transform body;
	int direction = 1;
	float counter = 0;
	float rot;
	float shooterCounter = 0f;
	AudioSource audioSource;

	private float speed = 0;


	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		body =  transform.Find ("Mech/Root/Pelvis/Body");
		audioSource = GetComponent<AudioSource> ();
        animator.speed = 0;
    }
	

	void LateUpdate() {
		counter += Time.deltaTime;
		if (counter > 3) {
			direction *= -1;
			counter = 0;
		}
		//rot = rot + Time.deltaTime *20f  * direction;

		//body.localRotation = Quaternion.Euler (new Vector3 (rot, 180f, 0f));

		shooterCounter += Time.deltaTime;

		//if (shooterCounter > 7) {
		//	animator.SetBool ("ShootSmallCanon", true);
		//	StartCoroutine ("FadoutBigCanon01");
		//	StartCoroutine ("FadoutBigCanon02");
		//}
		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
			//make movement forward and set animation to walking
			if (speed <= 1)
			{
				speed += Time.deltaTime * 2;
			}

			if (speed >= 0.8f)
			{
				animator.speed = 1;
				transform.Translate(Vector3.forward * Time.deltaTime * 3);
            }
		} 
		else if (speed >= 0)
        {
            
                speed -= Time.deltaTime * 2;

        }
        animator.speed = speed;
        animator.SetFloat("speed", speed);
    }


	//Big Canons


	void ShootBigCanonA() {
		
		audioSource.clip = audioBigCanon;
		audioSource.Play ();

		Color c = BigCanon01L.material.GetColor ("_TintColor");
		c.a = 1f;
		BigCanon01L.material.SetColor("_TintColor",  c);
		BigCanon01R.material.SetColor("_TintColor",  c);
		StartCoroutine ("FadoutBigCanon01");
	}

	IEnumerator FadoutBigCanon01() {
		Color c = BigCanon01L.material.GetColor ("_TintColor");
		while (c.a > 0) {
			c.a -= 0.1f;
			BigCanon01L.material.SetColor("_TintColor",  c);
			BigCanon01R.material.SetColor("_TintColor",  c);
			yield return null;
		}
	}

	void ShootBigCanonB() {

		audioSource.clip = audioBigCanon;
		audioSource.Play ();

		Color c = BigCanon01L.material.GetColor ("_TintColor");
		c.a = 1f;
		BigCanon02L.material.SetColor("_TintColor",  c);
		BigCanon02R.material.SetColor("_TintColor",  c);
		StartCoroutine ("FadoutBigCanon02");
	}

	IEnumerator FadoutBigCanon02() {
		Color c = BigCanon02L.material.GetColor ("_TintColor");
		while (c.a > 0) {
			c.a -= 0.1f;
			BigCanon02L.material.SetColor("_TintColor",  c);
			BigCanon02R.material.SetColor("_TintColor",  c);
			yield return null;
		}
	}


	// Small Canons


	void ShootSmallCanonA() {

		audioSource.clip = audioSmallCanon;
		audioSource.Play ();

		Color c = SmallCanon01L.material.GetColor ("_TintColor");
		c.a = 1f;
		SmallCanon01L.material.SetColor("_TintColor",  c);
		SmallCanon01R.material.SetColor("_TintColor",  c);
		StartCoroutine ("FadoutSmallCanon01");
	}

	IEnumerator FadoutSmallCanon01() {
		Color c = SmallCanon01L.material.GetColor ("_TintColor");
		while (c.a > 0) {
			c.a -= 0.1f;
			SmallCanon01L.material.SetColor("_TintColor",  c);
			SmallCanon01R.material.SetColor("_TintColor",  c);
			yield return null;
		}
	}

	void ShootSmallCanonB() {

		audioSource.clip = audioSmallCanon;
		audioSource.Play ();

		Color c = SmallCanon01L.material.GetColor ("_TintColor");
		c.a = 1f;
		SmallCanon02L.material.SetColor("_TintColor",  c);
		SmallCanon02R.material.SetColor("_TintColor",  c);
		StartCoroutine ("FadoutSmallCanon02");
	}

	IEnumerator FadoutSmallCanon02() {
		Color c = SmallCanon02L.material.GetColor ("_TintColor");
		while (c.a > 0) {
			c.a -= 0.1f;
			SmallCanon02L.material.SetColor("_TintColor",  c);
			SmallCanon02R.material.SetColor("_TintColor",  c);
			yield return null;
		}
	}





}

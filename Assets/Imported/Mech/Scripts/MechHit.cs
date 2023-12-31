using UnityEngine;

public class MechHit : MonoBehaviour {

	public AudioClip AudioExplosion;

	int walkCycleCounter = 0;
	float rootMotionOffsetWalk = 5.2f;
	Animator animator;
	AudioSource ASource;


	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		ASource = GetComponent<AudioSource> ();
	}
	

	void LateUpdate() {

		Transform body = transform.Find ("Mech/Root/Pelvis/Body");
		body.Rotate(new Vector3(2f,0f,0));

	}


	void EndOfWalk() {
		walkCycleCounter++;
		transform.Translate (new Vector3 (0, 0, rootMotionOffsetWalk));

		if (walkCycleCounter == 5) {
			animator.SetTrigger ("Hit");
			ASource.clip = AudioExplosion;
			ASource.Play ();
		}
	}


}

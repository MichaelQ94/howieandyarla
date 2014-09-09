using UnityEngine;
using System.Collections;

public class ProjectileS : MonoBehaviour {

	public float lifeSpan = 4;

	public float strength = 25; // damage dealt to Howie when hit
	public float knockBackTime = 0.2f;
	public float howieKnockBackMult = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		//transform.LookAt(HowieS.H.transform.position);

		//lifeSpan -= Time.deltaTime;
		//print (rigidbody.velocity);

		if (lifeSpan <= 0){
			Destroy(gameObject);
		}
	
	}

	void OnTriggerEnter (Collider other) {

		if (other.gameObject.tag == "Player"){

			CameraShakeS.C.SmallShake();
			HowieS.H.TakeDamage(strength); // damage Howie
			CameraShakeS.C.TimeSleep(0.2f);
			Destroy(gameObject);
			HowieS.H.KnockBack(knockBackTime);
			HowieS.H.rigidbody.velocity = rigidbody.velocity*howieKnockBackMult;

		}

		if (other.gameObject.tag == "Wall"){

			Destroy(gameObject);

		}

		if (other.gameObject.tag == "Enemy"){

			if (other.GetComponent<EnemyS>().beingHeld){
				Destroy(gameObject);
			}
			
		}

	}
}

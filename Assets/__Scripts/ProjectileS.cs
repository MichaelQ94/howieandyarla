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
			HowieS howie = other.gameObject.GetComponent<HowieS>();
			CameraShakeS.C.SmallShake();
			howie.TakeDamage(strength); // damage Howie
			CameraShakeS.C.TimeSleep(0.2f);
			Destroy(gameObject);
			howie.KnockBack(knockBackTime);
			howie.rigidbody.velocity = rigidbody.velocity*howieKnockBackMult;

		}

		if (other.gameObject.tag == "Wall"){

			Destroy(gameObject);

		}

		if (other.gameObject.tag == "Enemy"){

			// get blocked by held and thrown enemies

			if (other.GetComponent<EnemyS>().beingHeld ||
			    other.GetComponent<EnemyS>().beingThrown){
				Destroy(gameObject);
			}
			
		}

	}
}

using UnityEngine;
using System.Collections;

public class ProjectileS : MonoBehaviour {

	public float lifeSpan = 4;
	public bool 	destroyAfterTime = false;

	public float strength = 25; // damage dealt to Howie when hit
	public float knockBackTime = 0.2f;
	public float howieKnockBackMult = 1;

	public bool friendly = false;
	public bool explodeOnImpact = false;

	public bool fireOnImpact = false;
	public GameObject	fireToSpawn;
	public float fireSpawnPosMult = 2;
	public int 	numFireToSpawn = 3;

	public bool smokeShot = false;

	public bool muzzleFlashOver = false;
	public float begSizeMult = 2;
	public float muzzleFlashTime = 0.2f;

	public GameObject	explosion;


	// Use this for initialization
	void Start () {

		transform.localScale*= begSizeMult;
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		muzzleFlashTime -= Time.deltaTime;
		if (muzzleFlashTime <= 0 && !muzzleFlashOver){
			transform.localScale/=begSizeMult;
			muzzleFlashOver = true;
		}

		//transform.LookAt(HowieS.H.transform.position);
		if (destroyAfterTime){
			lifeSpan -= Time.deltaTime;
		}
		//print (rigidbody.velocity);

		if (lifeSpan <= 0){
			Destroy(gameObject);
		}
	
	}

	public void ReturnToSender () {

		rigidbody.velocity*=-2;
		friendly = true;

	}

	void OnTriggerEnter (Collider other) {

		// send back projectiles if smoke shot
		if (other.gameObject.tag == "Projectile" && smokeShot){
			other.gameObject.GetComponent<ProjectileS>().ReturnToSender();
			Destroy(gameObject);
		}

		if (other.gameObject.tag == "Player" && !friendly){

			if (explodeOnImpact){
				Instantiate(explosion,transform.position,Quaternion.identity);
			}
			if (fireOnImpact){
				for (int i = 0; i < numFireToSpawn; i++ ){
					Vector3 fireSpawnLoc = transform.position;
					fireSpawnLoc.x += fireSpawnPosMult*Random.insideUnitCircle.x;
					fireSpawnLoc.y += fireSpawnPosMult*Random.insideUnitCircle.y;
					fireSpawnLoc.z = 3;
					Instantiate(fireToSpawn,fireSpawnLoc,Quaternion.identity);
				}
			}

			HowieS howie = other.gameObject.GetComponent<HowieS>();
			CameraShakeS.C.SmallShake();
			howie.TakeDamage(strength); // damage Howie
			CameraShakeS.C.TimeSleep(0.2f);
			Destroy(gameObject);
			howie.KnockBack(knockBackTime);
			howie.rigidbody.velocity = rigidbody.velocity*howieKnockBackMult;



		}

		if (other.gameObject.tag == "Wall"){

			if (explodeOnImpact){
				Instantiate(explosion,transform.position,Quaternion.identity);
			}
			if (fireOnImpact){
				for (int i = 0; i < numFireToSpawn; i++ ){
					Vector3 fireSpawnLoc = transform.position;
					fireSpawnLoc.x += fireSpawnPosMult*Random.insideUnitCircle.x;
					fireSpawnLoc.y += fireSpawnPosMult*Random.insideUnitCircle.y;
					fireSpawnLoc.z = 3;
					Instantiate(fireToSpawn,fireSpawnLoc,Quaternion.identity);
				}
			}

			Destroy(gameObject);

		}

		if (other.gameObject.tag == "Enemy"){

			if (explodeOnImpact){
				Instantiate(explosion,transform.position,Quaternion.identity);
			}
			if (fireOnImpact){
				for (int i = 0; i < numFireToSpawn; i++ ){
					Vector3 fireSpawnLoc = transform.position;
					fireSpawnLoc.x += fireSpawnPosMult*Random.insideUnitCircle.x;
					fireSpawnLoc.y += fireSpawnPosMult*Random.insideUnitCircle.y;
					fireSpawnLoc.z = 3;
					Instantiate(fireToSpawn,fireSpawnLoc,Quaternion.identity);
				}
			}

			// get blocked by held and thrown enemies

			if (other.gameObject.GetComponent<EnemyS>().beingHeld ||
			    other.gameObject.GetComponent<EnemyS>().beingThrown){
				Destroy(gameObject);
			}

			// damage enemy if friendly projectile
			if (friendly){
				// smokeshot should do way more knockback/stun
				if (smokeShot){
					other.gameObject.GetComponent<EnemyS>().EnemyKnockback(rigidbody.velocity*50,1,strength);
				}
				else{
					other.gameObject.GetComponent<EnemyS>().EnemyKnockback(rigidbody.velocity,0.2f,strength);
				}
				CameraShakeS.C.LargeShake();
				CameraShakeS.C.TimeSleep(0.1f);
				Destroy(gameObject);
			}
			
		}

	}
}

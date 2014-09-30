using UnityEngine;
using System.Collections;

public class EnemyS : MonoBehaviour {
	
	public float	minimumSpeed = 2;
	
	public Material	defaultMat;
	
	public Material	throwMaxMat;
	
	public bool		atSweetSpot = false;
	
	public GameObject sprite;

	// for hitstun (slight pause after getting hit)
	public bool		hitStunned = false;
	public float 	kickBackMult = 1;
	public float 	hitStunTime;
	
	// I"M GONNA TOTALLY REWORK VULNERABILITY AND HEALTH
	// WATCH OUT ;P

	public float 	shieldHealth;
	public float 	maxShieldHealth;
	public float 	shieldRecoverTimeMax = 2;
	public float 	shieldRecoverTime;
	public bool		vulnerable = false; // when true, can be grabbed/thrown/dealt damage

	public Texture	vulnerableTexture;

	public float 	enemyHealth;
	public float 	maxHealth = 30;
	public bool 	isDead = false;

	public bool 	cannotBeAbsorbed = false;
	public float 	requiredAbsorbTime = 2;

	public GameObject	shadow;


	public Texture deadTexture;

	public float 	deathSleepTime = 0.3f;
	
	public bool		beingThrown = false;
	public bool beingHeld = false;
	public int originalPhysicsLayer;

	public float 	strength = 0;
	
	public int nutritionValue = 25;
	public int energyType = 0; // 0 is colorA, 1 is colorB, 2 is colorC (blue, red, purple?)
	public int energyAmount = 0; // amount to give for metamorphosis

	public bool  canBeSetOnFire = true;
	public bool  onFire = false;
	public float fireTimeMax = 2;
	public float fireTimeCountdown;
	public float fireDamageMax = 0.2f;
	public float fireDamageCountdown;
	public float fireDamageToTake = 5;
	public GameObject	firePrefab;

	public float 	recycleTime = 10;

	public NewChompS	chompy;

	public LevelS level;
	
	// Use this for initialization
	public void EnemyStart () {
		level = (GameObject.FindGameObjectsWithTag("Level")[0]).GetComponent<LevelS>();
		chompy = (GameObject.FindGameObjectsWithTag("Yarla")[0]).GetComponent<NewChompS>();
		// access howie with level.howie.GetComponent<HowieS>()
		enemyHealth = maxHealth;
		originalPhysicsLayer = gameObject.layer;
		//print (originalPhysicsLayer);

	}


	
	// Update is called once per frame
	public void UpdateEnemy () {
		
		FixVelocity();
		CheckVuln();
		CheckFire();
		ManageMat();
		CheckDeath();

		if (isDead){

			renderer.material.SetTexture("_MainTex", deadTexture);
			//birdies.renderer.enabled = false;
			rigidbody.isKinematic = true;
			collider.enabled = false;

			shadow.renderer.enabled = renderer.enabled;

			recycleTime -= Time.deltaTime;
			if (recycleTime <= 0){
				Destroy(gameObject);
			}

		}

		
	}
	
	void FixVelocity () {
		
		if (Mathf.Abs(rigidbody.velocity.x) <= minimumSpeed
		    && Mathf.Abs(rigidbody.velocity.y) <= minimumSpeed
		    && !rigidbody.isKinematic){
			rigidbody.velocity = Vector3.zero;
		}
		
	}
	
	public void ManageMat() {
		
		/*if (atSweetSpot){
			sprite.renderer.material = throwMaxMat;
		}
		
		else{
				if (!knockedOut){
					sprite.renderer.material = defaultMat;
				}
				
			
		}*/
		
		
	}

	void CheckFire () {

		if (onFire){

			fireTimeCountdown -= Time.deltaTime;

			fireDamageCountdown -= Time.deltaTime;

			if (fireDamageCountdown <= 0){
				//print ("TOOK FIRE DAMAGE");
				Vector3 fireSpawnPos = transform.position;
				fireSpawnPos.z += 1;
				Instantiate(firePrefab,fireSpawnPos,Quaternion.identity);
				enemyHealth -= fireDamageToTake;
				fireDamageCountdown = fireDamageMax;
			}


			if (fireTimeCountdown <= 0){
				onFire = false;
			}
		}

	}
	
	void CheckVuln () {

		// shield health stuff
		if (vulnerable){
			shieldRecoverTime -= Time.deltaTime;
			if (shieldRecoverTime <= 0){
				vulnerable = false;
				shieldHealth = maxShieldHealth;
			}
		}
		else{
			if (shieldHealth <= 0){
				vulnerable = true;
				shieldRecoverTime = shieldRecoverTimeMax;
			}
		}
		
		if (rigidbody.velocity == Vector3.zero){
			beingThrown = false;
		}

		// do hitstun stuff here too
		if (hitStunned){
			hitStunTime -= Time.deltaTime;
			if (hitStunTime <= 0){
				hitStunned=false;
			}
		}

		if (beingHeld){
	
			shadow.renderer.enabled = false;
			/*if (!knockedOut){
				breakOutTime -= Time.deltaTime;
			}*/

		}
		else{
			shadow.renderer.enabled = renderer.enabled;
		}
		
		/*if (knockedOut){

			//print (CanBeAbsorbed());

			if (!isDead){
			birdies.renderer.enabled = true;
			}

			stunCountdown -= Time.deltaTime;

			if (stunCountdown <= 0){
				knockedOut = false;
			}
			
		}
		else{
			if (inWeakenedState()){
				birdies.renderer.enabled = true;
			}
			else{
				birdies.renderer.enabled = false;
			}
			
		}*/

		// the following code turns off melee enemy collisions with howie when being held by yarla and being thrown
		if (originalPhysicsLayer != LayerMask.NameToLayer("IgnorePlayer")){
			if (beingHeld || beingThrown){

				gameObject.layer = LayerMask.NameToLayer("IgnorePlayer");
				//print (beingHeld);
				//print (beingThrown);

			}
			else{

				//print ("On DEFAULT LAYER");
				gameObject.layer = LayerMask.NameToLayer("Default");

			}
		}
		
	}


	void CheckDeath () {

		if (!isDead){
		if (enemyHealth <= 0){
			isDead = true;
			CameraShakeS.C.TimeSleep(deathSleepTime);
				//birdies.renderer.enabled = false;
			//print("IM DEAD");
		}
		}

	}

	public void EnemyKnockback (Vector3 hitBackVel, float stunTime, float damage) {


		// to knock back enemy when hit by Yarla's attack
		// takes vel argument to set velocity to 
		// float argument for how long to hitstun enemy
                                		// and second float for damage

		hitStunTime = stunTime;
		hitStunned = true;
		if (!rigidbody.isKinematic){
			rigidbody.velocity = hitBackVel*Time.deltaTime*kickBackMult;
//				print(rigidbody.velocity);
		}

		// mult damage depending on stun or held state

		/*if (beingHeld){
			enemyHealth -= damage*chompy.holdingDamageMult;
		}
		else if (knockedOut || enemyHealth <= maxHealth/2){
			enemyHealth -= damage*chompy.stunnedDamageMult;
		}
		else{*/
		if (vulnerable){
			enemyHealth -= damage;
		}
		else{
			shieldHealth -= damage;
		}
		//}
		

	}

	// method to return if enemy can be absorbed by chompy head

	/*public bool CanBeAbsorbed(){

		if (cannotBeAbsorbed){
			return false;
		}
		else if (knockedOut){
			return true;
		}
		else if (inWeakenedState()){
			return true;
		}
		else{
			return false;
		}

	}*/

	/*public bool inWeakenedState(){

		if (enemyHealth <= maxHealth*lowHealthMult){
			return true;
		}
		else{
			return false;
		}

	}*/
}

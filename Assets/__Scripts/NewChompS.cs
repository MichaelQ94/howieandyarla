using UnityEngine;
using System.Collections;

public class NewChompS : MonoBehaviour {
	
	// script for chomp attack!
	// place on chompy head object

	
	public bool		charging = false; // true if player is charging chomp attack
	public float	timeHeld = 0; // how long in sec chomp has charged
	public float	exceedMult = 0.75f; // damage reduction if player holds chomp past sweet spot
	public float	chompSpot = 0.6f; // sweet spot time in sec

	public float 	chargeDelay; // offset charge start by small amount of time to prevent double attacks
	public float 	chargeDelayMax = 0.25f; // what to set charge delay to whenever one attacks
	
	public bool		chompButtonHeld = false; // true if button is being held for chomp
	
	public GameObject	chompTarget; // what to chomp

	public EnemyDetectS	enemyDetector; // keeps track on enemies in radius
	
	public float 	chompPauseTime = 0.004f; // sleep time in sec for a chomp attack

	public static NewChompS	N; // singleton "N" for new

	public float radiusMult = 0.25f;

	public float chompVel = 4000;
	public float hitBackMult = 3000;
	public float howieMovMult = 0.1f;

	public Material	defaultMat;
	public Material	sweetSpotMat;

	public bool attacking = false;
	public float attackTimeMax = 0.5f;
	public float attackTimeHoldMax;
	public float attackTime;

	public float minDamage = 15;
	public float maxDamageMult = 4;
	public float holdingDamageMult = 2;
	public float stunnedDamageMult = 1.5f;

	
	// Use this for initialization
	void Start () {
		N = this;

		attackTimeHoldMax = attackTimeMax/2;
	}
	
	// update every physics step
	void FixedUpdate () {
		
		// chomp should not work when we are just solo howie!
		// turn this on and off appropriately
		
		if (!HowieS.H.isHowieSolo){

			renderer.enabled = true;

			MoveChompHead();
			ChompAttack(); // method for charging chomp attack
			
		}
		
		// make sure to reset everything when switching to solo howie
		
		else{

			renderer.enabled = false;

			ResetChomp();
			
		}
		
	}

	void OnCollisionEnter (Collision other){

		// if chompy head collides with enemy trigger, check if attacking and deal damage
		if (other.gameObject.tag == "Enemy"){

			if (attacking && chompTarget != enemyDetector.enemyBeingHeld){

				DamageEnemy(other.gameObject.GetComponent<EnemyS>());

				// then turn off the attack
				attacking = false;

			}

		}

		if (other.gameObject.tag == "Wall"){

			//Return to player if hit wall
			attacking = false;

		}

	}

	void MoveChompHead () {

		if (!attacking){

		// accept input for proper platform (mac vs pc)
		if (Application.platform == RuntimePlatform.OSXEditor || 
		    Application.platform == RuntimePlatform.OSXPlayer ||
		    Application.platform == RuntimePlatform.OSXWebPlayer || 
		    Application.platform == RuntimePlatform.OSXDashboardPlayer){

			Vector3 currentPos = transform.localPosition;

			currentPos.x = Input.GetAxis("HorizontalMac")*radiusMult;
			currentPos.y = Input.GetAxis("VerticalMac")*radiusMult;

			transform.localPosition = currentPos;


		}
		
		// same as above but for pc
		if (Application.platform == RuntimePlatform.WindowsEditor || 
		    Application.platform == RuntimePlatform.WindowsPlayer ||
		    Application.platform == RuntimePlatform.WindowsWebPlayer){
			

			Vector3 currentPos = transform.localPosition;
			
			currentPos.x = Input.GetAxis("HorizontalPC")*radiusMult;
			currentPos.y = Input.GetAxis("VerticalPC")*radiusMult;
			
			transform.localPosition = currentPos;

		}

		}


	}
	
	// this charges the chomp attack when button is held
	public void ChompAttack () {

		//print (Input.GetAxisRaw("Fire2Mac"));

		if (enemyDetector.enemyToChomp != null){
			chompTarget = enemyDetector.enemyToChomp;
		}
		else{
		
			chompTarget = null;

		}

		//count down charge delay
		if (chargeDelay > 0){
			chargeDelay -= Time.deltaTime;
		}
		// then activate charging camera shake
		else{
			if (charging){
				CameraShakeS.C.continuousShaking = true;
				CameraShakeS.C.shake_intensity = 0.1f;
			}
		}


		if (chompTarget != enemyDetector.enemyBeingHeld){
		if (attackTime < attackTimeMax){

			if (attackTime == 0){
				// launch chompy head at enemy
				// this is the code that actually triggers the attack and makes the bite move
				if (chompTarget != null){



				Vector3	attackPos = chompTarget.transform.position;
				attackPos.z = transform.position.z;
				
				
				rigidbody.velocity = (attackPos - transform.position).normalized*chompVel*Time.deltaTime;


					// give Howie a bit of momentum in dir of bite too
					HowieS.H.KnockBack(0.1f);
					HowieS.H.rigidbody.velocity = (attackPos - transform.position).normalized*chompVel*howieMovMult*Time.deltaTime;

					// set charge delay to prevent double attacks
					chargeDelay = chargeDelayMax;

				}
			}

			attackTime += Time.deltaTime;

				// turn on collider ONLY WHEN ATTACKING
				collider.enabled = true;
		}
		else{
			attacking = false;

				// turn off collider when not attacking
				collider.enabled = false;
		}
		}
		else{
			if (attackTime < attackTimeHoldMax){
				
				if (attackTime == 0){
					// launch chompy head at enemy
					// this is the code that actually triggers the attack
					if (chompTarget != null){
						
						
						
						Vector3	attackPos = chompTarget.transform.position;
						attackPos.z = transform.position.z;
						
						
						rigidbody.velocity = (attackPos - transform.position).normalized*chompVel*Time.deltaTime;
						
						
						// give Howie a bit of momentum in dir of bite too
						HowieS.H.KnockBack(0.1f);
						HowieS.H.rigidbody.velocity = (attackPos - transform.position).normalized*chompVel*howieMovMult*Time.deltaTime;
						
						// set charge delay to prevent double attacks
						chargeDelay = chargeDelayMax;

						// Damage held enemy
						DamageEnemy(chompTarget.GetComponent<EnemyS>());
					}
				}
				
				attackTime += Time.deltaTime;
				
			}
			else{
				attacking = false;
			}
		}

		// change color when at sweetSpot
		if (timeHeld >= chompSpot-0.3f && timeHeld <= chompSpot){
			renderer.material = sweetSpotMat;
		}
		else{
			renderer.material = defaultMat;
		}
		
		// accept input for proper platform (mac vs pc)
		if (Application.platform == RuntimePlatform.OSXEditor || 
		    Application.platform == RuntimePlatform.OSXPlayer ||
		    Application.platform == RuntimePlatform.OSXWebPlayer || 
		    Application.platform == RuntimePlatform.OSXDashboardPlayer){
			
			// if chomp button is being held...
			if (Input.GetAxisRaw("Fire2Mac") > 0){

				chompButtonHeld = true; // set button down to true

					if (charging && chargeDelay <= 0){
				
						timeHeld += Time.deltaTime;

					}

				//perform initial attack 
					if (!attacking){

						if (!charging && chompTarget != null){

						attackTime = 0;
						attacking = true;

						}
					}
						else{
					
				

					// once initial attack is done, start charging
							
							charging = true; // set charging to true


					}

			}
			else{
				// if player lets go after charging...
				if (charging){

					// if any power has been stored, trigger another attack
					if (timeHeld > 0){

						// only attack if there's a target and chompDelay is over
						if (chompTarget != null){

							
							attackTime = 0;
							attacking = true;



						}

					}
					
						timeHeld = 0; // reset time held
					charging = false; // turn off charging
					CameraShakeS.C.continuousShaking = false; // turn off continuous shaking
					CameraShakeS.C.shake_intensity = 0;
				}
					
					chompButtonHeld = false; 
			}
		}
		
		// same as above but for pc
		if (Application.platform == RuntimePlatform.WindowsEditor || 
		    Application.platform == RuntimePlatform.WindowsPlayer ||
		    Application.platform == RuntimePlatform.WindowsWebPlayer){
			
			// if chomp button is being held...
			if (Input.GetAxisRaw("Fire2PC") > 0){
				
				chompButtonHeld = true; // set button down to true
				
				if (charging && chargeDelay <= 0){
					
					timeHeld += Time.deltaTime;
					
				}
				
				//perform initial attack 
				if (!attacking){
					
					if (!charging && chompTarget != null){
						
						
						attackTime = 0;
						attacking = true;
						
						// set charge delay to prevent double attack
						//chargeDelay = chargeDelayMax;
						
					}
				}
				else{
					
					
					
					// once initial attack is done, start charging
					
					charging = true; // set charging to true
					
					
				}
				
			}
			else{
				// if player lets go after charging...
				if (charging){
					
					// if any power has been stored, trigger another attack
					if (timeHeld > 0){
						
						// only attack if there's a target and chompDelay is over
						if (chompTarget != null && chargeDelay <= 0){
							
							attackTime = 0;
							attacking = true;
							
							
							
						}
						
					}
					
					timeHeld = 0; // reset time held
					charging = false; // turn off charging
					CameraShakeS.C.continuousShaking = false; // turn off continuous shaking
					CameraShakeS.C.shake_intensity = 0;
				}
				
				chompButtonHeld = false; 
			}
		}


		
		
	}
	
	// damages enemy when activated
	void DamageEnemy (EnemyS attackTarget) {

		Vector3 enemyHitBack = rigidbody.velocity.normalized*hitBackMult;

		// determine damage to deal based on charge time

		// if timeHeld is greater than zero, this is not the initial attack
		if (timeHeld > 0){

			if (timeHeld <= chompSpot){

				attackTarget.EnemyKnockback(enemyHitBack, 0.1f, minDamage*maxDamageMult*(timeHeld/chompSpot));

			}
			else{

				attackTarget.EnemyKnockback(enemyHitBack, 0.1f, minDamage*maxDamageMult*exceedMult);

			}

		}
		// if timeHeld is zero, this is the initial attack and does min damage
		else{

			//print ("INITIAL ATTACK");
			attackTarget.EnemyKnockback(enemyHitBack, 0.1f, minDamage);

		}

			// in future, camera shake and sleep should correlate with chomp Power

			CameraShakeS.C.LargeShake(); // shake and sleep camera for added effect
			CameraShakeS.C.TimeSleep(chompPauseTime);


		
	}
	
	void ResetChomp () {
		
		chompTarget = null;
		chompButtonHeld = false;
		charging = false;
		timeHeld = 0;
		chargeDelay = 0;
		
	}
}

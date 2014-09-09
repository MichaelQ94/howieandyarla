using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeavyEnemyS : EnemyS {

	public float 	walkSpeed = 200;
	public float 	chargeSpeed = 450;

	public Vector3	walkingVel;
	public Vector3	attackingVel;
	public Vector3	attackTarget;

	public bool charging = false; // this means charge up, not literally charging towards howie
	public bool attacking = false;

	public float changeWalkTargetCountdown;
	public float changeWalkTargetCountdownMin = 2;
	public float changeWalkTargetCountdownMax = 4;

	public float walkTargetRandomizationMult = 5;

	public float attackTimeCountdown;
	public float attackTimeCountdownMin = 3;
	public float attackTimeCountdownMax = 5;

	public float attackDistanceMin;
	public float attackDuration;
	public float attackDurationMax = 4;

	public float knockBackTime = 0.2f;
	public float howieKnockBackMult = 1000;

	public List<Texture> attackingTextures;
	public int 	currentAttackingTexture;
	public float changeAttackFrameTime;
	public float changeAttackFrameTimeMax = 0.04f;

	public List<Texture> chargingTextures;
	public int currentChargingTexture;
	public float changeChargeFrameTime;
	public float changeChargeFrameTimeMax = 0.04f;

	public List<Texture> walkingTextures;
	public int currentWalkingTextures;
	public float changeWalkFrameTime;
	public float changeWalkFrameTimeMax = 0.04f;

	private bool facingRight = false;


	

	// Use this for initialization
	void Start () {

		// include Enemy Start method from EnemyS!
		EnemyStart();
	
		attackTimeCountdown = attackTimeCountdownMax;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		// include enemy update method from EnemyS!
		UpdateEnemy();

		if (!isDead){
			if (!beingHeld && !beingThrown && !knockedOut && !hitStunned){
				TriggerAttacks();
				WalkAround();
			}
		}

		ManageSprite();

		//print (isHowieAround());
	
	}

	// check if howie is in attack range
	public bool isHowieAround(){

		float howieDistance = Vector3.Distance(new Vector3(HowieS.H.transform.position.x,
		                                                     HowieS.H.transform.position.y,
		                                                     transform.position.z), transform.position);

		if (howieDistance < attackDistanceMin){
			return true;
		}
		else{
			return false;
		}

	}

	void WalkAround () {

		if (!attacking && !charging){

			changeWalkTargetCountdown -= Time.deltaTime;

			if (changeWalkTargetCountdown <= 0){
						
				Vector3 moveTarget;
					
				// if howie is around, move around randomly
				if (isHowieAround()){
	
					moveTarget = transform.position;
	
					moveTarget.x += Random.insideUnitCircle.x*walkTargetRandomizationMult;
					moveTarget.y += Random.insideUnitCircle.y*walkTargetRandomizationMult;
	
				}
					// if howie not around, move towards general vicinity
				else{
	
					moveTarget = HowieS.H.transform.position;
					
					moveTarget.x += Random.insideUnitCircle.x*walkTargetRandomizationMult/2;
					moveTarget.y += Random.insideUnitCircle.y*walkTargetRandomizationMult/2;
					moveTarget.z = transform.position.z;
	
					}
	
				// once move target is set, change walk vel to reflect new destination
				walkingVel = (moveTarget - transform.position).normalized*walkSpeed;
	
				// reset change move target countdown
				changeWalkTargetCountdown = Random.Range(changeWalkTargetCountdownMin,changeWalkTargetCountdownMax);
			}
			
			// set velocity to walking velocity
			rigidbody.velocity = walkingVel*Time.deltaTime;
	
			// animate walk cycle
			if (changeWalkFrameTime > 0){
				changeWalkFrameTime -= Time.deltaTime;
			}
			else{
				currentWalkingTextures++;
				if (currentWalkingTextures > walkingTextures.Count - 1){
					currentWalkingTextures = 0;
				}
				changeWalkFrameTime = changeAttackFrameTimeMax;
			}
			renderer.material.SetTexture("_MainTex", walkingTextures[currentWalkingTextures]);
		}
	}

	void ManageSprite () {

		if (isDead){
			renderer.material.SetTexture("_MainTex", deadTexture);
		}
		else{
			if (rigidbody.velocity.x > 0){
				facingRight = true;
			}
			if (rigidbody.velocity.x < 0){
				facingRight = false;
			}
			
			// flip texture to face correct direction
			if (facingRight){
				renderer.material.SetTextureScale("_MainTex",new Vector2(1,-1));
			}
			else{
				renderer.material.SetTextureScale("_MainTex",new Vector2(-1,-1));
			}
		}

	}

	void TriggerAttacks () {

		if (isHowieAround()){

			// count down time until attack start
			if (!attacking && !charging){
				attackTimeCountdown -= Time.deltaTime;
				if (attackTimeCountdown <= 0){
					if (!charging){
					currentChargingTexture = 0;
					charging = true;
					}

					// dont' move while charging
					rigidbody.velocity = Vector3.zero;
				}
			}
			else{
				// handle charging
				if (charging){

					// animate charge
					if (changeChargeFrameTime > 0){
						changeChargeFrameTime -= Time.deltaTime;

					}
					else{
						currentChargingTexture++;

						// when done charging, launch attack
						if (currentChargingTexture > chargingTextures.Count - 1){
							charging = false;
							attacking = true;
							currentAttackingTexture = 0;
							attackDuration = attackDurationMax;

							// set attack target and velocity at time of charge complete
							attackTarget = HowieS.H.transform.position;
							attackTarget.z = transform.position.z;

							attackingVel = (attackTarget - transform.position).normalized*chargeSpeed;
						}
						changeChargeFrameTime = changeChargeFrameTimeMax;

					}

					// set appropriate frame
					if (currentChargingTexture <= chargingTextures.Count - 1){
					renderer.material.SetTexture("_MainTex", chargingTextures[currentChargingTexture]);
					}
				}

				// handle attacking
				if (attacking){
					// count down attack time
					if (attackDuration > 0){
						attackDuration -= Time.deltaTime;
					}
					// turn off attack at 0
					else{
						attacking = false;
						attackTimeCountdown = Random.Range(attackTimeCountdownMin, attackTimeCountdownMax);
						currentWalkingTextures = 0;
						changeWalkTargetCountdown = 0;
					}

					// animate the attack
					if (changeAttackFrameTime > 0){
						changeAttackFrameTime -= Time.deltaTime;
					}
					else{
						currentAttackingTexture++;
						if (currentAttackingTexture > attackingTextures.Count - 1){
							currentAttackingTexture = 0;
						}
						changeAttackFrameTime = changeAttackFrameTimeMax;
					}

					// set velocity of attack
					rigidbody.velocity = attackingVel*Time.deltaTime;

					// set appropriate attack frame
					renderer.material.SetTexture("_MainTex", attackingTextures[currentAttackingTexture]);
				}
			}
		}

	}

	void OnCollisionEnter (Collision other){
		
		// htting Howie!
		if (other.gameObject.tag == "Player" && !beingThrown){
			if (!beingHeld){
				HowieS.H.TakeDamage(strength);
				CameraShakeS.C.SmallShake();
				CameraShakeS.C.TimeSleep(0.2f);
				
				// give howie some knockback
				
				HowieS.H.KnockBack(knockBackTime);
				
				Vector3 howieFixPos = HowieS.H.transform.position;
				howieFixPos.z = transform.position.z;
				
				HowieS.H.rigidbody.velocity = (howieFixPos - transform.position).normalized*howieKnockBackMult*Time.deltaTime;

			}
		}
		
		
		if (other.gameObject.tag == "Wall" || other.gameObject.tag == "Enemy"){
			
			// if being thrown, get stunned! otherwise just bounce off
			if (beingThrown){
				knockedOut = true;
				stunCountdown = stunMax;
				if (other.gameObject.tag == "Enemy"){
					other.gameObject.GetComponent<EnemyS>().knockedOut = true;
					other.gameObject.GetComponent<EnemyS>().stunCountdown = stunMax;
				}
			}
			else{
				// bouncing code should be adjusted in future to trigger depending on contact point
				if (attacking){
					attacking = false;
					attackDuration = 0;
					attackTimeCountdown = Random.Range(attackTimeCountdownMin, attackTimeCountdownMax);
				}
				else{
				walkingVel*=-1;
			}
			
		}
		

		
	}
}

}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PaperMonsterS : EnemyS {
	
	public float 	walkSpeed = 200;
	public float 	chargeSpeed = 450;
	
	//public Vector3	walkingVel;
	public Vector3	attackingVel;
	public Vector3	attackTarget;

	public Vector3	teleTarget;

	
	//public bool charging = false; // this means charge up, not literally charging towards howie
	public bool attacking = false;

	public bool 	teleporting;
	public float teleCountdown;
	public float teleCountdownMin = 2;
	public float teleCountdownMax = 4;
	
	public float teleTargetRandomizationMult = 5;
	
	public float attackTimeCountdown;
	public float attackTimeCountdownMin = 3;
	public float attackTimeCountdownMax = 5;
	
	//public float attackDistanceMin;
	public float attackDuration;
	public float attackDurationMax = 4;
	
	public float knockBackTime = 0.2f;
	public float howieKnockBackMult = 1000;
	
	public List<Texture> attackingTextures;
	public int 	currentAttackingTexture;
	public float changeAttackFrameTime;
	public float changeAttackFrameTimeMax = 0.04f;
	
	public List<Texture> teleportingTextures;
	public int currentTeleTexture;
	public float changeTeleFrameTime;
	public float changeTeleFrameTimeMax = 0.04f;
	
	public List<Texture> idleTextures;
	public int currentIdleFrame;
	public float idleFrameTime;
	public float idleFrameTimeMax = 0.04f;

	public List<Texture> vulnTextures;
	public int currentVulnFrame;
	public float vulnFrameTime;
	public float vulnFrameTimeMax = 0.04f;

	public List<Texture> hitTextures;
	public int currentHitFrame;
	public float hitFrameTime;
	public float hitFrameTimeMax = 0.04f;

	public List<Texture> deadTextures;
	public int currentdeadFrame;
	public float deadFrameTime;
	public float deadFrameTimeMax = 0.04f;
	
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
			if (!beingHeld && !beingThrown && !hitStunned && !vulnerable){
				TriggerAttacks();
				Teleport();
			}
		}
		
		ManageSprite();
		
		//print (isHowieAround());
		
	}
	
	// check if howie is in attack range
	/*public bool isHowieAround(){
		
		float howieDistance = Vector3.Distance(new Vector3(level.howie.GetComponent<HowieS>().transform.position.x,
		                                                   level.howie.GetComponent<HowieS>().transform.position.y,
		                                                   transform.position.z), transform.position);
		
		if (howieDistance < attackDistanceMin){
			return true;
		}
		else{
			return false;
		}
		
	}*/
	
	void Teleport () {
		
		if (!attacking){
			
			teleCountdown -= Time.deltaTime;

			if (!teleporting){
				
				rigidbody.velocity = (level.howie.transform.position-transform.position).normalized
					*walkSpeed*Time.deltaTime;
			}
			
			if (teleCountdown <= 0){

				if (!teleporting){
				
				// if howie is around, move around randomly
				//if (isHowieAround()){
				
				teleTarget = level.howie.transform.position;
				
				teleTarget.x += Random.insideUnitCircle.x*teleTargetRandomizationMult;
				teleTarget.y += Random.insideUnitCircle.y*teleTargetRandomizationMult;
				teleTarget.z = transform.position.z;

					if (Vector3.Distance(teleTarget,level.howie.transform.position) > 10){

					currentTeleTexture = 0;
					changeTeleFrameTime = changeTeleFrameTimeMax;
					teleporting = true;
					rigidbody.velocity = Vector3.zero;
					}
				}
				

				else{
					// animate tele cycle
					if (changeTeleFrameTime > 0){
						changeTeleFrameTime -= Time.deltaTime;
					}
					else{
						currentTeleTexture++;
						if (currentTeleTexture > teleportingTextures.Count*0.75f){
							transform.position = teleTarget;
						}
						if (currentTeleTexture > teleportingTextures.Count - 1){
							teleCountdown = Random.Range(teleCountdownMin,teleCountdownMax);
							teleporting = false;
							currentTeleTexture = 0;
						}
						changeTeleFrameTime = changeTeleFrameTimeMax;
					}
					renderer.material.SetTexture("_MainTex", teleportingTextures[currentTeleTexture]);
				}
			}
			

		}
	}
	
	void ManageSprite () {
		
		if (isDead){
			// animate death state
			if (currentdeadFrame < deadTextures.Count-1){
				deadFrameTime -= Time.deltaTime;
			}
			if (deadFrameTime <= 0){
				currentdeadFrame++;
				deadFrameTime = deadFrameTimeMax;
			}
			renderer.material.SetTexture("_MainTex",deadTextures[currentdeadFrame]);
		}
		else if (hitStunned){
			// "hit back" tendril
			if (!vulnerable){
				renderer.material.SetTexture("_MainTex",hitStunTexture);
			}
			else{
				hitFrameTime -= Time.deltaTime;
				if (hitFrameTime <= 0){
					currentHitFrame++;
					if (currentHitFrame >= hitTextures.Count-1){
						currentHitFrame = 0;
					}
					hitFrameTime = hitFrameTimeMax;
				}
				renderer.material.SetTexture("_MainTex",hitTextures[currentHitFrame]);
			}
		}
		else if (vulnerable){
			// animate vulnerable state
			if (currentVulnFrame < vulnTextures.Count-1){
				vulnFrameTime -= Time.deltaTime;
			}
			if (vulnFrameTime <= 0){
				currentVulnFrame++;
				vulnFrameTime = vulnFrameTimeMax;
			}
			renderer.material.SetTexture("_MainTex",vulnTextures[currentVulnFrame]);
		}
		else{

			currentVulnFrame = 0;
			currentHitFrame = 0;

			if (rigidbody.velocity.x > 0){
				facingRight = true;
			}
			if (rigidbody.velocity.x < 0){
				facingRight = false;
			}

			//idle
			if (!attacking && !teleporting){
				idleFrameTime -= Time.deltaTime;
				if (idleFrameTime <= 0){
					currentIdleFrame++;
					if (currentIdleFrame > idleTextures.Count-1){
						currentIdleFrame = 0;
					}
					idleFrameTime = idleFrameTimeMax;
				}

				renderer.material.SetTexture("_MainTex",idleTextures[currentIdleFrame]);
			}
			
			// flip texture to face correct direction
			if (facingRight){
				renderer.material.SetTextureScale("_MainTex",new Vector2(-1,-1));
			}
			else{
				renderer.material.SetTextureScale("_MainTex",new Vector2(1,-1));
			}
		}
		
	}
	
	void TriggerAttacks () {
		
		//if (isHowieAround()){
		
		// count down time until attack start
		if (!attacking && !teleporting){
			attackTimeCountdown -= Time.deltaTime;
			if (attackTimeCountdown <= 0){
			
				
				gameObject.layer = LayerMask.NameToLayer("IgnoreEnemies");

				attacking = true;
				currentAttackingTexture = 0;
				attackDuration = attackDurationMax;
						
				// set attack target and velocity at time of charge complete
				attackTarget = level.howie.transform.position;
				attackTarget.z = transform.position.z;
						
				attackingVel = (attackTarget - transform.position).normalized*chargeSpeed;
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
				
				gameObject.layer = LayerMask.NameToLayer(LayerMask.LayerToName(originalPhysicsLayer));
					//currentWalkingTextures = 0;
					//changeWalkTargetCountdown = 0;
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

		//}
		
	}
	
	void OnCollisionEnter (Collision other){
		
		// htting Howie!
		if (other.gameObject.tag == "Player" && !beingThrown){
			if (!beingHeld){
				level.howie.GetComponent<HowieS>().TakeDamage(strength);
				CameraShakeS.C.SmallShake();
				CameraShakeS.C.TimeSleep(0.2f);
				
				// give howie some knockback
				
				level.howie.GetComponent<HowieS>().KnockBack(knockBackTime);
				
				Vector3 howieFixPos = level.howie.GetComponent<HowieS>().transform.position;
				howieFixPos.z = transform.position.z;
				
				level.howie.GetComponent<HowieS>().rigidbody.velocity = 
					(howieFixPos - transform.position).normalized*howieKnockBackMult*Time.deltaTime;
				
			}
		}
		
		
		if (other.gameObject.tag == "Wall" || other.gameObject.tag == "Enemy"){
			
			// if being thrown, get stunned! otherwise just bounce off
			if (beingThrown){ 
				if (other.gameObject.tag == "Enemy"){
					
					other.gameObject.GetComponent<EnemyS>().EnemyKnockback(rigidbody.velocity,0.4f,throwDamage);
				}
				
				EnemyKnockback(-rigidbody.velocity,0.4f,hitWallDamage);
				/*knockedOut = true;
				stunCountdown = stunMax;
				if (other.gameObject.tag == "Enemy"){
					other.gameObject.GetComponent<EnemyS>().knockedOut = true;
					other.gameObject.GetComponent<EnemyS>().stunCountdown = stunMax;
				}*/
			}
			else{
				// bouncing code should be adjusted in future to trigger depending on contact point
				if (attacking){
					attacking = false;
					attackDuration = 0;
					attackTimeCountdown = Random.Range(attackTimeCountdownMin, attackTimeCountdownMax);
				}
				
			}
			
			
			
		}
	}
	
}

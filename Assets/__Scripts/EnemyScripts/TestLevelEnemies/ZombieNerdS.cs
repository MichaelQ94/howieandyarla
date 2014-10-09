using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZombieNerdS: EnemyS {
	
	// inherits EnemyS variables 
	
	// Gargoyle placeholder enemy!
	// moves randomly and shoots out fireballs every once in a while

	public float changeSpeedTime = 2;
	public float changeSpeedTimeMin = 2;
	public float changeSpeedTimeMax = 3;

	public float randomizeMovTargetMult = 30;
	
	// min and max speed for accel randomizations
	public float minSpeed;
	public float maxSpeed;
	
	// current mov speeds, used to try and reach target speeds
	public float currentSpeed;
	public Vector3	moveVel;
	
	// allows for intermittent pauses in movement 
	// (because nothing moves indefinitely without looking weird)
	public float pauseTime;
	public float pauseTimeMin;
	public float pauseTimeMax;
	
	// true when enemy is moving/not paused
	public bool  enemyMoving = true;
	
	// times for when to move and not move
	public float moveTime;
	// min and max time for randomization
	public float moveTimeMin;
	public float moveTimeMax;
	
	// how long to not move or shoot after being thrown
	public float throwStunTimeMax = 1;
	public float throwStunTime;
	public bool stunned = false;
	
	public Texture	defaultTexture;

	public bool 	shootingFireball = false;
	public int 		frameToShootFireball = 7;

	// fireball to spawn
	public GameObject	fireBall;
	public float 		accuracyMult = 0.5f; // changes aim slightly and randomly
	public float		fireBallSpeed = 200; // how fast fireball moves
	
	public float 		fireBallTime; // when to shoot fireball
	// these min and max randomize when to shoot next
	public float		fireBallTimeMin;
	public float 		fireBallTimeMax;

	// following are textures for different animation states
	public List<Texture>	walkTextures;
	public int currentWalkTexture = 0; // frame to display
	public float walkFrameRate = 0.042f; // duration per individual frame
	public float walkFrameRateCountdown = 0;

	public List<Texture>	attackTextures;
	public int currentAttackTextures = 0; 
	public float attackFrameRateMax = 0.042f; 
	public float attackFrameRate = 0;

	public List<Texture>	deadTextures;
	public int currentDeadTexture = 0; 
	public float deadFrameRateMax = 0.042f; 
	public float deadFrameRate = 0;

	public List<Texture>	vulnTextures;
	public int currentVulnTexture = 0; 
	public float vulnFrameRateMax = 0.042f; 
	public float vulnFrameRate = 0;
	
	// Use this for initialization
	void Start () {
		
		EnemyStart();
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		UpdateEnemy(); // needs to be called in all enemy updates
		
		// if not stunned or being held, move and shoot!
		if (!beingHeld && !beingThrown && !isDead && !hitStunned && !vulnerable){
			BasicMovement();
			ShootFireball();
			
			// intermittent pausing
			StopMovement();
		}


		// allow for attack anim when shooting fireball
		if (!shootingFireball){
			SpriteManager();
		}

		// find a more elegant way to do this
		if (isDead){
			shootingFireball = false;
		}
		
	}

	void SpriteManager () {

		if (isDead){
			// animate death state
			if (currentDeadTexture < deadTextures.Count-1){
				deadFrameRate -= Time.deltaTime;
			}
			if (deadFrameRate <= 0){
				currentDeadTexture++;
				deadFrameRate = deadFrameRateMax;
			}
			renderer.material.SetTexture("_MainTex",deadTextures[currentDeadTexture]);
		}
		else if (hitStunned){
			if (vulnerable){
				sprite.renderer.material.SetTexture("_MainTex",hitStunTexture);
			}
			else{
				if (attackFrameRate > 0){
					attackFrameRate -= Time.deltaTime;
				}
				else{
					attackFrameRate = attackFrameRateMax;
					currentAttackTextures++;
					if (currentAttackTextures > attackTextures.Count-1){
						currentAttackTextures = 0;
					}
				}
				renderer.material.SetTexture("_MainTex", attackTextures[currentAttackTextures]);
			}
		}
		else if (vulnerable){
			// animate vulnerable state
			if (currentVulnTexture < vulnTextures.Count-1){
				vulnFrameRate -= Time.deltaTime;
			}
			if (vulnFrameRate <= 0){
				currentVulnTexture++;
				vulnFrameRate = vulnFrameRateMax;
			}
			renderer.material.SetTexture("_MainTex",vulnTextures[currentVulnTexture]);
		}
		else{
			if (walkFrameRateCountdown > 0){
				walkFrameRateCountdown -= Time.deltaTime;
			}
			else{
				walkFrameRateCountdown = walkFrameRate;
				currentWalkTexture++;
				if (currentWalkTexture > walkTextures.Count-1){
					currentWalkTexture = 0;
				}
			}
			renderer.material.SetTexture("_MainTex", walkTextures[currentWalkTexture]);
		}

	}
	
	// this moves the gargoyle around randomly
	void BasicMovement () {

		//face Howie
		if (transform.position.x < chompy.transform.position.x){
			renderer.material.SetTextureScale("_MainTex",new Vector2(1,-1));
		}
		else{
			renderer.material.SetTextureScale("_MainTex",new Vector2(-1,-1));
		}
		
		// only apply movement if not hitstunned
		if (hitStunned){
			
			hitStunTime -= Time.deltaTime;
			
			// reset once time is at zero
			if (hitStunTime <= 0){
				hitStunned = false;
			}
			
		}
		
		// if not hitstunned...
		else{
			// if not moving, count down pause time and set vel to zero
			if (!enemyMoving){
				
				if (pauseTime > 0){
					
					if (!rigidbody.isKinematic){ // if we're not being held by Yarla...
						rigidbody.velocity = Vector3.zero;
					}
					
					pauseTime -= Time.deltaTime;
					
					
				}
				else{

					moveTime = Random.Range(moveTimeMin,moveTimeMax);
					enemyMoving = true;
					
					/*
					// set time we should be moving and turn moving on
					if (stunCountdown <= 0 && throwStunTime <= 0){
						moveTime = Random.Range(moveTimeMin,moveTimeMax);
						enemyMoving = true;
					}*/
					
				}
			}
			else{
				
				ChangeVelocity(); // randomize movement

				if (!shootingFireball){
					rigidbody.velocity = moveVel*Time.deltaTime;
				}
				else{
					rigidbody.velocity = Vector3.zero;
				}
				
				if (moveTime > 0){
					
					moveTime -= Time.deltaTime; // count down to new pause time
					
				}
				else{
					
					// once movement time is up, set vel to zero and set new pause time
					pauseTime = Random.Range(pauseTimeMin,pauseTimeMax);
					enemyMoving = false;
					if (!rigidbody.isKinematic){
						rigidbody.velocity = Vector3.zero;
					}
					
				}
				
			}
		}
		
		
		
	}
	
	// change vel of gargoyle randomly while moving
	void ChangeVelocity () {
		
		if (enemyMoving){


			
			//print (enemyMoving);
			
			if (changeSpeedTime > 0){
				
				changeSpeedTime -= Time.deltaTime; // count down to when speed changes next
				
			}
			else{

				currentSpeed = Random.Range(minSpeed,maxSpeed);
				


				Vector3 movTarget = transform.position;

				movTarget.x = transform.position.x + randomizeMovTargetMult*Random.insideUnitCircle.x;
			movTarget.y = transform.position.y + randomizeMovTargetMult*Random.insideUnitCircle.y;

				moveVel = (movTarget - transform.position).normalized*currentSpeed;

				changeSpeedTime = Random.Range(changeSpeedTimeMin,changeSpeedTimeMax);
		}
		
	}
	}
	
	// when to pause movement
	void StopMovement(){
		
		// when being held, don't move
		if (beingHeld){
			throwStunTime = throwStunTimeMax;
			//if (!rigidbody.isKinematic){
			//	rigidbody.velocity = Vector3.zero;
			//}
			enemyMoving = false;
		}
		else{
			// when stunned, don't move and count down to unstun
			if (throwStunTime > 0){
				stunned = true;
				throwStunTime -= Time.deltaTime;
				enemyMoving = false;	
				//if (!rigidbody.isKinematic){
				//	rigidbody.velocity = Vector3.zero;
				//}
			}
			else{
				// when stun time reaches zero, turn off stun and start moving again
				if (stunned){
					enemyMoving = true;
					stunned = false;
				}
			}
		}
		
	}
	
	
	// for shooting fireballs at Howie
	void ShootFireball () {
		
		// count down to next shoot time
		if (fireBallTime > 0){

			attackFrameRate = attackFrameRateMax;
			currentAttackTextures = 0;
			shootingFireball = false;
			fireBallTime -= Time.deltaTime;
			
		}
		
		// once time is reached...
		else{

			shootingFireball = true;

			// animate, then halfway through spawn fireball
			attackFrameRate -= Time.deltaTime;
			if (attackFrameRate <= 0){
				currentAttackTextures++;
				if (currentAttackTextures >= attackTextures.Count-1){
					shootingFireball = false;
					
					fireBallTime = Random.Range(fireBallTimeMin, fireBallTimeMax);
				}
			}

			renderer.material.SetTexture("_MainTex",attackTextures[currentAttackTextures]);

			if (currentAttackTextures == frameToShootFireball){
	
				// have fireball spawn at gargoyle position and howie z position (to ensure hit)
				Vector3 projectilePos = transform.position;
					projectilePos.z = level.howie.GetComponent<HowieS>().transform.position.z;
				
				// instantiate that fireball
				GameObject fireBall1 = 
					Instantiate(fireBall, projectilePos, Quaternion.identity)
						as GameObject;
				fireBall1.tag = "Projectile";
				
				//offset look target to approx. less accuracy
				// first projectile veers off in the negative direction
				
				Vector3	target1 = level.howie.GetComponent<HowieS>().transform.position;
				target1.x -= Random.Range(0.5f,1)*accuracyMult;
				target1.y -= Random.Range(0.5f,1)*accuracyMult;
	
				
				// shoots out! (add vel)
				fireBall1.rigidbody.velocity = (target1-transform.position).normalized*
				                             fireBallSpeed*Time.deltaTime;
	
			}
			
			
		}
		
	}
	
	void OnCollisionEnter ( Collision other ){
		
		// these guys currently don't damage the player when they hit them!
		// that's ok too, unless we want to change that
		
		if (other.gameObject.tag == "Wall"){ 
			
			if (beingThrown){
				CameraShakeS.C.SmallShake();
				CameraShakeS.C.TimeSleep(0.02f);
				
				if (vulnerable){
					enemyHealth -= hitWallDamage;
				}
				else{
					shieldHealth -= hitWallDamage;
				}
				
				//knockedOut = true;
				//stunCountdown = stunMax;
			}
			else{
				moveVel*=-1;
			}
			
		}
		
		if (other.gameObject.tag == "Enemy") {
			
			if (beingThrown || other.gameObject.GetComponent<EnemyS>().beingThrown){
				CameraShakeS.C.SmallShake();
				CameraShakeS.C.TimeSleep(0.02f);
				
				
				other.gameObject.GetComponent<EnemyS>().EnemyKnockback(rigidbody.velocity,0.4f,throwDamage);
				
				
				EnemyKnockback(-rigidbody.velocity,0.4f,hitWallDamage);
				//knockedOut = true;
				//stunCountdown = stunMax;
			}
			
			
			else {
				moveVel *= -1;
			}
			
		}
		
	}
}

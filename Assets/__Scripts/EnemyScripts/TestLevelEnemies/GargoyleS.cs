using UnityEngine;
using System.Collections;

public class GargoyleS : EnemyS {

	// inherits EnemyS variables 

	// Gargoyle placeholder enemy!
	// moves randomly and shoots out fireballs every once in a while

	// what speed to move up to
	public float targetSpeedX;
	public float targetSpeedY;

	// min and max speed for accel randomizations
	public float minSpeed;
	public float maxSpeed;

	// current mov speeds, used to try and reach target speeds
	public float currentSpeedX;
	public float currentSpeedY;

	// how quicly speed changes
	public float accelRate;

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

	// when to change target speed
	public float changeSpeedTime;
	// randomize when target speed changes
	public float changeSpeedMin;
	public float changeSpeedMax;

	// how long to not move or shoot after being thrown
	public float throwStunTimeMax = 1;
	public float throwStunTime;
	public bool stunned = false;

	// fireball to spawn
	public GameObject	fireBall;
	public float 		accuracyMult = 0.5f; // changes aim slightly and randomly
	public float		fireBallSpeed = 200; // how fast fireball moves

	public float 		fireBallTime; // when to shoot fireball
	// these min and max randomize when to shoot next
	public float		fireBallTimeMin;
	public float 		fireBallTimeMax;

	// Use this for initialization
	void Start () {
	
		EnemyStart();

	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		UpdateEnemy(); // needs to be called in all enemy updates

		// if not stunned or being held, move and shoot!
		if (!beingHeld && !beingThrown && !isDead){
			BasicMovement();
			ShootFireball();

			// intermittent pausing
			StopMovement();
		}



	}

	// this moves the gargoyle around randomly
	void BasicMovement () {

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

				// randomize target x and z speeds
			targetSpeedX = Random.Range(minSpeed,maxSpeed);
			
				// 1/2 change for neg speed
			float chanceForNegX = Random.Range(0,100);
			//print(chanceForNegX);
			if (chanceForNegX > 50){
				targetSpeedX *= -1;
			}

			targetSpeedY = Random.Range(minSpeed,maxSpeed);

			float chanceForNegY = Random.Range(0,100);
			//print(chanceForNegY);
			if (chanceForNegY > 50){
				targetSpeedY *= -1;
			}

				// set time for when to change speed next
			changeSpeedTime = Random.Range(changeSpeedMin,changeSpeedMax);
		}

			// accel based on current speed and relation to target speed
		if (currentSpeedX < targetSpeedX - 1){

			currentSpeedX += accelRate*Time.deltaTime;

		}
		if (currentSpeedX > targetSpeedX + 1){
			currentSpeedX -= accelRate*Time.deltaTime;
		}

		if (currentSpeedY < targetSpeedY - 1){
			
			currentSpeedY += accelRate*Time.deltaTime;
			
		}
		if (currentSpeedY > targetSpeedY + 1){
			currentSpeedY -= accelRate*Time.deltaTime;
		}

			// grab current move speed and set currentSpeedX and Z
		Vector3 movementVel = rigidbody.velocity;
		movementVel.x = currentSpeedX;
		movementVel.y = currentSpeedY;

			// set velocity back into gargoyle to actually change vel
		rigidbody.velocity = movementVel*Time.deltaTime;
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

			fireBallTime -= Time.deltaTime;

		}

		// once time is reached...
		else{

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

			fireBall1.transform.LookAt(target1); // fireball aims at howie

			// shoots out! (add vel)
			fireBall1.rigidbody.AddForce(fireBall1.transform.forward*
			                                 fireBallSpeed*Time.deltaTime);

			// fireball number 2
			
			GameObject fireBall2 = 
				Instantiate(fireBall, projectilePos, Quaternion.identity)
					as GameObject;

			// middle shot aims right at Howie
			Vector3	target2 = level.howie.GetComponent<HowieS>().transform.position;

			fireBall2.transform.LookAt(target2);
			fireBall2.rigidbody.AddForce(fireBall2.transform.forward*
			                             fireBallSpeed*Time.deltaTime);

			// fireball number three
			GameObject fireBall3 = 
				Instantiate(fireBall, projectilePos, Quaternion.identity)
					as GameObject;

			// veers off positive direction

			Vector3	target3 = level.howie.GetComponent<HowieS>().transform.position;
			target3.x += Random.Range(0.5f,1)*accuracyMult;
			target3.y += Random.Range(0.5f,1)*accuracyMult;
			
			fireBall3.transform.LookAt(target3);

			//fireBall3.transform.rotation.Set(0,0,fireBall3.transform.rotation.z,0); 
			fireBall3.rigidbody.AddForce(fireBall3.transform.forward*
			                             fireBallSpeed*Time.deltaTime);

			//print (target1);
			//print (target2);
			//print (target3);

			// reset shoot time
			fireBallTime = Random.Range(fireBallTimeMin, fireBallTimeMax);


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

			if (!rigidbody.isKinematic){
				rigidbody.velocity *= -1;
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

			
			if (!rigidbody.isKinematic){
				rigidbody.velocity *= -1;
			}
			
		}
		
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrawlerS : EnemyS {

	public float 	walkSpeed = 150; // normal speed
	public float 	attackSpeed = 300; // attack speed

	public bool 	howieNearby = false; // if Howie not nearby, act idle
	public bool 	scared = false; // to be turned on when alone and Howie near


	public Vector3	movTarget;
	public float 	changeMovTargetCountdown = 0;
	public float 	changeMovTargetMinTime = 2;
	public float 	changeMovTargetMaxTime = 4;

	public float 	randomizeMovTargetMult = 4;

	public float 	triggerAttackCountdown = 2;
	public float 	triggerAttackCountdownMin = 2;
	public float 	triggerAttackCountdownMax = 3;

	// for now just changing mats to indicate vulnerability, should be textures in future
	public Material	invulnMat;
	public Material	vulnMat;

	public float 	attackDuration = 2;
	public float 	attackDurationMax = 2;
	public bool		attacking = false;

	public float 	attackReverseTimeMax = 1.5f; // have enemy move back a bit after hitting Howie
	public float 	attackReverseCountdown;

	public float 	knockBackTime = 0.2f; // variables for Howie knockback when hitting him
	public float 	howieKnockBackMult = 2;

	// following are textures for different animation states
	public List<Texture>	walkTextures;
	public int currentWalkTexture = 0; // frame to display
	public float walkFrameRate = 0.042f; // duration per individual frame
	public float walkFrameRateCountdown = 0;

	public List<Texture>	vulnerableTextures;
	public int 		currentVulnerableTexture = 0;
	public float vulnerableFrameRate = 0.042f; // duration per individual frame
	public float vulnerableFrameRateCountdown = 0;

	public List<Texture>	attackingTextures;
	public int 		currentAttackingTexture = 0;
	public float attackingFrameRate = 0.042f; // duration per individual frame
	public float attackingFrameRateCountdown = 0;

	private bool facingRight = false;


	// script for Creepy Crawler enemies

	// Use this for initialization
	void Start () {

		EnemyStart();
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		//print (rigidbody.velocity);
	
		UpdateEnemy();

		if (!isDead){
			if (!beingHeld && !beingThrown && !knockedOut && !hitStunned){
				if (!scared){
					TriggerAttacks();
				}
				MoveAround();
			}
			ManageVulnerability();
		}
		
		ManageSprite();

	}

	void TriggerAttacks () {

		if (!attacking){
			if (triggerAttackCountdown > 0){
				if (howieNearby){
				triggerAttackCountdown -= Time.deltaTime;
				}	
			}
			else{
				attackDuration = attackDurationMax;
				attacking = true;
			}
		}

	}

	void MoveAround () {

		// if attacking, head straight for Howie!
		if (attacking){
			movTarget = HowieS.H.transform.position;

			// count down attack time
			attackDuration -= Time.deltaTime;
			if (attackDuration <= 0){
				attacking = false;

				// calculate time until next attack
				triggerAttackCountdown = Random.Range(triggerAttackCountdownMin,triggerAttackCountdownMax);
			}
		}
		else{

			if (changeMovTargetCountdown > 0){
				changeMovTargetCountdown -= Time.deltaTime;


			}
			else{

				movTarget = transform.position;

				// if scared, run away from howie!
				if (scared){
					// change x of target
					if (transform.position.x > HowieS.H.transform.position.x){
						movTarget.x = transform.position.x+randomizeMovTargetMult*Random.insideUnitCircle.x;
					}
					else{
						movTarget.x = transform.position.x-randomizeMovTargetMult*Random.insideUnitCircle.x;
					}
					// change y of target
					if (transform.position.y > HowieS.H.transform.position.y){
						movTarget.y = transform.position.y+randomizeMovTargetMult*Random.insideUnitCircle.y;
					}
					else{
						movTarget.y = transform.position.y-randomizeMovTargetMult*Random.insideUnitCircle.x;
					}
				}

				// if not scared...
				else{
					if (howieNearby){
						// if howie is around, head towards him
						movTarget.x = HowieS.H.transform.position.x + randomizeMovTargetMult*Random.insideUnitCircle.x;
						movTarget.y = HowieS.H.transform.position.y + randomizeMovTargetMult*Random.insideUnitCircle.y;
					}
					else{
						// if not scared and howie's not around, move idly and randomly!
						movTarget.x = transform.position.x + randomizeMovTargetMult*Random.insideUnitCircle.x;
						movTarget.y = transform.position.y + randomizeMovTargetMult*Random.insideUnitCircle.y;
					}
				}
				
				// when countdown ends, change movtarget to randomize movement
				changeMovTargetCountdown = Random.Range(changeMovTargetMinTime, changeMovTargetMaxTime);
				//print ("change move target!");
			}

			
			movTarget.z = transform.position.z;
			//print (movTarget);


		}

		// this is the part of the code that actually sets the velocity towards the moveTarget
		
		// change speed depending on state of hostility
		if (invulnerable){
		if (attacking || scared){
			rigidbody.velocity = (movTarget - transform.position).normalized*attackSpeed*Time.deltaTime;
				//print(movTarget - transform.position);
		}
		else{
			
			rigidbody.velocity = (movTarget - transform.position).normalized*walkSpeed*Time.deltaTime;

				//print (movTarget - transform.position);
		}
		}

		// if in reverse time mode, count down that time
		if (attackReverseCountdown > 0){
			attackReverseCountdown-=Time.deltaTime;
		}

	}

	void ManageVulnerability () {

		//placing scared stuff in here too
		float distanceToHowie = Vector3.Distance(new Vector3(HowieS.H.transform.position.x,
		                                                     HowieS.H.transform.position.y,
		                                                     transform.position.z), transform.position);
		if (distanceToHowie < 30){
			howieNearby = true;
		}
		else{
			howieNearby = false;
		}

		// took out crawler detection so following code is out of date
		/*
		if (howieNearby){
			if (activeRad.otherCrawlers.Count > 0){
				scared = false;
			}
			else{
				scared = true;
			}
		}
		else{
			scared = false;
		}*/

		if (knockedOut || beingHeld){
			invulnerable = false;
			if (!beingHeld){
				renderer.material = vulnMat;
			}
		}
		else{
			invulnerable = true;
			if (!beingHeld){
				renderer.material = invulnMat;
			}
		}

	}

	void ManageSprite () {

		if (isDead){
			renderer.material.SetTexture("_MainTex", deadTexture);
		}
		else{

			// have crawler face left/right appropriately
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

			if (invulnerable){
			if (attacking){
				if (attackingFrameRateCountdown > 0){
					attackingFrameRateCountdown -= Time.deltaTime;
				}
				else{
					attackingFrameRateCountdown = attackingFrameRate;
					currentAttackingTexture++;
					if (currentAttackingTexture > attackingTextures.Count-1){
						currentAttackingTexture = 0;
					}
				}
				renderer.material.SetTexture("_MainTex", attackingTextures[currentAttackingTexture]);
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
				else{
					if (vulnerableFrameRateCountdown > 0){
						vulnerableFrameRateCountdown -= Time.deltaTime;
					}
					else{
						vulnerableFrameRateCountdown = vulnerableFrameRate;
						currentVulnerableTexture++;
						if (currentVulnerableTexture > vulnerableTextures.Count-1){
							currentVulnerableTexture = 0;
						}
					}
					renderer.material.SetTexture("_MainTex", vulnerableTextures[currentVulnerableTexture]);
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

			// set time for reversing vel and reverse vel

			rigidbody.velocity*=-1;
			attackReverseCountdown = attackReverseTimeMax;
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
				changeMovTargetCountdown = 0;
			}
			
		}

		// getting hit by Yarla -- trigger stun to make vulnerable
		if (other.gameObject.tag == "Yarla"){
			stunCountdown = stunMax;
			knockedOut = true;
		}

	}
	
}

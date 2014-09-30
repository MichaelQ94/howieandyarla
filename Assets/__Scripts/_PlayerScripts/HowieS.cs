using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HowieS : MonoBehaviour {
	
	public Vector3	charVel;
	public float 	maxSpeed = 5;
	
	public float		maxHandRadius = 5;
	public float 		maxHandSpeed = 100;
	public Vector3		handVel;
	public Vector3		handPos;
	public GameObject	hand;
	
	public Material	defaultMat;
	public Material	defaultMatAlt;
	
	public float	animCycleTime = 0.8f;
	public float	animCycleCount = 0;
	
	public float	chargeSpeedMultiplier = 0.6f;
	
	public bool			knockedBack = false;
	public float		kickBackMax = 0.2f;
	public float		kickBackCountdown = 0.2f;
	
	//public float 		mouseSensitivity = 2.5f;
	
	public GameObject	attachedSprite; // what to animate
	public List<Texture>	howieWalkCycle; // walking frames
	public List<Texture>	howieSoloWalkCycle; // just howie walking frames

	public GameObject	yarlaShadow;
	public GameObject	howieShadow;
	
	public int 			currentWalkSprite = 0; // what frame is being displayed
	public float 		walkCycleRate = 0.2f; // how often to change frame
	public float 		walkCycleHowieSoloRate = 0.042f; // how often to change frame as solo howie
	public float 		walkCycleRateCountdown; // keeps track of frame changes
	
	public bool 		isHowieSolo = false; // true when in Howie solo mode
	public float 		howieSoloSpeedMult = 2; // mult walk speed when Howie solo
	
	public bool 		metaActive = false; // THIS IS TOTALLY TEMP JUST TO TEST FIRE STUFF
	public List<MetaGeneralS>	equippedMetas;
	public List<Texture>		fireHowieAndYarlaTexts; // seriously I mean it
	
	public bool 		facingRight = true; // facing left or right?
	
	public static float maxHealth = 100; // max health for Howie
	public float overchargeHealth = 200; // overcharge health for howie
	public static float health = 100; // Howie's health
	public string	gameOverScene; // name of game over scene

	// these vars concern dynamic health system
	public float healthRecoverRate = 20;
	public float overChargeDiminishRate = 10;
	public float diminishRateHowieMult = 0.5f;

	public float lastTimeHit;
	public float timeToTriggerRecover = 3;

	public bool inCombat = true;

	public int blueEnergyAmt = 0;
	public int redEnergyAmt = 0;
	public int purpleEnergyAmt = 0;

	public YarlaS	yarla;

	public HowieCtrl	howieCtrl;

	
	// Use this for initialization
	void Start () {

		kickBackCountdown = kickBackMax;
		walkCycleRateCountdown = walkCycleRate;
		yarla = GameObject.FindGameObjectsWithTag ("YarlaS") [0].GetComponent<YarlaS> ();

		howieCtrl = new HowieCtrl (this);
		
	}
	
	void Update () {
		
		// howie switch needs to be in update to correctly read button down inputs
		if (!metaActive && inCombat){
			CheckHowieSwitch();
		}
		if (!isHowieSolo){
			ActivateMeta();
		}
		
	}
	
	void FixedUpdate () {
		
		//IdleAnim ();
		Walk ();
		MoveHand ();
		Animate();
		UpdateHealth();
		
		
		
	}

	void ActivateMeta () {

		howieCtrl.ActivateMeta();

	}


	
	void Animate () {
		
		if (rigidbody.velocity.x > 0){
			facingRight = true;
		}
		if (rigidbody.velocity.x < 0){
			facingRight = false;
		}

		// flip texture to face correct direction
		if (facingRight){
			attachedSprite.renderer.material.SetTextureScale("_MainTex",new Vector2(1,1));
		}
		else{
			attachedSprite.renderer.material.SetTextureScale("_MainTex",new Vector2(-1,1));
		}
		
		
		if (Mathf.Abs(rigidbody.velocity.x) > 0
		    || Mathf.Abs(rigidbody.velocity.y) > 0){
			
			walkCycleRateCountdown -= Time.deltaTime;
			
			if (walkCycleRateCountdown <= 0){
				currentWalkSprite++;
				if (!isHowieSolo){
					if (metaActive){
						// change this meta-based texture swapping ASAP!
						if (currentWalkSprite > fireHowieAndYarlaTexts.Count-1){
							currentWalkSprite = 0;
						}
					}
					else{
						if (currentWalkSprite > howieWalkCycle.Count-1){
							currentWalkSprite = 0;
						}
					}
				}
				else{
					if (currentWalkSprite > howieSoloWalkCycle.Count-1){
						currentWalkSprite = 0;
					}
				}
				if (!isHowieSolo){
					walkCycleRateCountdown = walkCycleRate;
				}
				else{
					walkCycleRateCountdown = walkCycleHowieSoloRate;
				}
			}
		}
		else{
			currentWalkSprite = 0;
		}
		
		if (!isHowieSolo){
			// change this whole meta-based texture swapping ASAP!!
			if (metaActive){
				attachedSprite.renderer.material.SetTexture("_MainTex",fireHowieAndYarlaTexts[currentWalkSprite]);
			}
			else{
				attachedSprite.renderer.material.SetTexture("_MainTex",howieWalkCycle[currentWalkSprite]);
			}
			attachedSprite.transform.localScale = new Vector3(1,1,1);
			yarlaShadow.renderer.enabled = true;
			howieShadow.renderer.enabled = false;
		}
		else{
			attachedSprite.renderer.material.SetTexture("_MainTex",howieSoloWalkCycle[currentWalkSprite]);
			attachedSprite.transform.localScale = new Vector3(0.75f,0.75f,1);
			yarlaShadow.renderer.enabled = false;
			howieShadow.renderer.enabled = true;
		}
		
	}
	
	void Walk () {
		
		howieCtrl.Walk();
		
	}
	
	void MoveHand () {
		
		howieCtrl.MoveHand();
		
	}
	
	public void KnockBack(float timeK){
		
		kickBackMax = timeK;
		kickBackCountdown = timeK;
		knockedBack = true;
		
	}
	
	void IdleAnim(){
		
		if (Mathf.Abs(rigidbody.velocity.x) < 1 &&
		    Mathf.Abs(rigidbody.velocity.y) < 1){
			animCycleCount += Time.deltaTime;
		}
		if (animCycleCount >= animCycleTime){
			animCycleCount = 0;
		}
		if (animCycleCount < animCycleTime/2){
			renderer.material = defaultMat;
		}
		else{
			renderer.material = defaultMatAlt;
		}
		
	}
	
	public void TakeDamage (float damageAmount){
		
		// method for other things to call when we want Howie to get damaged
		// subtracts from health equal to amt in argument
		
		// we can also use this to set a consistent screen shake & time sleep if we so desire
		
		health -= damageAmount;

		// set lastTimeHit to current time
		lastTimeHit = Time.time;
		
		// I'm also going to include a death state trigger here
		//though we may find we want to change how this works later
		
		if (health <= 0){
			GameOver();
		}
		
		
	}
	
	public void CheckHowieSwitch () {
		
		howieCtrl.CheckHowieSwitch();
		
	}

	public void GainAbsorbStats (float recoverHealth, int energyType, int EnergyAmt) {

		health += recoverHealth;
		//print ("ABSORB");

		if (energyType == 0){
			blueEnergyAmt += EnergyAmt;
		}
		if (energyType == 1){
			redEnergyAmt += EnergyAmt;
		}
		if (energyType == 2){
			purpleEnergyAmt += EnergyAmt;
		}

	}

	void DisableCombat () {

		if (!inCombat){
			isHowieSolo = true;
		}

	}

	void UpdateHealth () {

		// if in combat, utilize health system
		if (inCombat){

			// if health is greater than max, slowly decrease
			if (health > maxHealth+1){

				// if over overcharge health, fix
				if (health > overchargeHealth){
					health = overchargeHealth;
				}

				// diminish overcharge based on Howie status
				if (isHowieSolo){
					health -= overChargeDiminishRate*diminishRateHowieMult*Time.deltaTime;
				}
				else{
					health -= overChargeDiminishRate*Time.deltaTime;
				}

			}

			else if (health < maxHealth - 1){

				// start recovering health on certain timer after x time not getting hit
				if ((Time.time - lastTimeHit) > timeToTriggerRecover){
					health += healthRecoverRate*Time.deltaTime;
				}

			}
			else{
				health = maxHealth;
			}

		}
		// if not in combat, recover health to max
		else{

			if (health < maxHealth){
				health += healthRecoverRate*Time.deltaTime;
			}
			else{
				health = maxHealth;
			}

		}

	}
	
	public void GameOver (){
		
		// loads game over screen when called
		Application.LoadLevel(gameOverScene);
		
	}
	
	
}

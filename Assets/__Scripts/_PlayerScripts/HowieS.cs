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

	public float 		defaultYarlaPosX;
	public float 		defaultYarlaPosY;
	
	//public float 		mouseSensitivity = 2.5f;
	
	public GameObject	attachedSprite; // what to animate
	public List<Texture>	howieWalkCycle; // walking frames
	public List<Texture>	howieSoloWalkCycle; // just howie walking frames

	public List<Texture>	howieToYarlaFrames;
	public int 				howieToYarlaCurrentFrame;
	public float 			howieToYarlaFrameRate;
	public float 			howieToYarlaFrameRateMax = 0.012f;
	public bool				isTransforming = false;

	public GameObject	yarlaShadow;
	public GameObject	howieShadow;

	public GameObject	hitEffect;
	
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
	
	public static float maxHealth = 50; // max health for Howie
	public float overchargeHealth = 100; // overcharge health for howie
	public static float health = 50; // Howie's health
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
	public string platformType;
	public HowieCtrl	howieCtrl;

	// for moving yarla back on rest
	//public float 		restXPos = 3;
	//public float 		restYPos = 3;
	//public float 		resetPosSpeed = 1;

	public bool		presentationMode = false;
	public bool 	dontDie = false;

	// Use this for initialization
	void Start () {

		kickBackCountdown = kickBackMax;
		walkCycleRateCountdown = walkCycleRate;
		yarla = GameObject.FindGameObjectsWithTag ("YarlaS") [0].GetComponent<YarlaS> ();


		howieCtrl = new HowieCtrl (this);
		

		platformType = Events.Environment.getPlatform();

	}
	
	void Update () {
		
		// howie switch needs to be in update to correctly read button down inputs
		if (!metaActive){
			CheckHowieSwitch();
		}
		if (!isHowieSolo){
			ActivateMeta();
		}

		if (presentationMode){
			if (Input.GetKeyDown(KeyCode.I)){
				dontDie = !dontDie;
			}
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

		string[] checkInputs = Input.GetJoystickNames();
		int numInputs = checkInputs.Length;

		// check to make sure player has metas equipped
		if (equippedMetas.Count > 0 && !metaActive){

			if (numInputs > 0){

				// check for platform
				//same but for pc
					if (Input.GetButtonDown("Meta1" + platformType)){

					//print ("AHHH");

						// make sure player has enough energy,
						// if so subtract and activate
						if (blueEnergyAmt >= equippedMetas[0].blueEnergyReq &&
						    redEnergyAmt >= equippedMetas[0].redEnergyReq &&
						    purpleEnergyAmt >= equippedMetas[0].purpleEnergyReq){
							
							blueEnergyAmt -= equippedMetas[0].blueEnergyReq;
							redEnergyAmt -= equippedMetas[0].redEnergyReq;
							purpleEnergyAmt -= equippedMetas[0].purpleEnergyReq;
							
							equippedMetas[0].Activate ();
							metaActive = true;
							currentWalkSprite = 0;
							
						}
						
					}

				}

			else{
				
				// check for activate meta 1 button
				if (Input.GetKeyDown(KeyCode.Alpha1)){
					// make sure player has enough energy,
					// if so subtract and activate
					if (blueEnergyAmt >= equippedMetas[0].blueEnergyReq &&
					    redEnergyAmt >= equippedMetas[0].redEnergyReq &&
					    purpleEnergyAmt >= equippedMetas[0].purpleEnergyReq){
						
						blueEnergyAmt -= equippedMetas[0].blueEnergyReq;
						redEnergyAmt -= equippedMetas[0].redEnergyReq;
						purpleEnergyAmt -= equippedMetas[0].purpleEnergyReq;
						
						equippedMetas[0].Activate ();
						metaActive = true;
						currentWalkSprite = 0;
						
					}
				}
				
			}
			}

			

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

		// animate tendril deploy/retract
		if (isTransforming){

			yarlaShadow.renderer.enabled = false;
			howieShadow.renderer.enabled = true;
			attachedSprite.transform.localScale = new Vector3(1,1,1);

			howieToYarlaFrameRate -= Time.deltaTime;

			if (howieToYarlaFrameRate <= 0){
					
				if (isHowieSolo){
					howieToYarlaCurrentFrame --;
					if (howieToYarlaCurrentFrame <= 0){
						isTransforming = false;
						isHowieSolo = !isHowieSolo;
					}
				}
				else{
					howieToYarlaCurrentFrame ++;
					if (howieToYarlaCurrentFrame >= howieToYarlaFrames.Count-1){
						isTransforming = false;
						isHowieSolo = !isHowieSolo;
					}
				}
				howieToYarlaFrameRate = howieToYarlaFrameRateMax;
			}
			//print ("WOO");
			//print (howieToYarlaCurrentFrame);
			attachedSprite.renderer.material.SetTexture("_MainTex", howieToYarlaFrames[howieToYarlaCurrentFrame]);


		}
		else{
			if (Mathf.Abs(rigidbody.velocity.x) > 0
			    || Mathf.Abs(rigidbody.velocity.y) > 0 || isHowieSolo){
				
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
		
	}
	
	void Walk () {
		
		if (knockedBack){
			
			kickBackCountdown -= Time.deltaTime;
			
			if (kickBackCountdown <= 0){
				knockedBack = false;
			}
			
		}
		else{
			
			charVel = rigidbody.velocity;
			charVel.x = Input.GetAxis("Horizontal" + platformType)*maxSpeed*Time.deltaTime;
			charVel.y = Input.GetAxis("Vertical" + platformType)*maxSpeed*Time.deltaTime;
			
			if (yarla.holding){
				charVel *= chargeSpeedMultiplier;
			}
			
			// speed up if Howie solo!
			if (isHowieSolo){
				charVel *= howieSoloSpeedMult;
			}
			
			// set speed back into rigidbody if not transforming
			if (isTransforming){
				rigidbody.velocity = Vector3.zero;
			}
			else{
				rigidbody.velocity = charVel;
			}
		}
		
	}
	
	void MoveHand () {
		
		string[] checkInputs = Input.GetJoystickNames();
		
		int inputNumber = checkInputs.Length;
		if (inputNumber <= 0 && Screen.showCursor){
			//Screen.showCursor = false;
		}
		//print (inputNumber);
		
		
		Vector3 mousePos = CameraShakeS.C.camera.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = transform.position.z;

		
		float mouseDistance = Vector3.Distance(transform.position,mousePos);
		
		
		if (!yarla.launched){
			if (!yarla.holding){
				if (inputNumber > 0){
					handPos.x = Input.GetAxis("SecondHorizontal" + platformType)*maxHandRadius;
					handPos.y = Input.GetAxis("SecondVertical" + platformType)*maxHandRadius;
				}
				else{

					//print (mouseDistance);
					//print (maxHandRadius/hand.transform.localScale.x);

					if (mouseDistance < (maxHandRadius/hand.transform.localScale.x)){
						handPos = mousePos - transform.position;
						handPos.x *= hand.transform.localScale.x;
						handPos.y *= hand.transform.localScale.y;
					}
					else{
						handPos = mousePos - transform.position;
						handPos.Normalize();
						handPos *= maxHandRadius;
					}
				}
			}
			else{
				if (inputNumber > 0){
					handPos.x = Input.GetAxis("SecondHorizontal" + platformType)*maxHandRadius/2;
					handPos.y = Input.GetAxis("SecondVertical" + platformType)*maxHandRadius/2;	
				}
				else{
					if (mouseDistance < (maxHandRadius/hand.transform.localScale.x)){
						handPos = mousePos - transform.position;
						handPos.x *= hand.transform.localScale.x;
						handPos.y *= hand.transform.localScale.y;
					}
					else{
						handPos = mousePos - transform.position;
						handPos.Normalize();
						handPos *= maxHandRadius/2;
					}
				}
			}

			handPos.z = yarla.yarlaZ;
			
			//print(handPos);
			
			// only set handpos if not solo howie, else make it zero!
			//if (!isHowieSolo){

				hand.transform.localPosition = handPos;
			//}
		//	else{
		//		handPos.x = 0;
		//		handPos.z = 0;
			//	hand.transform.localPosition = handPos;
			//}
			
			
			//print (maxHandRadius*Mathf.Cos(mouseAngle));
			
		}
		
		//float handDistance = Vector3.Distance(hand.transform.position, transform.position);
		
		//float speedDiff = handDistance/maxHandRadius;
		
		//handVel = hand.rigidbody.velocity;
		
		//handVel.x = Input.GetAxis("SecondHorizontal")*maxHandSpeed*Time.deltaTime;
		//handVel.y = Input.GetAxis("SecondVertical")*maxHandSpeed*Time.deltaTime;
		
		//hand.rigidbody.velocity = handVel;
		
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

		/*Vector3 bloodOffset = transform.position;
		bloodOffset.x += Random.insideUnitCircle.x;
		bloodOffset.y += Random.insideUnitCircle.y;

		Instantiate(hitEffect, bloodOffset, Quaternion.Euler(new Vector3(0,0,Random.Range(0,359))));*/

		// set lastTimeHit to current time
		lastTimeHit = Time.time;
		
		// I'm also going to include a death state trigger here
		//though we may find we want to change how this works later
		
		if (health <= 0){
			if (!dontDie){
				GameOver();
			}
			else{
				health = 1;
			}
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

	public void ActivateMetamorphosis()
	{
		//Activate proper metamorphosis based on the selection made by the user.
	}
	
	public void GameOver (){

		Events.Environment.reloadScene = Application.loadedLevelName;
		// loads game over screen when called
		Application.LoadLevel(gameOverScene);
		
	}
	
	
}

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

	
	// Use this for initialization
	void Start () {

		kickBackCountdown = kickBackMax;
		walkCycleRateCountdown = walkCycleRate;
		yarla = GameObject.FindGameObjectsWithTag ("YarlaS") [0].GetComponent<YarlaS> ();
		
	}
	
	void Update () {
		
		// howie switch needs to be in update to correctly read button down inputs
		if (!metaActive){
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

		string[] checkInputs = Input.GetJoystickNames();
		int numInputs = checkInputs.Length;

		// check to make sure player has metas equipped
		if (equippedMetas.Count > 0 && !metaActive){

			if (numInputs > 0){

				// check for platform
				if (Application.platform == RuntimePlatform.OSXEditor || 
				    Application.platform == RuntimePlatform.OSXPlayer ||
				    Application.platform == RuntimePlatform.OSXWebPlayer || 
				    Application.platform == RuntimePlatform.OSXDashboardPlayer){

					if (Input.GetButtonDown("Meta1Mac")){

						// make sure player has enough energy,
						// if so subtract and activate
						if (blueEnergyAmt > equippedMetas[0].blueEnergyReq &&
						    redEnergyAmt > equippedMetas[0].redEnergyReq &&
						    purpleEnergyAmt > equippedMetas[0].purpleEnergyReq){

							blueEnergyAmt -= equippedMetas[0].blueEnergyReq;
							redEnergyAmt -= equippedMetas[0].redEnergyReq;
							purpleEnergyAmt -= equippedMetas[0].purpleEnergyReq;

							equippedMetas[0].Activate ();
							metaActive = true;
							currentWalkSprite = 0;

						}
					}

				}

				//same but for pc
				if (Application.platform == RuntimePlatform.WindowsEditor || 
				    Application.platform == RuntimePlatform.WindowsPlayer ||
				    Application.platform == RuntimePlatform.WindowsWebPlayer){

					if (Input.GetButtonDown("Meta1PC")){

						// make sure player has enough energy,
						// if so subtract and activate
						if (blueEnergyAmt > equippedMetas[0].blueEnergyReq &&
						    redEnergyAmt > equippedMetas[0].redEnergyReq &&
						    purpleEnergyAmt > equippedMetas[0].purpleEnergyReq){
							
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

			else{

				// check for activate meta 1 button
				if (Input.GetKeyDown(KeyCode.Alpha1)){
					// make sure player has enough energy,
					// if so subtract and activate
					if (blueEnergyAmt > equippedMetas[0].blueEnergyReq &&
					    redEnergyAmt > equippedMetas[0].redEnergyReq &&
					    purpleEnergyAmt > equippedMetas[0].purpleEnergyReq){
						
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
		
		if (knockedBack){
			
			kickBackCountdown -= Time.deltaTime;
			
			if (kickBackCountdown <= 0){
				knockedBack = false;
			}
			
		}
		else{
			
			charVel = rigidbody.velocity;
			
			if (Application.platform == RuntimePlatform.OSXEditor || 
			    Application.platform == RuntimePlatform.OSXPlayer ||
			    Application.platform == RuntimePlatform.OSXWebPlayer || 
			    Application.platform == RuntimePlatform.OSXDashboardPlayer){
				charVel.x = Input.GetAxis("HorizontalMac")*maxSpeed*Time.deltaTime;
				charVel.y = Input.GetAxis("VerticalMac")*maxSpeed*Time.deltaTime;
			}
			
			if (Application.platform == RuntimePlatform.WindowsEditor || 
			    Application.platform == RuntimePlatform.WindowsPlayer ||
			    Application.platform == RuntimePlatform.WindowsWebPlayer){
				charVel.x = Input.GetAxis("HorizontalPC")*maxSpeed*Time.deltaTime;
				charVel.y = Input.GetAxis("VerticalPC")*maxSpeed*Time.deltaTime;
			}
			
			if (yarla.holding){
				charVel *= chargeSpeedMultiplier;
			}
			
			// speed up if Howie solo!
			if (isHowieSolo){
				charVel *= howieSoloSpeedMult;
			}
			
			// set speed back into rigidbody
			rigidbody.velocity = charVel;
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
					if (Application.platform == RuntimePlatform.OSXEditor || 
					    Application.platform == RuntimePlatform.OSXPlayer ||
					    Application.platform == RuntimePlatform.OSXWebPlayer || 
					    Application.platform == RuntimePlatform.OSXDashboardPlayer){
						handPos.x = Input.GetAxis("SecondHorizontalMac")*maxHandRadius;
						handPos.y = Input.GetAxis("SecondVerticalMac")*maxHandRadius;
					}
					if (Application.platform == RuntimePlatform.WindowsEditor || 
					    Application.platform == RuntimePlatform.WindowsPlayer ||
					    Application.platform == RuntimePlatform.WindowsWebPlayer){
						handPos.x = Input.GetAxis("SecondHorizontalPC")*maxHandRadius;
						handPos.y = Input.GetAxis("SecondVerticalPC")*maxHandRadius;
					}
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
					if (Application.platform == RuntimePlatform.OSXEditor || 
					    Application.platform == RuntimePlatform.OSXPlayer ||
					    Application.platform == RuntimePlatform.OSXWebPlayer || 
					    Application.platform == RuntimePlatform.OSXDashboardPlayer){
						handPos.x = Input.GetAxis("SecondHorizontalMac")*maxHandRadius/2;
						handPos.y = Input.GetAxis("SecondVerticalMac")*maxHandRadius/2;	
					}
					if (Application.platform == RuntimePlatform.WindowsEditor || 
					    Application.platform == RuntimePlatform.WindowsPlayer ||
					    Application.platform == RuntimePlatform.WindowsWebPlayer){
						handPos.x = Input.GetAxis("SecondHorizontalPC")*maxHandRadius/2;
						handPos.y = Input.GetAxis("SecondVerticalPC")*maxHandRadius/2;	
					}
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
			
			//print(handPos);
			
			// only set handpos if not solo howie, else make it zero!
			if (!isHowieSolo){
				hand.transform.localPosition = handPos;
			}
			else{
				hand.transform.localPosition = Vector3.zero;
			}
			
			
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

		// set lastTimeHit to current time
		lastTimeHit = Time.time;
		
		// I'm also going to include a death state trigger here
		//though we may find we want to change how this works later
		
		if (health <= 0){
			GameOver();
		}
		
		
	}
	
	public void CheckHowieSwitch () {
		
		// switch between Howie solo and H&Y at button press
		
		// for simplicity's sake, currently you can NOT switch while holding an enemy
		
		if (!yarla.holding){
			
			// check for platform and switch at button press (A on controller, shift on key)
			
			// check if using controller
			string[] checkInputs = Input.GetJoystickNames();
			
			int inputNumber = checkInputs.Length;
			
			// if using controller
			if (inputNumber > 0){
				if (Application.platform == RuntimePlatform.OSXEditor || 
				    Application.platform == RuntimePlatform.OSXPlayer ||
				    Application.platform == RuntimePlatform.OSXWebPlayer || 
				    Application.platform == RuntimePlatform.OSXDashboardPlayer){
					if (Input.GetButtonDown("SwitchCharMac")){
						isHowieSolo=!isHowieSolo;
						GameObject chompers = GameObject.FindGameObjectsWithTag("Yarla")[0];
						//deactivates/activates chomping ability based on whether howie is solo
						chompers.renderer.enabled = !isHowieSolo;
						// always reset current frame to ensure no errors
						currentWalkSprite = 0;
					}
					
				}
				if (Application.platform == RuntimePlatform.WindowsEditor || 
				    Application.platform == RuntimePlatform.WindowsPlayer ||
				    Application.platform == RuntimePlatform.WindowsWebPlayer){
					if (Input.GetButtonDown("SwitchCharPC")){
						isHowieSolo=!isHowieSolo;
						GameObject chompers = GameObject.FindGameObjectsWithTag("Yarla")[0];
						//deactivates/activates chomping ability based on whether howie is solo
						chompers.renderer.enabled = !isHowieSolo;
						// always reset current frame to ensure no errors
						currentWalkSprite = 0;
					}
					
				}
			}
			// if not using controller
			else{
				if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)){
					if (isHowieSolo){
						isHowieSolo = false;
					}
					else{
						isHowieSolo = true;
					}
					
					// always reset current frame to ensure no errors
					currentWalkSprite = 0;
				}
			}
		}
		
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
		
		// loads game over screen when called
		Application.LoadLevel(gameOverScene);
		
	}
	
	
}

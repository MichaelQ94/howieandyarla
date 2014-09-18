using UnityEngine;
using System.Collections;

public class YarlaS : MonoBehaviour {
	
	public GameObject	holdTarget;
	
	public float		throwMultiplierX = 1000;
	public float		throwMultiplierY = 1000;
	
	public float		prevXPos;
	public float		prevYPos;
	
	public bool			holding = false;

	public float 		grabTimeSleep = 0.1f;
	
	public float 		velCountdownMax = 0.01f;
	public float 		velCountdown = 0.01f;
	
	public bool			launchAxisDown = false;
	public bool			launched = false;
	public float 		launchSpeed = 1000;
	public float		launchMaxTime = 0.2f;
	public float		launchMaxCooldown = 0.2f;
	
	public float		sweetSpot = 0.4f;
	public float 		holdTime = 0;
	public float		maxThrowVel = 10000;
	
	public float		kickBackMultiplier = 0.25f;
	public float		kickBackTime = 0.2f;
	
	public float		throwExceedMult = 0.75f;

	public float 		enemyCapturedZ;
	
	public HowieS howie;
	public static		YarlaS	Y;
	
	void Start () {
		
		Y = this;
		
		velCountdown = velCountdownMax;
		launchMaxCooldown = launchMaxTime;
		
		howie = GameObject.FindGameObjectsWithTag ("Player") [0].GetComponent<HowieS> ();
		
	}

	void FixedUpdate () {
		// turn throw and grab off when just solo howie
		if (!howie.isHowieSolo){
			
			HoldEnemy ();
			renderer.enabled = true;
			
		}
		else{
			renderer.enabled = false;
			ResetPosition();
		}
	}
	
	void OnTriggerEnter ( Collider other ){
		
		if (other.gameObject.tag == "Enemy" && !holding){
			
			if (holdTarget == null){

				// only add if enemy is not dead and can be held

				if (!other.gameObject.GetComponent<EnemyS>().isDead && 
				    !other.gameObject.GetComponent<EnemyS>().cannotBeHeld){

					holdTarget = other.gameObject;
					//CameraShakeS.C.TimeSleep(grabTimeSleep);
				
				}
			}
			
			else if (holdTarget.gameObject.name != other.gameObject.name){
				if (!other.gameObject.GetComponent<EnemyS>().isDead && 
				    !other.gameObject.GetComponent<EnemyS>().cannotBeHeld){
					
					holdTarget = other.gameObject;
					//CameraShakeS.C.TimeSleep(grabTimeSleep);
					
				}
			}
			
			//print ("Hit Pickup!");
			
		}
		if (other.gameObject.tag == "Wall" && launched){
		//	print("ehhh");
			ReturnToHowie();
		}
		
	}
	
	void OnTriggerExit ( Collider other ){
		
		if (other.gameObject == holdTarget && !holding){
			holdTarget = null;
		}
		
	}

	void ResetPosition () {
	
		transform.localPosition = Vector3.zero;

		
	}
	
	void HoldEnemy () {
		
		string[] checkInputs = Input.GetJoystickNames();
		
		int inputNumber = checkInputs.Length;
		
		CheckHoldTarget();
		
		//print (Input.GetAxisRaw("Fire1"));
		//print (rigidbody.velocity);
		
		if (Application.platform == RuntimePlatform.OSXEditor || 
				Application.platform == RuntimePlatform.OSXPlayer ||
				Application.platform == RuntimePlatform.OSXWebPlayer || 
				Application.platform == RuntimePlatform.OSXDashboardPlayer){
			if (Input.GetAxis("Fire1Mac") > 0 && !launched && !holding && !launchAxisDown){
				if (inputNumber > 0){
					rigidbody.velocity = new Vector3
						(launchSpeed*Input.GetAxis("SecondHorizontalMac"),
						launchSpeed*Input.GetAxis("SecondVerticalMac"),0)*Time.deltaTime;
				}
				else{
					rigidbody.velocity = new Vector3
						(launchSpeed*howie.handPos.x,
						launchSpeed*howie.handPos.y,0)*Time.deltaTime/howie.maxHandRadius;
				}
				launchAxisDown = true;
				launched = true;
					//print ("Shoot!");
			}
		}
		if (Application.platform == RuntimePlatform.WindowsEditor || 
				Application.platform == RuntimePlatform.WindowsPlayer ||
				Application.platform == RuntimePlatform.WindowsWebPlayer){
			if (Input.GetAxis("Fire1PC") > 0 && !launched && !holding && !launchAxisDown){
				if (inputNumber > 0){
					rigidbody.velocity = new Vector3
						(launchSpeed*Input.GetAxis("SecondHorizontalPC"),
						launchSpeed*Input.GetAxis("SecondVerticalPC"),0)*Time.deltaTime;
				}
				else{
					rigidbody.velocity = new Vector3
						(launchSpeed*howie.handPos.x,
						 launchSpeed*howie.handPos.y,0)*Time.deltaTime/howie.maxHandRadius;
				}
				launchAxisDown = true;
				launched = true;
			}
		}
		
		if (Application.platform == RuntimePlatform.OSXEditor || 
				Application.platform == RuntimePlatform.OSXPlayer ||
				Application.platform == RuntimePlatform.OSXWebPlayer || 
				Application.platform == RuntimePlatform.OSXDashboardPlayer){
			if (launchAxisDown && Input.GetAxis("Fire1Mac") <= 0){
				launchAxisDown = false;
			}
		}
		
		if (Application.platform == RuntimePlatform.WindowsEditor || 
				Application.platform == RuntimePlatform.WindowsPlayer ||
				Application.platform == RuntimePlatform.WindowsWebPlayer){
			if (launchAxisDown && Input.GetAxis("Fire1PC") <= 0){
				launchAxisDown = false;
			}
		}
		
		if (launched){
			launchMaxCooldown -= Time.deltaTime;
			if (launchMaxCooldown <= 0){
				ReturnToHowie();
			}
			if (holdTarget != null){
				CameraShakeS.C.MicroShake();
				ReturnToHowie();
				holding = true;
				enemyCapturedZ = holdTarget.transform.position.z;
				holdTarget.rigidbody.velocity = Vector3.zero;
				holdTarget.transform.position = transform.position;
				holdTarget.transform.parent = transform;
				holdTarget.rigidbody.isKinematic = true;
				holdTarget.GetComponent<EnemyS>().beingHeld = true;
				launched = false;
			}
		}
		
		if (holding && holdTarget != null){
			
			holdTime += Time.deltaTime;
			
			if (holdTime >= sweetSpot - 0.3f && holdTime <= sweetSpot){
					holdTarget.GetComponent<EnemyS>().atSweetSpot = true;
			}
		
			else{
					holdTarget.GetComponent<EnemyS>().atSweetSpot = false;
			}
			
			if (Application.platform == RuntimePlatform.OSXEditor || 
				Application.platform == RuntimePlatform.OSXPlayer ||
				Application.platform == RuntimePlatform.OSXWebPlayer || 
				Application.platform == RuntimePlatform.OSXDashboardPlayer){
				
				if (Input.GetAxis("Fire1Mac") <= 0){
					
					holdTarget.transform.parent = null;
					holdTarget.rigidbody.isKinematic = false;
					Vector3 resetTargetPos = holdTarget.transform.position;
					resetTargetPos.z = enemyCapturedZ;
					holdTarget.transform.position = resetTargetPos;
					holdTarget.GetComponent<EnemyS>().beingHeld = false;
					holdTarget.GetComponent<EnemyS>().beingThrown = true;
					
					Vector3 throwVelocity;
					
					if (holdTime > sweetSpot){
						if (inputNumber > 0){
							throwVelocity = new Vector3 (maxThrowVel*Input.GetAxis("SecondHorizontalMac"),
							                             maxThrowVel*Input.GetAxis("SecondVerticalMac"), 0)*throwExceedMult
								*Time.deltaTime;
						}
						else{
							throwVelocity = new Vector3 (maxThrowVel*howie.handPos.x,
							                             maxThrowVel*howie.handPos.y, 0)*throwExceedMult
								*Time.deltaTime*2/howie.maxHandRadius;
						}
						howie.rigidbody.AddForce(throwVelocity*-1*kickBackMultiplier
						*Time.deltaTime*throwExceedMult,ForceMode.Impulse);
						holdTarget.GetComponent<EnemyS>().atSweetSpot = false;
					}
					else{
						if (inputNumber > 0){
							throwVelocity = new Vector3 (maxThrowVel*Input.GetAxis("SecondHorizontalMac"),
							                             maxThrowVel*Input.GetAxis("SecondVerticalMac"), 0)*(holdTime/sweetSpot)
								*Time.deltaTime;
						}
						else{
							throwVelocity = new Vector3 (maxThrowVel*howie.handPos.x,
							                             maxThrowVel*howie.handPos.y, 0)*(holdTime/sweetSpot)
								*Time.deltaTime*2/howie.maxHandRadius;
						}
						howie.rigidbody.AddForce(throwVelocity*-1*kickBackMultiplier
						*Time.deltaTime*(holdTime/sweetSpot),ForceMode.Impulse);
						holdTarget.GetComponent<EnemyS>().atSweetSpot = false;
						
					}
					
					CameraShakeS.C.MicroShake();
					holdTarget.rigidbody.velocity = throwVelocity;
					howie.KnockBack(kickBackTime);
					//print(throwVelocity);
					
					launchAxisDown = false;
					holding = false;
					holdTime = 0;
				}
			}
			
			if (Application.platform == RuntimePlatform.WindowsEditor || 
				Application.platform == RuntimePlatform.WindowsPlayer ||
				Application.platform == RuntimePlatform.WindowsWebPlayer){
				
				if (Input.GetAxis("Fire1PC") <= 0){
					
					holdTarget.transform.parent = null;
					holdTarget.rigidbody.isKinematic = false;
					Vector3 resetTargetPos = holdTarget.transform.position;
					resetTargetPos.z = enemyCapturedZ;
					holdTarget.transform.position = resetTargetPos;
					holdTarget.GetComponent<EnemyS>().beingHeld = false;
					holdTarget.GetComponent<EnemyS>().beingThrown = true;
					
					Vector3 throwVelocity;
					
					if (holdTime > sweetSpot){
						if (inputNumber > 0){
							throwVelocity = new Vector3 (maxThrowVel*Input.GetAxis("SecondHorizontalPC"),
							                             maxThrowVel*Input.GetAxis("SecondVerticalPC"), 0)*throwExceedMult
								*Time.deltaTime;
						}
						else{
							throwVelocity = new Vector3 (maxThrowVel*howie.handPos.x,
							                             maxThrowVel*howie.handPos.y, 0)*throwExceedMult
								*Time.deltaTime*2/howie.maxHandRadius;
						}
						howie.rigidbody.AddForce(throwVelocity*-1*kickBackMultiplier
						                            *Time.deltaTime*throwExceedMult,ForceMode.Impulse);
						holdTarget.GetComponent<EnemyS>().atSweetSpot = false;
					}
					else{
						if (inputNumber > 0){
							throwVelocity = new Vector3 (maxThrowVel*Input.GetAxis("SecondHorizontalPC"),
							                             maxThrowVel*Input.GetAxis("SecondVerticalPC"), 0)*(holdTime/sweetSpot)
								*Time.deltaTime;
						}
						else{
							throwVelocity = new Vector3 (maxThrowVel*howie.handPos.x,
							                             maxThrowVel*howie.handPos.y, 0)*(holdTime/sweetSpot)
								*Time.deltaTime*2/howie.maxHandRadius;
						}
						howie.rigidbody.AddForce(throwVelocity*-1*kickBackMultiplier
						                            *Time.deltaTime*(holdTime/sweetSpot),ForceMode.Impulse);
						holdTarget.GetComponent<EnemyS>().atSweetSpot = false;
						
					}
					
					CameraShakeS.C.MicroShake();
					holdTarget.rigidbody.velocity = throwVelocity;
					howie.KnockBack(kickBackTime);
					//print(throwVelocity);
					
					launchAxisDown = false;
					holding = false;
					holdTime = 0;
				}
			}
			
		}
		
		/*
		if (Input.GetAxis("Fire1") > -1 && holdTarget != null){
			
			holding = true;
			holdTarget.rigidbody.velocity = Vector3.zero;
			holdTarget.transform.position = transform.position;
			
		}
		
		if (Input.GetAxis("Fire1") <= -1 && holdTarget != null){
			
			print ("Throw!");
			
			
			
			Vector3 releaseVel = new Vector3((transform.position.x-prevXPos)/velCountdown,
				(transform.position.y-prevYPos)/velCountdown,0);
			
			//releaseVel.x *= throwMultiplierX*Time.deltaTime;
			//releaseVel.y *= throwMultiplierY*Time.deltaTime;
			
			if (holdTarget != null){
			holdTarget.rigidbody.velocity = releaseVel;
			}
			
			
		}
		
		if (Input.GetAxisRaw("Fire1") >= -1){
			holding = false;
		}
		
		if (holdTarget != null && holding){
			velCountdown -= Time.deltaTime;
			if (velCountdown <= 0){
				prevXPos = transform.position.x;
				prevYPos = transform.position.y;
				velCountdown = velCountdownMax;
			}
		}
	*/	
	}
	
	void ReturnToHowie() {
		
		rigidbody.velocity = Vector3.zero;
		//print ("MOVED!");
		transform.position = howie.transform.position;
		launchMaxCooldown = launchMaxTime;
		launched = false;
		
	}
	
	void CheckHoldTarget() {

		// remove held enemy if dead
		if (holdTarget != null){
			if (holdTarget.GetComponent<EnemyS>().isDead){
				holdTarget.transform.parent = null;
				Vector3 resetTargetPos = holdTarget.transform.position;
				resetTargetPos.z = enemyCapturedZ;
				holdTarget.transform.position = resetTargetPos;
				holdTarget = null;
			}
		}

		// turn off holding once no held target
		if (holdTarget == null){
			
			holdTime = 0;
			holding = false;
			
		}
		
	}
				
}

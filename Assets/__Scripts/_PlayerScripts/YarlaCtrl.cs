using UnityEngine;
using System;

public class YarlaCtrl
{
	public YarlaS yarla;
	public HowieS howie;
	public NewChompS chompHead;
	public Rigidbody rigidbody;

	public GameObject holdTarget;
	public bool	holding = false; //True if currently holding something
	public float holdTime = 0;
	public float sweetSpot = 0.4f;
	public float maxThrowVel = 10000;
	public float throwExceedMult = 0.75f;

	public float kickBackMultiplier = 0.25f;
	public float kickBackTime = 0.2f;

	public float enemyCapturedZ;

	//Launch attributes
	public float launchSpeed = 1000;
	public bool	launchAxisDown = false;
	public bool	launched = false;
	public float launchMaxCooldown = 0.2f;
	public float launchMaxTime = 0.2f;


	public YarlaCtrl ()
	{
	}

	public YarlaCtrl(YarlaS yarla)
	{
		//Gain access to yarla and acquire Yarla's attributes
		//(So we don't have to keep going into yarla to get them)
		//keep
		this.yarla = yarla;
		this.howie = yarla.howie;
		this.chompHead = yarla.chompHead;
		rigidbody = yarla.rigidbody;

		//port over?

	}

	public void HoldEnemy () {
		
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
				holdTarget.transform.position = yarla.transform.position;
				holdTarget.transform.parent = yarla.transform;
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

	public void OnTriggerEnter ( Collider other ){
		
		
		
		if (other.gameObject.tag == "Enemy" && !holding){
			
			if (holdTarget == null){
				
				// only add if enemy is not dead and can be held
				
				if (!other.gameObject.GetComponent<EnemyS>().isDead && 
				    !other.gameObject.GetComponent<EnemyS>().cannotBeHeld){
					holdTarget = other.gameObject;
					chompHead.timeToTriggerChomp = other.GetComponent<EnemyS>().requiredAbsorbTime;
					//CameraShakeS.C.TimeSleep(grabTimeSleep);
					
				}
			}
			
			else if (holdTarget.gameObject.name != other.gameObject.name){
				if (!other.gameObject.GetComponent<EnemyS>().isDead && 
				    !other.gameObject.GetComponent<EnemyS>().cannotBeHeld){
					
					holdTarget = other.gameObject;
					chompHead.timeToTriggerChomp = other.GetComponent<EnemyS>().requiredAbsorbTime;
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

	public void CheckHoldTarget() {
		
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

	public void OnTriggerExit ( Collider other ){
		
		if (other.gameObject == holdTarget && !holding){
			holdTarget = null;
		}
	}

	public void ReturnToHowie() {
		
		rigidbody.velocity = Vector3.zero;
		//print ("MOVED!");
		yarla.transform.position = howie.transform.position;
		launchMaxCooldown = launchMaxTime;
		launched = false;
		
	}
}
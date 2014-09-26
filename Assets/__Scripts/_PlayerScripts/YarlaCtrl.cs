using UnityEngine;
using System;

public class YarlaCtrl
{
	public YarlaS yarla;
	public HowieS howie;
	public Rigidbody rigidbody;

	public GameObject holdTarget;
	public bool	holding = false; //True if currently holding something
	public float holdTime = 0;
	public float sweetSpot = 0.4f;
	public float maxthrowvel = 10000;

	public float enemyCapturedZ;

	//Launch attributes
	public float launchSpeed = 1000;
	public bool	launchAxisDown = false;
	public bool	launched = false;
	public float launchMaxCooldown = 0.2f;


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
		rigidbody = yarla.rigidbody;

		//port over?
		holdTarget = yarla.holdTarget;
		holding = yarla.holding;
		launchSpeed = yarla.launchSpeed;
		launchAxisDown = yarla.launchAxisDown;
		launched = yarla.launched;
		launchMaxCooldown = yarla.launchMaxCooldown;

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
					
					yarla.holdTarget.transform.parent = null;
					yarla.holdTarget.rigidbody.isKinematic = false;
					Vector3 resetTargetPos = yarla.holdTarget.transform.position;
					resetTargetPos.z = yarla.enemyCapturedZ;
					yarla.holdTarget.transform.position = resetTargetPos;
					yarla.holdTarget.GetComponent<EnemyS>().beingHeld = false;
					yarla.holdTarget.GetComponent<EnemyS>().beingThrown = true;
					
					Vector3 throwVelocity;
					
					if (yarla.holdTime > yarla.sweetSpot){
						if (inputNumber > 0){
							throwVelocity = new Vector3 (yarla.maxThrowVel*Input.GetAxis("SecondHorizontalMac"),
							                             yarla.maxThrowVel*Input.GetAxis("SecondVerticalMac"), 0)*yarla.throwExceedMult
								*Time.deltaTime;
						}
						else{
							throwVelocity = new Vector3 (yarla.maxThrowVel*yarla.howie.handPos.x,
							                             yarla.maxThrowVel*yarla.howie.handPos.y, 0)*yarla.throwExceedMult
								*Time.deltaTime*2/yarla.howie.maxHandRadius;
						}
						yarla.howie.rigidbody.AddForce(throwVelocity*-1*yarla.kickBackMultiplier
						                         *Time.deltaTime*yarla.throwExceedMult,ForceMode.Impulse);
						yarla.holdTarget.GetComponent<EnemyS>().atSweetSpot = false;
					}
					else{
						if (inputNumber > 0){
							throwVelocity = new Vector3 (yarla.maxThrowVel*Input.GetAxis("SecondHorizontalMac"),
							                             yarla.maxThrowVel*Input.GetAxis("SecondVerticalMac"), 0)*(yarla.holdTime/yarla.sweetSpot)
								*Time.deltaTime;
						}
						else{
							throwVelocity = new Vector3 (yarla.maxThrowVel*yarla.howie.handPos.x,
							                             yarla.maxThrowVel*yarla.howie.handPos.y, 0)*(yarla.holdTime/yarla.sweetSpot)
								*Time.deltaTime*2/yarla.howie.maxHandRadius;
						}
						yarla.howie.rigidbody.AddForce(throwVelocity*-1*yarla.kickBackMultiplier
						                         *Time.deltaTime*(yarla.holdTime/yarla.sweetSpot),ForceMode.Impulse);
						yarla.holdTarget.GetComponent<EnemyS>().atSweetSpot = false;
						
					}
					
					CameraShakeS.C.MicroShake();
					yarla.holdTarget.rigidbody.velocity = throwVelocity;
					yarla.howie.KnockBack(yarla.kickBackTime);
					//print(throwVelocity);
					
					yarla.launchAxisDown = false;
					yarla.holding = false;
					yarla.holdTime = 0;
				}
			}
			
			if (Application.platform == RuntimePlatform.WindowsEditor || 
			    Application.platform == RuntimePlatform.WindowsPlayer ||
			    Application.platform == RuntimePlatform.WindowsWebPlayer){
				
				if (Input.GetAxis("Fire1PC") <= 0){
					
					yarla.holdTarget.transform.parent = null;
					yarla.holdTarget.rigidbody.isKinematic = false;
					Vector3 resetTargetPos = yarla.holdTarget.transform.position;
					resetTargetPos.z = yarla.enemyCapturedZ;
					yarla.holdTarget.transform.position = resetTargetPos;
					yarla.holdTarget.GetComponent<EnemyS>().beingHeld = false;
					yarla.holdTarget.GetComponent<EnemyS>().beingThrown = true;
					
					Vector3 throwVelocity;
					
					if (yarla.holdTime > yarla.sweetSpot){
						if (inputNumber > 0){
							throwVelocity = new Vector3 (yarla.maxThrowVel*Input.GetAxis("SecondHorizontalPC"),
							                             yarla.maxThrowVel*Input.GetAxis("SecondVerticalPC"), 0)*yarla.throwExceedMult
								*Time.deltaTime;
						}
						else{
							throwVelocity = new Vector3 (yarla.maxThrowVel*yarla.howie.handPos.x,
							                             yarla.maxThrowVel*yarla.howie.handPos.y, 0)*yarla.throwExceedMult
								*Time.deltaTime*2/yarla.howie.maxHandRadius;
						}
						yarla.howie.rigidbody.AddForce(throwVelocity*-1*yarla.kickBackMultiplier
						                               *Time.deltaTime*yarla.throwExceedMult,ForceMode.Impulse);
						yarla.holdTarget.GetComponent<EnemyS>().atSweetSpot = false;
					}
					else{
						if (inputNumber > 0){
							throwVelocity = new Vector3 (yarla.maxThrowVel*Input.GetAxis("SecondHorizontalPC"),
							                             yarla.maxThrowVel*Input.GetAxis("SecondVerticalPC"), 0)*(yarla.holdTime/yarla.sweetSpot)
								*Time.deltaTime;
						}
						else{
							throwVelocity = new Vector3 (yarla.maxThrowVel*yarla.howie.handPos.x,
							                             yarla.maxThrowVel*yarla.howie.handPos.y, 0)*(yarla.holdTime/yarla.sweetSpot)
								*Time.deltaTime*2/yarla.howie.maxHandRadius;
						}
						yarla.howie.rigidbody.AddForce(throwVelocity*-1*yarla.kickBackMultiplier
						                         *Time.deltaTime*(yarla.holdTime/yarla.sweetSpot),ForceMode.Impulse);
						yarla.holdTarget.GetComponent<EnemyS>().atSweetSpot = false;
						
					}
					
					CameraShakeS.C.MicroShake();
					yarla.holdTarget.rigidbody.velocity = throwVelocity;
					yarla.howie.KnockBack(yarla.kickBackTime);
					//print(throwVelocity);
					
					yarla.launchAxisDown = false;
					yarla.holding = false;
					yarla.holdTime = 0;
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

	public void CheckHoldTarget() {
		
		// remove held enemy if dead
		if (yarla.holdTarget != null){
			if (yarla.holdTarget.GetComponent<EnemyS>().isDead){
				yarla.holdTarget.transform.parent = null;
				Vector3 resetTargetPos = yarla.holdTarget.transform.position;
				resetTargetPos.z = yarla.enemyCapturedZ;
				yarla.holdTarget.transform.position = resetTargetPos;
				yarla.holdTarget = null;
			}
		}
		
		// turn off holding once no held target
		if (yarla.holdTarget == null){
			
			yarla.holdTime = 0;
			yarla.holding = false;
			
		}
		
	}

	public void ReturnToHowie() {
		
		yarla.rigidbody.velocity = Vector3.zero;
		//print ("MOVED!");
		yarla.transform.position = yarla.howie.transform.position;
		yarla.launchMaxCooldown = yarla.launchMaxTime;
		yarla.launched = false;
		
	}
}
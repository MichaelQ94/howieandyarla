using UnityEngine;
using System;

public class YarlaCtrl
{
	public YarlaS yarla;

	public YarlaCtrl ()
	{
	}

	public YarlaCtrl(YarlaS yarla)
	{
		this.yarla = yarla;
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
			if (Input.GetAxis("Fire1Mac") > 0 && !yarla.launched && !yarla.holding && !yarla.launchAxisDown){
				if (inputNumber > 0){
					yarla.rigidbody.velocity = new Vector3
						(yarla.launchSpeed*Input.GetAxis("SecondHorizontalMac"),
						 yarla.launchSpeed*Input.GetAxis("SecondVerticalMac"),0)*Time.deltaTime;
				}
				else{
					yarla.rigidbody.velocity = new Vector3
						(yarla.launchSpeed*yarla.howie.handPos.x,
						 yarla.launchSpeed*yarla.howie.handPos.y,0)*Time.deltaTime/yarla.howie.maxHandRadius;
				}
				yarla.launchAxisDown = true;
				yarla.launched = true;
				//print ("Shoot!");
			}
		}
		if (Application.platform == RuntimePlatform.WindowsEditor || 
		    Application.platform == RuntimePlatform.WindowsPlayer ||
		    Application.platform == RuntimePlatform.WindowsWebPlayer){
			if (Input.GetAxis("Fire1PC") > 0 && !yarla.launched && !yarla.holding && !yarla.launchAxisDown){
				if (inputNumber > 0){
					yarla.rigidbody.velocity = new Vector3
						(yarla.launchSpeed*Input.GetAxis("SecondHorizontalPC"),
						 yarla.launchSpeed*Input.GetAxis("SecondVerticalPC"),0)*Time.deltaTime;
				}
				else{
					yarla.rigidbody.velocity = new Vector3
						(yarla.launchSpeed*yarla.howie.handPos.x,
						 yarla.launchSpeed*yarla.howie.handPos.y,0)*Time.deltaTime/yarla.howie.maxHandRadius;
				}
				yarla.launchAxisDown = true;
				yarla.launched = true;
			}
		}
		
		if (Application.platform == RuntimePlatform.OSXEditor || 
		    Application.platform == RuntimePlatform.OSXPlayer ||
		    Application.platform == RuntimePlatform.OSXWebPlayer || 
		    Application.platform == RuntimePlatform.OSXDashboardPlayer){
			if (yarla.launchAxisDown && Input.GetAxis("Fire1Mac") <= 0){
				yarla.launchAxisDown = false;
			}
		}
		
		if (Application.platform == RuntimePlatform.WindowsEditor || 
		    Application.platform == RuntimePlatform.WindowsPlayer ||
		    Application.platform == RuntimePlatform.WindowsWebPlayer){
			if (yarla.launchAxisDown && Input.GetAxis("Fire1PC") <= 0){
				yarla.launchAxisDown = false;
			}
		}
		
		if (yarla.launched){
			yarla.launchMaxCooldown -= Time.deltaTime;
			if (yarla.launchMaxCooldown <= 0){
				ReturnToHowie();
			}
			if (yarla.holdTarget != null){
				CameraShakeS.C.MicroShake();
				ReturnToHowie();
				yarla.holding = true;
				yarla.enemyCapturedZ = yarla.holdTarget.transform.position.z;
				yarla.holdTarget.rigidbody.velocity = Vector3.zero;
				yarla.holdTarget.transform.position = yarla.transform.position;
				yarla.holdTarget.transform.parent = yarla.transform;
				yarla.holdTarget.rigidbody.isKinematic = true;
				yarla.holdTarget.GetComponent<EnemyS>().beingHeld = true;
				yarla.launched = false;
			}
		}
		
		if (yarla.holding && yarla.holdTarget != null){
			
			yarla.holdTime += Time.deltaTime;
			
			if (yarla.holdTime >= yarla.sweetSpot - 0.3f && yarla.holdTime <= yarla.sweetSpot){
				yarla.holdTarget.GetComponent<EnemyS>().atSweetSpot = true;
			}
			
			else{
				yarla.holdTarget.GetComponent<EnemyS>().atSweetSpot = false;
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
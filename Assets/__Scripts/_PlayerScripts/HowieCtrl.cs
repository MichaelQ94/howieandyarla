using UnityEngine;
using System;

public class HowieCtrl 
{
	
	public YarlaS	yarla;
	public HowieS	howie;

	public HowieCtrl ()
	{
	}

	public HowieCtrl(HowieS howiee)
	{

		howie = howiee;
		yarla = howie.yarla;
		//Gain access to Howie and acquire Howie's attributes
		//(So we don't have to keep going into Howie to get them)
		//keep
		
		//port over?
		
	}

	public void ActivateMeta () {
		
		string[] checkInputs = Input.GetJoystickNames();
		int numInputs = checkInputs.Length;
		
		// check to make sure player has metas equipped
		if (howie.equippedMetas.Count > 0 && !howie.metaActive){
			
			if (numInputs > 0){
				
				// check for platform
				if (Application.platform == RuntimePlatform.OSXEditor || 
				    Application.platform == RuntimePlatform.OSXPlayer ||
				    Application.platform == RuntimePlatform.OSXWebPlayer || 
				    Application.platform == RuntimePlatform.OSXDashboardPlayer){
					
					if (Input.GetButtonDown("Meta1Mac")){
						
						// make sure player has enough energy,
						// if so subtract and activate
						if (howie.blueEnergyAmt > howie.equippedMetas[0].blueEnergyReq &&
						    howie.redEnergyAmt > howie.equippedMetas[0].redEnergyReq &&
						    howie.purpleEnergyAmt > howie.equippedMetas[0].purpleEnergyReq){
							
							howie.blueEnergyAmt -= howie.equippedMetas[0].blueEnergyReq;
							howie.redEnergyAmt -= howie.equippedMetas[0].redEnergyReq;
							howie.purpleEnergyAmt -= howie.equippedMetas[0].purpleEnergyReq;
							
							howie.equippedMetas[0].Activate ();
							howie.metaActive = true;
							howie.currentWalkSprite = 0;

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
						if (howie.blueEnergyAmt > howie.equippedMetas[0].blueEnergyReq &&
						    howie.redEnergyAmt > howie.equippedMetas[0].redEnergyReq &&
						    howie.purpleEnergyAmt > howie.equippedMetas[0].purpleEnergyReq){
							
							howie.blueEnergyAmt -= howie.equippedMetas[0].blueEnergyReq;
							howie.redEnergyAmt -= howie.equippedMetas[0].redEnergyReq;
							howie.purpleEnergyAmt -= howie.equippedMetas[0].purpleEnergyReq;

							howie.equippedMetas[0].Activate ();
							howie.metaActive = true;
							howie.currentWalkSprite = 0;
							
						}
						
					}
					
				}
			}
			
			else{
				
				// check for activate meta 1 button
				if (Input.GetKeyDown(KeyCode.Alpha1)){
					// make sure player has enough energy,
					// if so subtract and activate
					if (howie.blueEnergyAmt > howie.equippedMetas[0].blueEnergyReq &&
					    howie.redEnergyAmt > howie.equippedMetas[0].redEnergyReq &&
					    howie.purpleEnergyAmt > howie.equippedMetas[0].purpleEnergyReq){
						
						howie.blueEnergyAmt -= howie.equippedMetas[0].blueEnergyReq;
						howie.redEnergyAmt -= howie.equippedMetas[0].redEnergyReq;
						howie.purpleEnergyAmt -= howie.equippedMetas[0].purpleEnergyReq;
						
						howie.equippedMetas[0].Activate ();
						howie.metaActive = true;
						howie.currentWalkSprite = 0;
						
					}
				}
				
			}
			
		}
		
	}

	public void Walk () {
		
		if (howie.knockedBack){
			
			howie.kickBackCountdown -= Time.deltaTime;

			if (howie.kickBackCountdown <= 0){
				howie.knockedBack = false;
			}
			
		}
		else{
			
			howie.charVel = howie.rigidbody.velocity;
			
			if (Application.platform == RuntimePlatform.OSXEditor || 
			    Application.platform == RuntimePlatform.OSXPlayer ||
			    Application.platform == RuntimePlatform.OSXWebPlayer || 
			    Application.platform == RuntimePlatform.OSXDashboardPlayer){
				howie.charVel.x = Input.GetAxis("HorizontalMac")*howie.maxSpeed*Time.deltaTime;
				howie.charVel.y = Input.GetAxis("VerticalMac")*howie.maxSpeed*Time.deltaTime;
			}
			
			if (Application.platform == RuntimePlatform.WindowsEditor || 
			    Application.platform == RuntimePlatform.WindowsPlayer ||
			    Application.platform == RuntimePlatform.WindowsWebPlayer){
				howie.charVel.x = Input.GetAxis("HorizontalPC")*howie.maxSpeed*Time.deltaTime;
				howie.charVel.y = Input.GetAxis("VerticalPC")*howie.maxSpeed*Time.deltaTime;
			}
			
			if (yarla.yarlaCtrl.holding){
				howie.charVel *= howie.chargeSpeedMultiplier;
			}
			
			// speed up if Howie solo!
			if (howie.isHowieSolo){
				howie.charVel *= howie.howieSoloSpeedMult;
			}
			
			// set speed back into rigidbody
			howie.rigidbody.velocity = howie.charVel;
		}
		
	}

	public void CheckHowieSwitch () {
		
		// switch between Howie solo and H&Y at button press
		
		// for simplicity's sake, currently you can NOT switch while holding an enemy
		
		if (!yarla.yarlaCtrl.holding){
			
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
						howie.isHowieSolo=!howie.isHowieSolo;
						GameObject chompers = GameObject.FindGameObjectsWithTag("Yarla")[0];
						//deactivates/activates chomping ability based on whether howie is solo
						chompers.renderer.enabled = !howie.isHowieSolo;
						// always reset current frame to ensure no errors
						howie.currentWalkSprite = 0;
					}
					
				}
				if (Application.platform == RuntimePlatform.WindowsEditor || 
				    Application.platform == RuntimePlatform.WindowsPlayer ||
				    Application.platform == RuntimePlatform.WindowsWebPlayer){
					if (Input.GetButtonDown("SwitchCharPC")){
						howie.isHowieSolo=!howie.isHowieSolo;
						GameObject chompers = GameObject.FindGameObjectsWithTag("Yarla")[0];
						//deactivates/activates chomping ability based on whether howie is solo
						chompers.renderer.enabled = !howie.isHowieSolo;
						// always reset current frame to ensure no errors
						howie.currentWalkSprite = 0;
					}
					
				}
			}
			// if not using controller
			else{
				if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)){
					if (howie.isHowieSolo){
						howie.isHowieSolo = false;
					}
					else{
						howie.isHowieSolo = true;
					}
					
					// always reset current frame to ensure no errors
					howie.currentWalkSprite = 0;
				}
			}
		}
		
	}

	public void MoveHand () {
		
		string[] checkInputs = Input.GetJoystickNames();
		
		int inputNumber = checkInputs.Length;
		if (inputNumber <= 0 && Screen.showCursor){
			//Screen.showCursor = false;
		}
		//print (inputNumber);
		
		
		Vector3 mousePos = CameraShakeS.C.camera.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = howie.transform.position.z;
		
		
		float mouseDistance = Vector3.Distance(howie.transform.position,mousePos);
		
		
		if (!yarla.yarlaCtrl.launched){
			if (!yarla.yarlaCtrl.holding){
				if (inputNumber > 0){
					if (Application.platform == RuntimePlatform.OSXEditor || 
					    Application.platform == RuntimePlatform.OSXPlayer ||
					    Application.platform == RuntimePlatform.OSXWebPlayer || 
					    Application.platform == RuntimePlatform.OSXDashboardPlayer){
						howie.handPos.x = Input.GetAxis("SecondHorizontalMac")*howie.maxHandRadius;
						howie.handPos.y = Input.GetAxis("SecondVerticalMac")*howie.maxHandRadius;
					}
					if (Application.platform == RuntimePlatform.WindowsEditor || 
					    Application.platform == RuntimePlatform.WindowsPlayer ||
					    Application.platform == RuntimePlatform.WindowsWebPlayer){
						howie.handPos.x = Input.GetAxis("SecondHorizontalPC")*howie.maxHandRadius;
						howie.handPos.y = Input.GetAxis("SecondVerticalPC")*howie.maxHandRadius;
					}
				}
				else{
					
					//print (mouseDistance);
					//print (maxHandRadius/hand.transform.localScale.x);
					
					if (mouseDistance < (howie.maxHandRadius/howie.hand.transform.localScale.x)){
						howie.handPos = mousePos - howie.transform.position;
						howie.handPos.x *= howie.hand.transform.localScale.x;
						howie.handPos.y *= howie.hand.transform.localScale.y;
					}
					else{
						howie.handPos = mousePos - howie.transform.position;
						howie.handPos.Normalize();
						howie.handPos *= howie.maxHandRadius;
					}
				}
			}
			else{
				if (inputNumber > 0){
					if (Application.platform == RuntimePlatform.OSXEditor || 
					    Application.platform == RuntimePlatform.OSXPlayer ||
					    Application.platform == RuntimePlatform.OSXWebPlayer || 
					    Application.platform == RuntimePlatform.OSXDashboardPlayer){
						howie.handPos.x = Input.GetAxis("SecondHorizontalMac")*howie.maxHandRadius/2;
						howie.handPos.y = Input.GetAxis("SecondVerticalMac")*howie.maxHandRadius/2;	
					}
					if (Application.platform == RuntimePlatform.WindowsEditor || 
					    Application.platform == RuntimePlatform.WindowsPlayer ||
					    Application.platform == RuntimePlatform.WindowsWebPlayer){
						howie.handPos.x = Input.GetAxis("SecondHorizontalPC")*howie.maxHandRadius/2;
						howie.handPos.y = Input.GetAxis("SecondVerticalPC")*howie.maxHandRadius/2;	
					}
				}
				else{
					if (mouseDistance < (howie.maxHandRadius/howie.hand.transform.localScale.x)){
						howie.handPos = mousePos - howie.transform.position;
						howie.handPos.x *= howie.hand.transform.localScale.x;
						howie.handPos.y *= howie.hand.transform.localScale.y;
					}
					else{
						howie.handPos = mousePos - howie.transform.position;
						howie.handPos.Normalize();
						howie.handPos *= howie.maxHandRadius/2;
					}
				}
			}
			
			//print(handPos);
			
			// only set handpos if not solo howie, else make it zero!
			if (!howie.isHowieSolo){
				howie.hand.transform.localPosition = howie.handPos;
			}
			else{
				howie.hand.transform.localPosition = Vector3.zero;
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
}


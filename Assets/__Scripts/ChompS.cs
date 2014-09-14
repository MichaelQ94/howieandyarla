using UnityEngine;
using System.Collections;

public class ChompS : MonoBehaviour {
	
	// script for chomp attack!
	// currently (8/13/14) only keeps track of size of placeholder indicator
	// and flat out destroys enemies instead of doing x amount of damage
	
	public float	maxXScale = 2; // how big circle should get width wise
	public float	maxYScale = 1; // same but for height
	public float	maxAlpha = 0.95f; // transparency at max size
	
	public float	shakeIntensityMax = 0.05f; // how intense the camera shake should be while charging
	
	public bool		charging = false; // true if player is charging chomp attack
	public float	timeHeld = 0; // how long in sec chomp has charged
	public float	exceedMult = 0.75f; // damage reduction if player holds chomp past sweet spot
	public float	chompSpot = 0.6f; // sweet spot time in sec
	
	public bool		chompButtonHeld = false; // true if button is being held for chomp
	
	public GameObject	chompTarget; // what to chomp
	
	public float 	chompPauseTime = 0.004f; // sleep time in sec for a chomp attack
	
	public static	ChompS 	O; // singleton
	
	// Use this for initialization
	void Start () {
		
		// set singleton to this
		O = this;
	}
	
	// update every physics step
	void FixedUpdate () {
		
		// chomp should not work when we are just solo howie!
		// turn this on and off appropriately
		
		if (!HowieS.H.isHowieSolo){
			
			ChargeChomp(); // method for charging chomp attack
			ChangeIndicator(); // resizes chomp indicator as player charges
			
		}
		
		// make sure to reset everything when switching to solo howie
		
		else{
			
			ResetChomp();
			
		}
		
	}
	
	// this charges the chomp attack when button is held
	public void ChargeChomp () {
		
		// accept input for proper platform (mac vs pc)
		if (Application.platform == RuntimePlatform.OSXEditor || 
		    Application.platform == RuntimePlatform.OSXPlayer ||
		    Application.platform == RuntimePlatform.OSXWebPlayer || 
		    Application.platform == RuntimePlatform.OSXDashboardPlayer){
			
			// if chomp button is being held...
			if (Input.GetAxisRaw("Fire2Mac") > 0){
				charging = true; // set charging to true
				timeHeld += Time.deltaTime; // add up charge time by time passed
				chompButtonHeld = true; // set button down to true
			}
			else{
				// if player lets go after charging...
				if (charging){
					timeHeld = 0; // reset time held
					
					// activate chomp attack
					if (chompButtonHeld){
						CameraShakeS.C.shake_intensity = 0; // end charge shaking
						chompButtonHeld = false; 
						DamageEnemy(); // deal damage to enemy
					}
					charging = false; // turn off charging
				}
			}
		}
		
		// same as above but for pc
		if (Application.platform == RuntimePlatform.WindowsEditor || 
		    Application.platform == RuntimePlatform.WindowsPlayer ||
		    Application.platform == RuntimePlatform.WindowsWebPlayer){
			
			if (Input.GetAxisRaw("Fire2PC") > 0){
				charging = true;
				timeHeld += Time.deltaTime;
				chompButtonHeld = true;
			}
			else{
				if (charging){
					timeHeld = 0;
					if (chompButtonHeld){
						CameraShakeS.C.shake_intensity = 0;
						chompButtonHeld = false;
						DamageEnemy();
					}
					charging = false;
				}
			}
		}
		
	}
	
	// resize and recolor chomp indicator (placeholder)
	void ChangeIndicator () {
		
		Vector3 indicatorScale = transform.localScale; // how big indicator is (x,y,z)
		Color	indicatorAlpha = renderer.material.color; // used to set transparency of indicator
		
		// if charging and not at sweet spot
		if (timeHeld > 0 && timeHeld <= chompSpot){
			
			// grow size according to time held
			indicatorScale.x = maxXScale * (timeHeld/chompSpot);
			indicatorScale.y = maxYScale * (timeHeld/chompSpot);
			
			// become more opaque
			indicatorAlpha.a = maxAlpha * (timeHeld/chompSpot);
			
			// shake the camera continuously
			// shake grows in intensity based on time held
			CameraShakeS.C.shake_intensity = shakeIntensityMax* (timeHeld/chompSpot);
			CameraShakeS.C.continuousShaking = true;
			CameraShakeS.C.shaking = true;
			
		}
		// time held is at max
		else if (timeHeld > chompSpot){
			
			// set size and transparency to max
			indicatorScale.x = maxXScale;
			
			indicatorAlpha.a = maxAlpha;
			
			// adjust shaking accordingly
			CameraShakeS.C.shake_intensity = shakeIntensityMax * exceedMult;
			CameraShakeS.C.continuousShaking = true;
			CameraShakeS.C.shaking = true;
			
		}
		
		// when not charging, reset all values
		else{
			
			indicatorAlpha.a = 0;
			
			indicatorScale = Vector3.zero;
			
			CameraShakeS.C.continuousShaking = false;
			CameraShakeS.C.shaking = false;
			
		}
		
		// need to set scale and color at end back into object, otherwise no change is made
		transform.localScale = indicatorScale;
		
		renderer.material.color = indicatorAlpha;
		
	}
	
	// damages enemy when activated
	void DamageEnemy () {
		
		/*// if yarla is holding something, set it as target
		if (YarlaS.Y.holdTarget != null){
			chompTarget = YarlaS.Y.holdTarget;
		}
		// if there's something to bite, destroy it
		if (chompTarget != null){
			Destroy(chompTarget.gameObject);
			CameraShakeS.C.LargeShake(); // shake and sleep camera for added effect
			CameraShakeS.C.TimeSleep(chompPauseTime);
			chompTarget = null; // chomp target is let go
		}
		
		else{
			CameraShakeS.C.SmallShake(); // if nothing to bite, do smaller camera shake
		}*/
		
	}
	
	void ResetChomp () {
		
		chompTarget = null;
		chompButtonHeld = false;
		charging = false;
		timeHeld = 0;
		
		Vector3 indicatorScale = transform.localScale; // how big indicator is (x,y,z)
		Color	indicatorAlpha = renderer.material.color; // used to set transparency of indicator
		
		indicatorScale.x = 0;
		indicatorScale.y = 0;
		
		indicatorAlpha.a = 0;
		
		transform.localScale = indicatorScale;
		renderer.material.color = indicatorAlpha;
		
	}
}

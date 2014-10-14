using UnityEngine;
using System.Collections;

public class CameraShakeS : MonoBehaviour {
	// Variables for shaking
	public Vector3				originPosition;
	public float				shake_intensity;
	public float				shake_decay;
	
	public float 				camZ = -10;
	
	public float				smallShakeIntensity = 1.5f;
	public float				smallShakeDuration = 0.1f;
	
	public float				largeShakeIntensity = 3;
	public float				largeShakeDuration = 0.2f;
	
	public float				sleepDuration;
	
	public bool					continuousShaking = false;
	public bool 				shaking = false;
	public bool					sleeping = false;
	
	public static CameraShakeS	C;
	
	void Start(){
		
		C = this;
		
		originPosition = transform.position;
		
	}
	
	// Update is called once per frame
	void Update(){
		if(shake_intensity > 0 && !sleeping){
			//print ("SHAKING");
			if (!continuousShaking){
				Vector3 camPos = transform.position;
	    	    camPos.x += Random.insideUnitSphere.x * shake_intensity;
				camPos.y += Random.insideUnitSphere.y/2 * shake_intensity;
				transform.position = camPos;
			}
			if (continuousShaking){
				Vector3 poiPos = GetComponent<CameraFollow2D>().centerPos;
				poiPos.z = camZ;
				poiPos.x += Random.insideUnitSphere.x * shake_intensity;
				poiPos.y += Random.insideUnitSphere.y/2 * shake_intensity;
				transform.position = poiPos;
			}
	        shake_intensity -= shake_decay;
    	}

	}
	
	void FixedUpdate () {
		
		if (sleeping){
			
			Time.timeScale = 0.1f;
			
			sleepDuration -= Time.deltaTime/Time.timeScale;
			if (sleepDuration <= 0){
				Time.timeScale = 1;
				sleeping = false;
			}
			
		}
		
		
		if (shake_intensity <= 0 && shaking){
			this.transform.position = originPosition;
			shaking = false;
		}

	}
	
	public void MicroShake(){
		originPosition = transform.position;
    	shake_intensity = smallShakeIntensity/2;
    	shake_decay = smallShakeIntensity/(2*smallShakeDuration);
		shaking = true;
	}
	
	public void SmallShake(){
		originPosition = transform.position;
    	shake_intensity = smallShakeIntensity;
    	shake_decay = smallShakeIntensity/smallShakeDuration;
		shaking = true;
	}
	
	public void LargeShake(){
		originPosition = transform.position;
    	shake_intensity = largeShakeIntensity;
    	shake_decay = largeShakeIntensity/largeShakeDuration;
		shaking = true;
	}
	
	public void TimeSleep(float sleepTime) {
		
		sleepDuration = sleepTime;
		sleeping = true;
	}
}















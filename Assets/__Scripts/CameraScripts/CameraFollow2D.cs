using UnityEngine;
using System.Collections;

public class CameraFollow2D : MonoBehaviour {
	public GameObject			poi;
	public Vector3				centerPos;
	public float				camEasing = 0.1f;
	public float 				camZ = -10;
	public Vector3				camOffset;

	public float		orthoZoomOutSpeed = 0.5f;

	public float 		orthoMinSize;
	public float 		orthoOriginSize;
	public float 		minMaxMult = 0.8f;

	public NewChompS	attachedChomp;
	public YarlaS		attachedYarla;

	public bool 		doNotChangeSize; // for debug mode purposes

	void Start () {

		orthoOriginSize = camera.orthographicSize;
		orthoMinSize = orthoOriginSize*minMaxMult;
		poi = GameObject.Find ("AdaptiveCameraPoint");
		attachedChomp = GameObject.Find ("Chompy").GetComponent<NewChompS>();
		attachedYarla = GameObject.Find ("Yarla").GetComponent<YarlaS>();

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		// [] get the position of the golfball
		Vector3 poiPos = poi.transform.position;
		poiPos += camOffset;
		// [] set the xy of this to that of the golfball
		Vector3 camPos = this.transform.position;
		camPos.x = (1-camEasing)*camPos.x + camEasing*poiPos.x;
		camPos.y = (1-camEasing)*camPos.y + camEasing*poiPos.y;
		camPos.z = camZ;
		centerPos = camPos;
		
		
		if (!CameraShakeS.C.shaking){
			this.transform.position = camPos;
		}

		
		// change ortho size when charging absorb attack
		if (!doNotChangeSize){
			if (attachedChomp.charging && attachedYarla.yarlaCtrl.holding){
				if (attachedChomp.timeHeld < attachedChomp.timeToTriggerChomp){
					camera.orthographicSize = (orthoOriginSize-
					                           (orthoOriginSize-orthoMinSize)*attachedChomp.timeHeld/attachedChomp.timeToTriggerChomp);
				}
				else{
					camera.orthographicSize = orthoMinSize;
				}
			}
			else{
				if (camera.orthographicSize < orthoOriginSize){
					camera.orthographicSize += orthoZoomOutSpeed*Time.deltaTime;
				}
				else{
					camera.orthographicSize = orthoOriginSize;
				}
			}
		}
	}
}






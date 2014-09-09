using UnityEngine;
using System.Collections;

public class CameraFollow2D : MonoBehaviour {
	public GameObject			poi;
	public Vector3				centerPos;
	public float				camEasing = 0.1f;
	public float 				camZ = -10;
	public Vector3				camOffset;
	
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
	}
}






using UnityEngine;
using System.Collections;

public class CameraDebugZoomInOutS : MonoBehaviour {


	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.UpArrow)){
			camera.orthographicSize++;
		}
		if (Input.GetKeyDown(KeyCode.DownArrow)){
			if (camera.orthographicSize > 1){
				camera.orthographicSize--;
			}
		}
	
	}
}

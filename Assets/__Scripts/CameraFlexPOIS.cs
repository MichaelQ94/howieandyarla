using UnityEngine;
using System.Collections;

public class CameraFlexPOIS : MonoBehaviour {

	
	// Update is called once per frame
	void FixedUpdate () {

		transform.position = (NewChompS.N.transform.position + YarlaS.Y.transform.position)/2;
	
	}
}

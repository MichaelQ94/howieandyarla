using UnityEngine;
using System.Collections;

public class CameraFlexPOIS : MonoBehaviour {

	
	// Update is called once per frame
	void FixedUpdate () {

		if (HowieS.H.isHowieSolo){
			transform.position = HowieS.H.transform.position;
		}
		else{
			transform.position = (NewChompS.N.transform.position + GameObject.FindGameObjectWithTag("YarlaS").transform.position)/2;
		}
	
	}
}

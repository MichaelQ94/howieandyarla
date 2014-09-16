using UnityEngine;
using System.Collections;

public class CameraFlexPOIS : MonoBehaviour {

	public GameObject howie;
	// Update is called once per frame

	void Start(){
		howie = GameObject.FindGameObjectsWithTag("Player")[0];
		// access howie with level.howie.GetComponent<HowieS>()
	}

	void FixedUpdate () {

		if (howie.GetComponent<HowieS>().isHowieSolo){
			transform.position = howie.GetComponent<HowieS>().transform.position;
		}
		else{
			transform.position = (NewChompS.N.transform.position + YarlaS.Y.transform.position)/2;
		}
	
	}
}

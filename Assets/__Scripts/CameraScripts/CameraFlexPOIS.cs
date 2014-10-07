using UnityEngine;
using System.Collections;

public class CameraFlexPOIS : MonoBehaviour {

	public YarlaS	yarla;
	public NewChompS chompHead;

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
			transform.position = (howie.transform.position + yarla.transform.position*3 + chompHead.gameObject.transform.position)/5;
		}
	
	}
}

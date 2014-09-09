using UnityEngine;
using System.Collections;

public class CheckVelocityS : MonoBehaviour {
	
	// script used for printing Velocity of attached object

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		print (rigidbody.velocity);
	
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Events;

public class NPCActiveRadiusS : MonoBehaviour {

	// place this on sphere collider around enemies who need to detect Howie or other copies of itself
	
	public bool	howieIsInRange;
	public NPCS parent;
	public NPCActiveRadiusS() {
	}
	void Start() {
		parent = transform.parent.gameObject.GetComponent<NPCS> ();
	}
	void OnTriggerEnter (Collider other){
		howieIsInRange = Environment.checkIfPlayerCollision (other);
	}
	void Update() {
		if (howieIsInRange && Dialogue.shouldTalk()) {
			parent.speak();
		}
	}


	void OnTriggerExit (Collider other){
		howieIsInRange = Environment.checkIfPlayerCollision (other);
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Events; 

public class EnemyActivationRadiusS : MonoBehaviour {
	// place this on sphere collider around enemies who need to detect Howie or other copies of itself
	public bool howieIsInRange = false;
	
	void OnTriggerEnter (Collider other){
		howieIsInRange = Environment.checkIfPlayerCollision (other);
	}


	void OnTriggerExit (Collider other){
		howieIsInRange = !(Environment.checkIfPlayerCollision(other));
	}
}

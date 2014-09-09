using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyActivationRadiusS : MonoBehaviour {

	// place this on sphere collider around enemies who need to detect Howie or other copies of itself


	public bool 	howieIsInRange = false;



	void OnTriggerEnter (Collider other){

		if (other.gameObject.tag == "Player"){
			howieIsInRange = true;
		}

		

	}


	void OnTriggerExit (Collider other){

		if (other.gameObject.tag == "Player"){
			howieIsInRange = false;
		}
		
		

	}
}

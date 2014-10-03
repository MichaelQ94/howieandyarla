using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnEnemyOnOffS : MonoBehaviour {

	public List<GameObject> objectsToTurnOn;
	public List<GameObject> objectsToTurnOff;

	void OnTriggerEnter (Collider other){

		// when the player hits an object with this script, turn on/off objects in list accordingly

		if (other.gameObject.tag == "Player"){
			if (objectsToTurnOn.Count > 0){
				for (int i = 0; i < objectsToTurnOn.Count; i++){
					objectsToTurnOn[i].SetActive(true);
				}
			}

			if (objectsToTurnOff.Count > 0){
				for (int i = 0; i < objectsToTurnOff.Count; i++){
					objectsToTurnOff[i].SetActive(false);
				}
			}
		}

	}
}

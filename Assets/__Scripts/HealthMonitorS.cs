using UnityEngine;
using System.Collections;

public class HealthMonitorS : MonoBehaviour {

	// place on GUIText object
	// changes GUIText to display player health

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		guiText.text = "Health: " + HowieS.health + "/" + HowieS.maxHealth;
	
	}
}

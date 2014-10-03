using UnityEngine;
using System;

public class HealthMonitorS : MonoBehaviour {

	public HowieS howie;

	// place on GUIText object
	// changes GUIText to display player health

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		guiText.text = "Health: " + Math.Round(HowieS.health) + "/" + HowieS.maxHealth
			+ "\nB: " + howie.blueEnergyAmt + " R: " + howie.redEnergyAmt + " P: " + howie.purpleEnergyAmt;
	
	}
}

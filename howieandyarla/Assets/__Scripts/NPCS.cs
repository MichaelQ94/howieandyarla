using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Events;

public class NPCS : MonoBehaviour {
	 
	// Use this for initialization
	private LevelS level;
	public string[] phrases;
	private int count = 0;
	public NPCS(string[] phrases) {
		this.phrases = phrases;
	}
	void Start() {
		level = (GameObject.FindGameObjectsWithTag("Level")[0]).GetComponent<LevelS>();
	}
	// Update is called once per frame
	void Update () {

	}
	public void speak() {
		Environment.pause (true);
		if (count < this.phrases.Length) {
			Dialogue.printMessage(phrases[count]);
			count++;
			return;
		}
		Dialogue.printMessage("");
		Environment.pause (false);
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCS : MonoBehaviour {
	 
	// Use this for initialization
	private LevelS level;
	private IList<string> phrases;
	private GameObject trigger;
	private int count = 0;
	public EventHandlers events = new EventHandlers();
	public NPCS(IList<string> phrases) {
		this.phrases = phrases;
	}
	void Start() {
		level = (GameObject.FindGameObjectsWithTag("Level")[0]).GetComponent<LevelS>();
	}
	// Update is called once per frame
	void Update () {

	}
	public void speak() {
		if (count < this.phrases.Count) {
			print (phrases[count]);
			count++;
			return;
		}
		print(this.phrases[this.phrases.Count-1]);
	}
}

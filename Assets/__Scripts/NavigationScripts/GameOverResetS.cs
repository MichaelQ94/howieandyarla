using UnityEngine;
using System.Collections;

public class GameOverResetS : MonoBehaviour {

	// simple game over script
	// resets game after few seconds
	// put on camera or anything in game over screen

	public float resetTime = 3; // time in seconds to reset at
	public string	levelToResetTo; // name of scene to reset game to

	// Use this for initialization
	void Start () {

		// reset time scale to 1
		Time.timeScale = 1;
		levelToResetTo = Events.Environment.reloadScene;
	
	}
	
	// Update is called once per frame
	void Update () {

		if (resetTime > 0){
			resetTime -= Time.deltaTime;
		}
		else{

			// we can change how this works later
			// but for now I'll also reset Howie's health on the scene reset

			HowieS.health = HowieS.maxHealth;

			Application.LoadLevel(levelToResetTo);
		}
	
	}
}

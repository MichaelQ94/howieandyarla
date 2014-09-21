using UnityEngine;
using System.Collections;

public class LevelS : MonoBehaviour {

	public ArrayList enemies;

	public GameObject howie;

	public float healthMultiplier;
	public float dmgMultiplier;

	// Use this for initialization
	void Start () {
		enemies = new ArrayList();
		dmgMultiplier = 1.0f;
		healthMultiplier = 1.0f;
		howie = GameObject.FindGameObjectsWithTag("Player")[0];
	}
	
	// Update is called once per frame
	void Update () {
		//disable/enable enemies
	
	}
}

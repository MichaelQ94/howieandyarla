using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnEnemiesOnTimerS : MonoBehaviour {

	public List<GameObject>	enemiesToSpawn;

	public float 	spawnCountdown;
	public float 	spawnCountdownMax = 10;

	public float 	timeBeforeSpawn;
	public GameObject	portalSpawn;
	public bool 		spawnedPortal = false;

	public bool activated;

	// Use this for initialization
	void Start () {
	
		spawnCountdown = spawnCountdownMax;

	}
	
	// Update is called once per frame
	void Update () {
	
		if (activated){

			spawnCountdown -= Time.deltaTime;

			if (spawnCountdown < timeBeforeSpawn && !spawnedPortal){
				Instantiate(portalSpawn,transform.position,Quaternion.identity);
				spawnedPortal = true;
			}

			if (spawnCountdown <= 0){

				int enemyToSpawn = Mathf.RoundToInt(Random.Range(0,enemiesToSpawn.Count));

				Instantiate(enemiesToSpawn[enemyToSpawn],transform.position,Quaternion.identity);

				spawnedPortal = false;
				spawnCountdown = spawnCountdownMax;

			}
		}

	}
}

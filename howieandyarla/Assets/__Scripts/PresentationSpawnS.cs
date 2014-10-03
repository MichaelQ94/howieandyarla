using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PresentationSpawnS : MonoBehaviour {
	
	public List<GameObject> enemiesToSpawn;

	public float minX;
	public float maxX;
	public float minY;
	public float maxY;
	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		CheckSpawn();
		
	
	}
	
	void CheckSpawn () {
		
		if (Application.platform == RuntimePlatform.OSXEditor || 
				Application.platform == RuntimePlatform.OSXPlayer ||
				Application.platform == RuntimePlatform.OSXWebPlayer || 
				Application.platform == RuntimePlatform.OSXDashboardPlayer){
			if (Input.GetButtonDown("Fire3Mac")){
			
					SpawnEnemy();
			
			}
		}
		
		if (Application.platform == RuntimePlatform.WindowsEditor || 
				Application.platform == RuntimePlatform.WindowsPlayer ||
				Application.platform == RuntimePlatform.WindowsWebPlayer){
			if (Input.GetButtonDown("Fire3PC")){
			
					SpawnEnemy();
			
			}
		}
		
	}
	
	void SpawnEnemy() {

		int enemyToSpawn = Mathf.RoundToInt(Random.Range(0,enemiesToSpawn.Count));

		Vector3 spawnPoint = enemiesToSpawn[enemyToSpawn].transform.position;
		spawnPoint.x = Random.Range(minX,maxX);
		spawnPoint.y = Random.Range(minY,maxY);

		Instantiate(enemiesToSpawn[enemyToSpawn],spawnPoint,
			Quaternion.identity);
		
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyDetectS : MonoBehaviour {

	// script to be placed on spherical collider around Howie
	// used to determine appropriate target for chomp attack

	public List<GameObject> enemiesInRange;

	public GameObject	enemyBeingHeld;

	public GameObject	closestEnemy;
	public float 		shortestDistance;
	public float 		dangerDistance = 2;

	public YarlaS		yarla;
	public NewChompS	chompy;

	public GameObject	enemyToChomp;

	public LevelS level;
	// Use this for initialization
	void Start () {

		level = (GameObject.FindGameObjectsWithTag("Level")[0]).GetComponent<LevelS>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		RemoveDeadEnemies();
		FindClosestEnemy();
		ChooseBiteTarget();
	
	}

	void ChooseBiteTarget () {

		if (enemiesInRange.Count > 0){

		// order of priority:
		// 1. Dangerously close enemies
		// 2. Held enemy
		// 3. Enemy closest to chompy head

		// first find held enemy if any
		if (yarla.yarlaCtrl.holdTarget != null){
			enemyBeingHeld = yarla.yarlaCtrl.holdTarget;
		}
		else{
			enemyBeingHeld = null;
		}

		if (shortestDistance < dangerDistance || enemyBeingHeld == null){
			enemyToChomp = closestEnemy;
		}
		else{
			enemyToChomp = enemyBeingHeld;
		}

		}

		else{

			enemyToChomp = null;
			closestEnemy = null;
			shortestDistance = 0;
			enemyBeingHeld = null;

		}

	}

	void FindClosestEnemy () {

		for (int i = 0; i < enemiesInRange.Count; i++){

			Vector3 enemyPos = enemiesInRange[i].transform.position;
			enemyPos.z = chompy.transform.position.z;

			float distToHowie = Vector3.Distance(enemyPos,level.howie.GetComponent<HowieS>().transform.position);

			if (distToHowie < shortestDistance || shortestDistance == 0){
				shortestDistance = distToHowie;
				closestEnemy = enemiesInRange[i];
			}

		}

	}

	void RemoveDeadEnemies () {

		for (int i = 0; i < enemiesInRange.Count; i++){

			if (enemiesInRange[i] == null ||
			    enemiesInRange[i].GetComponent<EnemyS>().isDead){
				enemiesInRange.Remove(enemiesInRange[i]);
			}

		}

		if (enemyToChomp != null && enemyToChomp.GetComponent<EnemyS>().isDead){
			enemyToChomp = null;
		}

	}

	void OnTriggerEnter (Collider other) {

		if (other.gameObject.tag == "Enemy"){

			if (!other.GetComponent<EnemyS>().isDead){
				enemiesInRange.Add (other.gameObject);
			}

		}

	}

	void OnTriggerExit (Collider other){

		if (other.gameObject.tag == "Enemy"){

			enemiesInRange.Remove (other.gameObject);
			
		}

	}
}

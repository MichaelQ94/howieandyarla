using UnityEngine;
using System.Collections;

public class FireMetaS : MetaGeneralS {

	public GameObject	firePrefab;
	public GameObject	fireBallPrefab;

	public float 		fireBallSpeed;
	public float 		fireBallRateOfFire = 2;
	public float 		fireBallROFCountdown;
	public float 		fireBallAccMult = 1;

	public float 		initialFireSpawnRad = 3;
	public int			numFireToSpawnInitial = 6;
	public bool 		spawnedInitialFire = false;

	// Use this for initialization
	void Start () {
	
		attachedHowie = gameObject.GetComponent<HowieS>();

	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (activated){
			UpdateMeta();
			FireBallAttack();
			if (!spawnedInitialFire){
				ActivateFire();
			}
		}
		else{
			spawnedInitialFire = false;
		}
	
	}

	void FireBallAttack (){

		if (fireBallROFCountdown > 0){
			fireBallROFCountdown -= Time.deltaTime;
		}
		else{

			string[] checkInputs = Input.GetJoystickNames();
			int numInputs = checkInputs.Length;

			if (numInputs > 0){

				if (Application.platform == RuntimePlatform.OSXEditor || 
				    Application.platform == RuntimePlatform.OSXPlayer ||
				    Application.platform == RuntimePlatform.OSXWebPlayer || 
				    Application.platform == RuntimePlatform.OSXDashboardPlayer){

				}

			}
			else{

			}

		}

	}

	void ActivateFire(){

		for (int i = 0; i < numFireToSpawnInitial; i++){
			print ("GO");

			Vector3 spawnFirePos = attachedHowie.transform.position;
			spawnFirePos.z += 1;
			spawnFirePos.x += Random.insideUnitCircle.x*initialFireSpawnRad;
			spawnFirePos.y += Random.insideUnitCircle.y*initialFireSpawnRad;

			Instantiate(firePrefab,spawnFirePos,Quaternion.identity);

		}

		spawnedInitialFire = true;

	}
}

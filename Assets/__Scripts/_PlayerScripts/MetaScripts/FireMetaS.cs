using UnityEngine;
using System.Collections;

public class FireMetaS : MetaGeneralS {

	public GameObject	firePrefab;
	public GameObject	fireBallPrefab;
	public GameObject	smokeShotPrefab;

	public float DURATION_MAX = 10;
	public float duration;

	public float 		fireBallSpeed;
	public float 		fireBallRateOfFire = 2;
	public float 		fireBallROFCountdown;
	public float 		fireBallAccMult = 1;

	public float 		smokeShotSpeed = 500;
	public float 		smokeShotROF = 1;
	public float 		smokeShotROFCountdown;
	public float 		smokeShotAccMult = 5;
	public float 		smokeShotNumBullets = 6;

	public float 		spawnFireRate = 0.3f;
	public float 		spawnFireRateMax = 0.3f;

	public float 		initialFireSpawnRad = 3;
	public int			numFireToSpawnInitial = 6;
	public bool 		spawnedInitialFire = false;

	public bool 		fireAxisDown = false;
	public bool 		smokeAxisDown = false;

	public GameObject	attachedYarla;

	// Use this for initialization
	void Start () {
	
		attachedHowie = gameObject.GetComponent<HowieS>();
		attachedYarla = attachedHowie.yarla.gameObject;

	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (activated){
			UpdateMeta();
			FireBallAttack();
			SmokeShotAttack();
			SpawnFire();
			if (!spawnedInitialFire){
				ActivateFire();
			}
		}
		else{
			spawnedInitialFire = false;
		}
	
	}

	// left trigger attack
	void SmokeShotAttack () {

		// this logic tree def needs refactoring
		
		if (smokeShotROFCountdown > 0){
			smokeShotROFCountdown -= Time.deltaTime;
		}
		else{
			
			string[] checkInputs = Input.GetJoystickNames();
			int numInputs = checkInputs.Length;
			
			if (numInputs > 0){
				
				if (Application.platform == RuntimePlatform.OSXEditor || 
				    Application.platform == RuntimePlatform.OSXPlayer ||
				    Application.platform == RuntimePlatform.OSXWebPlayer || 
				    Application.platform == RuntimePlatform.OSXDashboardPlayer){
					
					if (Input.GetAxis("Fire2Mac") < 0){
						
						smokeAxisDown = false;
						
					}
					
					
					if (Input.GetAxis("Fire2Mac") > 0 && !smokeAxisDown){

						for (int i = 0; i < smokeShotNumBullets; i++){

							Vector3 smokeSpawn = attachedYarla.transform.position;
							smokeSpawn.z -= 2;
							
							GameObject	smokeToShoot = Instantiate(smokeShotPrefab,smokeSpawn,Quaternion.identity) as GameObject;
							
							Vector3 smokeTarget = attachedYarla.transform.position;
							smokeTarget.x += Random.insideUnitCircle.x*smokeShotAccMult;
							smokeTarget.y += Random.insideUnitCircle.y*smokeShotAccMult;
							
							Vector3 smokeShotVel = (smokeTarget-attachedHowie.transform.position).normalized
								*smokeShotSpeed*Time.deltaTime;
							
							smokeToShoot.rigidbody.velocity = smokeShotVel;
						}
						
						smokeAxisDown = true;
						smokeShotROFCountdown = smokeShotROF;
						
						CameraShakeS.C.SmallShake();
						
					}
				}
				
				if (Application.platform == RuntimePlatform.WindowsEditor || 
				    Application.platform == RuntimePlatform.WindowsPlayer ||
				    Application.platform == RuntimePlatform.WindowsWebPlayer){
					
					if (Input.GetAxis("Fire2PC") < 0){
						
						smokeAxisDown = false;
						
					}
					
					
					if (Input.GetAxis("Fire2PC") > 0 && !smokeAxisDown){
						
						for (int i = 0; i < smokeShotNumBullets; i++){
							
							Vector3 smokeSpawn = attachedYarla.transform.position;
							smokeSpawn.z -= 2;
							
							GameObject	smokeToShoot = Instantiate(smokeShotPrefab,smokeSpawn,Quaternion.identity) as GameObject;
							
							Vector3 smokeTarget = attachedYarla.transform.position;
							smokeTarget.x += Random.insideUnitCircle.x*smokeShotAccMult;
							smokeTarget.y += Random.insideUnitCircle.y*smokeShotAccMult;
							
							Vector3 smokeShotVel = (smokeTarget-attachedHowie.transform.position).normalized
								*smokeShotSpeed*Time.deltaTime;
							
							smokeToShoot.rigidbody.velocity = smokeShotVel;
						}
						
						smokeAxisDown = true;
						smokeShotROFCountdown = smokeShotROF;
						
						CameraShakeS.C.SmallShake();
						
					}
				}
				
			}
			else{
				if (Input.GetMouseButtonUp(1) || Input.GetKeyUp(KeyCode.Space)){
					
					smokeAxisDown = false;
					
				}
				
				
				if ((Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space)) && !smokeAxisDown){
					
					for (int i = 0; i < smokeShotNumBullets; i++){
						
						Vector3 smokeSpawn = attachedYarla.transform.position;
						smokeSpawn.z -= 2;
						
						GameObject	smokeToShoot = Instantiate(smokeShotPrefab,smokeSpawn,Quaternion.identity) as GameObject;
						
						Vector3 smokeTarget = attachedYarla.transform.position;
						smokeTarget.x += Random.insideUnitCircle.x*smokeShotAccMult;
						smokeTarget.y += Random.insideUnitCircle.y*smokeShotAccMult;
						
						Vector3 smokeShotVel = (smokeTarget-attachedHowie.transform.position).normalized
							*smokeShotSpeed*Time.deltaTime;
						
						smokeToShoot.rigidbody.velocity = smokeShotVel;
					}
					
					smokeAxisDown = true;
					smokeShotROFCountdown = smokeShotROF;
					
					CameraShakeS.C.SmallShake();
					
				}
				
			}
			
		}

	}

	// right trigger attack
	void FireBallAttack (){


		// this logic tree def needs refactoring

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

					if (Input.GetAxis("Fire1Mac") < 0){
						
						fireAxisDown = false;
						
					}


					if (Input.GetAxis("Fire1Mac") > 0 && !fireAxisDown){

						Vector3 fireBallSpawn = attachedYarla.transform.position;
						fireBallSpawn.z -= 2;

						GameObject	fireBallToShoot = Instantiate(fireBallPrefab,fireBallSpawn,Quaternion.identity) as GameObject;

						Vector3 fireBallTarget = attachedYarla.transform.position;
						fireBallTarget.x += Random.insideUnitCircle.x*fireBallAccMult;
						fireBallTarget.y += Random.insideUnitCircle.y*fireBallAccMult;

						Vector3 fireBallVel = (fireBallTarget-attachedHowie.transform.position).normalized
						*fireBallSpeed*Time.deltaTime;

						fireBallToShoot.rigidbody.velocity = fireBallVel;

						fireAxisDown = true;
						fireBallROFCountdown = fireBallRateOfFire;

						CameraShakeS.C.LargeShake();
					
					}
				}

				if (Application.platform == RuntimePlatform.WindowsEditor || 
				    Application.platform == RuntimePlatform.WindowsPlayer ||
				    Application.platform == RuntimePlatform.WindowsWebPlayer){

					if (Input.GetAxis("Fire1PC") < 0){
						
						fireAxisDown = false;
						
					}
					
					if (Input.GetAxis("Fire1PC") > 0 && !fireAxisDown){
						
						Vector3 fireBallSpawn = attachedYarla.transform.position;
						fireBallSpawn.z -= 2;
						
						GameObject	fireBallToShoot = Instantiate(fireBallPrefab,fireBallSpawn,Quaternion.identity) as GameObject;
						
						Vector3 fireBallTarget = attachedYarla.transform.position;
						fireBallTarget.x += Random.insideUnitCircle.x*fireBallAccMult;
						fireBallTarget.y += Random.insideUnitCircle.y*fireBallAccMult;
						
						Vector3 fireBallVel = (fireBallTarget-attachedHowie.transform.position).normalized
							*fireBallSpeed*Time.deltaTime;

						
						fireBallToShoot.rigidbody.velocity = fireBallVel;
						
						fireAxisDown = true;
						fireBallROFCountdown = fireBallRateOfFire;

						CameraShakeS.C.LargeShake();
						
					}
				}

			}
			else{
				if (Input.GetMouseButtonUp(0)){
					fireAxisDown = false;
				}

				if (Input.GetMouseButtonDown(0)){

					Vector3 fireBallSpawn = attachedYarla.transform.position;
					fireBallSpawn.z -= 2;
					
					GameObject	fireBallToShoot = Instantiate(fireBallPrefab,fireBallSpawn,Quaternion.identity) as GameObject;
					
					Vector3 fireBallTarget = attachedYarla.transform.position;
					fireBallTarget.x += Random.insideUnitCircle.x*fireBallAccMult;
					fireBallTarget.y += Random.insideUnitCircle.y*fireBallAccMult;
					
					Vector3 fireBallVel = (fireBallTarget-attachedHowie.transform.position).normalized
						*fireBallSpeed*Time.deltaTime;
					
					fireBallToShoot.rigidbody.velocity = fireBallVel;
					
					fireAxisDown = true;
					fireBallROFCountdown = fireBallRateOfFire;
					
					CameraShakeS.C.LargeShake();

				}

			}

		}

	}

	void ActivateFire(){

		for (int i = 0; i < numFireToSpawnInitial; i++){
			//print ("GO");

			Vector3 spawnFirePos = attachedHowie.transform.position;
			spawnFirePos.z += 1;
			spawnFirePos.x += Random.insideUnitCircle.x*initialFireSpawnRad;
			spawnFirePos.y += Random.insideUnitCircle.y*initialFireSpawnRad;

			Instantiate(firePrefab,spawnFirePos,Quaternion.identity);

		}

		spawnedInitialFire = true;

	}

	void SpawnFire(){

		spawnFireRate -= Time.deltaTime;

		if (spawnFireRate <= 0){
			Vector3 spawnFirePos = attachedHowie.transform.position;
			spawnFirePos.z += 1;
			Instantiate(firePrefab,spawnFirePos,Quaternion.identity);
			spawnFireRate = spawnFireRateMax;
		}

	}
}

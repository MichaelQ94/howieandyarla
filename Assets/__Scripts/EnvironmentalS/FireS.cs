using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireS : MonoBehaviour {

	public List<Texture>	animationFrames;
	public float changeFrameTime = 0.3f;
	public float changeFrameCountdown;
	public int currentFrame;

	public float lifeTime = 3;
	
	void FixedUpdate () {

		AnimateFire();
		FireDecay();
	
	}

	void FireDecay (){

		lifeTime -= Time.deltaTime;
		if (lifeTime <= 0){
			Destroy(gameObject);
		}

	}

	void AnimateFire () {

		changeFrameCountdown -= Time.deltaTime;
		if (changeFrameCountdown <= 0){

			currentFrame ++;
			if (currentFrame > animationFrames.Count - 1){
				currentFrame = 0;
			}

			renderer.material.SetTexture("_MainTex", animationFrames[currentFrame]);

			changeFrameCountdown = changeFrameTime;

		}

	}

	void OnTriggerStay (Collider other) {

		if (other.gameObject.tag == "Enemy"){

			EnemyS	collidedEnemy = other.gameObject.GetComponent<EnemyS>();

			if (collidedEnemy.canBeSetOnFire && !collidedEnemy.onFire){
				collidedEnemy.fireDamageCountdown = collidedEnemy.fireDamageMax;
				collidedEnemy.fireTimeCountdown = collidedEnemy.fireTimeMax;
				collidedEnemy.onFire = true;

				// place a fire object on enemy's back
				Vector3 followFirePos = other.transform.position;
				followFirePos.z += 1;
				GameObject followFire = Instantiate(gameObject,followFirePos,Quaternion.identity) as GameObject;
				followFire.GetComponent<FireS>().lifeTime = collidedEnemy.fireTimeMax;
				followFire.transform.parent = other.gameObject.transform;
			}
		}

	}
}

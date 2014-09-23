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
			if (other.gameObject.GetComponent<EnemyS>().canBeSetOnFire){
				other.gameObject.GetComponent<EnemyS>().onFire = true;
			}
		}

	}
}

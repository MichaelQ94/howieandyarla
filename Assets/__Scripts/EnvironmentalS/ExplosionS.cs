using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExplosionS : MonoBehaviour {
	
	public float explosionLifeTime = 0.8f;
	public float halfExploTime;

	public GameObject attachedSprite;
	public List<Texture>	explosionFrames;
	public int 	currentExploFrame = 0;
	public float 	explosionFrameRate = 0.04f;
	public float 	explosionFrameRateMax = 0.04f;

	
	// Use this for initialization
	void Start () {
		
		halfExploTime = explosionLifeTime*0.25f; 
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (explosionLifeTime > 0){
			explosionLifeTime -= Time.deltaTime;
		}
		else{
			renderer.enabled = false;
			//Destroy(gameObject);
		}
		
		if (explosionLifeTime > halfExploTime){
			renderer.material.color = Color.black;
		}
		else{
			renderer.material.color = Color.white;
		}

		// animate attached sprite
		explosionFrameRate -= Time.deltaTime;
		if (explosionFrameRate <= 0){

			currentExploFrame ++;
			if (currentExploFrame >= explosionFrames.Count-1){
				Destroy(gameObject);
			}

			explosionFrameRate = explosionFrameRateMax;
			attachedSprite.renderer.material.SetTexture("_MainTex",explosionFrames[currentExploFrame]);
		}
		
	}
}

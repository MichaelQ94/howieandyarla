using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SingleAnimObjectS : MonoBehaviour {

	// for any object that appears, plays an animation, then destroys itself

	public List<Texture> animFrames;
	public float 	frameRateMax = 0.04f;
	public float 	frameRateCountdown;

	private	int 	currentFrame;

	// Use this for initialization
	void Start () {

		frameRateCountdown = frameRateMax;
	
	}
	
	// Update is called once per frame
	void Update () {

		frameRateCountdown -= Time.deltaTime;
		if (frameRateCountdown <= 0){
			currentFrame++;
			if (currentFrame >= animFrames.Count-1){
				Destroy(gameObject);
			}
			renderer.material.SetTexture("_MainTex",animFrames[currentFrame]);
		}
	
	}
}

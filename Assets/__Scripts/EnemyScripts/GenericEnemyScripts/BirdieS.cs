using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BirdieS : MonoBehaviour {

	public float animRate = 0.4f; // how often to switch frames
	public float countdownToNextFrame;
	public int 	 currentFrame;

	public List<Texture>	stunAnimFrames;
	public List<Texture>	vulnAnimFrames;

	public EnemyS	attachedEnemyS;

	void Start () {
		attachedEnemyS = transform.parent.gameObject.GetComponent<EnemyS>();
	}

	
	// Update is called once per frame
	void Update () {

		if (attachedEnemyS == null){
			attachedEnemyS = transform.parent.gameObject.GetComponent<EnemyS>();
		}
	
		// if renderer is on, animate
		if (renderer.enabled){
			countdownToNextFrame -= Time.deltaTime;
			if (countdownToNextFrame <= 0){
				currentFrame++;
				if (currentFrame > stunAnimFrames.Count-1){
					currentFrame = 0;
				}
				countdownToNextFrame = animRate;
			}
			if (attachedEnemyS.inWeakenedState() && !attachedEnemyS.knockedOut){
				renderer.material.SetTexture("_MainTex", vulnAnimFrames[currentFrame]);
			}
			else if(attachedEnemyS.knockedOut){
				renderer.material.SetTexture("_MainTex", stunAnimFrames[currentFrame]);
			}
			else{}
		}

	}
}

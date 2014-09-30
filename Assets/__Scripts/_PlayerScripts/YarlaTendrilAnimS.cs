using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class YarlaTendrilAnimS : MonoBehaviour {

	public Transform	targetHead;
	private	float 		originalZ;

	public float 	wiggleRate = 0.1f;
	public float 	wiggleRateMax = 0.1f;

	public int 		currentWiggleFrame = 0;

	public List<Texture>	wiggleFrames;

	// Use this for initialization
	void Start () {

		originalZ = transform.localPosition.z;
	
	}

	void FixedUpdate () {

		FaceHead();
		TendrilWiggle();

	
	}

	void ResetPosition () {

		if (!renderer.enabled){
			transform.localPosition = Vector3.zero;
		}

	}

	void TendrilWiggle () {

		if (renderer.enabled){

			wiggleRate -= Time.deltaTime;

			if (wiggleRate <= 0){

				currentWiggleFrame++;

				if (currentWiggleFrame > wiggleFrames.Count-1){

					currentWiggleFrame = 0;

				}

				wiggleRate = wiggleRateMax;

			}

			renderer.material.SetTexture("_MainTex",wiggleFrames[currentWiggleFrame]);
		}
	}

	void FaceHead(){

		renderer.enabled = targetHead.gameObject.renderer.enabled;

		Vector3 fixPos = (targetHead.localPosition-transform.localPosition)/2;
		fixPos.z = originalZ;
		transform.localPosition = fixPos;

		Vector3	fixScale = transform.localScale;
		float fixScaleDist = Vector3.Distance(new Vector3(targetHead.localPosition.x,targetHead.localPosition.y,transform.localPosition.z),
		                                      transform.localPosition);
		fixScale.x = fixScaleDist*1.75f;
		fixScale.y = fixScaleDist/3;
		transform.localScale = fixScale;

		float lookAngle = Mathf.Atan2 (targetHead.localPosition.y,targetHead.localPosition.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (new Vector3(0,0,lookAngle));

	}
}

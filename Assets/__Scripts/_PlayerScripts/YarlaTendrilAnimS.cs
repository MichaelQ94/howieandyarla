using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class YarlaTendrilAnimS : MonoBehaviour {

	public Transform	targetHead;
	private	float 		originalZ;

	public float 	wiggleRate = 0.1f;
	public float 	wiggleRateMax = 0.1f;

	public int 		currentWiggleFrame = 0;

	public YarlaS	attachedYarlaS;
	public NewChompS attachedChompS;

	public HowieS	howieScript;

	public GameObject	attachedHowie;

	public List<Texture>	wiggleFrames;
	public List<Texture>	wiggleFramesFire;

	public float 		dontShowTime;

	// Use this for initialization
	void Start () {

		originalZ = transform.localPosition.z;

		attachedHowie = GameObject.Find("HowieSprite");
		howieScript = GameObject.Find("Howie").GetComponent<HowieS>();

		if (targetHead.GetComponent<YarlaS>() != null){
			attachedYarlaS = targetHead.GetComponent<YarlaS>();
		}
		if (targetHead.GetComponent<NewChompS>() != null){
			attachedChompS = targetHead.GetComponent<NewChompS>();
		}


	
	}

	void FixedUpdate () {

		/*if (attachedHowie == null){
			if (attachedChompS != null){
				attachedHowie = attachedChompS.howie.gameObject;
			}
			if (attachedYarlaS != null){
				attachedHowie = attachedYarlaS.howie.gameObject;
			}
		}*/

		FaceHead();
		TendrilWiggle();

		/*if (dontShowTime > 0){
			dontShowTime -= Time.deltaTime;
			renderer.enabled = false;
		}
		else{
			if (!renderer.enabled){
				renderer.enabled = true;
			}
		}*/



	
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

			// change so i can detect what meta is active
			if (howieScript.metaActive){
				renderer.material.SetTexture("_MainTex",wiggleFramesFire[currentWiggleFrame]);
			}
			else{
				renderer.material.SetTexture("_MainTex",wiggleFrames[currentWiggleFrame]);
			}
		}
	}

	void FaceHead(){

		renderer.enabled = targetHead.gameObject.renderer.enabled;

		Vector3 fixPos = (targetHead.localPosition-attachedHowie.transform.localPosition)/2;
		fixPos.z = originalZ;
		transform.localPosition = fixPos;

		Vector3	fixScale = transform.localScale;
		float fixScaleDist = Vector3.Distance(new Vector3(targetHead.localPosition.x,targetHead.localPosition.y,transform.localPosition.z),
		                                      transform.localPosition);
		fixScale.x = fixScaleDist*1.25f;
		if (attachedChompS != null){
			if (!attachedChompS.attacking){
				fixScale.y = fixScaleDist*1.25f;
			}
		}
		if (attachedYarlaS != null){
			if (!attachedYarlaS.launched){
				fixScale.y = fixScaleDist*1.25f;
			}
		}
		transform.localScale = fixScale;

		float lookAngle = Mathf.Atan2 (targetHead.localPosition.y,targetHead.localPosition.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (new Vector3(0,0,lookAngle));

	}
}

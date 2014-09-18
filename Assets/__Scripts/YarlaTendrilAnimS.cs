using UnityEngine;
using System.Collections;

public class YarlaTendrilAnimS : MonoBehaviour {

	public Transform	targetHead;
	private	float 		originalZ;

	// Use this for initialization
	void Start () {

		originalZ = transform.localPosition.z;
	
	}

	void FixedUpdate () {

		FaceHead();

	
	}

	void ResetPosition () {

		if (!renderer.enabled){
			transform.localPosition = Vector3.zero;
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
		fixScale.x = fixScale.y = fixScaleDist;
		transform.localScale = fixScale;

		float lookAngle = Mathf.Atan2 (targetHead.localPosition.y,targetHead.localPosition.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (new Vector3(0,0,lookAngle));

	}
}

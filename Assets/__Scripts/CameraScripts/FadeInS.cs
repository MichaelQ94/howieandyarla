using UnityEngine;
using System.Collections;

public class FadeInS : MonoBehaviour {

	public float fadeInRate = 2;
	
	// Update is called once per frame
	void Update () {

		if (renderer.enabled){
			if (renderer.material.color.a > 0){
				Color	fadeCol = renderer.material.color;
				fadeCol.a -= fadeInRate*Time.deltaTime;
				renderer.material.color = fadeCol;
			}
			else{
				renderer.enabled = false;
			}
		}
	
	}
}

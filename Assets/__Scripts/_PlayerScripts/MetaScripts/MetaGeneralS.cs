using UnityEngine;
using System.Collections;

public class MetaGeneralS : MonoBehaviour {

	public int blueEnergyReq;
	public int redEnergyReq;
	public int purpleEnergyReq;

	public float durationMax = 10;
	public float duration = 10;

	public bool activated = false;

	public HowieS	attachedHowie;

	public void UpdateMeta(){

		duration -= Time.deltaTime;
		if (duration <= 0){
			attachedHowie.metaActive = false;
			attachedHowie.currentWalkSprite = 0;
			activated = false;
		}

	}

	public void Activate (){

		duration = durationMax;
		activated = true;

	}

}

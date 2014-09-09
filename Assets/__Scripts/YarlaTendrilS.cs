using UnityEngine;
using System.Collections;

public class YarlaTendrilS : MonoBehaviour {

	public TrailRenderer	yarlaGrabRenderer;
	public TrailRenderer	yarlaAttackRenderer;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


		if (HowieS.H.isHowieSolo){

			yarlaGrabRenderer.enabled = false;
			yarlaAttackRenderer.enabled = false;

		}
		else{

			if (NewChompS.N.attacking){
				yarlaAttackRenderer.enabled = true;
			}
			else{
				yarlaAttackRenderer.enabled = false;
			}

			if (YarlaS.Y.launched){
				yarlaGrabRenderer.enabled = true;
			}
			else{
				yarlaGrabRenderer.enabled = false;
			}

		}
	
	}
}
